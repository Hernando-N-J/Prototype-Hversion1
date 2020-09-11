using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// CHECKOUT OnConnectedToMaster to control whether show or not control panel using a bool value
// TODO: set PhotonNetwork.LevelLoadingProgress();

namespace Test2Sc
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private GameObject connectingStatusPanel;
        [SerializeField] private GameObject lobbyPanel;
        // CHECKOUT if lobbyPanel GO is empty, it appears as Unchanged. If not, it shows actual GO attached to it.

        [SerializeField] private byte maxPlayersInRoom = 4;

        private Launcher _instance;

        private string _levelName;
        private string _gameVersion = "1.0";
        private string _disconnectedCause;
        
        private bool _hasJoinedRoom;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>

        private void Awake()
        {
            if (_instance != null) Destroy(this.gameObject);
            else _instance = this;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            ShowPanel(controlPanel);
            
            if(!PhotonNetwork.IsConnected)
                Debug.Log("--- player is connected? => " + PhotonNetwork.IsConnected );

            _levelName = "TestSc2";
        }

        public void Connect()
        {
            if (!PhotonNetwork.IsConnected) Debug.Log("--- IsPlayerConnected? " + PhotonNetwork.IsConnected);
            
            ShowPanel(connectingStatusPanel);
            
            PhotonNetwork.ConnectUsingSettings();
            
            Debug.Log("*** Is the player connected? " + PhotonNetwork.IsConnected);
            
            // keep track of when to join a room, because when we come back from the game
            // we will get a callback that we are connected, so we need to know what to do then
            PhotonNetwork.GameVersion = _gameVersion;
        }
        
        public override void OnConnectedToMaster() => ShowPanel(lobbyPanel);

        public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

        public override void OnJoinedRoom()
        {
            Debug.Log("Player has joined a room");
            PhotonNetwork.LoadLevel(_levelName);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogWarning("-- OnJoinRandomFailed. Message: " + message + ". ReturnCode: " + returnCode +
                             " \n---Next line: CreateRoom()");
            var randomRoomName = "Room" + Random.Range(0, 1000);
            var roomOptions = new RoomOptions {IsOpen = true, IsVisible = true, MaxPlayers = maxPlayersInRoom};

            //The code above replaced the below code block
            // RoomOptions roomOptions = new RoomOptions();
            // roomOptions.IsOpen = true;
            // roomOptions.IsVisible = true;
            // roomOptions.MaxPlayers = maxPlayersInRoom;
            
            PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("--- Player is disconnected. Cause: " + cause);
            ShowPanel(controlPanel);
        }

        // I created this method because I was tired of SettingActive to true or false the panels
        // in several methods in this script
        private void ShowPanel([NotNull] Object panelGO)
        {
            GameObject[] panelsArray = {controlPanel, connectingStatusPanel, lobbyPanel};
            foreach (var go in panelsArray)
            {
                go.SetActive(false);
                if (go == panelGO) go.SetActive(true);
            }
        }
    }
}


/*
Debug.Log(PhotonNetwork.NickName + " is connected to photon server");
Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
*/


