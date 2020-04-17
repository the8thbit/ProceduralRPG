using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class EquiptmentButton : MonoBehaviour, IPointerClickHandler
{

    public Image ItemImage;

    private EquiptmentGUI2 EquiptmentGUI;
    private EquiptmentSlot Slot;

    private ItemStack ItemStack;

    public void SetButton(EquiptmentGUI2 parent, EquiptmentSlot slot, ItemStack item)
    {
        EquiptmentGUI = parent;
        Slot = slot;
        ItemStack = item;
        if(item != null)
            ItemImage.sprite = item.Item.GetItemImage();
    }

    public void OnLeftClick()
    {
        if(ItemStack != null)
        {
            EquiptmentGUI.UnEquipt(Slot);
            ItemImage.sprite = null;
            ItemStack = null;
        }
    }
    public void OnRightClick()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
}