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
        public bool inmortal = false;
        [SerializeField]
        private int live = 100;
        public Faction faction = Faction.NO_FACTION;



        public int Live {
            set { live = value > 0 ? value : 0; }
            get { return live; }
        }

        public bool IsAlive() { return live > 0; }
    }
}