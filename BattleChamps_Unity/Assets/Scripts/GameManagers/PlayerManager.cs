using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public List<GameObject> allPlayers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        allPlayers.Clear();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!allPlayers.Contains(player))
            {
                allPlayers.Add(player);
                cameraFollow.targetPositions.Add(player.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerJoined()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!allPlayers.Contains(player))
            {
                allPlayers.Add(player);
                cameraFollow.targetPositions.Add(player.transform);

                StartCoroutine(wait(player));
            }
        }
    }
    IEnumerator wait(GameObject player)
    {
        PlayerUI_Handler playerUI = player.GetComponent<PlayerUI_Handler>();
        player.GetComponent<CharacterStats>().playerID = allPlayers.Count - 1;
        yield return new WaitForSeconds(0.1f);
        UIManager.Instance.AddColour(playerUI.playerColour, allPlayers.Count);
    }
    public void PlayerLeft()
    {
        allPlayers.Clear ();
        cameraFollow.targetPositions.Clear();
        PlayerJoined();
    }

    public void ResetPlayers()
    {
        allPlayers.Clear();
        cameraFollow.targetPositions.Clear();
        PlayerJoined();
    }
}
