using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Base : ScriptableObject
{
    public string abilityName;
    public float cooldownTime;
    public float activeTime;

    public virtual void ActivateAbility(GameObject parent)
    { }
}
