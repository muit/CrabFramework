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
    public List<DataRow> rows;


    #if UNITY_EDITOR
    public void OnGUI() {

    }
    #endif
}
