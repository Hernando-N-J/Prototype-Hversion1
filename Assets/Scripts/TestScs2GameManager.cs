using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//photonview is in the playerPrefab script - TestSc2.cs
//It can be used only after joined a room, 
//This script allows us to join a room

public class TestScs2GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Instantiate is possible only if Player is in a Room
    public void InstantiatePlayer()
    {
        float randomX = Random.Range(5, -5);
        float randomZ = Random.Range(5, -5);
        Debug.Log("Random x: " + randomX + " ---  Random Z: " + randomZ);
        Vector3 v = new Vector3(randomX, 1, randomZ);
        if (playerPrefab != null)
            PhotonNetwork.Instantiate(this.playerPrefab.name, v, Quaternion.identity);

    }

    private void Start()
    {
        //b1.gameObject.SetActive(false);
        //b2.gameObject.SetActive(false);
        //b3.gameObject.SetActive(false);
        //b4.gameObject.SetActive(false);
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
        Debug.Log("POV OnJoineRoom called");
        Debug.Log("Joined to Room = ** " + PhotonNetwork.CurrentRoom.Name);
        InstantiatePlayer();
    }
 
}
