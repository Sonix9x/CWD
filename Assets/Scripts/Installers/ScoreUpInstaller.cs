using CWD.Scripts.Modules.Clicker.Controller;
using CWD.Scripts.Modules.Clicker.Pool;
using CWD.Scripts.Modules.Clicker.View;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Installer
{
    public class ScoreUpInstaller : MonoInstaller
    {
        [SerializeField] private ScoreUpView _scoreUpPrefab;
        [SerializeField] private RectTransform _scoreUpContainer;

        [SerializeField] private int _initialPoolSize = 10;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<ScoreUpView, ScoreUpPool>()
                .WithInitialSize(_initialPoolSize)
                .FromComponentInNewPrefab(_scoreUpPrefab)
                .UnderTransform(_scoreUpContainer);

            Container.Bind<ScoreUpController>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}