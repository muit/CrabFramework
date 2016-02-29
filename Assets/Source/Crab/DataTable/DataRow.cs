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

        public bool boolValue;
        public int intValue;
        public float floatValue;
        public string stringValue;
        public Color colorValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Vector4 vector4Value;


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