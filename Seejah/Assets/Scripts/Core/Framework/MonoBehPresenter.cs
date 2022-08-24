using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.Presenters
{
    public class MonoBehPresenter : MonoBehaviour, IDisposable
    {
        private CompositeDisposable _disposables;

        private void Awake()
        {
            _disposables = new CompositeDisposable();
        }

        private void OnDisable()
        {
            Dispose();
        }

        public T AddForDispose<T>(T disposableObject) where T: IDisposable
        {
            _disposables.Add(disposableObject);
            return disposableObject;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
