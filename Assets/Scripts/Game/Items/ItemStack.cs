using UnityEngine;
using UnityEditor;
[System.Serializable]
public class ItemStack
{
    public Item Item { get; private set; }
    public int Count { get; private set; }
    public ItemStack(Item item, int count = 1)
    {
        Item = item;
        Count = count;
    }
    public void AddToStack(int add = 1)
    {
        Count += add;
    }
}