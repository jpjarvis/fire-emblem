using FireEmblem.Model.Map;

namespace FireEmblem.MapView
{
    public interface IMapGenerator
    {
        void GenerateMap(Map map);
    }
}