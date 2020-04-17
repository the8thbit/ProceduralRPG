using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Holds onto the equipt items of an entity
/// TODO - Add slots for arrow on backs + enchanted items?
/// </summary>
[System.Serializable]
public class EquiptmentManager
{
    //Array holding all equipt items
    public HumanoidEntity Entity { get; private set; }
    private ItemStack[] EquiptItems;
    private Armor[] EquiptArmor;

    public EquiptmentManager(HumanoidEntity entity)
    {
        Entity = entity;
        EquiptItems = new ItemStack[7];
        EquiptArmor = new Armor[5];
    }

    public ItemStack[] AllItems()
    {
        return EquiptItems;
    }

    public bool HasMeleeWeapon()
    {
        ItemStack weaponHand = EquiptItems[(int)EquiptmentSlot.weapon];
        if (weaponHand != null && weaponHand.Item is Weapon && !(weaponHand.Item is RangeWeapon))
            return true;
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if (itStack.Item is Weapon && !(itStack.Item is RangeWeapon))
                return true;
        }
        return false;
    }

    public void EquiptMeleeWeapon()
    {
        ItemStack weaponHand = EquiptItems[(int)EquiptmentSlot.weapon];
        if (weaponHand != null && weaponHand.Item is Weapon && !(weaponHand.Item is RangeWeapon))
            return;
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if (itStack.Item is Weapon && !(itStack.Item is RangeWeapon))
            {
                EquiptItem(itStack, EquiptmentSlot.weapon);
                return;
            }
        }
    }

    public bool HasRangeWeapon()
    {
        ItemStack weaponHand = EquiptItems[(int)EquiptmentSlot.weapon];
        if (weaponHand != null && (weaponHand.Item is RangeWeapon))
            return true;
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if ((itStack.Item is RangeWeapon))
                return true;
        }
        return false;
    }
    public void EquiptRangeWeapon()
    {
        ItemStack weaponHand = EquiptItems[(int)EquiptmentSlot.weapon];
        if (weaponHand != null && (weaponHand.Item is RangeWeapon))
            return;
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if ((itStack.Item is RangeWeapon))
            {
                EquiptItem(itStack, EquiptmentSlot.weapon);
                return;
            }
                
        }
    }



    /// <summary>
    /// Unequipts the item from the specified slot, then adds back to inventory
    /// checks if it is a weapon & informs weapon controller if so.
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public ItemStack UnequiptItem(EquiptmentSlot slot)
    {
        //Remove item from slot and add it to inventory
        ItemStack s = EquiptItem(null, slot);
        if(s != null)
            Entity.Inventory.AddItemStack(s);
        //If the slot is the weapon slot, then unequipt the weapon
        if (slot == EquiptmentSlot.weapon && s.Item is Weapon)
        {
            Entity.CombatManager.SetEquiptWeapon(null);
            if (Entity.GetLoadedEntity() != null)
                Entity.GetLoadedEntity().EquiptmentManager.UnequiptItem(slot);
        }
        else if(s.Item is Armor)
        {
            EquiptArmor[(int)s.Item.EquiptableSlot] = null;
            Entity.CombatManager.SetEquiptArmor(EquiptArmor);
            if (Entity.GetLoadedEntity() != null)
                Entity.GetLoadedEntity().EquiptmentManager.UnequiptItem(slot);

        }
        return s;
    }

    /// <summary>
    /// Checks if the given slot can have an item placed in it
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool IsSlotFree(EquiptmentSlot slot)
    {
        int slotID = (int)slot;
        //If item is not null, return false
        if (EquiptItems[slotID] != null)
            return false;

        if(slot == EquiptmentSlot.offhand)  //Sield/defence slot
        {
            //Check weapon slot
            ItemStack weaponSlot = EquiptItems[(int)EquiptmentSlot.weapon];
            if(weaponSlot != null)
            {
                //If its a two handed weapon, then left slot is not free
                if ((weaponSlot.Item as Weapon).IsTwoHanded)
                    return false;
            }

        }

        return true;
    }
    /// <summary>
    /// Returns the equipt item from the given slot
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public ItemStack GetEquiptItem(EquiptmentSlot slot)
    {
        return EquiptItems[(int)slot];
    }

    /// <summary>
    /// Equipts the given item to the given slot.
    /// Returns the item that was in slot previously (returns null if empty)
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    public ItemStack EquiptItem(ItemStack item, EquiptmentSlot slot)
    {

        int slotID = (int)slot;
        ItemStack curItem = EquiptItems[slotID];
        EquiptItems[slotID] = item;

        if(slot == EquiptmentSlot.weapon)
        {
            if (Entity.GetLoadedEntity() != null)
                Entity.GetLoadedEntity().EquiptmentManager.EquiptItemStack(item, slot);
            if(item != null)
            {
                Entity.CombatManager.SetEquiptWeapon(item.Item as Weapon);
            }
        }
        

        return curItem;
    }

    /// <summary>
    /// Calculates the total armour value for this entity
    /// It does this by getting the base value of each armour element, and multiplying
    /// it by the relevent skill bonus
    /// It then sums each
    /// </summary>
    /// <returns></returns>
    public float GetArmourValue()
    {
        float sum = 0;
        for(int i=0; i <(int)EquiptmentSlot.weapon; i++)
        {
            ItemStack item = GetEquiptItem((EquiptmentSlot)i);
            if(item != null && item.Item!=null)
            {
                if(item.Item is HeavyArmor)
                {
                    sum += (item.Item as HeavyArmor).BaseProtection * Entity.SkillTree.HeavyArmour.GetBonus();
                }else if(item.Item is LightArmour)
                {
                    sum += (item.Item as LightArmour).BaseProtection * Entity.SkillTree.LightArmour.GetBonus();
                }
            }
        }
        return sum;
    }
}

public enum EquiptmentSlot
{
    head, chest,  legs, feet, hands, weapon, offhand

}