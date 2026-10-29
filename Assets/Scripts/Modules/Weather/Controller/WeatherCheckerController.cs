using System.Collections;
using System.Threading;
using CWD.Scripts.Modules.Common.Controller;
using CWD.Scripts.Modules.Weather.Model;
using CWD.Scripts.Modules.Weather.Service;
using CWD.Scripts.Modules.Weather.View;
using CWD.Scripts.Modules.Weather.Web;
using UnityEngine;

namespace CWD.Scripts.Modules.Weather.Controller
{
    public class WeatherController : TabControllerBase
    {
        [Header("Dependencies")] 
        [SerializeField] private WeatherView _view;

        [SerializeField] private WeatherRequestQueue _requestQueue;

        [Header("Settings")] 
        [SerializeField] private float _requestInterval = 5f;

        private WeatherService _weatherService;
        private Coroutine _updateCoroutine;

        private bool _isWeatherTabActive;

        private void Awake()
        {
            _weatherService = new WeatherService();
        }

        public override void OnTabOpened()
        {
            if (_isWeatherTabActive)
            {
                return;
            }

            _isWeatherTabActive = true;

            _updateCoroutine = StartCoroutine(UpdateWeatherLoop());
        }

        public override void OnTabClosed()
        {
            if (!_isWeatherTabActive)
            {
                return;
            }

            _isWeatherTabActive = false;

            if (_updateCoroutine != null)
            {
                StopCoroutine(_updateCoroutine);
                _updateCoroutine = null;
            }

            _requestQueue.CancelAll();
        }

        private IEnumerator UpdateWeatherLoop()
        {
            while (_isWeatherTabActive)
            {
                _requestQueue.Enqueue(token => FetchWeather(token));

                yield return new WaitForSeconds(_requestInterval);
            }

            _updateCoroutine = null;
        }

        private IEnumerator FetchWeather(CancellationToken token)
        {
            WeatherResponse response = null;
            string error = null;
            yield return _weatherService.GetForecast(token,
                                                     result => response = result,
                                                     message => error = message);

            if (token.IsCancellationRequested)
            {
                yield break;
            }

            if (!string.IsNullOrEmpty(error))
            {
                _view.ShowError(error);
                yield break;
            }

            if (response?.Properties?.Periods == null || response.Properties.Periods.Length == 0)
            {
                _view.ShowError("Прогноз погоды отсутствует.");
                yield break;
            }

            WeatherPeriod today = null;

            foreach (WeatherPeriod period in response.Properties.Periods)
            {
                if (period.IsDaytime && period.Name != null && period.Name.StartsWith("Today"))
                {
                    today = period;
                    break;
                }
            }

            if (today == null)
            {
                today = response.Properties.Periods[0];
            }

            var model = new WeatherModel();
            model.Update(today);

            Sprite icon = null;

            yield return _weatherService.GetIcon(today.Icon,
                                                 token,
                                                 result => icon = result,
                                                 message => Debug.LogWarning($"Weather icon: {message}"));

            if (token.IsCancellationRequested)
            {
                if (icon != null)
                {
                    Destroy(icon.texture);
                }

                yield break;
            }

            _view.Display(model, icon);
        }
    }
}