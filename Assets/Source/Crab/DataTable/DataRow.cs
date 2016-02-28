using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class DataRow
{
    [HideInInspector]
    public AttributeContainer attributes = new AttributeContainer();

    public DataRowType type {
        set {
            attributes.Clear();

            DataRow.AttributeContainer attrs = value.attributes;
            foreach (string key in attrs.Keys) {
                attributes.Add(key, new Attribute(attrs[key].type));
            }
        }
    }



    [System.Serializable]
    public class AttributeContainer : SerializableDictionary<string, Attribute> { }

    [System.Serializable]
    public class Attribute
    {
        public enum Type
        {
            Bool,
            Int,
            Float,
            String,
            Color,
            Vector2,
            Vector3,
            Vector4
        }

        public Type type = Type.Bool;

        [System.NonSerialized]
        public bool boolValue;
        [System.NonSerialized]
        public int intValue;
        [System.NonSerialized]
        public float floatValue;
        [System.NonSerialized]
        public string stringValue;
        [System.NonSerialized]
        public Color colorValue;
        [System.NonSerialized]
        public Vector2 vector2Value;
        [System.NonSerialized]
        public Vector3 vector3Value;
        [System.NonSerialized]
        public Vector4 vector4Value;

        
        public Attribute(Type type = Type.Bool)
        {
            this.type = type;
        }
        

#if UNITY_EDITOR
        public void OnGUI()
        {
            switch (type)
            {
                case Type.Bool:
                    boolValue = EditorGUILayout.Toggle(boolValue);
                    break;
                case Type.Int:
                    intValue = EditorGUILayout.IntField(intValue);
                    break;
                case Type.Float:
                    floatValue = EditorGUILayout.FloatField(floatValue);
                    break;
                case Type.String:
                    stringValue = EditorGUILayout.TextField(stringValue);
                    break;
                case Type.Color:
                    colorValue = EditorGUILayout.ColorField(colorValue);
                    break;
                case Type.Vector2:
                    vector2Value = EditorGUILayout.Vector2Field("", vector2Value);
                    break;
                case Type.Vector3:
                    vector3Value = EditorGUILayout.Vector3Field("", vector3Value);
                    break;
                case Type.Vector4:
                    vector4Value = EditorGUILayout.Vector4Field("", vector4Value);
                    break;
                default:
                    EditorGUILayout.LabelField("Can't show this variable");
                    break;
            }
        }
#endif
    }
}

[CustomPropertyDrawer(typeof(DataRow.Attribute))]
public class AttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property.FindPropertyRelative("type"));
        EditorGUI.EndProperty();
    }
}