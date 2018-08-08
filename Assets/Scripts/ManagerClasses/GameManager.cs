using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers
{
	public class GameManager : SemiSingleton<GameManager>
	{
		int _spawnerCount;
		[SerializeField] Text _spawnerCountText;
		[SerializeField] string _gameVictroyMessage;
		[SerializeField] string _gameDefeatMessage;
		[SerializeField] Text _gameOverText;
		[SerializeField] UnityEvent OnGameOverEvent,
		OnGameRestartEvent;

		void Awake ()
		{
			OnGameRestartEvent.Invoke ();
		}

		public void SetSpawnerCount (int count_)
		{
			_spawnerCount = count_;
			_spawnerCountText.text = _spawnerCount.ToString ();
		}

		public void OnSpawnerDeath ()
		{
			_spawnerCount--;
			_spawnerCountText.text = _spawnerCount.ToString ();
			if (_spawnerCount == 0) OnGameOver (true);
		}

		public void OnGameOver (bool gameOver_)
		{
			OnGameOverEvent.Invoke ();
			SettingsMenu._Instance.ToggleMenu ();
			if (gameOver_) _gameOverText.text = _gameVictroyMessage;
			else _gameOverText.text = _gameDefeatMessage;
		}
	}
}