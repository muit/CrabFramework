using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Crab.Components
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class CAttributes : MonoBehaviour
    {
        private Entity me;
        void Awake()
        {
            me = GetComponent<Entity>();
        }

        //Attributes
        public bool inmortal = false;
        [SerializeField]
        private int live = 100;
        public string faction;



        public int Live {
            set { live = value > 0 ? value : 0; }
            get { return live; }
        }

        public bool IsAlive() { return live > 0; }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Crab.Components.CAttributes))]
public class CAttributesEditor : Editor {
    Crab.Components.CAttributes t;
    SerializedProperty faction;

    void Awake() {
        t = target as Crab.Components.CAttributes;

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