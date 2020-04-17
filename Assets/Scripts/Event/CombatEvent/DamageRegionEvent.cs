using UnityEngine;
using UnityEditor;

public interface INewDamageRegionEvent : IEventListener
{
    void NewDamageRegionEvent(NewDamageRegion ev);
}

public class NewDamageRegion : IEvent
{
    public WeaponDamageRegion DamageRegion { get; private set; }
    public Entity Cause { get; private set; }
    public float Damage { get; private set; }
    public DamageType DamageType { get; private set; }
    public object[] args { get; private set; }
    public bool HasArgs { get { return args != null; } }
    public NewDamageRegion(Entity cause, float damageAmount, DamageType type, WeaponDamageRegion r, object[] args=null)
    {
        Cause = cause;
        DamageRegion = r;
        Damage = damageAmount;
        DamageType = type;
        this.args = args;
    }
}
