using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] attackVoicelines;

    // Start is called before the first frame update
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAttackVoiceline()
    {
        audioSource.PlayOneShot(attackVoicelines[Random.Range(0, attackVoicelines.Length)]);
    }
}
