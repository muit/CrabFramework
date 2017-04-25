using UnityEngine;
using System.Collections.Generic;
using Crab;

public class BulletPool : List<Bullet>
{
    public readonly int size;

    public BulletPool(int size) : base()
    {
        this.size = size;
    }

    public Bullet Create(Bullet prefab, Vector3 position, Entity owner = null)
    {
        return Create(prefab, position, Quaternion.identity, owner);
    }

    public Bullet Create(Bullet prefab, Vector3 position, Quaternion rotation, Entity owner = null)
    {
        Bullet bullet;
        if (Count < size)
        {
            //Create a new bullet
            bullet = Bullet.Instantiate(prefab, position, rotation) as Bullet;

            //Assign owner
            bullet.owner = owner;
            Add(bullet);
        }
        else
        {
            //Limit exceeded. Reuse bullet
            bullet = this[0];
            RemoveAt(0);

            if (!bullet) {
                //Create a new bullet if the actual one was destroyed
                bullet = Bullet.Instantiate(prefab, position, rotation) as Bullet;
            } else {
                bullet.transform.position = position;
                bullet.transform.rotation = rotation;
                bullet.OnStart();
            }
            //Assign owner
            bullet.owner = owner;

            Add(bullet);
        }
        return bullet;
    }
}
