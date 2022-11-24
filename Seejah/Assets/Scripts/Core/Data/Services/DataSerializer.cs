using UnityEngine;

namespace Assets.Scripts.Core.Data.Services
{
    public class UnityJsonDataSerializer : IDataSerializer
    {
        public string SerializeFrom<T>(T data)
        {
            return JsonUtility.ToJson(data);
        }

        public T DeserializeTo<T>(string serializedData)
        {
            return JsonUtility.FromJson<T>(serializedData);
        }
    }
}