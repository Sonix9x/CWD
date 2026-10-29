using CWD.Scripts.Modules.Clicker.Model;
using CWD.Scripts.Modules.Clicker.View;
using CWD.Scripts.Modules.Clicker.Config;
using CWD.Scripts.Modules.Clicker.Enum;
using System.Collections;
using CWD.Scripts.Modules.Clicker.Helper;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Controller
{
    public class EnergyController : MonoBehaviour
    {
        [SerializeField] private ButtonView _buttonView;

        [SerializeField] private EnergyView _energyView;

        private ClickerConfig _clickerConfig;
        private EnergyModel _energyModel;
        private CanClickChecker _canClickChecker;

        private Coroutine _autoRestEnergyRoutine;
        
        [Inject]
        public void Construct(ClickerConfig clickerConfig, EnergyModel energyModel, CanClickChecker canClickChecker)
        {
            _clickerConfig = clickerConfig;

            _energyModel = energyModel;
            _canClickChecker = canClickChecker;
        }

        private void OnEnable()
        {
            UpdateEnergyView();

            _autoRestEnergyRoutine = StartCoroutine(AutoRestEnergyRoutine());

            _buttonView.OnButtonClicked += HandleOnButtonClicked;
            _energyModel.OnEnergyChanged += OnEnergyChanged;
        }

        private IEnumerator AutoRestEnergyRoutine()
        {
            yield return new WaitForSecondsRealtime(_clickerConfig.EnergyAutoRefillDelay);
            RestEnergy();
        }

        private void OnDisable()
        {
            _buttonView.OnButtonClicked -= HandleOnButtonClicked;
            _energyModel.OnEnergyChanged -= OnEnergyChanged;
        }

        private void HandleOnButtonClicked(ClickReason clickReason)
        {
            ButtonClicked(clickReason);
        }

        private void ButtonClicked(ClickReason clickReason)
        {
            _energyModel.UpdateEnergy(GetPriceForClick(clickReason));
            UpdateButtonState();
        }

        private int GetPriceForClick(ClickReason clickReason)
        {
            switch (clickReason)
            {
                case ClickReason.Manual:
                    return -_clickerConfig.EnergyPriceForClick;
                case ClickReason.Auto:
                    return -_clickerConfig.EnergyPriceForAutoClick;
                default:
                    return 0;
            }
        }

        private void RestEnergy()
        {
            StopCoroutine(_autoRestEnergyRoutine);
            _autoRestEnergyRoutine = StartCoroutine(AutoRestEnergyRoutine());

            _energyModel.UpdateEnergy(_clickerConfig.EnergyAutoRefill);

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            _buttonView.SetButtonEnabled(_canClickChecker.CanClick(ClickReason.Manual));
        }

        private void UpdateEnergyView()
        {
            _energyView.SetEnergyText($"{_energyModel.CurrentEnergy}/{_clickerConfig.MaxEnergy}");
        }
        
        private void OnEnergyChanged()
        {
            UpdateEnergyView();
        }
    }
}