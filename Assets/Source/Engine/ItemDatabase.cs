using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ItemData {
    public string name;
    public GameObject mesh;

    public int GetId() {
        return ItemDatabase.Get.db.IndexOf(this);
    }
}

public class ItemDatabase : MonoBehaviour {
    // Static singleton property
    private static ItemDatabase instance;
    public static ItemDatabase Get
    {
        get
        {
            return instance ? instance : instance = FindObjectOfType<ItemDatabase>();
        }
        private set { instance = value; }
    }



    public List<ItemData> db = new List<ItemData>();

    public ItemData FindByName(string name) {
        return db.Find(x => x.name == name);
    }

    public ItemData FindById(int index)
    {
        return db[index];
    }

    public List<string> GetNames() {
        List<string> names = new List<string>();
        db.ForEach(x => names.Add(x.name));
        return names;
    }
}

[System.Serializable]
public class ItemType {
    public ItemType(int index) {
        id = index;
    }

    public int id;
    
    public ItemData GetData() {
        return ItemDatabase.Get.FindById(id);
    }


    public static bool operator ==(ItemType i1, ItemType i2)
    {
        return i1 != null && i2 != null && i1.id == i2.id;
    }

    public static bool operator !=(ItemType i1, ItemType i2)
    {
        return !(i1 == i2);
    }
}

[CustomPropertyDrawer(typeof(ItemType))]
public class ItemTypeDrawer : PropertyDrawer
{
    private string[] names;
    private SerializedProperty id;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (names == null)
            names = ItemDatabase.Get.GetNames().ToArray();
        if (id == null)
            id = property.FindPropertyRelative("id");


        EditorGUI.BeginProperty(position, label, property);

        id.intValue = EditorGUI.Popup(new Rect(position.x, position.y, position.width, position.height), id.intValue, names);
        
        EditorGUI.EndProperty();
    }
}