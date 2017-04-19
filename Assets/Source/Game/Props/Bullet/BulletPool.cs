using UnityEngine;
using System.Collections.Generic;

public class BulletPool : List<Bullet>
{

    public readonly int size;

    public BulletPool(int size) : base()
    {
        this.size = size;
    }

    public Bullet Create(Bullet prefab, Vector3 position)
    {
        return Create(prefab, position, Quaternion.identity);
    }

    public Bullet Create(Bullet prefab, Vector3 position, Quaternion rotation)
    {
        Bullet bullet;
        if (Count < size)
        {
            //Create a new bullet
            bullet = Bullet.Instantiate(prefab, position, rotation) as Bullet;
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
                bullet.Reset();
            }
            Add(bullet);
        }
        return bullet;
    }
}
