using System;

namespace NewRelic.Agent.Core.Utilities
{
    public class EventSubscription<T> : IDisposable
    {
        private readonly Action<T> _callback;

        public EventSubscription(Action<T> callback)
        {
            _callback = callback;
            EventBus<T>.Subscribe(_callback);
        }

        public void Dispose()
        {
            EventBus<T>.Unsubscribe(_callback);
        }
    }
}
