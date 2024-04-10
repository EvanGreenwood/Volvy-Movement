using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Character
{
    public EnemyInput EnemyInput => CharacterInput as EnemyInput;

    public RatState State { get; set; }
}

public enum RatState
{
    Targeting,
    Attacking,
    Stunned,
    Dying
}