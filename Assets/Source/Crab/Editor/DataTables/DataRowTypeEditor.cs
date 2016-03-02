using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DataRowType))]
public class DataRowTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_name"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attributes"));

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
#endif