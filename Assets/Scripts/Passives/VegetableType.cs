using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public partial class VegetableType : ScriptableEnum
{
    public Sprite icon;
    public VegetableObject worldPrefab;
    public StomachVegetable stomachPrefab;
    public Shrapnel seedPrefab;
    public Bomb bombPrefab;
    public RuleTrigger eatTrigger;
}
