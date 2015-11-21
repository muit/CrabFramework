using UnityEngine;
using UnityEditor;
using System.Collections;

public class ToolMenu : EditorWindow {
    private GUIStyle foldOutStyle = new GUIStyle();

    private bool showEntity = true;
    private bool showEntityComponents = false;
    private bool showItem = true;


    //Entity Data
    private string entity_name = "Entity";
    private Vector3 entity_position;
    private bool cAttributes;
    private bool cInventory;
    private bool cMovement;
    private bool cPerception;
    private bool cState;

    //Item Data
    private string item_name = "Item";
    private Vector3 item_position;



    [MenuItem("GameObject/Create Other/Crab Entity")]
    private static void CreateEntity() {
        GameObject entityObj = new GameObject("Entity");
        entityObj.AddComponent(typeof(Entity));
    }

    [MenuItem("GameObject/Create Other/Crab Item")]
    private static void CreateItem() {
        GameObject entityObj = new GameObject("Item");
        entityObj.AddComponent(typeof(Item));
    }

    void OnGUI() {
        titleContent = new GUIContent("Crab Tools");

        foldOutStyle.fontStyle = FontStyle.Bold;

        if (GUILayout.Button(showEntity ? "↖ Entity" : "↓ Entity", EditorStyles.boldLabel))
        {
            showEntity = !showEntity;
        }
        if (showEntity)
        {
            EditorGUI.indentLevel++;

            entity_name = EditorGUILayout.TextField(entity_name);
            EditorGUILayout.Space();

            entity_position = EditorGUILayout.Vector3Field("", entity_position);


            showEntityComponents = EditorGUILayout.Foldout(showEntityComponents, "Components");
            if (showEntityComponents)
            {
                EditorGUI.indentLevel++;
                cAttributes = EditorGUILayout.Toggle("Attributes", cAttributes);
                cInventory = EditorGUILayout.Toggle("Inventory", cInventory);
                cMovement = EditorGUILayout.Toggle("Movement", cMovement);
                cPerception = EditorGUILayout.Toggle("Perception", cPerception);
                cState = EditorGUILayout.Toggle("State", cState);
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Create Entity", EditorStyles.miniButtonMid))
        {
            if (string.IsNullOrEmpty(entity_name))
            {
                Debug.LogWarning("Crab: A name is required to create a new Entity");
                return;
            }

            GameObject entityObj = new GameObject(entity_name);
            entityObj.transform.position = entity_position;

            entityObj.AddComponent(typeof(Entity));

            if (cAttributes) entityObj.AddComponent(typeof(CAttributes));
            if (cInventory) entityObj.AddComponent(typeof(CInventory));
            if (cMovement) entityObj.AddComponent(typeof(CMovement));
            if (cPerception) entityObj.AddComponent(typeof(CPerception));
            if (cState) entityObj.AddComponent(typeof(CState));
        }

        if (showEntity) { EditorGUILayout.Space(); }





        if (GUILayout.Button(showItem ? "↖ Item" : "↓ Item", EditorStyles.boldLabel))
        {
            showItem = !showItem;
        }
        if (showItem)
        {
            EditorGUI.indentLevel++;

            item_name = EditorGUILayout.TextField(item_name);
            EditorGUILayout.Space();

            item_position = EditorGUILayout.Vector3Field("", item_position);
            
            EditorGUI.indentLevel--;
        }
        
        if (GUILayout.Button("Create Item", EditorStyles.miniButtonMid))
        {
            if (string.IsNullOrEmpty(entity_name))
            {
                Debug.LogWarning("Crab: A name is required to create a new Item");
                return;
            }

            GameObject entityObj = new GameObject(item_name);
            entityObj.transform.position = item_position;

            entityObj.AddComponent(typeof(Item));
        }


    }
}

