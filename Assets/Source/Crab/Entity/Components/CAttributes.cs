using UnityEngine;
using Crab.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Crab.Entities
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class CAttributes : MonoBehaviour
    {
        private Entity me;
        void Awake()
        {
            me = GetComponent<Entity>();
            maxLive = live;
        }

        //Attributes
        public bool inmortal = false;
        [SerializeField]
        private int live = 100;
        public string faction;
        
        private int maxLive = 100;


        public int Live {
            set {
                live = value > 0 ? value : 0;
                if (live > maxLive)
                    maxLive = live;
            }
            get { return live; }
        }

        public bool IsAlive() { return live > 0; }

        public float LivePercentage {
            get { return live / maxLive*100; }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CAttributes))]
public class CAttributesEditor : Editor {
    CAttributes t;
    SerializedProperty faction;

    void Awake() {
        t = target as CAttributes;

        faction = serializedObject.FindProperty("faction");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "faction");
        FactionDBEditor.FactionField(faction);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif