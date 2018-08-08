using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimedEvent
{
	[Tooltip ("Time (seconds) it takes for the event to be ready again.")]
	[SerializeField] float _eventTriggerDelay;
	[SerializeField] protected UnityEvent _event;

	bool _eventReady = true;
	Action _currentCallback = null;

	public bool IsEventReady ()
	{
		if (_eventReady) return true;
		return false;
	}

	/// <summary>
	/// Resets the timer if you pass the MonoBehaviour through.
	/// </summary>
	public void TriggerEvent (MonoBehaviour _mono = null)
	{
		_event.Invoke ();
		_eventReady = false;
		if (_mono != null) _mono.StartCoroutine (ResetEvent ());
	}

	public IEnumerator ResetEvent ()
	{
		yield return new WaitForSeconds (_eventTriggerDelay);
		_eventReady = true;

		if (_currentCallback != null)
		{
			_currentCallback.Invoke ();
			_currentCallback = null;
		}
	}

	public void OnEventComplete (Action action_)
	{
		_currentCallback = action_;
	}

}