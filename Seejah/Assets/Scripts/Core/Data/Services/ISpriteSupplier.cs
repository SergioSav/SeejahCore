using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public interface ISpriteSupplier
    {
        Sprite GetSprite(int id);
    }
}
