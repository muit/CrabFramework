using UnityEngine;
using System.Collections.Generic;

namespace Crab.Utils
{
    public class ObjectPool : List<Item>
    {

        public readonly int size;

        public ObjectPool(int size) : base()
        {
            this.size = size;
        }

        public Item Create(Item prefab, Vector3 position)
        {
            return Create(prefab, position, Quaternion.identity);
        }

        public Item Create(Item prefab, Vector3 position, Quaternion rotation)
        {
            Item item;
            if (Count < size)
            {
                item = Item.Instantiate(prefab, position, rotation) as Item;
                Add(item);
            }
            else
            {
                item = this[0];
                RemoveAt(0);
                item.transform.position = position;
                item.transform.rotation = rotation;
                item.Reset();
                Add(item);
            }
            return item;
        }
    }
}