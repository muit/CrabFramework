using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

public class DataTableEditor : EditorWindow {
    [MenuItem("Crab/DataTables")]
    public static void ShowDataTableEditor()
    {
        EditorWindow.GetWindow(typeof(DataTableEditor));
    }

    void OnGUI() {
    }
}
#endif