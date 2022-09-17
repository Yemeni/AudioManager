using System;

namespace CarterGames.Assets.AudioManager
{
    public class Evt
    {
        private event Action Action = delegate { };

        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise() => Action.Invoke();

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action listener)
        {
            Action -= listener;
            Action += listener;
        }

        
        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action listener) => Action -= listener;
        
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    public class Evt<T>
    {
        private event Action<T> Action = delegate { };

        
        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T param) => Action.Invoke(param);

        
        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T> listener)
        {
            Action -= listener;
            Action += listener;
        }
        

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T> listener) => Action -= listener;

        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
}