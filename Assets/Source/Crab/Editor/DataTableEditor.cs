using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

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
    SerializedProperty rows;
    IEnumerable<FieldInfo> fields;

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

        if (m_dataTable)
        {
            serialized = new SerializedObject(m_dataTable);
            rows = serialized.FindProperty("rows");
            fields = m_dataTable.type.SystemType.GetFields().AsEnumerable<FieldInfo>().OrderBy(field => field.MetadataToken);
        }
    }

    void OnGUI()
    {
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

        GUILayout.BeginHorizontal();

        //Draw Ids
        GUILayout.BeginVertical();
        GUILayout.Label("Id", EditorStyles.miniButtonMid);
        for (int i = 0, len = m_dataTable.rows.Count; i < len; i++)
        {
            GUILayout.Label("" + i);
        }
        GUILayout.EndVertical();

        //Draw Properties


        if (m_dataTable.type != null)
        {
            foreach (FieldInfo attr in fields)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(attr.Name, EditorStyles.miniButtonMid);

                for (int i = 0; i < rows.arraySize; i++)
                {
                    /*
                    SerializedProperty row = rows.GetArrayElementAtIndex(i);
                    SerializedProperty prop = row.FindPropertyRelative(attr.Name);

                    if (prop != null)
                        EditorGUILayout.PropertyField(prop);
                    */

                    OnPropertyGUI(i, attr);
                }
                GUILayout.EndVertical();
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();



        if (GUI.changed && serialized != null)
        {
            serialized.ApplyModifiedProperties();
        }
    }

    void OnPropertyGUI(int id, FieldInfo field)
    {
        Debug.Log(field.FieldType.AssemblyQualifiedName);
        object value = field.GetValue(Convert.ChangeType(m_dataTable.rows[id], field.ReflectedType));
        object finalValue = null;

        if (field.FieldType.Name == "UInt16" ||
            field.FieldType.Name == "UInt32" ||
            field.FieldType.Name == "UInt64" ||
            field.FieldType.Name == "Int16" ||
            field.FieldType.Name == "Int32" ||
            field.FieldType.Name == "Int64")
        {
            finalValue = EditorGUILayout.IntField((int)value);
        }
        else
        {
            switch (field.FieldType.Name)
            {
                case "bool":
                    finalValue = EditorGUILayout.Toggle((bool)value);
                    break;
                case "Single":
                    finalValue = EditorGUILayout.FloatField((float)value);
                    break;
                case "String":
                    finalValue = EditorGUILayout.TextField((string)value);
                    break;
                case "Vector2":
                    finalValue = EditorGUILayout.Vector2Field("", (Vector2)value);
                    break;
                case "Vector3":
                    finalValue = EditorGUILayout.Vector3Field("", (Vector3)value);
                    break;
                case "Color":
                    finalValue = EditorGUILayout.ColorField((Color)value);
                    break;
                default:
                    EditorGUILayout.LabelField("Can't show a " + field.FieldType.Name + "variable");
                    break;
            }
        }
        
        if (finalValue != null)
        {
            field.SetValue(m_dataTable.rows[id], finalValue);
        }
    }
}
#endif