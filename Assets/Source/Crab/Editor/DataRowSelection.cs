using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

public class DataRowSelection : EditorWindow {

    [MenuItem("Assets/Create/Crab/DataTable")]
    private static void CreateDataTable()
    {
        //Show Row Type Selection
        var window = EditorWindow.CreateInstance<DataRowSelection>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        window.assetPath = path + "/New DataTable.asset";

        window.position = new Rect(Screen.width / 2, Screen.height / 2, 200, 50);
        window.ShowUtility();
    }


    public string assetPath;
    private System.Type[] rowTypes;
    private string[] rowTypeNames;
    private int selectedRowType = 0;
    
    void OnGUI() {

        //Get all row types
        if (rowTypes == null)
        {
            rowTypes = Assembly.GetAssembly(typeof(DataRow)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(DataRow)))
                .ToArray();

            rowTypeNames = rowTypes.Select(t => t.Name).ToArray();
        }

        EditorGUILayout.LabelField("Select the datatable row type:");

        selectedRowType = EditorGUILayout.Popup(selectedRowType, rowTypeNames, EditorStyles.toolbarPopup);

        GUILayout.Space(10);

        //Disable button is there is no row types
        GUI.enabled = rowTypes.Length > 0;

        if (GUILayout.Button("Create", EditorStyles.toolbarButton)) {
            DataTable dataTable = ScriptableObject.CreateInstance<DataTable>();
            dataTable.type = new SerializableSystemType(rowTypes[selectedRowType]);
            ProjectWindowUtil.CreateAsset(dataTable, assetPath);
            this.Close();
        }

        GUI.enabled = true;
    }
}
#endif