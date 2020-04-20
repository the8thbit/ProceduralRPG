using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// Attached to a prefab. Contains information to display
/// items in an inventory as a button
/// </summary>
public class ItemButton : MonoBehaviour, IPointerClickHandler
{

    public Image Image;
    public Text Text;

    private DisplayInventoryGUI DisplayInvent;

    public Item Item;

    public void SetDisplayInventory(DisplayInventoryGUI dispInv)
    {
        DisplayInvent = dispInv;
    }

    public void SetItem(Item itSt)
    {
        Sprite sprite = itSt.GetItemImage();
        if (sprite != null)
        {
            Image.sprite = sprite;
        }
        Text.text = itSt.Name;
        Item = itSt;
    }
    

    public void OnLeftClick()
    {
        DisplayInvent.RemoveItemFromInventory(this);
    }
    public void OnRightClick()
    {
        DisplayInvent.EquiptItem(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }else if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
}