using System;
using CWD.Scripts.Modules.Dogs.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CWD.Scripts.Modules.Dogs.View
{
    public class DogBreedIconView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private GameObject _loader;

        public string BreedId { get; private set; }

        public void Initialize(int index, BreedData breed, Action<BreedData> onClick)
        {
            BreedId = breed.id;

            _nameText.text = $"{index} - {breed.attributes.name}";

            _loader.SetActive(false);

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onClick?.Invoke(breed));
        }

        public void SetLoading(bool isLoading)
        {
            _loader.SetActive(isLoading);
        }
    }
}