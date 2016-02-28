using Crab.Entities;
using UnityEngine;
using UnityEditor;
using Crab;

public class ToolMenu : EditorWindow
{
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

    private string[] itemTypes;
    private int itemIndex = 0;

    void OnEnabled() {
        UpdateItems();
    }

    void OnFocus() {
        UpdateItems();
    }
    void OnHierarchyChange() {
        UpdateItems();
    }



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
        if (!MainEditor.IsSetup()) {
            GUILayout.Space(30);
            if (GUILayout.Button("Setup the Scene")) {
                MainEditor.Setup();
            }
            return;
        }

        if (itemTypes == null) UpdateItems();


        titleContent = new GUIContent("Crab Tools");

        foldOutStyle.fontStyle = FontStyle.Bold;

        if (GUILayout.Button(showEntity ? "↖ Entity" : "↓ Entity", EditorStyles.boldLabel)) {
            showEntity = !showEntity;
        }
        if (showEntity) {
            EditorGUI.indentLevel++;

            entity_name = EditorGUILayout.TextField(entity_name);
            EditorGUILayout.Space();

            entity_position = EditorGUILayout.Vector3Field("", entity_position);


            showEntityComponents = EditorGUILayout.Foldout(showEntityComponents, "Components");
            if (showEntityComponents) {
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

        if (GUILayout.Button("Create Entity", EditorStyles.miniButtonMid)) {
            if (string.IsNullOrEmpty(entity_name)) {
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





        if (GUILayout.Button(showItem ? "↖ Item" : "↓ Item", EditorStyles.boldLabel)) {
            showItem = !showItem;
            if (showItem) UpdateItems();
        }
        if (showItem) {
            EditorGUI.indentLevel++;

            itemIndex = EditorGUILayout.Popup(itemIndex, itemTypes);
            EditorGUILayout.Space();

            item_name = EditorGUILayout.TextField(item_name);

            item_position = EditorGUILayout.Vector3Field("", item_position);

            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Create Item", EditorStyles.miniButtonMid)) {
            if (string.IsNullOrEmpty(item_name)) {
                Debug.LogWarning("Crab: A name is required to create a new Item");
                return;
            }

            GameObject entityObj = new GameObject(item_name);
            entityObj.transform.position = item_position;

            Item item = entityObj.AddComponent(typeof(Item)) as Item;
            item.attributes = GetDB().FindById(itemIndex);

            if (item.attributes.mesh) {
                GameObject meshObj = GameObject.Instantiate(item.attributes.mesh, item_position, Quaternion.identity) as GameObject;
                meshObj.transform.parent = entityObj.transform;
                meshObj.name = "mesh";
            }

        }
    }

    private void UpdateItems() {
        if (MainEditor.IsSetup())
            itemTypes = GetDB().GetNames().ToArray();
    }

    ItemDatabase itemDB;
    private ItemDatabase GetDB() {
        return itemDB ? itemDB : itemDB = FindObjectOfType<ItemDatabase>();
    }
}
