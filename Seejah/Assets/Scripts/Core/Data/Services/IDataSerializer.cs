namespace Assets.Scripts.Core.Data.Services
{
    public interface IDataSerializer
    {
        string SerializeFrom<T>(T data);
        T DeserializeTo<T>(string serializedData);
    }
}