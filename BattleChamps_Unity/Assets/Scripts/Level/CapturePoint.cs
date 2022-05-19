using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePoint : MonoBehaviour
{
    public float currentTime;
    public float timeToCapture = 3;

    public GameObject previous;
    public bool paused;

    public int playersIn;
    public List<GameObject> players;

    public Image ringCounter;
    public Color[] playerColours;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentTime = 0;
        playersIn = 0;
        paused = true;
        players = new List<GameObject> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Count > 0)
        { paused = playersIn > 1 ? true : false; }
        else { paused = true; }

        ringCounter.color = playerColours[previous.GetComponent<CharacterHandler>().playerConfig.teamNum];

        if (currentTime > 0 && !paused)
        {
            currentTime -= Time.deltaTime;
        }
        else if (currentTime <= 0 && !paused)
        { PointCaptured(); Mode_ConquestManager.Instance.SpawnNewPoint(); Destroy(gameObject); }
    }

    public void PointCaptured()
    {
        Mode_ConquestManager.Instance.AddPoints(previous.GetComponent<CharacterHandler>().playerConfig.PlayerIndex);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            players.Add(other.gameObject);

            if (previous == null)
            {
                previous = other.gameObject;
            }

            if (GameModeManager.Instance.teamsActive)
            {
                if (other.gameObject.GetComponent<CharacterHandler>().playerConfig.teamNum != previous.GetComponent<CharacterHandler>().playerConfig.teamNum)
                {
                    currentTime = timeToCapture;
                    playersIn++;
                }

                foreach (GameObject player in players)
                {
                    if (other.gameObject.GetComponent<CharacterHandler>().playerConfig.teamNum != player.GetComponent<CharacterHandler>().playerConfig.teamNum)
                    {
                        previous = player;
                    }
                    else
                    { }
                }
            }
            else 
            {
                if (other.gameObject.GetComponent<CharacterHandler>().playerConfig.teamNum != previous.GetComponent<CharacterHandler>().playerConfig.teamNum)
                {
                    playersIn++;
                }
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject p in players)
            {
                if (other.gameObject == p)
                { players.Remove(other.gameObject); }
            }

            if (GameModeManager.Instance.teamsActive)
            {
                if (other.gameObject.GetComponent<CharacterHandler>().playerConfig.teamNum != previous.GetComponent<CharacterHandler>().playerConfig.teamNum)
                {
                    if (players.Count != 0)
                    {
                        currentTime = timeToCapture;
                        playersIn--;

                        if (other.gameObject == previous)
                        { previous = players[0]; }
                    }
                }
                else
                { }
            }
            else
            {
                if (players.Count != 0)
                {
                    playersIn--;

                    if (other.gameObject == previous)
                    { previous = players[0]; }
                }
            }
        }
    }
}
