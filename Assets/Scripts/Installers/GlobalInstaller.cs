using CWD.Scripts.Modules.Clicker.Config;
using CWD.Scripts.Modules.Clicker.Helper;
using CWD.Scripts.Modules.Clicker.Model;
using Zenject;

namespace CWD.Scripts.Modules.Clicker.Installer
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ClickerConfig>()
                     .FromScriptableObjectResource("Configs/ClickerSettings")
                     .AsSingle();
            
            Container.Bind<ScoreModel>().AsSingle().WithArguments(0.0f);
            Container.Bind<EnergyModel>()
                     .FromMethod(ctx =>
                     {
                         var config = ctx.Container.Resolve<ClickerConfig>();
                         return new EnergyModel(config.MaxEnergy, config.MaxEnergy);
                     })
                     .AsSingle();

            Container.Bind<CanClickChecker>().AsSingle();
        }
    }
}