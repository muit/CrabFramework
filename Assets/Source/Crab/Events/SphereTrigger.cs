#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Crab.Events
{
    using UnityEngine;

    public class SphereTrigger : Trigger
    {
        public float radius = 0.5f;
    }


#if UNITY_EDITOR
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
#endif
}