using UnityEngine;

namespace C_.Package
{
    public class InventoryItem
    {
        public Sprite ItemIcon;
        public int ItemID;
        public string ItemName;
        public int Number;
        public int SlotID;

        //构造函数，初始化物品属性
        public InventoryItem(Sprite icon, string name,int id, int number, int sid)
        {
            ItemIcon = icon;
            ItemID = id;
            ItemName = name;
            Number = number;
            SlotID = sid;
        }

        public void AddQuantity(int amount)
        {
            Number += amount;
        }
    }

}

