using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class EquiptmentButton : MonoBehaviour, IPointerClickHandler
{

    public Image ItemImage;

    private EquiptmentGUI2 EquiptmentGUI;
    private LoadedEquiptmentPlacement Slot;

    private Item Item;

    public void SetButton(EquiptmentGUI2 parent, LoadedEquiptmentPlacement slot, Item item)
    {
        EquiptmentGUI = parent;
        Slot = slot;
        Item = item;
        if(item != null)
            ItemImage.sprite = Item.GetItemImage();
    }

    public void OnLeftClick()
    {
        if(Item != null)
        {
            EquiptmentGUI.UnEquipt(Slot);
            ItemImage.sprite = null;
            Item = null;
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