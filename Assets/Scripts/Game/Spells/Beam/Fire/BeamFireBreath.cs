using UnityEngine;
using UnityEditor;

public class BeamFireBreath : Beam
{
    public override string ResourceCode => "fire_breath";

    public override float MaxLength => 5;
    private float DPS = 20;
    public BeamFireBreath(SpellFireBreath fireBreath)
    {
       
    }

    public override GameObject GenerateBeamObject()
    {
        return ResourceManager.GetBeamObject(ResourceCode);
    }

    public override void InternalOnCollision(Collider other)
    {

        LoadedEntity le = other.gameObject.GetComponent<LoadedEntity>();
        if (le != null)
            le.Entity.CombatManager.DealDamage(DPS * Time.deltaTime, DamageType.FIRE);


    }

    public override void Update()
    {
    }
}