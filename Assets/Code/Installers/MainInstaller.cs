using TN141.Combat;
using TN141.Combat.Factories;
using TN141.Configs;
using Zenject;

namespace TN141.Installers
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ConfigService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EventDispatcher>().AsSingle();
            Container.BindInterfacesAndSelfTo<StatFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuffFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<CombatController>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}