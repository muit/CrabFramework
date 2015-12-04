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
            serializedObject.Update();

            EditorGUILayout.LabelField("Affected Layers");
            t.affectedLayers = Util.LayerMaskField(t.affectedLayers);

            EditorGUILayout.LabelField("Size");
            st.radius = EditorGUILayout.FloatField(st.radius);

            EditorGUILayout.LabelField("Fires Event", EditorStyles.largeLabel);
            t.eventFired = EditorGUILayout.ObjectField("", t.eventFired, typeof(Crab.Event)) as Crab.Event;

            EditorGUILayout.LabelField("Finishes Event", EditorStyles.largeLabel);
            t.eventFinished = EditorGUILayout.ObjectField("", t.eventFinished, typeof(Crab.Event)) as Crab.Event;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Don't edit the collider.", EditorStyles.boldLabel);

            serializedObject.ApplyModifiedProperties();
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