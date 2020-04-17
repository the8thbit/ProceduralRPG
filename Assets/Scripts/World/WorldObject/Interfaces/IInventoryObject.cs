using UnityEngine;
using UnityEditor;

public interface IInventoryObject
{
    Inventory GetInventory();
    void OnRemoveItem();
    void OnAddItem();
}