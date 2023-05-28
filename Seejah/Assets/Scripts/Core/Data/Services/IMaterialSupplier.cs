using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public interface IMaterialSupplier
    {
        Material GetMaterial(int id);
    }
}
