using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
	public class AudioManager : Singleton<AudioManager>
	{
		public AudioMixerGroup mixerGroup;
		public Sound[] sounds;

		protected override void Awake ()
		{
			base.Awake ();

			foreach (Sound s in sounds)
			{
				s._source = gameObject.AddComponent<AudioSource> ();
				s._source.clip = s._clip;
				s._source.loop = s._loop;

				s._source.outputAudioMixerGroup = mixerGroup;
			}
		}

		public void StopSound (string sound)
		{
			Sound s = Array.Find (sounds, item => item._name == sound);
			if (s == null)
			{
				Debug.LogWarning ("Sound: " + name + " not found!");
				return;
			}
			s._source.Stop ();
			s._source.volume = 0.0f;
		}

		public void Play (string sound)
		{
			Sound s = Array.Find (sounds, item => item._name == sound);
			if (s == null)
			{
				Debug.LogWarning ("Sound: " + name + " not found!");
				return;
			}

			s._source.volume = s._volume * (1f + UnityEngine.Random.Range (-s._volumeVariance / 2f, s._volumeVariance / 2f));
			s._source.pitch = s._pitch * (1f + UnityEngine.Random.Range (-s._pitchVariance / 2f, s._pitchVariance / 2f));

			s._source.Play ();
		}

		private void OnValidate ()
		{
			foreach (Sound sound in sounds)
			{
				if (sound._clip != null) sound._name = sound._clip.name;
			}
		}
	}
}