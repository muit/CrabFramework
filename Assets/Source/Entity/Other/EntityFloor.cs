using UnityEngine;
using System.Collections.Generic;
using Crab;
using Crab.Components;

namespace Crab.Utils
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]

    public class EntityFloor : MonoBehaviour
    {
        private Entity me;
        private CMovement movement;
        public LayerMask ignoreLayer;

        void Start()
        {
            me = GetComponentInParent<Entity>();
            movement = me.GetMovement();
        }

        List<Transform> insideObjects = new List<Transform>();

        void OnTriggerEnter(Collider col)
        {
            if (!IsInLayerMask(col.gameObject, ignoreLayer))
            {
                insideObjects.Add(col.transform);

                if (insideObjects.Count > 0)
                {
                    movement.StopFalling();
                }
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (!IsInLayerMask(col.gameObject, ignoreLayer))
            {
                insideObjects.Remove(col.transform);

                if (insideObjects.Count <= 0)
                {
                    movement.StartFalling();
                }
            }
        }

        public static bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            return ((mask.value & (1 << obj.layer)) > 0);
        }
    }
}