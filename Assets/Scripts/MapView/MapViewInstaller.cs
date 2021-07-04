using FireEmblem.Model.Data.Serialization;
using FireEmblem.Model.Map;
using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class MapViewInstaller : MonoInstaller
    {
        [SerializeField] private Grid grid;

        [SerializeField] private UnitPrefabProvider.Settings unitPrefabs;
        [SerializeField] private TilePrefabProvider.Settings tilePrefabs;
        
        public override void InstallBindings()
        {
            Container.Bind<IUnitPrefabProvider>().FromInstance(new UnitPrefabProvider(unitPrefabs));
            Container.Bind<ITilePrefabProvider>().FromInstance(new TilePrefabProvider(tilePrefabs));
            Container.Bind<IUnitStatsDisplayer>().To<ConsoleUnitStatsDisplayer>().AsTransient();

            Container.Bind<Grid>().FromInstance(grid).AsSingle();

            Container.Bind<UnitObjectManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TileObjectManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MapManager>().FromComponentInHierarchy().AsSingle();
            
            var map = Map.Create(MapLoader.LoadMap("Test"));
            Container.Bind<Map>().FromInstance(map);
        }
    }
}