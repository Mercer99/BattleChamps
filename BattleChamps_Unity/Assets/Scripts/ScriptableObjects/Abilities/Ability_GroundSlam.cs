using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability_GroundSlam : Ability_Base
{
    public float slamRange = 3;
    public float slamDamage = 10;

    public GameObject groundSlamObj;

    public override void ActivateAbility(GameObject parent)
    {
        parent.GetComponent<CharacterHandler>().charAnimator.Play("Base Layer.A_GroundSlam");

        GameObject slamObj = Instantiate(groundSlamObj, parent.transform);
        slamObj.GetComponent<GroundSlamBehaviour>().Damage(parent, slamRange, slamDamage);
    }
}
