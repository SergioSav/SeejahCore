using System;
using UnityEngine.Localization;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class TutorialData
    {
        public string Title;
        public int Id;
        public int ImageId;
        public LocalizedString Message;
    }
}
