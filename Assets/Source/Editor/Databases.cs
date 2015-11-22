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

    void OnFocus()
    {
        UpdateItems();
    }
    void OnHierarchyChange()
    {
        UpdateItems();
    }

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
            if(showItems) UpdateItems();
        }
        if (showItems)
        {
            int newIndex = EditorGUILayout.Popup(itemIndex, itemNames);
            if (newIndex != itemIndex)
            {
                if (newIndex != 0)
                {
                    SelectItem(newIndex);
                } else{
                    item = null;
                    itemName = "New Item";
                    itemMesh = null;
                }
            }
            itemIndex = newIndex;

            EditorGUI.indentLevel++;
            itemName = EditorGUILayout.TextField(itemName);
            itemMesh = EditorGUILayout.ObjectField(itemMesh, typeof(Mesh)) as Mesh;
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
                    itemIndex = item.GetId()+1;
                }
                item.name = itemName;
                item.mesh = itemMesh;

                if (!CreatingNewItem()) {
                    GetDB().db[itemIndex - 1] = item;
                }

                UpdateItems();
            }

            if (!CreatingNewItem() && GUILayout.Button("Remove", EditorStyles.miniButton))
            {
                GetDB().db.Remove(item);
                UpdateItems();
                SelectItem(itemIndex-1);
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

    private void SelectItem(int index) {
        item = GetDB().FindById(index-1);
        itemIndex = index;
        itemName = item.name;
        itemMesh = item.mesh;
    }

    private bool CreatingNewItem() {
        return itemIndex == 0;
    }

    ItemDatabase itemDB;
    private ItemDatabase GetDB() {
        return itemDB ? itemDB : itemDB = FindObjectOfType<ItemDatabase>();
    }
}
