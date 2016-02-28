using UnityEngine;
using System.Collections;
using Crab;

namespace Crab.Events
{
    public class ESpawn : Event
    {
        public EntityController prefab;

        protected override void OnGameStart(SceneScript scene)
        {

        }

        protected override void JustStarted()
        {
            Spawn();
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
