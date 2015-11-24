using UnityEngine;
using System.Collections;
using Crab;

[RequireComponent(typeof(Entity))]
[DisallowMultipleComponent]

[System.Serializable]
public class EntityController : MonoBehaviour{
    protected Entity me;

    void Start() {
        me = GetComponent<Entity>();
    }

    //Reusable Methods
    void EnterCombat(Entity target) {}
    void Update() {}
    void JustDead(Entity killer) {}
    void JustKilled(Entity victim) {}
    void AnyDamage(int damage, Entity damageCauser, DamageType damageType) { }


    //Public Methods
    private System.Guid guid;
    public System.Guid GetGuid() {
        return (guid == null)? guid : guid = System.Guid.NewGuid();
    }
}
