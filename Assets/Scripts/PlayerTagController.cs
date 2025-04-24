using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerTagController : MonoBehaviourPun
{
    [SerializeField] PlayerNickname playerNickPrefab;
    PlayerNickname playerNick;

    void Start()
    {
        var gameCanvas = GameObject.Find("Canvas");
        playerNick = GameObject.Instantiate(playerNickPrefab, gameCanvas.transform);
        playerNick.SetTarget(transform);

        if (photonView.IsMine)
        {
            var nick = photonView.Owner.NickName;
            UpdateNick(nick);
        }
        else photonView.RPC("RequestNick", photonView.Owner, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    void UpdateNick(string nickName)
    {
        if (playerNick != null) playerNick.SetName(nickName);
    }

    [PunRPC]
    void RequestNick(Player player)
    {
        photonView.RPC("UpdateNick", player, photonView.Owner.NickName);
    }
    void OnDestroyNick()
    {
        PhotonNetwork.Destroy(playerNick.gameObject);
    }
}
