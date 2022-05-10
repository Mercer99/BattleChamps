using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIStateNotification : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Popup(string stateText)
    {
        text.text = stateText;
        Destroy(gameObject, 1);
    }
}
