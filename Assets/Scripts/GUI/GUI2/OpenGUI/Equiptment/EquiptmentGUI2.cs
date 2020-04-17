using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquiptmentGUI2 : MonoBehaviour
{

    public EquiptmentButton[] EquiptmentSlots;


    private Player Player;
    private void OnEnable()
    {
        Player = GameManager.PlayerManager.Player;


        for(int i=0; i<7; i++)
        {

            ItemStack itSt = Player.EquiptmentManager.GetEquiptItem((EquiptmentSlot)i);
            EquiptmentSlots[i].SetButton(this, (EquiptmentSlot)i, itSt);
        }

    }



    public void UnEquipt(EquiptmentSlot slot)
    {
        ItemStack itst = Player.EquiptmentManager.UnequiptItem(slot);
        Player.Inventory.AddItemStack(itst);
    }


}
