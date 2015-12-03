using UnityEditor;

namespace Crab.Events
{
    using UnityEngine;
    using System.Collections.Generic;

    public class Trigger : MonoBehaviour
    {
        [System.NonSerialized]
        public Collider tCollider;

        public Crab.Event firedEvent;
        public Vector3 size;
        public LayerMask affectedLayers;

        void OnEnable() {
            tCollider = GetComponent<Collider>();

            tCollider.isTrigger = true;
            tCollider.enabled = true;
        }
        void OnTriggerEnter(Collider col) {
            if ((affectedLayers.value & 1 << col.gameObject.layer) == 1 << col.gameObject.layer) {
                Debug.Log("....");
                firedEvent.SendMessage("StartEvent");
            }
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
            t.affectedLayers = EditorGUILayout.LayerField(t.affectedLayers);

            EditorGUILayout.LabelField("Size");
            t.size = EditorGUILayout.Vector3Field("", t.size);

            EditorGUILayout.LabelField("Fired Event", EditorStyles.largeLabel);
            t.firedEvent = EditorGUILayout.ObjectField("", t.firedEvent, typeof(Crab.Event)) as Crab.Event;

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
