using System;
using UniRx;

namespace Assets.Scripts.Core.Framework
{
    public class DisposableContainer : IDisposable
    {
        private CompositeDisposable _disposables;

        public DisposableContainer()
        {
            _disposables = new CompositeDisposable();
        }

        public T AddForDispose<T>(T disposableObject) where T : IDisposable
        {
            _disposables.Add(disposableObject);
            return disposableObject;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _disposables = null;
        }
    }
}
