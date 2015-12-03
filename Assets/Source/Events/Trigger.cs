using UnityEditor;

namespace Crab.Events
{
    using UnityEngine;
    using CrabEditor;

    public class Trigger : MonoBehaviour
    {
        [System.NonSerialized]
        public Collider tCollider;

        public Crab.Event eventFired;
        public Crab.Event eventFinished;
        public Vector3 size;
        public LayerMask affectedLayers;

        void OnEnable() {
            tCollider = GetComponent<Collider>();

            tCollider.isTrigger = true;
            tCollider.enabled = true;
        }

        void OnTriggerEnter(Collider col) {
            if (IsInLayerMask(col.gameObject, affectedLayers)) {
                eventFired.SendMessage("StartEvent");
                eventFinished.SendMessage("FinishEvent");
            }
        }

        private bool IsInLayerMask(GameObject obj, LayerMask layerMask) {
            if ((layerMask.value & (1 << obj.layer)) > 0)
                return true;
            else
                return false;
        }
    }
    

    [CustomEditor(typeof(Trigger))]
    public class TriggerEditor : Editor
    {
        protected Trigger t;

        void Awake()
        {
            t = target as Trigger;

            //Find Collider or Create it
            t.tCollider = t.GetComponent<Collider>();
            if (!t.tCollider)
            {
                t.tCollider = t.gameObject.AddComponent<BoxCollider>();
            }
            t.tCollider.isTrigger = true;
            UpdateCollider();
        }

        public override void OnInspectorGUI()
        {
            UpdateGUI();
            if (GUI.changed)
            {
                t.tCollider.isTrigger = true;
                UpdateCollider();

                EditorUtility.SetDirty(target);
            }
        }

        protected virtual void UpdateGUI() {
            serializedObject.Update();

            EditorGUILayout.LabelField("Affected Layers");
            t.affectedLayers = Util.LayerMaskField(t.affectedLayers);

            EditorGUILayout.LabelField("Size");
            t.size = EditorGUILayout.Vector3Field("", t.size);

            EditorGUILayout.LabelField("Fires Event", EditorStyles.largeLabel);
            t.eventFired = EditorGUILayout.ObjectField("", t.eventFired, typeof(Crab.Event)) as Crab.Event;

            EditorGUILayout.LabelField("Finishes Event", EditorStyles.largeLabel);
            t.eventFinished  = EditorGUILayout.ObjectField("", t.eventFinished, typeof(Crab.Event)) as Crab.Event;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Don't edit the collider.", EditorStyles.boldLabel);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void UpdateCollider()
        {
            BoxCollider collider = t.tCollider as BoxCollider;
            if (collider) {
                collider.size = t.size;
            }
        }
    }
}