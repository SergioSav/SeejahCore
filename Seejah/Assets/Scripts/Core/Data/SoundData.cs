using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class SoundData
    {
        public string Title;
        public int Id;
        public SoundType Type;
        public List<AudioClip> SoundList;
    }
}
