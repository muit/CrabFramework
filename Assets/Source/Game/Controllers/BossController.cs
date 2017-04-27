using UnityEngine;
using Crab;

public class BossController : AIController {
    protected override void EnterCombat(Entity target)
    {
        base.EnterCombat(target);
    }

    enum Phases {
        SPAWN_MOBS
    }

    protected override void Update()
    {
        base.Update();
    }
}
