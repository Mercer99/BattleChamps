using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackOutScreen : MonoBehaviour
{
    public TextMeshProUGUI playerNumText;
    public Image playerColourImage;

    public void StartPanel(Color pColour, int playerNum)
    {
        transform.root.GetComponent<Animator>().SetBool("AnimBoolMenuActive", true);
        playerNumText.text = "PLAYER " + (playerNum + 1);
        playerColourImage.color = pColour;
    }

    public void DisablePause()
    {
        transform.root.GetComponent<Animator>().SetBool("AnimBoolMenuActive", false);
        StartCoroutine(Disable());
    }
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(1.05f);
        PlayerConfigurationManager.Instance.Menu_BackToGame();
    }    
}
