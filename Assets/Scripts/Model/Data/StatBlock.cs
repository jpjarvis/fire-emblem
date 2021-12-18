using System;
using UnityEngine;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class StatBlock : IStatBlock
    {
        [SerializeField] private int hp;
        [SerializeField] private int strength;
        [SerializeField] private int magic;
        [SerializeField] private int dexterity;
        [SerializeField] private int luck;
        [SerializeField] private int speed;
        [SerializeField] private int defence;
        [SerializeField] private int resistance;
        [SerializeField] private int movement;

        public int Hp => hp;
        public int Strength => strength;
        public int Magic => magic;
        public int Dexterity => dexterity;
        public int Luck => luck;
        public int Speed => speed;
        public int Defence => defence;
        public int Resistance => resistance;
        public int Movement => movement;
    }
}