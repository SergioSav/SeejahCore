using System;

namespace Assets.Scripts.Core.Utils
{
    public class TimeServiceEvent
    {
        private long _startTimestamp;
        private int _duration;

        //public Guid UniqId { get; private set; }

        private bool _needRemove;

        public Action OnDone { get; set; }
        public Action OnProgress { get; set; }

        public TimeServiceEvent(long startTimestamp, int duration)
        {
            _startTimestamp = startTimestamp;
            _duration = duration;

            //UniqId = Guid.NewGuid(); // for debug
        }

        public bool HasProgressObserver => OnProgress != null;

        public long ElapsedTime(long nowTimestamp) => nowTimestamp - _startTimestamp;

        public bool IsComplete(long nowTimestamp) => nowTimestamp >= _startTimestamp + _duration;

        public bool NeedRemove => _needRemove;

        public void ProcessProgress()
        {
            OnProgress?.Invoke();
        }

        public void ProcessDone()
        {
            if (_needRemove) return;
            OnDone?.Invoke();
            _needRemove = true;
        }
    }
}
