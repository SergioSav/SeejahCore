using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Utils.AudioService
{
    public class AudioClipSource
    {
        private readonly List<AudioClip> _source;

        public AudioClipSource(List<AudioClip> source)
        {
            _source = source;
        }

        public AudioClip GetFirst()
        {
            if (_source.Count > 0)
                return _source[0];
            return null;
        }

        public AudioClip GetRandom(RandomProvider random)
        {
            if (_source.Count == 1)
                return _source[0];
            else if (_source.Count > 1)
                return _source.Random(random);
            return null;
        }
    }
}