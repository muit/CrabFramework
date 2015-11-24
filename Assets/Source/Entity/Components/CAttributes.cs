using UnityEngine;
using System.Collections;
using Crab;

namespace Crab.Components
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class CAttributes : MonoBehaviour
    {
        private Entity me;
        void Awake()
        {
            me = GetComponent<Entity>();
        }

        //Attributes
        public System.UInt16 live = 100;
    }
}