using System;

namespace Assets.Scripts.Core.Utils
{
    public static class TimeServiceExtensions
    {
        public static TimeServiceEvent Then(this TimeServiceEvent evt, Action onDone)
        {
            evt.OnDone = onDone;
            return evt;
        }

        public static TimeServiceEvent InProgress(this TimeServiceEvent evt, Action onProgress)
        {
            evt.OnProgress = onProgress;
            return evt;
        }
    }
}
