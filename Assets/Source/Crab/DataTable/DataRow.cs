using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class DataRow
{
    [SerializeField, HideInInspector]
    private AttributeContainer attributes = new AttributeContainer();

    public DataRowType type
    {
        set
        {
            attributes.Clear();

            DataRow.AttributeContainer attrs = value.attributes;
            foreach (string key in attrs.Keys)
            {
                attributes.Add(key, new Attribute(attrs[key].type));
            }
        }
    }



    /*****************
     * Value Handlers
     *****************/
    public bool GetBool(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.Bool);
        if (attribute != null)
            return attribute.boolValue;
        return false;
    }

    public int GetInt(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.Int);
        if (attribute != null)
            return attribute.intValue;
        return default(int);
    }

    public float GetFloat(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.Float);
        if (attribute != null)
            return attribute.floatValue;
        return default(float);
    }

    public string GetString(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.String);
        if (attribute != null)
            return attribute.stringValue;
        return null;
    }

    public Color GetColor(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.Color);
        if (attribute != null)
            return attribute.colorValue;
        return default(Color);
    }

    public Vector2 GetVector2(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.Vector2);
        if (attribute != null)
            return attribute.vector2Value;
        return Vector2.zero;
    }

    public Vector3 GetVector3(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.Vector3);
        if (attribute != null)
            return attribute.vector2Value;
        return Vector3.zero;
    }

    public Vector4 GetVector4(string attribute_name)
    {
        Attribute attribute = GetAttributeOfType(attribute_name, Attribute.Type.Vector4);
        if (attribute != null)
            return attribute.vector2Value;
        return Vector4.zero;
    }


    //Find Attribute by name and type
    private Attribute GetAttributeOfType(string name, Attribute.Type type)
    {
        Attribute attribute = attributes[name];
        if (attribute == null)
        {
            Debug.LogWarning("Unknow attribute " + name);
            return null;
        }
        if (attribute.type != type)
        {
            Debug.LogWarning("Attribute " + name + " is not a " + Enum.GetName(typeof(Attribute.Type), type));
            return null;
        }

        return attribute;
    }



    /*************
     * SubClasses
     *************/
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
            Vector4,
            Texture,
            Audio,
            GameObject,
            Mesh
        }

        public Type type = Type.Bool;

        public bool boolValue;
        public int intValue;
        public float floatValue;
        public string stringValue;
        public Color colorValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Vector4 vector4Value;
        public Texture2D textureValue;
        public AudioClip audioValue;
        public GameObject goValue;
        public Mesh meshValue;


        public Attribute(Type type = Type.Bool)
        {
            this.type = type;
        }


#if UNITY_EDITOR
        public static void OnPaintId(int id)
        {
            if (id < 0) return;

            Rect position = GUILayoutUtility.GetRect(30, EditorGUIUtility.singleLineHeight);
            GUIStyle style = GUI.skin.GetStyle("Tooltip");
            style.alignment = TextAnchor.UpperCenter;
            EditorGUI.LabelField(position, "" + id, style);
        }

        public static void OnPaintValue(SerializedProperty attribute)
        {
            Rect position = GUILayoutUtility.GetRect(100, EditorGUIUtility.singleLineHeight);

            if (attribute == null)
            {
                Debug.Log(attribute);
                return;
            }

            string property;
            switch ((Type)attribute.FindPropertyRelative("type").enumValueIndex)
            {
                case Type.Bool:
                    property = "boolValue";
                    break;
                case Type.Int:
                    property = "intValue";
                    break;
                case Type.Float:
                    property = "floatValue";
                    break;
                case Type.String:
                    property = "stringValue";
                    break;
                case Type.Color:
                    property = "colorValue";
                    break;
                case Type.Vector2:
                    property = "vector2Value";
                    break;
                case Type.Vector3:
                    property = "vector3Value";
                    break;
                case Type.Vector4:
                    property = "vector4Value";
                    break;
                case Type.Texture:
                    property = "textureValue";
                    break;
                case Type.Audio:
                    property = "audioValue";
                    break;
                case Type.GameObject:
                    property = "goValue";
                    break;
                case Type.Mesh:
                    property = "meshValue";
                    break;
                default:
                    EditorGUILayout.LabelField("Can't show this variable");
                    return;
            }
            SerializedProperty value = attribute.FindPropertyRelative(property);

            if (value != null)
            {
                EditorGUI.PropertyField(position, value, new GUIContent(""));
            }
        }
#endif
    }
}

#if UNITY_EDITOR
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
#endif
