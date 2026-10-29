using CWD.Scripts.Modules.Clicker.Enum;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWD.Scripts.Modules.Clicker.View
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public Action<ClickReason> OnButtonClicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void HandleButtonClicked()
        {
            OnButtonClicked?.Invoke(ClickReason.Manual);
        }

        public void ClickButtonExternal()
        {
            StartCoroutine(ExternalPressRoutine());
        }

        private IEnumerator ExternalPressRoutine()
        {
            var pointerData = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(gameObject, pointerData, ExecuteEvents.pointerDownHandler);

            yield return new WaitForSecondsRealtime(0.1f);

            ExecuteEvents.Execute(gameObject, pointerData, ExecuteEvents.pointerUpHandler);

            OnButtonClicked?.Invoke(ClickReason.Auto);
        }

        public void SetButtonEnabled(bool isEnabled)
        {
            _button.enabled = isEnabled;
        }
    }
}
