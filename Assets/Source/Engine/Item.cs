using UnityEngine;
using System.Collections;

namespace Crab
{
    public class Item : MonoBehaviour
    {

        public ItemData attributes;

        void Start()
        {
            OnGameStart(SceneScript.Instance);
        }

        public virtual void Reset()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        //Events
        protected virtual void OnGameStart(SceneScript scene) { }
    }
}