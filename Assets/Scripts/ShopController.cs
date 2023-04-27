using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private ShopItemUI[] shopItems;

    [SerializeField] private float additionalVelocity;
    [SerializeField] private int additionalCarry;

    public static ShopController Instance;
    //Getters

    public float AdditionalVelocity { get { return additionalVelocity; } }
    public int AdditionalCarry { get { return additionalCarry; } }

    private void Awake() => Instance = this;

    public void BuyItem(ShopItemUI item)
    {
        var itemValue = item.ItemScriptable.ExtraPointsLevels[item.ItemActualLevel].UpgradeValue;
        if (GameController.Instance.ActualMoney < itemValue)
            return;

        item.ItemBought();
        RefreshBuffs();
    }

    void RefreshBuffs()
    {
        additionalVelocity = 0;
        additionalCarry = 0;
        foreach(ShopItemUI item in shopItems) {
            additionalVelocity += item.ItemExtraSpeed;
            additionalCarry += item.ItemExtraCarry;
        }
        GameController.Instance.UpdateInformations();
    }
}
