using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Data", menuName = "Crab/DataRow", order = 1)]
public class DataRowType : ScriptableObject {
    public string Name {
        get { return m_name; }
    }
    [SerializeField]
    private string m_name;

    public DataRow.AttributeContainer attributes = new DataRow.AttributeContainer();
}


