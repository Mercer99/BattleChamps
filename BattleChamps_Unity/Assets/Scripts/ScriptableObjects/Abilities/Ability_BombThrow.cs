using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability_BombThrow : Ability_Base
{
    public GameObject bombObject;
    public override void ActivateAbility(GameObject parent)
    {
        Vector3 offset = Vector3.up;
        parent.GetComponent<CharacterHandler>().charAnimator.Play("Attack Layer.A_BombThrow");
        GameObject bomb = Instantiate(bombObject, parent.transform.position + offset, parent.transform.rotation);
        bomb.GetComponent<BombBehaviour>().StartBomb(parent);
        bomb.GetComponent<BombBehaviour>().forward = parent.transform;
        bomb.GetComponent<BombBehaviour>().StartCoroutine(bomb.GetComponent<BombBehaviour>().BombExplosion());
        bomb.GetComponent<BombBehaviour>().playerInt = parent.GetComponent<CharacterStats>().playerID;
    }
}
