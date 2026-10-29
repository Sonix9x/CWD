using TMPro;
using UnityEngine;

namespace CWD.Scripts.Modules.Dogs.View
{
    public class BreedPopupView : MonoBehaviour
    {
        [SerializeField] private GameObject _popup;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public void Show(string title, string description)
        {
            _titleText.text = title;

            _descriptionText.text = string.IsNullOrWhiteSpace(description) ? "Описание отсутствует." : description;

            _popup.SetActive(true);
        }

        public void Hide()
        {
            _popup.SetActive(false);
        }
    }
}