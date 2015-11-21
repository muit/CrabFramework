using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Databases : EditorWindow
{
    //Factions
    private bool showFactions = true;
    
    //Items
    private bool showItems = true;
    private bool showItemsAdvanced;
    private string itemName = "None";
    private Mesh itemMesh;
    private ItemData item;

    private string[] itemNames;
    private int itemIndex = 0;

    void OnGUI() {
        titleContent = new GUIContent("Crab Databases");

        if (GUILayout.Button((showFactions ? "↖ " : "↓ ") + "Factions", EditorStyles.boldLabel))
        {
            showFactions = !showFactions;
        }
        if (showFactions)
        {
            
        }



        if (GUILayout.Button((showItems ? "↖ " : "↓ ") + "Items", EditorStyles.boldLabel))
        {
            showItems = !showItems;
        }
        if (showItems)
        {
            int newIndex = EditorGUILayout.Popup(itemIndex, itemNames);
            if (newIndex != itemIndex) {
                itemIndex = newIndex;

                UpdateItems();

                item = GetDB().FindById(itemIndex+1);
                itemName = item.name;
                itemMesh = item.mesh;
            }

            EditorGUI.indentLevel++;
            itemName = EditorGUILayout.TextField(itemName);
            itemMesh = EditorGUILayout.ObjectField(itemMesh ,typeof(Mesh)) as Mesh;
            /*
            if (GUILayout.Button((showItemsAdvanced ? "    ↖ " : "    ↓ ") + "Advanced", EditorStyles.label))
            {
                showItemsAdvanced = !showItemsAdvanced;
            }
            if (showItemsAdvanced)
            {
                EditorGUI.indentLevel++;
                //itemClass = EditorGUILayout.ObjectField(itemClass, typeof(ItemData)) as ItemData;
                EditorGUI.indentLevel--;
            }*/

            if (GUILayout.Button(CreatingNewItem()? "Create": "Save", EditorStyles.miniButton))
            {
                if (CreatingNewItem())
                {
                    item = new ItemData();
                    GetDB().db.Add(item);
                    UpdateItems();
                    itemIndex = item.GetId();
                }
                item.name = itemName;
                item.mesh = itemMesh;
            }
            if (!CreatingNewItem() && GUILayout.Button("Remove", EditorStyles.miniButton)) {
                
            }

            EditorGUI.indentLevel--;
        }
    }

    private void UpdateItems()
    {
        List<string> names = GetDB().GetNames();
        names.Insert(0, "* New Item");
        itemNames = names.ToArray();

    }

    private bool CreatingNewItem() {
        return itemIndex == 0;
    }

    ItemDatabase itemDB;
    private ItemDatabase GetDB() {
        return itemDB ? itemDB : itemDB = FindObjectOfType<ItemDatabase>();
    }
}
