using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

public class DataRowEditor : EditorWindow
{
    [MenuItem("Assets/Create/Crab/DataRow")]
    private static void CreateDataRowType()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        DataRowType dataTable = ScriptableObject.CreateInstance<DataRowType>();
        ProjectWindowUtil.CreateAsset(dataTable, path + "/New DataRow.asset");
    }

    [MenuItem("Crab/DataRows")]
    public static void ShowDataRowEditor()
    {
        EditorWindow.GetWindow(typeof(DataRowEditor));
    }

    private DataRowType m_dataRow;

    void OnSelectionChange()
    {
        //Load the selected datatable

        DataRowType selection = null;

        if (!selection)
            selection = Selection.activeObject as DataRowType;

        m_dataRow = selection;
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUI.enabled = m_dataRow != null;
        if (GUILayout.Button("+ Add Property", EditorStyles.toolbarButton))
        {
            m_dataRow.attributes.Add("new property", new DataRow.Attribute());
        }
        GUI.enabled = true;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginArea(new Rect(0, 20, 300, position.height - 20));

        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(300, 20, position.width - 310, position.height - 30), EditorStyles.textArea);

        if(m_dataRow != null)
        {
            Dictionary<string, DataRow.Attribute> attrs = m_dataRow.attributes;
            foreach (KeyValuePair<string, DataRow.Attribute> entry in attrs)
            {
                GUILayout.Button(entry.Key, EditorStyles.miniButtonLeft);
            }
        }

        GUILayout.EndArea();
    }
}
#endif