#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using CrabEditor;
#endif
using System.Collections.Generic;
using System.Linq;

namespace Crab.Events
{
    using UnityEngine;

    public class Trigger : MonoBehaviour
    {
        [System.NonSerialized]
        public Collider tCollider;

        public List<Crab.Event> eventsFired = new List<Crab.Event>();
        public List<Crab.Event> eventsFinished = new List<Crab.Event>();
        public Vector3 size;
        public LayerMask affectedLayers;

        public bool debug = true;

        void OnEnable() {
            tCollider = GetComponent<Collider>();

            tCollider.isTrigger = true;
            tCollider.enabled = true;
        }

        void OnTriggerEnter(Collider col) {
            if (IsInLayerMask(col.gameObject, affectedLayers)) {

                eventsFired.ForEach(x => {
                    if (x && x.isActiveAndEnabled) x.SendMessage("StartEvent");
                });
                eventsFinished.ForEach(x => {
                    if (x && x.isActiveAndEnabled) x.SendMessage("FinishEvent");
                });
            }
        }

        private bool IsInLayerMask(GameObject obj, LayerMask layerMask) {
            return (layerMask.value & (1 << obj.layer)) > 0;
        }


        //Render Event Conections
        void OnDrawGizmos() {
            if (!debug)
                return;

            Color gizmosColor = Gizmos.color;

            Gizmos.color = Color.red;
            eventsFinished.ForEach(x => {
                if (x) Gizmos.DrawLine(transform.position, x.transform.position);
            });

            Gizmos.color = Color.green;
            eventsFired.ForEach(x => {
                if (x) Gizmos.DrawLine(transform.position, x.transform.position);
            });

            Gizmos.color = gizmosColor;
        }
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(Trigger))]
    public class TriggerEditor : Editor
    {
        protected Trigger t;
        private ReorderableList firedEvents;
        private ReorderableList finishedEvents;

        private void OnEnable() {
            firedEvents = new ReorderableList(serializedObject,
                    serializedObject.FindProperty("eventsFired"),
                    true, true, true, true);
            firedEvents.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Fired Events");
            };
            firedEvents.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = firedEvents.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };


            finishedEvents = new ReorderableList(serializedObject,
                    serializedObject.FindProperty("eventsFinished"),
                    true, true, true, true);
            finishedEvents.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Finished Events");
            };
            finishedEvents.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = finishedEvents.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };
        }

        void Awake()
        {
            t = target as Trigger;

            //Find Collider or Create it
            t.tCollider = t.GetComponent<Collider>();
            UpdateCollider();
            t.tCollider.isTrigger = true;
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.LabelField("Affected Layers");
            t.affectedLayers = Util.LayerMaskField(t.affectedLayers);

            UpdateGUI();

            
            //EditorGUILayout.LabelField("Fires Event", EditorStyles.largeLabel);
            firedEvents.DoLayoutList();
            
            //EditorGUILayout.LabelField("Finishes Event", EditorStyles.largeLabel);
            finishedEvents.DoLayoutList();


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            t.debug = EditorGUILayout.Toggle("Debug", t.debug);
            EditorGUILayout.LabelField("Don't edit the collider.", EditorStyles.boldLabel);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                t.tCollider.isTrigger = true;
                UpdateCollider();

                EditorUtility.SetDirty(target);
            }
        }

        protected virtual void UpdateGUI() {
            EditorGUILayout.LabelField("Size");
            t.size = EditorGUILayout.Vector3Field("", t.size);
        }

        protected virtual void UpdateCollider()
        {
            if (!t.tCollider)
            {
                t.tCollider = t.gameObject.AddComponent<BoxCollider>();
            }

            BoxCollider collider = t.tCollider as BoxCollider;
            if (collider) {
                collider.size = t.size;
            }
        }
    }
    #endif
}