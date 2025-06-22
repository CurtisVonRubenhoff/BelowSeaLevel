// Event Data Base Interface

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEventData
{
}

// Abstract Event Data Class
public abstract class EventData : IEventData
{
    public DateTime Timestamp { get; private set; }
    
    protected EventData()
    {
        Timestamp = DateTime.Now;
    }
}

// Event Channel Interface
public interface IEventChannel
{
    void Clear();
}

// Typed Event Channel
public class EventChannel<T> : IEventChannel where T : IEventData
{
    private readonly List<Action<T>> _listeners = new List<Action<T>>();

    public void Subscribe(Action<T> listener)
    {
        if (listener != null && !_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void Unsubscribe(Action<T> listener)
    {
        if (listener != null && _listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }

    public void Broadcast(T eventData)
    {
        if (eventData != null)
        {
            // Create a copy of listeners to avoid modification during iteration
            var listenersCopy = new List<Action<T>>(_listeners);
            
            foreach (var listener in listenersCopy)
            {
                try
                {
                    listener?.Invoke(eventData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error invoking event listener for {typeof(T).Name}: {e.Message}");
                }
            }
        }
    }

    public void Clear()
    {
        _listeners.Clear();
    }

    public int ListenerCount => _listeners.Count;
}

// Event Bus
public class EventBus : Singleton<EventBus>
{
    private readonly Dictionary<Type, IEventChannel> _channels = new Dictionary<Type, IEventChannel>();

    // Get or create a channel for the specified event type
    private EventChannel<T> GetChannel<T>() where T : IEventData
    {
        var eventType = typeof(T);
        
        if (!_channels.TryGetValue(eventType, out var channel))
        {
            channel = new EventChannel<T>();
            _channels[eventType] = channel;
        }

        return (EventChannel<T>)channel;
    }

    // Subscribe to an event type
    public static void Subscribe<T>(Action<T> listener) where T : IEventData
    {
        if (Instance != null)
        {
            Instance.GetChannel<T>().Subscribe(listener);
        }
    }

    // Unsubscribe from an event type
    public static void Unsubscribe<T>(Action<T> listener) where T : IEventData
    {
        if (Instance != null)
        {
            Instance.GetChannel<T>().Unsubscribe(listener);
        }
    }

    // Broadcast an event
    public static void Broadcast<T>(T eventData) where T : IEventData
    {
        if (Instance != null)
        {
            Instance.GetChannel<T>().Broadcast(eventData);
        }
    }

    // Clear all listeners for a specific event type
    public static void ClearChannel<T>() where T : IEventData
    {
        if (Instance != null)
        {
            Instance.GetChannel<T>().Clear();
        }
    }

    // Clear all channels
    public static void ClearAllChannels()
    {
        if (Instance != null)
        {
            foreach (var channel in Instance._channels.Values)
            {
                channel.Clear();
            }
            Instance._channels.Clear();
        }
    }

    // Get listener count for debugging
    public static int GetListenerCount<T>() where T : IEventData
    {
        if (Instance != null)
        {
            return Instance.GetChannel<T>().ListenerCount;
        }
        return 0;
    }

    private void OnDestroy()
    {
        ClearAllChannels();
    }
}