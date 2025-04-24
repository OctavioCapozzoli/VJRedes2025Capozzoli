using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using System;

public class Instantiator : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject[] playerSpawnPoints;
    Player[] characters;
    GameObject[] player;
    public float playerCount = 0;
    public float playersRequired = 2;

    private int spawnIndex;

    private void Awake()
    {
        playerSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        HandlePlayerSpawning();
    }

    [PunRPC]
    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        MovementController[] character = GameObject.FindObjectsOfType<MovementController>();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("son 2");
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("No son 2");
        }
    }

    public void HandlePlayerSpawning()
    {
        GameObject playerSpawnPoint = playerSpawnPoints[UnityEngine.Random.Range(0, playerSpawnPoints.Length - 1)];
        //SpawnCharacter("Player", playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        //photonView.RPC("SpawnPlayer", RpcTarget.All, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        var playersCount = PhotonNetwork.PlayerList.Length;

        //character = GameObject.FindObjectOfType<CharacterModel>();
        //character = PhotonView.Find(PhotonNetwork.LocalPlayer.ActorNumber).gameObject.GetComponent<CharacterModel>();

        CheckSpawnPoints();

    }

    void CheckSpawnPoints()
    {
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        if (spawnPoints.Length > 0)
        {

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].IsAvaiable)
                {
                    spawnPoints[i].IsAvaiable = false;

                    if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
                    {
                        SpawnCharacter("Player", spawnPoints[1].transform.position, Quaternion.identity);
                    }

                    if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                    {
                        SpawnCharacter("Player", spawnPoints[2].transform.position, Quaternion.identity);
                    }

                    return;
                }
            }
        }
    }

    //void CheckSpawnPoints()
    //{
    //    SpawnPoint[] sp = GameObject.FindObjectsOfType<SpawnPoint>();
    //    if (spawnIndex >= sp.Length) spawnIndex = 0;
    //    Vector3 position = sp[spawnIndex].transform.position;
    //    SpawnCharacter("Player",  position, Quaternion.identity);
    //}

    public void SpawnCharacter(string characterPrefName, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate(characterPrefName, position, rotation);
    }
}
