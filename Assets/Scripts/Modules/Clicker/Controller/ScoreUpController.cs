using CWD.Scripts.Modules.Clicker.Config;
using CWD.Scripts.Modules.Clicker.Enum;
using CWD.Scripts.Modules.Clicker.Helper;
using CWD.Scripts.Modules.Clicker.Model;
using CWD.Scripts.Modules.Clicker.Pool;
using CWD.Scripts.Modules.Clicker.View;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Controller
{
    public class ScoreUpController : MonoBehaviour
    {
        [SerializeField] private ButtonView _buttonView;
        [SerializeField] private RectTransform _scoreUpContainer;

        private CanClickChecker _canClickChecker;
        private ClickerConfig _settings;
        private ScoreUpPool _scoresPool;

        [Inject]
        public void Construct(CanClickChecker canClickChecker, ClickerConfig settings, ScoreUpPool scoresPool)
        {
            _canClickChecker = canClickChecker;
            _settings = settings;
            _scoresPool = scoresPool;
        }

        private void OnEnable()
        {
            _buttonView.OnButtonClicked += HandleOnButtonClicked;
        }

        private void OnDisable()
        {
            _buttonView.OnButtonClicked -= HandleOnButtonClicked;
            _scoresPool.DespawnAll();
        }

        private void HandleOnButtonClicked(ClickReason clickReason)
        {
            if (!_canClickChecker.CanClick(clickReason))
            {
                return;
            }

            var model = new ScoreUpModel(_settings.ClickReward);

            ScoreUpView particle = _scoresPool.Spawn();

            particle.transform.SetParent(_scoreUpContainer, false);
            particle.Play(model);
        }
    }
}