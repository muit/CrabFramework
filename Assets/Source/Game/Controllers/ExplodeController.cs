using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;



public class ExplodeController : AIController
{
    enum ExplodeEv
    {
        EXPLODE       = 43523,
        AFTER_EXPLODE = 56733
    }

    [Header("Explosion")]
    public int explosionDamage = 50;
    public float explodeDistance = 2;
    public ParticleSystem explodeParticlePrefab;

    private Entity currentExplodeTarget;

    protected override void CombatUpdate()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            Entity target = targets[i];
            if (Me.DistanceTo(target) <= explodeDistance &&
                !events.IsRunning((int)ExplodeEv.EXPLODE) &&
                !events.IsRunning((int)ExplodeEv.AFTER_EXPLODE))
            {
                currentExplodeTarget = target;
                events.RegistryEvent((int)ExplodeEv.EXPLODE, 1000);
            }
        }
    }

    protected override void OnEvent(int id)
    {
        switch ((ExplodeEv)id)
        {
            case ExplodeEv.EXPLODE:
                if (currentExplodeTarget)
                {
                    if (Me.DistanceTo(currentExplodeTarget) <= explodeDistance)
                    {
                        if (explodeParticlePrefab)
                        {
                            ParticleSystem.Instantiate(explodeParticlePrefab, transform.position, transform.rotation);
                        }
                        Me.Movement.CancelMovement();

                        events.RegistryEvent((int)ExplodeEv.AFTER_EXPLODE, 500);
                    }
                }
                break;

            case ExplodeEv.AFTER_EXPLODE:
                if (currentExplodeTarget)
                {
                    currentExplodeTarget.Damage(explosionDamage, Me);
                    Me.Die();
                }
                break;
            default:
                break;
        }

        base.OnEvent(id);
    }
}
