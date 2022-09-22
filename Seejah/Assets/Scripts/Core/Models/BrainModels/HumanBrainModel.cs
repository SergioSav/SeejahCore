using Assets.Scripts.Core.Framework;

namespace Assets.Scripts.Core.Models
{
    public class HumanBrainModel : DisposableContainer, IBrain
    {
        public bool IsHuman => true;
    }
}
