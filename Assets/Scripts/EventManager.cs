using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Dictionary that will contain all of the events
    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("No event manager in the scene");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    // Used in the OnEnable
    // The object that subscribes to the particular event will react whenever it is triggered and will run the CALLBACK ACTION specified in the listener
    public void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        
        // if we find the event in the dictionary we add the LISTENER
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        // otherwise we add the LISTENER and then add the event to the dictionary
        // to ensure that there are no doubles
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    // Used in the OnDisable
    //
    public void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null)
        {
            return;
        }

        UnityEvent thiEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thiEvent))
        {
            thiEvent.RemoveListener(listener);
        }
    }

    // This method is for the SENDER side, it will Invoke the method
    // we pass as an argumen only if it finds it inside of the dictionary
    public void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
