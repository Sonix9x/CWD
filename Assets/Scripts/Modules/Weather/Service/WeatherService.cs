using System;
using System.Collections;
using System.Threading;
using CWD.Scripts.Modules.Weather.Web;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace CWD.Scripts.Modules.Weather.Service
{
    public class WeatherService
    {
        private const string ForecastUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public IEnumerator GetForecast(CancellationToken token, Action<WeatherResponse> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(ForecastUrl))
            {
                request.SetRequestHeader("User-Agent", "UnityWeatherApp/1.0");

                UnityWebRequestAsyncOperation operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    if (token.IsCancellationRequested)
                    {
                        request.Abort();
                        yield break;
                    }

                    yield return null;
                }


                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(request.error);
                    yield break;
                }

                WeatherResponse response;

                try
                {
                    response = JsonConvert.DeserializeObject<WeatherResponse>(request.downloadHandler.text);
                }
                catch (Exception exception)
                {
                    onError?.Invoke(exception.Message);
                    yield break;
                }

                if (response?.Properties?.Periods == null || response.Properties.Periods.Length == 0)
                {
                    onError?.Invoke("API не вернул прогноз погоды.");
                    yield break;
                }

                onSuccess?.Invoke(response);
            }
        }

        private string GetHighQualityIconUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            return url.Replace("size=medium", "size=large");
        }

        public IEnumerator GetIcon(string url, CancellationToken token, Action<Sprite> onSuccess, Action<string> onError)
        {
            if (string.IsNullOrEmpty(url))
            {
                onError?.Invoke("URL иконки отсутствует.");
                yield break;
            }

            url = GetHighQualityIconUrl(url);

            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                UnityWebRequestAsyncOperation operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    if (token.IsCancellationRequested)
                    {
                        request.Abort();
                        yield break;
                    }

                    yield return null;
                }

                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(request.error);
                    yield break;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                if (texture == null)
                {
                    onError?.Invoke("Не удалось загрузить иконку.");
                    yield break;
                }

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                onSuccess?.Invoke(sprite);
            }
        }
    }
}