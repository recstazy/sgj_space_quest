using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overload
{
    [CreateAssetMenu(menuName = "Sound/Random Sound", order = 131)]
    public class RandomSound : ScriptableObject
    {
        private struct RandomComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return Random.Range(-1, 2);
            }
        }

        #region Fields

        [SerializeField]
        private AudioClip[] _clips;

        private int _currentIndex = 0;
        private int[] _clipIndices;

        #endregion

        #region Properties

        #endregion

        /// <summary> Get random clip from clips array </summary>
        public AudioClip GetClip()
        {
            if (_clipIndices == null || _clipIndices.Length != _clips.Length)
            {
                CreateIndices();
                Shuffle();
            }

            if (_currentIndex >= _clipIndices.Length)
            {
                _currentIndex = 0;
                Shuffle();
            }

            var clipIndex = _clipIndices[_currentIndex];
            _currentIndex++;
            return _clips[clipIndex];
        }

        private void CreateIndices()
        {
            _clipIndices = new int[_clips.Length];

            for (int i = 0; i < _clipIndices.Length; i++)
            {
                _clipIndices[i] = i;
            }
        }

        private void Shuffle()
        {
            System.Array.Sort(_clipIndices, new RandomComparer());
        }
    }
}