using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public interface IPrefabPrototypeSupplier
    {
        T GetPrototype<T>(int id) where T : Object;
    }
}
