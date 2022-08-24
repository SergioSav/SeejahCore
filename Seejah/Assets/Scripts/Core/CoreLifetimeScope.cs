using Assets.Scripts.Core;
using VContainer;
using VContainer.Unity;

public class CoreLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //builder.RegisterEntryPoint<Game>(Lifetime.Singleton);
    }
}
