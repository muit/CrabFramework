using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public abstract class DictionaryDrawer<TK, TV> : PropertyDrawer
{
}

[CustomPropertyDrawer(typeof(DataRow.AttributeContainer))]
public class AttributeContainerDrawer : DictionaryDrawer<string, DataRow.Attribute> {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {



        EditorGUI.BeginProperty(position, label, property);
        DataRow.AttributeContainer dictionary = fieldInfo.GetValue(property.serializedObject.targetObject) as DataRow.AttributeContainer;

        if (GUI.Button(new Rect(position.x + position.width-30, position.y, 15, 15), "+")) {
            if(!dictionary.Contains("New Attribute"))
                dictionary.Add("New Attribute", new DataRow.Attribute());
        }
        if (GUI.Button(new Rect(position.x + position.width - 15, position.y, 15, 15),"x"))
        {
            dictionary.Clear();
        }

        if (property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 15), property.isExpanded, label)) {
            
            int i = 0;
            Dictionary<string, string> renames = new Dictionary<string, string>();

            foreach (string key in dictionary.Keys) {
                string newKey = GUI.TextField(new Rect(position.x + 10, position.y + 20 + i*15, position.width / 2 - 5, 15), key);
                if (newKey != key) {
                    renames.Add(key, newKey);
                }
                DataRow.Attribute attr = dictionary[key];
                attr.type = (DataRow.Attribute.Type)EditorGUI.EnumPopup(new Rect(position.x + position.width / 2 + 10, position.y + 20 + i * 15, position.width / 2 - 10, 15), attr.type);

                i++;
            }

            foreach (KeyValuePair<string, string> rename in renames) {
                dictionary.RenameKey(rename.Key, rename.Value);
            }


        }

        fieldInfo.SetValue(property.serializedObject.targetObject, dictionary);

        EditorGUI.EndProperty();
    }
}
