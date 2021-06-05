using System;

namespace FireEmblem.Common
{
    public class Rng
    {
        private static Rng _instance;
        
        public static Rng Instance => _instance ??= new Rng();

        private readonly Random _random;

        private Rng()
        {
            _random = new Random();
        }
        
        private Rng(int seed)
        {
            _random = new Random(seed);
        }

        public int GetRandomInt(int max)
        {
            return _random.Next(max);
        }

        public bool PerformCheck(int chance)
        {
            return GetRandomInt(100) < chance;
        }
    }
}