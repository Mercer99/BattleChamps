using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode_AttritionManager : Singleton<Mode_AttritionManager>
{
    public CameraFollow cameraFollow;
    public List<GameObject> allPlayers = new List<GameObject>();

    public GameObject[] players;

    // Start is called before the first frame update
    void Awake()
    {
        allPlayers.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerJoined(GameObject player)
    {
        player.GetComponent<CharacterStats>().playerID = allPlayers.Count;


        allPlayers.Add(player);
        cameraFollow.targetPositions.Add(player.transform);
    }
}
