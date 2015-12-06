using UnityEditor;

namespace Crab.Events
{
    using UnityEngine;
    using CrabEditor;

    public class SphereTrigger : Trigger
    {
        public float radius = 0.5f;
    }


    [CustomEditor(typeof(SphereTrigger))]
    public class SphereTriggerEditor : TriggerEditor
    {
        private SphereTrigger st;

        protected override void UpdateGUI()
        {
            EditorGUILayout.LabelField("Radius");
            st.radius = EditorGUILayout.FloatField(st.radius);
        }

        protected override void UpdateCollider()
        {
            if (!t.tCollider)
            {
                t.tCollider = t.gameObject.AddComponent<SphereCollider>();
            }

            st = t as SphereTrigger;

            SphereCollider collider = t.tCollider as SphereCollider;
            if (collider)
            {
                collider.radius = st.radius;
            }
        }
    }
}