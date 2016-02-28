using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class DataRowType : ScriptableObject {
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Crab/DataRow")]
    private static void CreateDataRowType()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        DataRowType dataTable = ScriptableObject.CreateInstance<DataRowType>();
        ProjectWindowUtil.CreateAsset(dataTable, path + "/New DataRow.asset");
    }
#endif

    public string Name {
        get { return m_name; }
    }
    [SerializeField]
    private string m_name;

    public DataRow.AttributeContainer attributes = new DataRow.AttributeContainer();
}


