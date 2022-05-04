using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability_Whirlwind : Ability_Base
{
    private GameObject parentObj;

    public override void ActivateAbility(GameObject parent)
    {
        parentObj = parent;
        parent.GetComponent<CharacterHandler>().charAnimator.Play("Base Layer.A_Whirlwind");
        parentObj.GetComponent<CharacterHandler>().activateWhirlwind = true;
    }
}
