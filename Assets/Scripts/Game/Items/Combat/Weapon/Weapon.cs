using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class Weapon : Item
{
    public static readonly ItemTag[] WEAPON_TAGS = new ItemTag[] { ItemTag.ITEM, ItemTag.WEAPON };
    protected Weapon(ItemMetaData meta = null) : base(WEAPON_TAGS, meta)
    {
    }
    public override string SpriteSheetTag => "weapons";

    public abstract float Damage { get; }
    public abstract float WeaponCooldown { get; }
    public abstract float WeaponAttackTime { get; }

    public abstract float AttackStaminaUse { get; }
    public abstract float WeaponRange { get; }
    public abstract DamageType DamageType { get; }
    public abstract bool IsTwoHanded { get; }

    //TODO - add weapon damage args (enchantments, knockback)

}