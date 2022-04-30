using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomiserMenuOptions : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public GameObject arrows;

    public CustomiserController menuButtonController;
    public int thisIndex;
    [SerializeField] GameObject menuPanelToOpen;

    // Start is called before the first frame update
    void Start()
    {
        arrows.SetActive(false);
        text1.color = text2.color = new Color(255, 255, 255, 150);
    }

    public void Selected()
    {
        arrows.SetActive(true);
        text1.color = text2.color = new Color(255, 255, 255, 255);
    }
    public void Unselected()
    {
        arrows.SetActive(false);
        text1.color = text2.color = new Color(255, 255, 255, 150);
    }

    // Update is called once per frame
    void Update()
    {
        if (menuButtonController.playerJoined == false)
        { Unselected(); }

        if (menuButtonController.index == thisIndex)
        { Selected(); }
        else { Unselected(); }
    }
}
