using System;
using System.Collections.Generic;
using CWD.Scripts.Modules.Dogs.Model;
using TMPro;
using UnityEngine;

namespace CWD.Scripts.Modules.Dogs.View
{
    public class DogBreedsView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private DogBreedIconView _breedItemPrefab;
        [SerializeField] private GameObject _listLoader;
        [SerializeField] private TextMeshProUGUI _errorText;

        private readonly List<DogBreedIconView> _items = new List<DogBreedIconView>();

        public void ShowLoader(bool value)
        {
            _listLoader.SetActive(value);
        }

        public void ShowError(string message)
        {
            _errorText.text = message;
            _errorText.gameObject.SetActive(true);
        }

        public void HideError()
        {
            _errorText.gameObject.SetActive(false);
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }

            _items.Clear();
        }

        public void ShowBreeds(BreedData[] breeds, Action<BreedData> onBreedClicked)
        {
            Clear();
            HideError();

            var count = Mathf.Min(10, breeds.Length);

            for (var i = 0; i < count; i++)
            {
                var breed = breeds[i];

                var item = Instantiate(_breedItemPrefab, _content);

                item.Initialize(i + 1, breed, onBreedClicked);

                _items.Add(item);
            }
        }

        public void SetBreedLoading(string breedId, bool isLoading)
        {
            foreach (var item in _items)
            {
                if (item.BreedId == breedId)
                {
                    item.SetLoading(isLoading);
                    return;
                }
            }
        }

        public void HideAllBreedLoaders()
        {
            foreach (var item in _items)
            {
                item.SetLoading(false);
            }
        }
    }
}