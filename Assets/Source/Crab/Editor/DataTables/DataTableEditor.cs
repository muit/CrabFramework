using UnityEngine;
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
    private SerializedObject type;

    public void OnInspectorUpdate()
    {
        OnSelectionChange();
    }

    void OnSelectionChange()
    {
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

        if (m_dataTable != selection)
        {
            m_dataTable = selection;
            Setup();
            Repaint();
        }
            
    }

    void Setup() {
        if (!m_dataTable)
            return;

        serialized = new SerializedObject(m_dataTable);
        type = new SerializedObject(m_dataTable.type);
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUI.enabled = m_dataTable != null;
        if (GUILayout.Button("+ Add Row", EditorStyles.toolbarButton))
        {
            if (m_dataTable)
                m_dataTable.AddNew();
        }
        GUI.enabled = true;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (!m_dataTable)
            return;

        //Load Type
        if (serialized == null || type == null)
            Setup();

        //Update Properties
        serialized.Update();
        type.Update();

        SerializedProperty rows = serialized.FindProperty("rows");


        GUILayout.BeginHorizontal();
        //Draw Ids
        GUILayout.BeginVertical();
        GUILayout.Label("Id", EditorStyles.miniButtonMid);
        for (int i = 0, len = m_dataTable.rows.Count; i < len; i++)
        {
            DataRow.Attribute.OnPaintId(i);
        }
        GUILayout.EndVertical();

        //Draw Properties
        if (m_dataTable.type != null)
        {
            SerializedProperty keys = type.FindProperty("attributes").FindPropertyRelative("m_keys");
            for (int e = 0; e < keys.arraySize; e++)
            {
                GUILayout.BeginVertical();
                SerializedProperty key = keys.GetArrayElementAtIndex(e);
                GUILayout.Label(key.stringValue, EditorStyles.miniButtonMid);
                
                for (int i = 0; i < rows.arraySize; i++)
                {
                    SerializedProperty row = rows.GetArrayElementAtIndex(i);
                    SerializedProperty attrs = row.FindPropertyRelative("attributes");

                    if (attrs.FindPropertyRelative("m_values").arraySize != keys.arraySize)
                    {
                        //Change
                    }

                    DataRow.Attribute.OnPaintValue(attrs.FindPropertyRelative("m_values").GetArrayElementAtIndex(e));
                }
                GUILayout.EndVertical();
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(m_dataTable);
            serialized.ApplyModifiedProperties();
        }
    }
}
#endif