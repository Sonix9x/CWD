using CWD.Scripts.Modules.Weather.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CWD.Scripts.Modules.Weather.View
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _weatherText;
        [SerializeField] private Image _weatherIcon;

        public void Display(WeatherModel model, Sprite icon)
        {
            _weatherText.text = $"Сегодня - {model.Temperature}{model.TemperatureUnit}";

            _weatherIcon.sprite = icon;
            _weatherIcon.enabled = icon != null;
        }

        public void ShowError(string message)
        {
            Debug.LogError($"Weather error: {message}");
        }
    }
}