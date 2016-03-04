using UnityEngine;
using UnityEditor;

namespace Crab.Dialogues
{
    [CustomPropertyDrawer(typeof(DialogueAsset))]
    public class CanvasEditor : PropertyDrawer
    {
        public Vector2 position;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int tileX = Mathf.CeilToInt(position.width);
            int tileY = Mathf.CeilToInt(position.height);

            for (int x = 0; x < tileX; x++)
            {
                for (int y = 0; y < tileY; y++)
                {
                    /*GUI.DrawTexture(new Rect(offset.x + x * width,
                                               offset.y + y * height,
                                               width, height),
                                     );*/
                }
            }
        }
    }
}
