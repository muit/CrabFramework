using UnityEngine;
using System.Collections;

namespace Crab
{
    public class DamageVolume : MonoBehaviour
    {
        Entity m_owner;

        public bool producesDamage = false;

        void OnStart()
        {
            m_owner = GetComponentInParent<Entity>();
        }

        void OnTriggerEnter(Collider col)
        {
            DamageVolume damageVol = col.GetComponent<DamageVolume>();
            if (damageVol && damageVol.producesDamage)
            {

            }
        }

        void OnTriggerExit()
        {

        }
    }
}