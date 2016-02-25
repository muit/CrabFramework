using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class DataTable : ScriptableObject
{
    public Type type = typeof(DataRow);
    public DataRow referenceRow;
    public List<DataRow> rows;

    void OnEnable() {
        referenceRow = (DataRow)Activator.CreateInstance(type);
    }

    #if UNITY_EDITOR
    public void OnGUI() {

    }
    #endif
}
