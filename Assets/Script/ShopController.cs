using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    public void BuyItem(ItemSO item){
        if(playerStatus.sunflower < item.price)
            return;

        if(playerStatus.heldItem != null)
            return;

        playerStatus.sunflower -= item.price;
    }
}
