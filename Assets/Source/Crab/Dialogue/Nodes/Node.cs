using System;
using UnityEngine;
using System.Collections.Generic;

namespace Crab.Dialogues
{
    [Serializable]
    public class Node
    {
        [SerializeField]
        protected List<Node> inputs = new List<Node>();

        [SerializeField]
        protected List<Node> outputs = new List<Node>();

        protected virtual bool AddParent(Node node)
        {
            if (node != this)
            {
                if(!inputs.Contains(node))
                    inputs.Add(node);
                return true;
            }
            return false;
        }

        public virtual bool AddChildren(Node node)
        {
            if (node != this)
            {
                if (node.AddParent(this))
                {
                    if (!outputs.Contains(node))
                        outputs.Add(node);
                    return true;
                }
            }
            return false;
        }


        public List<Node> GetChildrens()
        {
            return new List<Node>(outputs);
        }

        public List<Node> GetParents()
        {
            return new List<Node>(inputs);
        }
    }
}