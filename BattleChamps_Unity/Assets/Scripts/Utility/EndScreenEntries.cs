using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndScreenEntries : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI playerText;
    public Image playerColour;

    public void SpawnEntry(string player, Color colour)
    {
        GetComponent<TextMeshProUGUI>().text = player;
        playerColour.color = colour;
    }
}
