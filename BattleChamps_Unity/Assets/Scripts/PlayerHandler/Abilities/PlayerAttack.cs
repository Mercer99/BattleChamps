using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public Collider swordCollider;

    public GameObject sword;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
       // {
        //    animator.Play("Base Layer.Anim_Sword_Swing", 0, 0f);
       // }
    }

   // public void UpdateSwordCollider()
    //{
        //swordCollider.enabled = !swordCollider.enabled;
        //Debug.Log(swordCollider.enabled);
    //}

    
}
