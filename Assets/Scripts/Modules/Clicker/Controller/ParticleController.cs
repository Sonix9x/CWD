using CWD.Scripts.Modules.Clicker.Enum;
using CWD.Scripts.Modules.Clicker.Helper;
using CWD.Scripts.Modules.Clicker.Pool;
using CWD.Scripts.Modules.Clicker.View;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Controller
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] private ButtonView _buttonView;
        [SerializeField] private RectTransform _particlesContainer;

        private CanClickChecker _canClickChecker;
        private ParticlePool _particlePool;

        [Inject]
        public void Construct(CanClickChecker canClickChecker, ParticlePool particlePool)
        {
            _canClickChecker = canClickChecker;
            _particlePool = particlePool;
        }

        private void OnEnable()
        {
            _buttonView.OnButtonClicked += HandleOnButtonClicked;
        }

        private void OnDisable()
        {
            _buttonView.OnButtonClicked -= HandleOnButtonClicked;
            _particlePool.DespawnAll();
        }

        private void HandleOnButtonClicked(ClickReason clickReason)
        {
            if (!_canClickChecker.CanClick(clickReason))
            {
                return;
            }

            var particle = _particlePool.Spawn();
            particle.transform.SetParent(_particlesContainer, false);
            particle.Play();

        }
    }
}