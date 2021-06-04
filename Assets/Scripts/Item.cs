namespace FireEmblem
{
    public class Item
    {
        public string Name { get; set; }
        
        public Durability Durability { get; set; }
        
        public EquippableItemData EquippableItemData { get; set; }
        
        public bool HasDurability()
        {
            return Durability != null;
        }

        public bool IsEquippable()
        {
            return EquippableItemData != null;
        }
    }
}