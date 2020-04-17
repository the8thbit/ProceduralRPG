using UnityEngine;
using UnityEditor;

public class Arrow : Projectile
{
    public override string ResourceCode => "Arrow";
    private float damage;
    public Arrow()
    {
        this.DestroyOnCollision = true;
        this.ProjectileSpeed = 10;
        damage = 20;
    }

    public override GameObject GenerateProjectileObject()
    {
        return ResourceManager.GetProjectileObject(ResourceCode);
    }

    public override void InternalOnCollision(Collider other)
    {
        Entity e = GetHitEntity(other);
        if(e != null)
        {
            e.CombatManager.DealDamage(damage, DamageType.SHARP);
        }

    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}