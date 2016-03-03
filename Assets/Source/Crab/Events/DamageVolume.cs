using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Crab.Utils;

namespace Crab.Events
{
    public class DamageVolume : Trigger
    {
        public Entity owner;
        public int damage = 5;
        public bool perSecond = false;

        private List<Entity> entitiesInside = new List<Entity>();
        private Delay delay = new Delay(1000, true);

        void Update() {
            int count = entitiesInside.Count;
            if (count > 0) {
                if (delay.Over()) {

                    for (int i = 0; i < count; i++) {
                        entitiesInside[i].Damage(damage, owner);
                    }
                    delay.Start();
                }
            }
        }

        protected override void OnEnter(GameObject go)
        {
            Entity entity = go.GetComponent<Entity>();
            if (entity)
            {
                if (!perSecond) {
                    entity.Damage(damage, owner);
                    return;
                }

                if (!entitiesInside.Contains(entity))
                {
                    entitiesInside.Add(entity);
                }
            }
        }

        protected override void OnExit(GameObject go)
        {
            Entity entity = go.GetComponent<Entity>();
            if (entity)
            {
                entitiesInside.Remove(entity);
            }
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(DamageVolume), true)]
    public class DamageVolumeEditor : TriggerEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnGUI();

            if (GUI.changed)
            {
                OnChange();
            }
        }

        protected override void OnGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("perSecond"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("owner"));
            GUILayout.Space(10);
            base.OnGUI();
        }
    }
#endif
}
