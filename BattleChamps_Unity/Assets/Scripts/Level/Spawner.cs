using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnable;
    
    private void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        Instantiate(spawnable, gameObject.transform);
        yield return new WaitForSeconds(5);
        StartCoroutine(spawn());
    }
}
