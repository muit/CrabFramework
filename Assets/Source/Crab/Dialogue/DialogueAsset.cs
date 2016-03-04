using UnityEngine;
using System;
using System.Collections.Generic;

namespace Crab.Dialogues
{
    [CreateAssetMenu(fileName = "Data", menuName = "Crab/Dialogue", order = 1)]
    public class DialogueAsset : ScriptableObject
    {
        public List<Node> nodes = new List<Node>();
        
        public int GetID(Node node) {
            return nodes.IndexOf(node);
        }

        public Node GetNode(int id) {
            return nodes[id];
        }

        public void CreateNode() {
            Node node = new Node();
            node.dialogue = this;
            nodes.Add(node);
        }
    }
}
