﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
/// <summary>
/// Holds onto the equipt items of an entity
/// TODO - Add slots for arrow on backs + enchanted items?
/// </summary>
[System.Serializable]
public class EquiptmentManager
{
    //Array holding all equipt items
    public HumanoidEntity Entity { get; private set; }
    private Armor[] EquiptArmor;

    private Dictionary<LoadedEquiptmentPlacement, EquiptableItem> DefaultItems;

    [System.NonSerialized]
    private LoadedEquiptmentManager LoadedEquiptmentManager;

    private Dictionary<LoadedEquiptmentPlacement, EquiptableItem> EquiptItems;


    public EquiptmentManager(HumanoidEntity entity)
    {
        Entity = entity;
        EquiptItems = new Dictionary<LoadedEquiptmentPlacement, EquiptableItem>();
        EquiptArmor = new Armor[5];
        DefaultItems = new Dictionary<LoadedEquiptmentPlacement, EquiptableItem>();
    }


    public void SetLoadedEquiptmentManager(LoadedEquiptmentManager lem)
    {
        LoadedEquiptmentManager = lem;
    }
    public LoadedEquiptmentManager GetLoadedEquiptmentManager()
    {
        return LoadedEquiptmentManager;
    }

    public EquiptableItem GetDefaultItem(LoadedEquiptmentPlacement lep)
    {
        EquiptableItem item;
        DefaultItems.TryGetValue(lep, out item);
        return item;
    }

   //Returns true if a weapon is either in hand, or in weapon sheath
    public bool HasWeapon { get {
            return (EquiptItems.ContainsKey(LoadedEquiptmentPlacement.weaponHand) && EquiptItems[LoadedEquiptmentPlacement.weaponHand] != null) ||
                  (EquiptItems.ContainsKey(LoadedEquiptmentPlacement.weaponSheath) && EquiptItems[LoadedEquiptmentPlacement.weaponSheath] != null);} }
    //Returns true if a weapon is in hand
    public bool WeaponReady { get { return (EquiptItems.ContainsKey(LoadedEquiptmentPlacement.weaponHand) && EquiptItems[LoadedEquiptmentPlacement.weaponHand] != null); } }



    /// <summary>
    /// Returns true if the entity has a melee weapon either equipt, or in
    /// their inventory
    /// </summary>
    /// <returns></returns>
    public bool HasMeleeWeapon()
    {

        EquiptableItem weapon;

        //Check sheath and hand for melee weapon
        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.weaponSheath, out weapon);        
        if (weapon != null && !(weapon is RangeWeapon))
            return true;

        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.weaponHand, out weapon);
        if (weapon != null && !(weapon is RangeWeapon))
            return true;

        //If a melee weapon isnt equipt, check the inventory
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if (itStack.Item is Weapon && !(itStack.Item is RangeWeapon))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inHand"></param>
    public void EquiptMeleeWeapon(bool inHand=false)
    {

        EquiptableItem weapon;

        //Check sheath for melee weapon
        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.weaponSheath, out weapon);
        if (weapon != null && !(weapon is RangeWeapon))
        {
            //If there is one, we check if we should put it directly in hand
            UnequiptItem(LoadedEquiptmentPlacement.weaponSheath);
            EquiptItem(LoadedEquiptmentPlacement.weaponHand, weapon);
        }
        
        //If the hand already has a melee weapon, return
        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.weaponHand, out weapon);
        if (weapon != null && !(weapon is RangeWeapon))
            return;

        //otherwise check the inventory for a melee weapon
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if (itStack.Item is Weapon && !(itStack.Item is RangeWeapon))
            {
                EquiptItem(LoadedEquiptmentPlacement.weaponHand, itStack.Item as EquiptableItem);
                return;
            }
        }
    }
    /// <summary>
    /// Returns true if the entity has a ranged weapon either equipt, or in
    /// their inventory
    /// </summary>
    /// <returns></returns>
    public bool HasRangeWeapon()
    {
        EquiptableItem weapon;

        //Check sheath and hand for melee weapon
        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.backSheath, out weapon);
        if (weapon != null && (weapon is RangeWeapon))
            return true;

        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.weaponHand, out weapon);
        if (weapon != null && (weapon is RangeWeapon))
            return true;

        //If a melee weapon isnt equipt, check the inventory
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if (itStack.Item is Weapon && (itStack.Item is RangeWeapon))
                return true;
        }
        return false;
    }
    public void EquiptRangeWeapon()
    {
        EquiptableItem weapon;

        //Check sheath for melee weapon
        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.backSheath, out weapon);
        if (weapon != null && (weapon is RangeWeapon))
        {
            UnsheathWeapon(LoadedEquiptmentPlacement.backSheath);
            
        }

        //If the hand already has a melee weapon, return
        EquiptItems.TryGetValue(LoadedEquiptmentPlacement.weaponHand, out weapon);
        if (weapon != null && (weapon is RangeWeapon))
            return;

        //otherwise check the inventory for a melee weapon
        foreach (ItemStack itStack in Entity.Inventory.GetItems())
        {
            if (itStack.Item is Weapon && (itStack.Item is RangeWeapon))
            {
                EquiptItem(LoadedEquiptmentPlacement.weaponHand, itStack.Item as EquiptableItem);
                return;
            }
        }
    }

    
    /// <summary>
    /// Removes an item from the specified slot
    /// returns the item to user
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public EquiptableItem UnequiptItem(LoadedEquiptmentPlacement slot)
    {
        EquiptableItem remove;
        if (EquiptItems.TryGetValue(slot, out remove))
        {
            Debug.Log("[EquiptmentManager] Entity " + Entity + " Unequipt " + remove + " from slot " + slot);
            EquiptItems.Remove(slot);
        }
            
        //If entity is loaded, remove equiptment object
        LoadedEquiptmentManager?.SetEquiptmentItem(slot, null);

        //Check if a default item exists for this slot
        if (DefaultItems.ContainsKey(slot))
        {
            LoadedEquiptmentManager?.SetEquiptmentItem(slot, DefaultItems[slot]);
        }


        return remove;
    }
    /// <summary>
    /// Equipts the specified item to the specified slot
    /// Returns the item in the slot previous
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public EquiptableItem EquiptItem(LoadedEquiptmentPlacement slot, EquiptableItem item)
    {
        EquiptableItem removed = UnequiptItem(slot);
        EquiptItems.Add(slot, item);

        Debug.Log("Item " + item + " added to slot " + slot);
        LoadedEquiptmentManager?.SetEquiptmentItem(slot, item);
        if (slot == LoadedEquiptmentPlacement.weaponHand)
            Entity.CombatManager.SetEquiptWeapon(item as Weapon);
        return removed;
    }

    public EquiptableItem EquiptItem(Item item_, bool unsheathed=false)
    {
        if (!(item_ is EquiptableItem))
            return null;
        EquiptableItem item = item_ as EquiptableItem;
        LoadedEquiptmentPlacement toPut = (LoadedEquiptmentPlacement)item.EquiptableSlot;
        Debug.Log("[EquiptmentManager] " + item.EquiptableSlot + "_" + toPut);
        if(item is Weapon)
        {
            if (unsheathed)
            {
                toPut = LoadedEquiptmentPlacement.weaponHand;
            }else if(item is RangeWeapon)
            {
                toPut = LoadedEquiptmentPlacement.backSheath;
            }
            else
            {
                toPut = LoadedEquiptmentPlacement.weaponSheath;
            }
        }
        
        return EquiptItem(toPut, item);
    }


    public void UnsheathWeapon(LoadedEquiptmentPlacement sheathedPosition)
    {

        if (EquiptItems.ContainsKey(sheathedPosition))
        {
            Debug.Log(LoadedEquiptmentManager.HAND_R + "_" + LoadedEquiptmentManager.GetObjectInSlot(sheathedPosition));
            LoadedEquiptmentManager.UnsheathWeapon(sheathedPosition);
            EquiptItems[LoadedEquiptmentPlacement.weaponHand] = EquiptItems[sheathedPosition];
            EquiptItems[sheathedPosition] = null;
            Entity.CombatManager.SetEquiptWeapon(EquiptItems[LoadedEquiptmentPlacement.weaponHand] as Weapon);


        }


    }




    /// <summary>
    /// Returns the equipt item from the given slot
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public EquiptableItem GetEquiptItem(LoadedEquiptmentPlacement slot)
    {
        EquiptableItem equipt;
        EquiptItems.TryGetValue(slot, out equipt);
        return equipt;
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
            Item item = GetEquiptItem((LoadedEquiptmentPlacement)i);
            if(item != null )
            {
                if(item is HeavyArmor)
                {
                    sum += (item as HeavyArmor).BaseProtection * Entity.SkillTree.HeavyArmour.GetBonus();
                }else if(item is LightArmour)
                {
                    sum += (item as LightArmour).BaseProtection * Entity.SkillTree.LightArmour.GetBonus();
                }
            }
        }
        return sum;
    }

    /// <summary>
    /// Adds a new default item to its relevent slot
    /// </summary>
    /// <param name="item"></param>
    public void AddDefaultItem(EquiptableItem item)
    {
        //item must be of type default
        if (!item.IsDefault)
        {
            Debug.LogError("Default items must be of type default");
            return;
        }
        LoadedEquiptmentPlacement slot = (LoadedEquiptmentPlacement)item.EquiptableSlot;
        //Check if we have a default item in this slot already
        if (DefaultItems.ContainsKey(slot))
        {
            Debug.LogError("Entity " + Entity + " already has a default item in slot " + slot.ToString());
            return;
        }
        //If valid, we add the item
        DefaultItems.Add(slot, item);
        //If there is currently no item equipt in this slot, we equipt it
        if (GetEquiptItem(slot) == null)
            LoadedEquiptmentManager?.SetEquiptmentItem(slot, item);
    }
}

public enum EquiptmentSlot
{
    head, chest,  legs, feet, hands, weapon, offhand

}