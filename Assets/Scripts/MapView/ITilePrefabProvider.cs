using System;
using UnityEngine;

namespace FireEmblem.MapView
{
    public interface ITilePrefabProvider
    {
        GameObject GetMoveTilePrefab();
    }

    internal class TilePrefabProvider : ITilePrefabProvider
    {
        private readonly GameObject _moveTilePrefab;

        public TilePrefabProvider(Settings settings)
        {
            _moveTilePrefab = settings.moveTilePrefab;
        }

        public GameObject GetMoveTilePrefab() => _moveTilePrefab;
        
        [Serializable]
        public class Settings
        {
            public GameObject moveTilePrefab;
        }
    }
}