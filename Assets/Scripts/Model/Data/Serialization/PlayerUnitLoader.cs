using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace FireEmblem.Model.Data.Serialization
{
    public static class PlayerUnitLoader
    {
        private const string PlayerUnitDataPath = "Data/PlayerUnits";
        
        public static List<UnitData> LoadPlayerUnits()
        {
            var json = Resources.Load<TextAsset>(PlayerUnitDataPath).text;
            return JsonConvert.DeserializeObject<List<UnitData>>(json);
        }
    }
}