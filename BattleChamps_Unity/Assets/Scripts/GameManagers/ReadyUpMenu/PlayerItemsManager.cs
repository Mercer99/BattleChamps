using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsManager : MonoBehaviour
{
    public GameObject[] headAccessories;
    public GameObject[] bodyAccessories;
    public GameObject[] weapons;

    public string headAccName;
    public string bodyAccName;
    public string weaponName;

    public int headAccNum;
    public int bodyAccNum;
    public int weaponNum;

    public ParticleSystem poof;

    public void Start()
    {
        headAccNum = 0;
        bodyAccNum = 0;
        weaponNum = 0;

        foreach (GameObject item in weapons)
        { item.SetActive(false); }
        weapons[weaponNum].SetActive(true);

        weaponName = weapons[weaponNum].name;

        foreach (GameObject item in headAccessories)
        { item.SetActive(false); }
        headAccessories[headAccNum].SetActive(true);

        headAccName = headAccessories[headAccNum].name;

        foreach (GameObject item in bodyAccessories)
        { item.SetActive(false); }
        bodyAccessories[bodyAccNum].SetActive(true);

        bodyAccName = bodyAccessories[bodyAccNum].name;
    }

    public void ChangeWeapon()
    {
        foreach (GameObject item in weapons)
        { item.SetActive(false); }
        weapons[weaponNum].SetActive(true);

        weaponName = weapons[weaponNum].name;
    }
    public void ChangeHead()
    {
        foreach (GameObject item in headAccessories)
        { item.SetActive(false); }
        headAccessories[headAccNum].SetActive(true);

        headAccName = headAccessories[headAccNum].name;
    }
    public void ChangeBody()
    {
        foreach (GameObject item in bodyAccessories)
        { item.SetActive(false); }
        bodyAccessories[bodyAccNum].SetActive(true);

        bodyAccName = bodyAccessories[bodyAccNum].name;
    }
}
