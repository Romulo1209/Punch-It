using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PunchIt/Item")]
public class ItemScriptable : ScriptableObject
{
    public string Name;
    public string Description;
    public ExtraPoints[] ExtraPointsLevels;
}
[System.Serializable]
public struct ExtraPoints
{
    public float ExtraSpeed;
    public int ExtraCarry;
    public float UpgradeValue;
    public Material ItemMaterial;
}