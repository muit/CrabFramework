using UnityEngine;
using System;
using System.Collections.Generic;

namespace Crab.Dialogues
{
    [CreateAssetMenu(fileName = "Data", menuName = "Crab/Dialogue", order = 1)]
    public class Dialogue : ScriptableObject
    {
        public List<Node> nodes = new List<Node>();
    }
}
