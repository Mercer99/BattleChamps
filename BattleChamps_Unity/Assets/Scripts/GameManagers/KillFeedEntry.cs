using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillFeedEntry : MonoBehaviour
{
    public TextMeshProUGUI killFeedText;

    private void OnEnable()
    {
        GetComponent<Animator>();
    }

    public void KillEntryDestroy()
    {
        Destroy(gameObject);
    }
}
