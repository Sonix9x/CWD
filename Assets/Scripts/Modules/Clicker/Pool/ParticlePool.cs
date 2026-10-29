using CWD.Scripts.Modules.Clicker.View;
using System.Collections.Generic;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Pool
{
    public class ParticlePool : MonoMemoryPool<ParticleView>
    {
        private readonly HashSet<ParticleView> _activeItems = new();

        protected override void OnSpawned(ParticleView item)
        {
            base.OnSpawned(item);
            _activeItems.Add(item);
        }

        protected override void OnDespawned(ParticleView item)
        {
            base.OnDespawned(item);
            _activeItems.Remove(item);
        }

        public void DespawnAll()
        {
            foreach (var item in new List<ParticleView>(_activeItems))
            {
                Despawn(item);
            }
        }
    }
}