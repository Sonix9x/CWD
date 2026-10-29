using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace CWD.Scripts.Modules.Weather.Web
{
    public class WeatherRequestQueue : MonoBehaviour
    {
        private class RequestJob
        {
            public Func<CancellationToken, IEnumerator> RoutineFactory;
            public CancellationTokenSource Cancellation;
        }

        private readonly Queue<RequestJob> _queue = new();

        private RequestJob _activeJob;
        private Coroutine _processor;

        public void Enqueue(Func<CancellationToken, IEnumerator> routineFactory)
        {
            if (_activeJob != null || _queue.Count > 0)
            {
                return;
            }

            var job = new RequestJob
            {
                RoutineFactory = routineFactory,
                Cancellation = new CancellationTokenSource()
            };

            _queue.Enqueue(job);

            _processor ??= StartCoroutine(ProcessQueue());
        }

        public void CancelAll()
        {
            while (_queue.Count > 0)
            {
                RequestJob job = _queue.Dequeue();

                job.Cancellation.Cancel();
                job.Cancellation.Dispose();
            }

            if (_activeJob != null)
            {
                _activeJob.Cancellation.Cancel();
            }
        }

        private IEnumerator ProcessQueue()
        {
            while (_queue.Count > 0)
            {
                _activeJob = _queue.Dequeue();

                RequestJob job = _activeJob;

                yield return StartCoroutine(job.RoutineFactory(job.Cancellation.Token));

                job.Cancellation.Dispose();

                if (_activeJob == job)
                {
                    _activeJob = null;
                }
            }

            _processor = null;
        }

        private void OnDestroy()
        {
            CancelAll();
        }
    }
}