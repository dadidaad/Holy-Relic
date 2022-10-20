using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyEvent : UnityEvent<GameObject, string>
{

}
public class EventManager : MonoBehaviour
{
    public static EventManager eventManagerInstance;

    private Dictionary<string, MyEvent> Dictionary = new Dictionary<string, MyEvent>();

	void Awake()
	{
		if (eventManagerInstance == null)
		{
			eventManagerInstance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (eventManagerInstance != this)
		{
			Destroy(gameObject);
			return;
		}
	}
	public static void InvokeEvent(string eventName, GameObject obj, string param)
	{
		if (eventManagerInstance == null)
		{
			return;
		}
		MyEvent thisEvent = null;
		bool exist = eventManagerInstance.Dictionary.TryGetValue(eventName, out thisEvent);
		if (eventManagerInstance.Dictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.Invoke(obj, param);
		}
	}
	public static void StartListening(string eventName, UnityAction<GameObject, string> listener)
	{
		if (eventManagerInstance == null)
		{
			eventManagerInstance = FindObjectOfType(typeof(EventManager)) as EventManager;
			if (eventManagerInstance == null)
			{
				return;
			}
		}
		MyEvent thisEvent = null;
		bool exist = eventManagerInstance.Dictionary.TryGetValue(eventName, out thisEvent);
		if (exist)
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new MyEvent();
			thisEvent.AddListener(listener);
			eventManagerInstance.Dictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction<GameObject, string> listener)
	{
		if (eventManagerInstance == null)
		{
			return;
		}
		MyEvent thisEvent = null;
		bool exist = eventManagerInstance.Dictionary.TryGetValue(eventName, out thisEvent);
		if (exist)
		{
			thisEvent.RemoveListener(listener);
		}
	}

	
	void OnDestroy()
	{
		if (eventManagerInstance == this)
		{
			eventManagerInstance = null;
		}
	}
}
