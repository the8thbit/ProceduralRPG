using UnityEngine;
using UnityEditor;

public class DisplayInventoryGUI : MonoBehaviour
{
    public InventoryGUI2 Parent;

    public GameObject InventorySpace;


    public Inventory Inventory { get; private set; }
    private ItemTag CurrentItemTag;


    public bool IsMain { get; private set; }


    public void SetMain(bool main)
    {
        IsMain = main;
    }

    public void SetInventory(Inventory inventory)
    {
        Inventory = inventory;

        SetItemTag(CurrentItemTag);       
    }

    /// <summary>
    /// Takes the selected item and attempts to equipt it
    /// If a piece of equiptment is already in the relevent equiptment slot,
    /// it un-equipts it and adds it to the inventory.
    /// </summary>
    /// <param name="itBut"></param>
    public void EquiptItem(ItemButton itBut)
    {
        Debug.Log("equipting");
        //If not equiptable, we do nothing (return)
        if (!itBut.ItemStack.Item.IsEquiptable)
        {
            Debug.Log("not equiptable");
            return;
        }
            
        //Remove it from out inventory, and equipt it
        Inventory.RemoveItemStack(itBut.ItemStack);
        ItemStack toAdd = GameManager.PlayerManager.Player.EquiptmentManager.EquiptItem(itBut.ItemStack, itBut.ItemStack.Item.EquiptableSlot);
        Destroy(itBut.gameObject);
        //If the relevent slot had an item in before
        if(toAdd != null)
        {
            //Add item to inventory
            Inventory.AddItemStack(toAdd);
            //If its the current display item tag, add it to the display
            if (toAdd.Item.HasTag(CurrentItemTag))
                AddItemToSpace(toAdd);
        }

        if (!IsMain)
        {
            if (Inventory.IsEmpty)
            {
                Inventory = null;
                gameObject.SetActive(false);
            }
            GameManager.EventManager.InvokeNewEvent(new PlayerPickupItem(itBut.ItemStack.Item));
        }

    }

    /// <summary>
    /// Removes the item from the inventory.
    /// Checks if an open inventory is present to swap item
    /// </summary>
    /// <param name="itBut"></param>
    public void RemoveItemFromInventory(ItemButton itBut)
    {

        if (IsMain)
        {//If this is the main inventory, we find a secondary inventory
            Inventory second = Parent.GetSecondInventory();
            //if this inventory is null, we return as we cannot remove this item
            if (second == null)
                return;
            //We add the item to both the invetory, and to its thingy
            Parent.SecondInventory.Inventory.AddItemStack(itBut.ItemStack);
            Parent.SecondInventory.AddItemToSpace(itBut.ItemStack);
            this.Inventory.RemoveItemStack(itBut.ItemStack);
            Destroy(itBut.gameObject);
        }
        else
        {
            //if this is the second inventory, we add the object to the main inventory          

            Parent.MainInventory.AddItemToSpace(itBut.ItemStack);
            Parent.MainInventory.Inventory.AddItemStack(itBut.ItemStack);

            Destroy(itBut.gameObject);
            Inventory.RemoveItemStack(itBut.ItemStack);

            if (Inventory.IsEmpty)
            {
                Inventory = null;
                gameObject.SetActive(false);
            }
            GameManager.EventManager.InvokeNewEvent(new PlayerPickupItem(itBut.ItemStack.Item));


        }



    }


    public void SetItemTag(ItemTag nTag)
    {
        CurrentItemTag = nTag;
        Clear();
        if (Inventory == null)
            return;
        foreach(ItemStack itst in Inventory.GetItems())
        {
            if (itst.Item.HasTag(CurrentItemTag))
            {
                AddItemToSpace(itst);
            }
        }

    }


    private void AddItemToSpace(ItemStack item)
    {
        GameObject but = Instantiate(Parent.ItemButtonPrefab.gameObject);
        ItemButton itb = but.GetComponent<ItemButton>();
        itb.SetItemStack(item);
        but.transform.SetParent(InventorySpace.transform, false);
        itb.SetDisplayInventory(this);
    }


    private void OnDisable()
    {
        Clear();
    }

    private void Clear()
    {
        foreach (Transform child in InventorySpace.transform)
        {
            Destroy(child.gameObject);
        }
    }
}