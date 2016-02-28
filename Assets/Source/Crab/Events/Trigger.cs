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

        public List<EventLink> events = new List<EventLink>();
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
                Fire();
            }
        }

        public void Fire() {
            events.ForEach(x => {
                if (x != null && x.ev && x.ev.isActiveAndEnabled) x.ev.SendMessage("StartEvent");
            });
        }

        private bool IsInLayerMask(GameObject obj, LayerMask layerMask) {
            return (layerMask.value & (1 << obj.layer)) > 0;
        }


        //Render Event Conections
        void OnDrawGizmos() {
            if (!debug)
                return;

            Color gizmosColor = Gizmos.color;

            events.ForEach(x => {
                Gizmos.color = x.color;
                if (x != null && x.ev)
                {
                    Gizmos.DrawLine(transform.position, x.ev.transform.position);
                }
            });

            Gizmos.color = gizmosColor;
        }

        [System.Serializable]
        public sealed class EventLink {
            public Crab.Event ev;
            public Color color = Color.green;
        }
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(Trigger), true)]
    public class TriggerEditor : Editor
    {
        protected Trigger t;
        private ReorderableList events;

        private void OnEnable() {
            events = new ReorderableList(serializedObject,
                    serializedObject.FindProperty("events"),
                    true, true, true, true);
            events.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Activation");
            };
            events.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = events.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width- 60, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("ev"), GUIContent.none);

                SerializedProperty color = element.FindPropertyRelative("color");

                if (default(Color) == color.colorValue)
                    color.colorValue = Color.green;

                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width - 60, rect.y, 60, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("color"), GUIContent.none);
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
            
            UpdateGUI();

            events.DoLayoutList();

            t.debug = EditorGUILayout.Toggle("Debug", t.debug);


            if (GUI.changed)
            {
                t.tCollider.isTrigger = true;
                UpdateCollider();

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        protected virtual void UpdateGUI()
        {
            SerializedProperty layers = serializedObject.FindProperty("affectedLayers");
            layers.isExpanded = EditorGUILayout.Foldout(layers.isExpanded, "Collider Settings");

            if(layers.isExpanded)
            {
                EditorGUI.indentLevel++;
                t.affectedLayers = Util.LayerMaskField("Layers", t.affectedLayers);
                t.size = EditorGUILayout.Vector3Field("Size", t.size);
                EditorGUI.indentLevel--;
            }
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