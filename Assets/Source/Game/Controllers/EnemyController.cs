using UnityEngine;
using Crab;
using Crab.Entities;

public class EnemyController : FactionController
{
    void EnterCombat(Entity target) {
        Debug.Log("Attack!");
    }
    void JustDied(Entity killer) { }
    void JustKilled(Entity victim) { }
    public override void AnyDamage(int damage, Entity damageCauser, DamageType damageType) { }
}