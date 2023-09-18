using System;
using Interfaces;
using Weapons;

namespace Player
{
    [Serializable]
    public class InventoryDictionary : SerializableDictionary<InventoryItemType, IInventoryItem>
    {
        
    }
}