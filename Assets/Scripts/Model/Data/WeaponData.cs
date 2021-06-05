namespace FireEmblem.Model.Data
{
    public class WeaponData
    {
        public string Name { get; set; }
        public int Might { get; set; }
        
        public int Hit { get; set; }
        
        public int Crit { get; set; }
        
        public int Weight { get; set; }
        
        public int MinRange { get; set; }
        
        public int MaxRange { get; set; }
        
        public bool IsMagic { get; set; }

        public bool IsInRange(int range)
        {
            return range >= MinRange && range <= MaxRange;
        }
    }
}