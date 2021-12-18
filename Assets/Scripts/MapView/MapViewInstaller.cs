using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class MapViewInstaller : MonoInstaller
    {
        [SerializeField] private Grid grid;

        [SerializeField] private TilePrefabProvider.Settings tilePrefabs;
        
        public override void InstallBindings()
        {
            Container.Bind<ITilePrefabProvider>().FromInstance(new TilePrefabProvider(tilePrefabs));
            Container.Bind<IUnitStatsDisplayer>().To<ConsoleUnitStatsDisplayer>().AsTransient();

            Container.Bind<Grid>().FromInstance(grid).AsSingle();

            Container.Bind<TileObjectManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Map>().FromComponentInHierarchy().AsSingle();
        }
    }
}