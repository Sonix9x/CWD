using CWD.Scripts.Modules.Clicker.Model;
using CWD.Scripts.Modules.Clicker.View;
using CWD.Scripts.Modules.Clicker.Config;
using CWD.Scripts.Modules.Clicker.Enum;
using System.Collections;
using CWD.Scripts.Modules.Clicker.Helper;
using CWD.Scripts.Modules.Common.Controller;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Controller
{
    public class ClickerController : TabControllerBase
    {
        [SerializeField] private ButtonView _buttonView;
        
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private SoundView _soundView;

        private ClickerConfig _clickerConfig;
        private ScoreModel _scoreModel;
        private CanClickChecker _canClickChecker;
        
        private Coroutine _autoClickRoutine;
        
        [Inject]
        public void Construct(ClickerConfig clickerConfig, ScoreModel scoreModel, CanClickChecker canClickChecker)
        {
            _clickerConfig = clickerConfig;

            _scoreModel = scoreModel;

            _canClickChecker = canClickChecker;
        }
        
        public override void OnTabOpened()
        {
            UpdateScoreView();

            _autoClickRoutine = StartCoroutine(AutoClickRoutine());

            _buttonView.OnButtonClicked += HandleOnButtonClicked;
            _scoreModel.OnScoreChanged += OnScoreChanged;
        }

        public override void OnTabClosed()
        {
            StopCoroutine(_autoClickRoutine);
            _autoClickRoutine = null;

            _buttonView.OnButtonClicked -= HandleOnButtonClicked;
            _scoreModel.OnScoreChanged -= OnScoreChanged;
        }

        private IEnumerator AutoClickRoutine()
        {
            if (!_canClickChecker.CanClick(ClickReason.Auto))
            {
                yield return null;
                RestartCoroutine();
            }
            else
            {
                yield return new WaitForSecondsRealtime(_clickerConfig.AutoClickDelay);
                _buttonView.ClickButtonExternal();
            }
        }
        
        private void HandleOnButtonClicked(ClickReason clickReason)
        {
            ButtonClicked();
        }

        private void ButtonClicked()
        {
            if (!_canClickChecker.CanClick(ClickReason.Manual))
            {
                return;
            }

            _soundView.PlayAudio();

            RestartCoroutine();

            _scoreModel.UpdateScore(_clickerConfig.ClickReward);
        }

        private void RestartCoroutine()
        {
            StopCoroutine(_autoClickRoutine);
            _autoClickRoutine = StartCoroutine(AutoClickRoutine());
        }

        private void UpdateScoreView()
        {
            _scoreView.SetScoreText($"{_scoreModel.Score}");
        }

        private void OnScoreChanged()
        {
            UpdateScoreView();
        }
    }
}