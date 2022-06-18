using System;

namespace CarterGames.Assets.AudioManager
{
    public class Evt
    {
        private event Action action = delegate { };

        public void Raise()
        {
            action.Invoke();
        }

        public void Add(Action listener)
        {
            action -= listener;
            action += listener;
        }

        public void Remove(Action listener)
        {
            action -= listener;
        }
    }
    
    public class Evt<T>
    {
        private event Action<T> action = delegate { };

        public void Raise(T param)
        {
            action.Invoke(param);
        }

        public void Add(Action<T> listener)
        {
            action -= listener;
            action += listener;
        }

        public void Remove(Action<T> listener)
        {
            action -= listener;
        }
    }
}