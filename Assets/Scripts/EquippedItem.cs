namespace FireEmblem
{
    public class EquippedItem
    {
        public Item Item { get; private set; }
        
        public string Name
        {
            get { return Item.Name; }
        }

        public EquippableItemData EquippableItemData
        {
            get { return Item.EquippableItemData; }
        }

        public EquippedItem()
        {
            Item = new Item
            {
                EquippableItemData = EquippableItemData.Empty
            };
        }
        
        public bool Equip(Item item)
        {
            if (!item.IsEquippable())
            {
                return false;
            }

            Item = item;
            return true;
        }
    }
}