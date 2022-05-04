using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterMaster : MonoBehaviour
{
    public Teleporter tp1;
    public Teleporter tp2;

    public float teleporterCooldown = 5f;
    private float currentTeleporterCooldown = 0f;

    public void StartCD()
    { currentTeleporterCooldown = teleporterCooldown; }

    // Update is called once per frame
    void Update()
    {
        if (currentTeleporterCooldown > 0)
        {
            currentTeleporterCooldown -= Time.deltaTime;
            //tp1.CD_Canvas.SetActive(true);
            //tp2.CD_Canvas.SetActive(true);
            tp1.recentlyActivated = true;
            tp2.recentlyActivated = true;
            tp1.effect.SetActive(false);
            tp2.effect.SetActive(false);
            
            //tp1.CD_timer.fillAmount = currentTeleporterCooldown / teleporterCooldown;
            //tp2.CD_timer.fillAmount = currentTeleporterCooldown / teleporterCooldown;
        }
        else
        {
            //tp1.CD_Canvas.SetActive(false);
            //tp2.CD_Canvas.SetActive(false);
            tp1.recentlyActivated = false;
            tp2.recentlyActivated = false;
            tp1.effect.SetActive(true);
            tp2.effect.SetActive(true);
        }
    }
}
