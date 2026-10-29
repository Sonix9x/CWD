using CWD.Scripts.Modules.Clicker.Controller;
using CWD.Scripts.Modules.Clicker.Pool;
using CWD.Scripts.Modules.Clicker.View;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Installer
{
    public class ParticleInstaller : MonoInstaller
    {
        [SerializeField] private ParticleView _particlePrefab;
        [SerializeField] private RectTransform _particleContainer;

        [SerializeField] private int _initialPoolSize = 10;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<ParticleView, ParticlePool>()
                .WithInitialSize(_initialPoolSize)
                .FromComponentInNewPrefab(_particlePrefab)
                .UnderTransform(_particleContainer);

            Container.Bind<ParticleController>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}