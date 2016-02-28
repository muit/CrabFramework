using UnityEngine;
using System.Collections;
using Crab;

namespace Crab.Entities
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