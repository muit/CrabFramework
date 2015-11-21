using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemData {
    public string name;
    public Mesh mesh;

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
        return db.Select(x => x.name).ToList();
    }
}
