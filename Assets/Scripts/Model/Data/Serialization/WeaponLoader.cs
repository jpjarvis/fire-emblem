using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace FireEmblem.Model.Data.Serialization
{
    public static class WeaponLoader
    {
        private const string WeaponDataPath = "Data/Weapons";
        
        public static WeaponData LoadWeaponData(string weaponId)
        {
            var json = Resources.Load<TextAsset>(WeaponDataPath).text;
            return JsonConvert.DeserializeObject<Dictionary<string, WeaponData>>(json)[weaponId];
        }
    }
}