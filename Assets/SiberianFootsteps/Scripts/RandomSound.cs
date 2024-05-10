using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Recstazy.SiberianFootsteps
{
	[CreateAssetMenu(fileName = "NewRandomSound", menuName = "Sound/New Random Sound", order = 131)]
	public class RandomSound : ScriptableObject
	{
		#region Fields

		[SerializeField]
		private AudioClip[] _sounds;

		private List<int> _soundQueue;
		private int _currentIndex = 0;

		#endregion

		#region Properties

		#endregion

		public AudioClip GetSound()
        {
			if (_sounds.Length == 1) return _sounds[0];
			if (_soundQueue == null) _soundQueue = new List<int>();

			if (_soundQueue.Count != _sounds.Length)
			{
				_soundQueue.Clear();

				for (int i = 0; i < _sounds.Length; i++)
				{
					_soundQueue.Add(i);
				}

				Shuffle();
			}

			if (_currentIndex >= _soundQueue.Count) Shuffle();

			var clip = _sounds[_soundQueue[_currentIndex]];
			_currentIndex++;
			return clip;
		}

		public void SetSounds(params AudioClip[] _clips)
		{
			_sounds = _clips;
			_soundQueue = new List<int>();

			for (int i = 0; i < _sounds.Length; i++)
			{
				_soundQueue.Add(i);
			}

			Shuffle();
		}

		private void Shuffle()
		{
			_currentIndex = 0;

			for (int i = _soundQueue.Count - 1; i >= 0; i--)
			{
				int j = Random.Range(0, _soundQueue.Count);
				int temp = _soundQueue[j];
				_soundQueue[j] = _soundQueue[i];
				_soundQueue[i] = temp;
			}
		}
	}
}
