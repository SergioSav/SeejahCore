using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Rules;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Core.SceneInstallers
{
    public class LoadingInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<ConfigsStorage>(Lifetime.Singleton);

            builder.Register(c => c.Resolve<ConfigsStorage>().VisualDataList, Lifetime.Singleton);
            builder.Register<IPrefabPrototypeSupplier, PrefabPrototypeSupplier>(Lifetime.Singleton);
            builder.Register<ISpriteSupplier, SpriteSupplier>(Lifetime.Singleton);

            builder.Register(c => c.Resolve<ConfigsStorage>().GameRulesData, Lifetime.Singleton);
            builder.Register<GameRules>(Lifetime.Singleton);

            builder.Register(c => c.Resolve<ConfigsStorage>().CustomizationDataList, Lifetime.Singleton);
            builder.Register<CustomizationModel>(Lifetime.Singleton);

            builder.Register<IDataSerializer, UnityJsonDataSerializer>(Lifetime.Singleton);
            builder.Register<ISaveService, SaveService>(Lifetime.Singleton);
            builder.Register<UserModel>(Lifetime.Singleton);

            builder.Register<GameSettings>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
