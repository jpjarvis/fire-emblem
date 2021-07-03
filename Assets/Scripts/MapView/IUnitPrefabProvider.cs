using System;
using UnityEngine;

namespace FireEmblem.MapView
{
    public interface IUnitPrefabProvider
    {
        GameObject GetPlayerUnitPrefab();
        GameObject GetEnemyUnitPrefab();
    }

    internal class UnitPrefabProvider : IUnitPrefabProvider
    {
        private readonly GameObject _playerUnitPrefab;
        private readonly GameObject _enemyUnitPrefab;

        public UnitPrefabProvider(Settings settings)
        {
            _playerUnitPrefab = settings.playerUnitPrefab;
            _enemyUnitPrefab = settings.enemyUnitPrefab;
        }

        public GameObject GetPlayerUnitPrefab() => _playerUnitPrefab;
        public GameObject GetEnemyUnitPrefab() => _enemyUnitPrefab;
        
        [Serializable]
        public class Settings
        {
            public GameObject playerUnitPrefab;
            public GameObject enemyUnitPrefab;
        }
    }
}