using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class DataTable : ScriptableObject
{
    public SerializableSystemType type;
    public List<DataRow> rows = new List<DataRow>();

    void OnEnable() {
    }

    public void AddNew()
    {
        Debug.Log(type.SystemType);   
        Add((DataRow)Activator.CreateInstance(type.SystemType));
    }

    public void Add(DataRow dataRow) {
        if (!rows.Contains(dataRow))
            rows.Add(dataRow);
    }

    public T FindById<T>(int id)
        where T : DataRow
    {
        if (typeof(T) != type.SystemType)
        {
            Debug.LogError("Trying to find in a database with the incorrect type.");
            return null;
        }

        return (T) rows[id];
    }

    #if UNITY_EDITOR
    public void OnGUI() {
    }
    #endif
}
