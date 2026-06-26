using System;
using System.Collections.Generic;

namespace Assets.Scripts.Business.Services
{
    public abstract class Service
    {
        protected List<Action> actions = new List<Action>();

        public void AddSubscriber(Action callback)
        {
            actions.Add(callback);
        }

        public void RemoveSubscriber(Action callback)
        {
            actions.Remove(callback);
        }

        protected void ClearSubscribers()
        {
            actions.Clear();
        }

        protected void OnWorkFinished()
        {
            foreach (var action in actions) action();
            ClearSubscribers();
        }
    }
}
