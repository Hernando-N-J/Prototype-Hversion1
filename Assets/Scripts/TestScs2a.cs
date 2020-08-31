using UnityEngine;
using Photon.Pun;

//photonview is in the playerPrefab script - TestSc2.cs
//It can be used only after joined a room, 
//This script allows us to join a room

public class TestScs2a : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    bool joinedToRooom;
    bool readyToPlay;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Instantiate is possible only if Player is in a Room
    public void InstantiatePlayer()
    {
        Vector3 v = new Vector3(1, 1, 1);
        if (playerPrefab != null)
            PhotonNetwork.Instantiate(this.playerPrefab.name, v, Quaternion.identity);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat("POV OnJoinRandomFailed... {0} --- {1}", returnCode, message);
        PhotonNetwork.CreateRoom(default, default, default);
    }

    public override void OnJoinedRoom()
    {
        
        joinedToRooom = true;
        Debug.Log("POV OnJoineRoom called");
        Debug.Log("Joined to Room = ** " + joinedToRooom);
        InstantiatePlayer();
    }
}
