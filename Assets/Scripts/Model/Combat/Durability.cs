namespace FireEmblem.Model.Combat
{
    public class Durability
    {
        public int Max { get; set; }
        
        public int Current { get; private set; }

        public void Decrement()
        {
            Current--;
        }

        public void Restore()
        {
            Current = Max;
        }

        public bool IsEmpty()
        {
            return Current == 0;
        }
    }
}