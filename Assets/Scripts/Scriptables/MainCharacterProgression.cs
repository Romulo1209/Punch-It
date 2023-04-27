using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PunchIt/Player Progression")]
public class MainCharacterProgression : ScriptableObject
{
    public int MaxLevel;
    public AnimationCurve ExpCurve;
    public AnimationCurve ForceCurve;
    public AnimationCurve VelocityCurve;
    public AnimationCurve CarryCurve;
}
