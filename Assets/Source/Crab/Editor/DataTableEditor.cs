using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;

public class DataTableEditor : EditorWindow
{
    [MenuItem("Crab/DataTables")]
    public static void ShowDataTableEditor()
    {
        EditorWindow.GetWindow(typeof(DataTableEditor));
    }

    private DataTable m_dataTable;
    private SerializedObject serialized;

    void OnSelectionChange()
    {
        //Load the selected datatable

        DataTable selection = null;

        if (Selection.activeGameObject)
        {
            DataFinder finder = Selection.activeGameObject.GetComponent<DataFinder>();
            if (finder)
            {
                selection = finder.dataTable;
            }
        }

        if (!selection)
            selection = Selection.activeObject as DataTable;

        m_dataTable = selection;

        if(m_dataTable)
        {
            serialized = new SerializedObject(m_dataTable);
        }
    }

    void OnGUI()
    {
        if (!m_dataTable)
            return;

        GUILayout.BeginHorizontal();

        //Draw Ids
        GUILayout.BeginVertical();
        GUILayout.Label("Id", EditorStyles.miniButtonMid);
        for (int i = 0, len = m_dataTable.rows.Count; i < len; i++)
        {
            GUILayout.Label(""+i);
        }
        GUILayout.EndVertical();

        //Draw Properties

        /*
        SerializedProperty it = serialized.FindProperty("referenceRow").FindPropertyRelative("name").Copy();
        while (it.Next(true))
        {
            GUILayout.BeginVertical();
            GUILayout.Label(it.name, EditorStyles.miniButtonMid);
            GUILayout.EndVertical();
        }*/

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();



        if (GUI.changed)
        {
            serialized.ApplyModifiedProperties();
        }
    }
}
#endif