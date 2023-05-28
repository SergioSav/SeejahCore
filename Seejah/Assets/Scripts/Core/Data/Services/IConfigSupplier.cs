using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public interface IConfigSupplier
    {
        T GetConfig<T>(int id) where T : Object;
    }
}
