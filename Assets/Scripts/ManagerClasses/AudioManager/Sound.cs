using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
	[System.Serializable]
	public class Sound
	{
		public string _name;

		public AudioClip _clip;

		[Range (0f, 1f)]
		public float _volume = .75f;
		[Range (0f, 1f)]
		public float _volumeVariance = .1f;

		[Range (.1f, 3f)]
		public float _pitch = 1f;
		[Range (0f, 1f)]
		public float _pitchVariance = .1f;

		public bool _loop = false;

		public AudioMixerGroup _mixerGroup;

		[HideInInspector]
		public AudioSource _source;
	}

}