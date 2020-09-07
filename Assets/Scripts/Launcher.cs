using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace com.compA.gameA
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField] 
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject connectingText;

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        private string gameVersion = "1.0";
        string levelName = "TestSc2";
        
        private bool isPlayerConnected;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            connectingText.SetActive(false);
            controlPanel.SetActive(true);
            Debug.Log("-- Start was called");
        }

        public void Connect()
        {
            connectingText.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("** Launcher/Connect PlayBtn PN.IsConnected/PN.JRandRoom()");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isPlayerConnected = PhotonNetwork.ConnectUsingSettings();

                // keep track of when to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
                //PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public override void OnConnectedToMaster()
        {
            if (isPlayerConnected)
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("-- OCTMaster called PhotonNetwork.JoinRandomRoom ");
                isPlayerConnected = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            connectingText.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarningFormat("-- OnDisconnected called by PUN reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("-- OnJoinRandomFailed called by PUN \n--------- Next line: CreateRoom()");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("-- POV OnJoinedRoom called ");
            Debug.Log("-- POV OJR -- Client MasterClient? " + PhotonNetwork.IsMasterClient);

            //We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            Debug.Log("-- POV OJRoom -- *** Loading Scene Room for: " + playerCount);
            PhotonNetwork.LoadLevel(levelName);

            Debug.Log("-- POV OJRoom -- Number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}


