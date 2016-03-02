using System;

namespace Crab.Dialogues
{
    [Serializable]
    public class Entry : Node {
        protected override bool AddParent(Node node) { return false; }

        public override bool AddChildren(Node node)
        {
            if (node != this)
            {
                outputs.Clear();
                return base.AddChildren(node);
            }

            return false;
        }
    }
}