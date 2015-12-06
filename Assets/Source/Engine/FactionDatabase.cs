using UnityEngine;
using UnityEditor;
using System;
using Crab;


namespace Crab
{
    using Controllers;

    public class FactionDatabase : MonoBehaviour {
        // Static singleton property
        private static FactionDatabase instance;
        public static FactionDatabase Get {
            get {
                return instance ? instance : instance = FindObjectOfType<FactionDatabase>();
            }
            private set { instance = value; }
        }


        public const string NO_FACTION = "NoFaction";

        public static FactionController AddController(Entity entity, string type) {
            if (type == NO_FACTION) return null;

            string controllerName = type + "Controller";
            return entity.gameObject.AddComponent(Type.GetType(controllerName)) as FactionController;
        }

        public static int FindId(string name) {
            return Array.IndexOf(Get.factions, name);
        }

        //Class
        public string[] factions = { NO_FACTION };
    }
}


[CustomEditor(typeof(Crab.FactionDatabase))]
public class FactionDBEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
    }


    public static void FactionField(SerializedProperty faction, string name = "Faction") {
        faction.stringValue = FactionField(faction.stringValue, name);
    }

    public static string FactionField(string faction, string name = "Faction") {
        FactionDatabase db = FactionDatabase.Get;
        int id;
        if (!db || (id = FactionDatabase.FindId(faction)) == -1) return String.IsNullOrEmpty(faction)? FactionDatabase.NO_FACTION : faction;

        int index = EditorGUILayout.Popup(name, id, db.factions);
        return db.factions[index];
    }
}