using UnityEngine;
using UnityEditor;

public class FireBall : Projectile
{
    public override string ResourceCode => "fire_ball";

    public float Damage { get { return 10; } }

    public FireBall()
    {
        this.DestroyOnCollision = true;
        this.ProjectileSpeed = 8;
    }


    public override GameObject GenerateProjectileObject()
    {
        return ResourceManager.GetProjectileObject(ResourceCode);
    }

    public override void InternalOnCollision(Collider collision)
    {
        Entity e = GetHitEntity(collision);
        if(e != null)
        {

            e.CombatManager.DealDamage(Damage, DamageType.FIRE);
        }
    }

    public override void Update()
    {
    }
}