using UnityEngine;
using System.Collections.Generic;
using Crab.Utils;

namespace Crab
{


    public class EQS : MonoBehaviour
    {
        /* Parameters */
        public int points = 10;
        public float pointDistance = 1;
        public Vector3 offset;
        public float updateRate = 0.25f;
        [Space()]
        public VisibleFilter filter;

        [Header("Debug")]
        public Color color = Color.blue;
        public Color validColor = Color.green;
        /* End Parameters*/


        private HashSet<Vector3> map = new HashSet<Vector3>();
        private HashSet<Vector3> validPoints = new HashSet<Vector3>();

        Delay delay;

        void Start()
        {
            delay = new Delay((int)(updateRate * 1000), true);
        }

        void Update()
        {
            if (delay.Over())
            {
                //Wait for update rate
                RenderPoints();
                delay.Start();
            }
        }

        private void RenderPoints()
        {
            map.Clear();
            validPoints.Clear();

            Vector3 zero = transform.position + offset;

            for (int l = 0; l < points; l++)
            {
                for (int p = 0; p < points; p++)
                {
                    Vector3 point = new Vector3(zero.x + pointDistance * (p - points / 2), zero.y, zero.z + pointDistance * (l - points / 2));
                    map.Add(point);

                    //Run filters
                    if (filter.IsValid(point))
                    {
                        validPoints.Add(point);
                    }
                }
            }
        }

        public void GetPoint()
        {

        }

        [System.Serializable]
        public class Filter
        {
            public virtual bool IsValid(Vector3 point)
            {
                return true;
            }
        }

        [System.Serializable]
        public class DistanceFilter : Filter
        {
            public Transform target;
            public float distance = 1f;

            public override bool IsValid(Vector3 point)
            {
                if (!target)
                    return false;

                if (Vector3.SqrMagnitude(target.position - point) > distance * distance)
                {
                    return false;
                }

                return true;
            }
        }

        [System.Serializable]
        public class VisibleFilter : Filter
        {
            public Transform target;
            public LayerMask affectedLayers;

            public override bool IsValid(Vector3 point)
            {
                if (!target)
                    return false;

                RaycastHit hit;
                if (Physics.Raycast(point, target.position - point, out hit, Vector3.Distance(target.position, point), affectedLayers))
                {
                    if (hit.transform == target)
                    {
                        return true;
                    }

                    return false;
                }
                return true;
            }
        }

        /* Debug */
        void OnDrawGizmos()
        {
            Color gizmosColor = Gizmos.color;

            Gizmos.color = new Color(color.r, color.g, color.b, 0.4f);
            foreach (Vector3 point in map)
            {
                Gizmos.DrawWireSphere(point, 0.25f);
            }

            Gizmos.color = new Color(validColor.r, validColor.g, validColor.b, 0.4f);
            foreach (Vector3 point in validPoints)
            {
                Gizmos.DrawWireSphere(point, 0.25f);
            }

            Gizmos.color = gizmosColor;
        }
        /* End Debug */
    }
}
