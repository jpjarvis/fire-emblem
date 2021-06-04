namespace FireEmblem
{
    public class EquippedWeapon
    {
        public Item Item { get; private set; }
        
        public string Name
        {
            get { return Item.Name; }
        }

        public WeaponData WeaponData
        {
            get { return Item.EquippableItemData.WeaponData; }
        }

        public EquippableItemData EquippableItemData
        {
            get { return Item.EquippableItemData; }
        }
        
        public void ExpendDurability()
        {
            Item.Durability.Decrement();
        }

        public bool IsBroken()
        {
            return Item.Durability.IsEmpty();
        }
        
        public bool Equip(Item item)
        {
            if (!item.IsEquippable())
            {
                return false;
            }
            
            if (!item.EquippableItemData.IsWeapon())
            {
                return false;
            }

            Item = item;
            return true;
        }
    }
}