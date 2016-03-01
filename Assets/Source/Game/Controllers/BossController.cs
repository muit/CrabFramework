using UnityEngine;
using Crab;

public class BossController : AIController {
    void EnterCombat(Entity target)
    {
        Debug.Log("Attack!");
    }
    void JustDead(Entity killer) { }
    void JustKilled(Entity victim) { }
    protected override void AnyDamage(int damage, Entity damageCauser, DamageType damageType) { }

    enum Phases {
        SPAWN_MOBS
    }

    void Update()
    {
        events.Update();
    }
}
