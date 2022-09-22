using System;

namespace Assets.Scripts.Core.Utils
{
    public class TimeServiceEvent
    {
        private int _startTimestamp;
        private int _duration;

        private bool _prot;

        public Action OnDone { get; set; }
        public Action OnProgress { get; set; }

        public TimeServiceEvent(int startTimestamp, int duration)
        {
            _startTimestamp = startTimestamp;
            _duration = duration;
        }

        public bool HasProgressObserver => OnProgress != null;

        public int ElapsedTime(int nowTimestamp) => nowTimestamp - _startTimestamp;

        public bool IsComplete(int nowTimestamp) => nowTimestamp >= _startTimestamp + _duration;

        public void ProcessProgress()
        {
            OnProgress?.Invoke();
        }

        public void ProcessDone()
        {
            if (_prot) return;
            OnDone?.Invoke();
            UnityEngine.Debug.Log($"ONDON! {_startTimestamp}-{_duration}");
            _prot = true;
        }
    }
}
