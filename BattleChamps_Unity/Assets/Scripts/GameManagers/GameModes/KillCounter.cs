using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public int playerID { get; set; }
    public int counter;

    private void OnEnable()
    {
        counter = 0;
    }
}
