using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Crab.Dialogues
{
    [Serializable]
    public class Node
    {
        public Node() {
        }

        [SerializeField, HideInInspector]
        protected DialogueAsset m_dialogue;

        public DialogueAsset dialogue {
            set {
                m_dialogue = value;
            }
            get {
                return m_dialogue;
            }
        }

        [SerializeField]
        protected List<int> inputs = new List<int>();
        [SerializeField]
        protected List<int> outputs = new List<int>();

        protected virtual bool AddParent(Node node)
        {
            int id = m_dialogue.GetID(node);

            if (node != this)
            {
                if(!inputs.Contains(id))
                    inputs.Add(id);
                return true;
            }
            return false;
        }

        public virtual bool AddChildren(Node node)
        {
            int id = m_dialogue.GetID(node);

            if (node != this)
            {
                if (node.AddParent(this))
                {
                    if (!outputs.Contains(id))
                        outputs.Add(id);
                    return true;
                }
            }
            return false;
        }
        
        public List<Node> GetChildrens()
        {
            return outputs.Select(x => m_dialogue.GetNode(x)).ToList();
        }

        public List<Node> GetParents()
        {
            return inputs.Select(x => m_dialogue.GetNode(x)).ToList();
        }
    }
}