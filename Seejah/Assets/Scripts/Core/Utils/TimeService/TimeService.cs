using Assets.Scripts.Core.Framework;
using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace Assets.Scripts.Core.Utils
{
    public interface ITimeService : IStartable, ITickable
    {
        public TimeServiceEvent Wait(int seconds);
        public TimeServiceEvent WaitRandom(int minSeconds, int maxSeconds);
    }

    public class TimeService : DisposableContainer, ITimeService
    {
        private RandomProvider _random;
        private List<TimeServiceEvent> _events;
        private List<TimeServiceEvent> _newEvents;

        public TimeService(RandomProvider random)
        {
            _random = random;
        }

        public TimeServiceEvent Wait(int seconds)
        {
            var evt = new TimeServiceEvent(Now, seconds);
            _newEvents.Add(evt);
            return evt;
        }
        public TimeServiceEvent WaitRandom(int minSeconds, int maxSeconds)
        {
            _random.GetRandom(minSeconds, maxSeconds, out int seconds);
            return Wait(seconds);
        }

        public void Start()
        {
            _events = new List<TimeServiceEvent>();
            _newEvents = new List<TimeServiceEvent>();
        }

        public void Tick()
        {
            _events.AddRange(_newEvents);
            _newEvents.Clear();
            foreach (var evt in _events)
            {
                if (evt.HasProgressObserver)
                    evt.ProcessProgress();
                if (evt.IsComplete(Now))
                    evt.ProcessDone();
            }
            _events.RemoveAll(evt => evt.NeedRemove);
        }

        private long Now => DateTimeOffset.Now.ToUnixTimeSeconds();

        public override void Dispose()
        {
            _events.Clear();
            _events = null;
            _random = null;
        }
    }
}
