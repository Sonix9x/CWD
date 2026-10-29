using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CWD.Scripts.Modules.Dogs.Web
{
    public class DogBreedRequestQueue : MonoBehaviour
    {
        private class Request
        {
            public string Key;
            public string Url;
            public Action<string> OnSuccess;
            public Action<string> OnError;

            public UnityWebRequest WebRequest;
            public bool IsCancelled;
        }

        private readonly Queue<Request> _queue = new Queue<Request>();

        private Request _activeRequest;
        private Coroutine _processingCoroutine;

        public void Enqueue(string key, string url, Action<string> onSuccess, Action<string> onError)
        {
            _queue.Enqueue(new Request
            {
                Key = key,
                Url = url,
                OnSuccess = onSuccess,
                OnError = onError
            });

            if (_processingCoroutine == null)
            {
                _processingCoroutine = StartCoroutine(ProcessQueue());
            }
        }

        public void Cancel(string key)
        {
            var remaining = new Queue<Request>();

            while (_queue.Count > 0)
            {
                var request = _queue.Dequeue();

                if (request.Key == key)
                {
                    request.IsCancelled = true;
                }
                else
                {
                    remaining.Enqueue(request);
                }
            }

            while (remaining.Count > 0)
            {
                _queue.Enqueue(remaining.Dequeue());
            }

            if (_activeRequest != null && _activeRequest.Key == key)
            {
                _activeRequest.IsCancelled = true;
                _activeRequest.WebRequest?.Abort();
            }
        }

        public void CancelAll()
        {
            while (_queue.Count > 0)
            {
                _queue.Dequeue().IsCancelled = true;
            }

            if (_activeRequest != null)
            {
                _activeRequest.IsCancelled = true;
                _activeRequest.WebRequest?.Abort();
            }
        }

        private IEnumerator ProcessQueue()
        {
            while (_queue.Count > 0)
            {
                var request = _queue.Dequeue();

                if (request.IsCancelled)
                    continue;

                _activeRequest = request;

                using (UnityWebRequest webRequest = UnityWebRequest.Get(request.Url))
                {
                    request.WebRequest = webRequest;

                    yield return webRequest.SendWebRequest();

                    if (!request.IsCancelled)
                    {
                        if (webRequest.result == UnityWebRequest.Result.Success)
                        {
                            request.OnSuccess?.Invoke(webRequest.downloadHandler.text);
                        }
                        else
                        {
                            request.OnError?.Invoke(webRequest.error);
                        }
                    }
                }

                if (_activeRequest == request)
                {
                    _activeRequest = null;
                }
            }

            _activeRequest = null;
            _processingCoroutine = null;
        }

        private void OnDestroy()
        {
            CancelAll();
        }
    }
}