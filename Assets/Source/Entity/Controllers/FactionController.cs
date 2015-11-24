using UnityEngine;
using System.Collections;

namespace Crab.Controllers
{
    public class FactionController : EntityController
    {
        public const Faction faction = Faction.NO_FACTION;

        void EnterCombat(Entity target) { }
        void Update() { }
        void JustDead(Entity killer) { }
        void JustKilled(Entity victim) { }
        void AnyDamage(int damage, Entity damageCauser, DamageType damageType) { }
    }
}