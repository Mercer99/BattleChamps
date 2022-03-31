using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerColour : MonoBehaviour
{
    public Image colourImage;
    public TextMeshProUGUI playerText;

    public void ChangeInfo(Color colour, int playerNum)
    {
        colourImage.color = colour;
        colourImage.color = colour;
        playerText.text = "Player " + playerNum.ToString() + " = ";
    }
}
