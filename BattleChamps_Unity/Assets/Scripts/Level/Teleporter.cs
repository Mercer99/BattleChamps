using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [HideInInspector]
    public bool recentlyActivated = false;
    public Material runeActive;
    public Material runeDeactive;

    public TeleporterMaster tpMaster;
    public GameObject otherTeleporter;

    public GameObject CD_Canvas;
    public Image CD_timer;

    private void Start()
    {
        CD_Canvas.SetActive(false);
    }

    private void Update()
    {
        GetComponent<Renderer>().material = recentlyActivated ? runeDeactive : runeActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (recentlyActivated == false)
            {
                tpMaster.StartCD();

                other.transform.position = otherTeleporter.transform.position;
                Debug.Log("Just Teleported: " + recentlyActivated);
            }
        }
    }
}
