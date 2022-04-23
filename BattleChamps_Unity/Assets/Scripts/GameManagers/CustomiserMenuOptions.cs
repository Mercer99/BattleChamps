using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomiserMenuOptions : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public GameObject arrows;

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
        
    }
}
