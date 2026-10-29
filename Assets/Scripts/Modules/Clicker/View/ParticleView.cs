using System.Collections;
using CWD.Scripts.Modules.Clicker.Pool;
using UnityEngine;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.View
{
    public class ParticleView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private ParticlePool _pool;
        private Coroutine _returnCoroutine;

        [Inject]
        public void Construct(ParticlePool pool)
        {
            _pool = pool;
        }

        public void Play()
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            _particleSystem.Play(true);

            if (_returnCoroutine != null)
            {
                StopCoroutine(_returnCoroutine);
            }

            _returnCoroutine = StartCoroutine(ReturnToPool());
        }

        private IEnumerator ReturnToPool()
        {
            yield return new WaitUntil(() => !_particleSystem.IsAlive(true));

            _returnCoroutine = null;
            _pool.Despawn(this);
        }

        private void OnDisable()
        {
            if (_returnCoroutine != null)
            {
                StopCoroutine(_returnCoroutine);
                _returnCoroutine = null;
            }

            if (_particleSystem != null)
            {
                _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }
}