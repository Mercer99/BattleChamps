using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackOutScreen : MonoBehaviour
{
    public TextMeshProUGUI playerNumText;
    public string playerNum;
    public Image playerColourImage;

    public void StartPanel(Color pColour)
    {
        transform.root.GetComponent<Animator>().SetBool("AnimBoolMenuActive", true);
        playerNumText.text = "PLAYER " + playerNum;
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
