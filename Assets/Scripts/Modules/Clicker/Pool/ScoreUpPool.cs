using CWD.Scripts.Modules.Clicker.View;
using System.Collections.Generic;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Pool
{
    public class ScoreUpPool : MonoMemoryPool<ScoreUpView>
    {
        private readonly HashSet<ScoreUpView> _activeItems = new();

        protected override void OnSpawned(ScoreUpView item)
        {
            base.OnSpawned(item);
            _activeItems.Add(item);
        }

        protected override void OnDespawned(ScoreUpView item)
        {
            base.OnDespawned(item);
            _activeItems.Remove(item);
        }

        public void DespawnAll()
        {
            foreach (var item in new List<ScoreUpView>(_activeItems))
            {
                Despawn(item);
            }
        }
    }
}