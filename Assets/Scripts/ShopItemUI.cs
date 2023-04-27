using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [Header("Informations")]
    [SerializeField] private ItemScriptable itemScriptable;
    [Space]
    [SerializeField] private int itemActualLevel = 0;
    [SerializeField] private float itemExtraSpeed;
    [SerializeField] private int itemExtraCarry;

    [Header("References")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemValue;
    [Space(10)]
    [SerializeField] private GameObject itemGameObject;
    private MeshRenderer render;

    //Getters

    public int ItemActualLevel { get { return itemActualLevel; } }
    public float ItemExtraSpeed { get { return itemExtraSpeed; } }
    public int ItemExtraCarry { get { return itemExtraCarry; } }
    public ItemScriptable ItemScriptable { get { return itemScriptable; } }

    private void Start() {
        Setup();
        render = itemGameObject.GetComponent<MeshRenderer>();
    } 
    void Setup()
    {
        itemName.text = itemScriptable.Name;
        itemDescription.text = itemScriptable.Description;
        if(itemActualLevel != itemScriptable.ExtraPointsLevels.Length - 1) {
            itemValue.text = itemScriptable.ExtraPointsLevels[itemActualLevel].UpgradeValue.ToString();
        }
        else {
            itemValue.text = "Max";
            GetComponent<Button>().interactable = false;
        }
    }

    public void ItemBought()
    {
        itemActualLevel++;

        itemExtraSpeed = itemScriptable.ExtraPointsLevels[itemActualLevel].ExtraSpeed;
        itemExtraCarry = itemScriptable.ExtraPointsLevels[itemActualLevel].ExtraCarry;

        itemGameObject.SetActive(true);
        GameController.Instance.ActualMoney -= (int)itemScriptable.ExtraPointsLevels[itemActualLevel - 1].UpgradeValue;
        render.sharedMaterial = itemScriptable.ExtraPointsLevels[itemActualLevel - 1].ItemMaterial;
        Setup();
    }
}
