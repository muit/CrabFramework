using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class DataTable : ScriptableObject
{
    public DataRowType type;
    public List<DataRow> rows = new List<DataRow>();

    void OnEnable() {
    }

    public void AddNew()
    {
        DataRow row = new DataRow();
        row.type = type;
        Add(row);
    }

    public void Add(DataRow dataRow) {
        if (!rows.Contains(dataRow))
            rows.Add(dataRow);
    }

    public DataRow FindById(int id)
    {
        return rows[id];
    }

    #if UNITY_EDITOR
    public void OnGUI() {
    }
    #endif
}
