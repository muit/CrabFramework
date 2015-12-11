using UnityEngine;
using System.Collections;
using Crab;

namespace Crab.Components
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class CInventory : MonoBehaviour
    {
        private Entity me;
        void Awake()
        {
            me = GetComponent<Entity>();
        }
    }
}

public struct ItemStack
{

}