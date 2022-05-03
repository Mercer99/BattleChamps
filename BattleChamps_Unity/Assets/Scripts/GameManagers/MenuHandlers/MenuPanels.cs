using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanels : MonoBehaviour
{
    Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SlideIn()
    {
        animator.Play("SlideMenuReverse");
    }
    public void SlideOut()
    {
        animator.Play("SlideMenu");
    }
}
