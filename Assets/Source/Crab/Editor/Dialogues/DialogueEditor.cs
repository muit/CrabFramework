using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

namespace Crab.Dialogues
{
    public class DialogueEditor : EditorWindow
    {
        // Opened Canvas
        public DialogueAsset m_dialogue;

        [MenuItem("Crab/Dialogue")]
        public static void ShowEditor()
        {
            DialogueEditor window = EditorWindow.GetWindow(typeof(DialogueEditor)) as DialogueEditor;
            
            window.minSize = new Vector2(800, 600);
            window.titleContent = new GUIContent("Dialogue");
        }

        public void OnInspectorUpdate()
        {
            OnSelectionChange();
        }

        void OnSelectionChange()
        {
            DialogueAsset selection = null;

            if (Selection.activeGameObject)
            {
                Dialogue finder = Selection.activeGameObject.GetComponent<Dialogue>();
                if (finder)
                {
                    selection = finder.dialogue;
                }
            }

            if (!selection)
                selection = Selection.activeObject as DialogueAsset;

            if (m_dialogue != selection)
            {
                m_dialogue = selection;
                Repaint();
            }

        }
    }
}