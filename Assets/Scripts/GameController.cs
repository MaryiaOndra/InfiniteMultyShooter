using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform firstPoint;
    [SerializeField] Transform secondPoint;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        RoomOptions _roomOptions = new RoomOptions();
        _roomOptions.IsVisible = false;
        _roomOptions.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("MyRoom", _roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom: player = {PhotonNetwork.LocalPlayer.ActorNumber}");

        Transform _targetTr = PhotonNetwork.LocalPlayer.ActorNumber == 1 ?
            firstPoint :
            secondPoint;

        PhotonNetwork.Instantiate("PlayerController", _targetTr.position, _targetTr.rotation);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"OnPlayerEnteredRoom: PLAYER = {newPlayer.ActorNumber}");
    }
}
