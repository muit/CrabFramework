using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Crab;

namespace Crab.Components
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class CInventory : MonoBehaviour
    {
        public int maxStacks = 16;
        public int maxStackSize = 64;


        List<ItemStack> content = new List<ItemStack>();

        private Entity me;
        void Awake()
        {
            me = GetComponent<Entity>();
        }


        public bool Add(ItemType type) {
            /*
            List<ItemStack> typeStacks = content.SelectMany(x => x.type == type) as List<ItemStack>;
            foreach (ItemStack stack in typeStacks) {
                if (stack.IsFull(maxStackSize)) {
                    continue;
                }
                stack.amount++;
                return true;
            }
            */
            return false;
        }
    }
}

public struct ItemStack
{
    public ItemType type;
    public int amount;

    public bool IsFull(int size) {
        return amount >= size;
    }
}