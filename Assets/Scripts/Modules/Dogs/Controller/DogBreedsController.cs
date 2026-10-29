using System;
using CWD.Scripts.Modules.Common.Controller;
using CWD.Scripts.Modules.Dogs.Model;
using CWD.Scripts.Modules.Dogs.View;
using CWD.Scripts.Modules.Dogs.Web;
using UnityEngine;
using UnityEngine.Serialization;

namespace CWD.Scripts.Modules.Dogs.Contoller
{
    public class DogBreedsController : TabControllerBase
    {
        private const string BaseUrl = "https://dogapi.dog/api/v2";
        private const string ListRequestKey = "breeds-list";
        private const string DetailsRequestKey = "breed-details";

        [SerializeField] private DogBreedRequestQueue _dogBreedRequestQueue;
        [SerializeField] private DogBreedsView _breedsView;
        [SerializeField] private BreedPopupView _popupView;

        private bool _isTabActive;
        private string _selectedBreedId;
        private int _selectionVersion;
        
        public override void OnTabOpened()
        {
            if (_isTabActive)
            {
                return;
            }

            _isTabActive = true;

            _breedsView.HideError();
            _breedsView.ShowLoader(true);

            _dogBreedRequestQueue.Enqueue(ListRequestKey, $"{BaseUrl}/breeds?page[size]=10", OnBreedsLoaded, OnBreedsError);
        }

        public override void OnTabClosed()
        {
            if (!_isTabActive)
            {
                return;
            }

            _isTabActive = false;

            _selectionVersion++;
            _selectedBreedId = null;

            _dogBreedRequestQueue.Cancel(ListRequestKey);
            _dogBreedRequestQueue.Cancel(DetailsRequestKey);

            _breedsView.ShowLoader(false);
            _breedsView.HideAllBreedLoaders();

            _popupView.Hide();
        }

        private void OnBreedsLoaded(string json)
        {
            if (!_isTabActive)
            {
                return;
            }

            _breedsView.ShowLoader(false);

            try
            {
                var response = JsonUtility.FromJson<BreedsResponse>(json);

                if (response == null || response.data == null)
                {
                    _breedsView.ShowError("Не удалось обработать список пород.");
                    return;
                }

                _breedsView.ShowBreeds(response.data, OnBreedSelected);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                _breedsView.ShowError("Ошибка обработки списка пород.");
            }
        }

        private void OnBreedsError(string error)
        {
            if (!_isTabActive)
            {
                return;
            }

            _breedsView.ShowLoader(false);

            _breedsView.ShowError($"Не удалось загрузить породы: {error}");
        }

        private void OnBreedSelected(BreedData breed)
        {
            if (!_isTabActive || breed == null)
            {
                return;
            }

            _dogBreedRequestQueue.Cancel(DetailsRequestKey);

            if (!string.IsNullOrEmpty(_selectedBreedId))
            {
                _breedsView.SetBreedLoading(_selectedBreedId, false);
            }

            var version = ++_selectionVersion;

            _selectedBreedId = breed.id;

            _popupView.Hide();

            _breedsView.SetBreedLoading(breed.id, true);

            var url = $"{BaseUrl}/breeds/{breed.id}";

            _dogBreedRequestQueue.Enqueue(DetailsRequestKey, url, json => OnBreedDetailsLoaded(json, breed.id, version),
                error => OnBreedDetailsError(error, breed.id, version));
        }

        private void OnBreedDetailsLoaded(string json, string requestedBreedId, int version)
        {
            if (!_isTabActive || version != _selectionVersion || requestedBreedId != _selectedBreedId)
            {
                return;
            }

            _breedsView.SetBreedLoading(requestedBreedId, false);

            try
            {
                var response = JsonUtility.FromJson<BreedResponse>(json);

                if (response == null || response.data == null || response.data.attributes == null)
                {
                    _breedsView.ShowError("Не удалось получить данные породы.");
                    return;
                }

                _popupView.Show(response.data.attributes.name, response.data.attributes.description);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                _breedsView.ShowError("Ошибка обработки данных породы.");
            }
        }

        private void OnBreedDetailsError(string error, string requestedBreedId, int version)
        {
            if (!_isTabActive || version != _selectionVersion || requestedBreedId != _selectedBreedId)
            {
                return;
            }

            _breedsView.SetBreedLoading(requestedBreedId, false);

            _breedsView.ShowError($"Не удалось загрузить данные породы: {error}");
        }
    }
}