using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace FireEmblem.Model.Data.Serialization
{
    public static class MapLoader
    {
        private static string GetMapPath(string mapId) => Path.Combine("Data/Maps", mapId);
        
        public static MapData LoadMap(string mapId)
        {
            var json = Resources.Load<TextAsset>(Path.Combine(GetMapPath(mapId), "Map")).text;
            return JsonConvert.DeserializeObject<MapData>(json);
        }
    }
}