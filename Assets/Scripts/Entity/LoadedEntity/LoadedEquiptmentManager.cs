using UnityEngine;
using System.Collections;

/// <summary>
/// Deals with equipted items (weapon + armour) for loaded entities
/// </summary>
public class LoadedEquiptmentManager : MonoBehaviour
{



    public GameObject HEAD, CHEST, LEGS, FOOT_L, FOOT_R, HAND_L, HAND_R;
   // public GameObject CHEST;
    //public GameObject LEGS;

    private GameObject[] EquiptItems;


    private LoadedEntity LoadedEntity;

    private Weapon Weapon;
    private GameObject WeaponObject;
    /// <summary>
    /// Sets the loaded entity of this instance to the specified LoadedEntity.
    /// Initiates 
    /// </summary>
    /// <param name="le"></param>
    public void SetLoadedEntity(LoadedEntity le)
    {
        LoadedEntity = le;
        EquiptItems = new GameObject[9];

        //RightHand = transform.Find("Human/Armature/Hips/Spine/Chest/Shoulder.R/Upper Arm.R/Lower Arm.R/Hand.R").gameObject;

    }


    /// <summary>
    /// Sets the current weapon to the specified weapon.
    /// If a weapon is already equipt, un-equipt it and destroy the game object
    /// If specified weapon is not null, then initiate its game object
    /// </summary>
    /// <param name="weapon"></param>
    public void SetEquiptWeapon(Weapon weapon)
    {
        //If no change, return
        if (weapon == Weapon)
            return;
        //If currently equipt is not null, destroy the game object
        if(Weapon != null)
        {
            Destroy(WeaponObject);
        }
        if(weapon != null)
        {
            //Equipt and load in weapon
            Weapon = weapon;
            WeaponObject = Instantiate(weapon.GetEquiptItem());
            //WeaponObject.transform.parent = RightHand.transform;
            WeaponObject.transform.localPosition = Vector3.zero;
            Debug.Log("HERERERERE");
        }
    }


    public void EquiptItem(Item item, EquiptmentSlot slot)
    {
        if (item == null)
            UnequiptItem(slot);
        if (item.EquiptableSlot != slot)
            throw new System.Exception("Equiptment not in correct slot");

        GameObject itemObj =   Instantiate(item.GetEquiptItem());
        GameObject secondObj = null;
        if(slot == EquiptmentSlot.hands || slot == EquiptmentSlot.feet)
        {
            secondObj = Instantiate(item.GetEquiptItem());
        }
        
        Debug.Log(itemObj);

        switch (slot)
        {
            case EquiptmentSlot.head:
                if (EquiptItems[0] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[0] = itemObj;
                EquiptItems[0].transform.parent = HEAD.transform;
                EquiptItems[0].transform.localPosition = Vector3.zero;
                return;
            case EquiptmentSlot.chest:
                if (EquiptItems[1] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[1] = itemObj;
                EquiptItems[1].transform.parent = CHEST.transform;
                EquiptItems[1].transform.localPosition = Vector3.zero;
                return;
            case EquiptmentSlot.legs:
                if (EquiptItems[2] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[2] = itemObj;
                EquiptItems[2].transform.parent = LEGS.transform;
                EquiptItems[2].transform.localPosition = Vector3.zero;
                return;
            case EquiptmentSlot.feet:
                if (EquiptItems[3] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[3] = itemObj;
                EquiptItems[3].transform.parent = FOOT_L.transform;
                EquiptItems[3].transform.localPosition = Vector3.zero;
                if (EquiptItems[4] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[4] = secondObj;
                EquiptItems[4].transform.parent = FOOT_R.transform;
                EquiptItems[4].transform.localPosition = Vector3.zero;
                return;

            case EquiptmentSlot.hands:
                if (EquiptItems[5] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[5] = itemObj;
                EquiptItems[5].transform.parent = HAND_L.transform;
                EquiptItems[5].transform.localPosition = Vector3.zero;
                if (EquiptItems[6] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[6] = secondObj;
                EquiptItems[6].transform.parent = HAND_R.transform;
                EquiptItems[6].transform.localPosition = Vector3.zero;
                return;
            case EquiptmentSlot.offhand:
                if (EquiptItems[7] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[7] = itemObj;
                EquiptItems[7].transform.parent = HAND_L.transform;
                EquiptItems[7].transform.localPosition = Vector3.zero;
                return;
            case EquiptmentSlot.weapon:
                if (EquiptItems[8] != null)
                {
                    UnequiptItem(slot);
                }
                EquiptItems[8] = itemObj;
                Debug.Log(HAND_R + "_" + EquiptItems[8]);
                EquiptItems[8].transform.parent = HAND_R.transform;
                EquiptItems[8].transform.localPosition = Vector3.zero;
                return;
        }

    }

    public void EquiptItemStack(ItemStack itSt, EquiptmentSlot slot)
    {
        if (itSt == null)
        {
            UnequiptItem(slot);
        }
        else
        {
            EquiptItem(itSt.Item, slot);
        }
    }

    public void UnequiptItem(EquiptmentSlot slot)
    {
        switch (slot)
        {
            case EquiptmentSlot.head:
                if (EquiptItems[0] != null)
                    Destroy(EquiptItems[0].gameObject);
                return;
            case EquiptmentSlot.chest:
                if (EquiptItems[1] != null)
                    Destroy(EquiptItems[1].gameObject);
                return;
            case EquiptmentSlot.legs:
                if (EquiptItems[2] != null)
                    Destroy(EquiptItems[2].gameObject);
                return;
            case EquiptmentSlot.feet:
                if (EquiptItems[3] != null)
                    Destroy(EquiptItems[3].gameObject);
                if (EquiptItems[4] != null)
                    Destroy(EquiptItems[4].gameObject);
                return;

            case EquiptmentSlot.hands:
                if (EquiptItems[5] != null)
                    Destroy(EquiptItems[5].gameObject);
                if (EquiptItems[6] != null)
                    Destroy(EquiptItems[6].gameObject);
                return;
            case EquiptmentSlot.offhand:
                if(EquiptItems[7] != null)
                    Destroy(EquiptItems[7].gameObject);
                return;
            case EquiptmentSlot.weapon:
                if (EquiptItems[8] != null)
                    Destroy(EquiptItems[8].gameObject);
                return;
        }

    }

}
