using UnityEngine;
using System.Collections;
using Crab.Controllers;

namespace Crab.Events
{
    public class ESpawn : Event
    {
        public EntityController prefab;
        public bool disableWhenDone = false;

        protected override void OnGameStart(SceneScript scene)
        {

        }

        protected override void JustStarted()
        {
            Spawn();
            if (disableWhenDone) {
                FinishEvent();
            }
        }

        protected EntityController Spawn(float height = 0)
        {
            if (prefab)
            {
                Vector3 position = transform.position;
                position.y += height;
                return Instantiate(prefab, position, transform.rotation) as EntityController;
            }
            return null;
        }
    }
}
