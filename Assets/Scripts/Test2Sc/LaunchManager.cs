using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// CHECKOUT OnConnectedToMaster to control whether show or not control panel using a bool value
// TODO: set PhotonNetwork.LevelLoadingProgress();

namespace Test2Sc
{
    public class LaunchManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private GameObject connectingStatusPanel;
        public GameObject lobbyPanel;
        // [SerializeField] private GameObject lobbyPanel;
        // TODO Change lobbyPanel as a new scene
        // CHECKOUT if lobbyPanel GO is empty, it appears as Unchanged. If not, it shows actual GO attached to it.

        [SerializeField] private byte maxPlayersInRoom = 4;

        private LaunchManager _instance;

        private string _levelName = "GameScene2";
        private string _gameVersion = "1.0";
        
        private bool _isConnected;
        
        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>

        private void Awake()
        {
            //if (instance != null) Destroy(this.gameObject);
            //else instance = this;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            ShowPanel(controlPanel);
            
            if(!PhotonNetwork.IsConnected)
                Debug.Log("--- Start... player is connected? => " + PhotonNetwork.IsConnected );

        }

        public void Connect()
        {
            ShowPanel(connectingStatusPanel);
            
            if (!PhotonNetwork.IsConnected)
                Debug.Log("--- Connect ... Player isn't connected...");
            
            _isConnected = PhotonNetwork.ConnectUsingSettings();

            Debug.Log("*** Connect ... Is the player connected? " + PhotonNetwork.IsConnected);
            
            // keep track of when to join a room, because when we come back from the game
            // we will get a callback that we are connected, so we need to know what to do then
            PhotonNetwork.GameVersion = _gameVersion;
        }
        
        public override void OnConnected() => Debug.Log("Connected to internet...");
        
        public override void OnConnectedToMaster()
        {
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded,
            // OnConnectedToMaster will be called, in that case we don't want to do anything.

            // TODO improve code
            //if(!_isConnected) PhotonNetwork.ConnectUsingSettings(); 
            // BUG (!_isConnected) return; anyway, the game works fine

            // TODO change lobbyPanel for a new scene when player returns after quitting match (quit button)

            if (!PhotonNetwork.IsConnected) Debug.Log(" --- LM OCTM Player is not connected");

            Debug.Log("--- LM OCTM is player connected?" + PhotonNetwork.IsConnected);
            Debug.Log("--- LM OCTM is player connected and ready?" + PhotonNetwork.IsConnectedAndReady);
           
            ShowPanel(lobbyPanel);
            
            Debug.Log(PhotonNetwork.NickName + " is connected to Photon server... t or f? ... " + PhotonNetwork.IsConnected);

            _isConnected = false;
        }

        public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
        
        public override void OnJoinedRoom()
        {
            Debug.Log("Player " + PhotonNetwork.NickName + " has joined the room ... " +
                      PhotonNetwork.CurrentRoom.Name);
            PhotonNetwork.LoadLevel(_levelName);
        } 

        public override void OnPlayerEnteredRoom(Player other)
        {
            // not seen if you're the player connecting
            Debug.LogFormat("--- OnPlayerEnteredRoom() The new player's nickname is => {0}", other.NickName);
            Debug.Log("-- OnPlayerEnteredRoom() Number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log(other.NickName+ " joined to Room"+PhotonNetwork.CurrentRoom.Name);

            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("-- --- OnPlayerEnteredRoom() -- Client " + PhotonNetwork.NickName + " *** is not master client... Synchronizing scene: " + _levelName);
                return;
            }
            
            //We only load a level if we are the first player,
            //else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            
            // called before OnPlayerLeftRoom
            Debug.Log("-- OnPlayerEnteredRoom()  Player " + PhotonNetwork.NickName + " *** is master client? => " +  PhotonNetwork.IsMasterClient + " ... Loading level: " + _levelName);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
           Debug.LogWarning("-- OnJoinRandomFailed. Message: " + message + ". ReturnCode: " + returnCode +
                             " \n---Next line: CreateRoom()");
           CreateAndJoinRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("--- Player is disconnected. Cause: " + cause);
            ShowPanel(controlPanel);
        }

        private void CreateAndJoinRoom()
        {
            var randomRoomName = "Room" + Random.Range(0, 1000);
            var roomOptions = new RoomOptions {IsOpen = true, IsVisible = true, MaxPlayers = maxPlayersInRoom};

            //The code above replaced the below code block
            // RoomOptions roomOptions = new RoomOptions();
            // roomOptions.IsOpen = true;
            // roomOptions.IsVisible = true;
            // roomOptions.MaxPlayers = maxPlayersInRoom;
            
            PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
        }
        
        // I created this method because I was tired of SettingActive to true or false the panels
        // in several methods in this script
        public void ShowPanel([NotNull] Object panelGo)
        {
            GameObject[] panelsArray = {controlPanel, connectingStatusPanel, lobbyPanel};
            foreach (var go in panelsArray)
            {
                go.SetActive(false);
                if (go == panelGo) go.SetActive(true);
            }
        }
    }
}
