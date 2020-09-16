using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// TODO Create a code to activate the Sphere collider only if the playerGO has a "Enemy" tag

namespace Test2Sc
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject playerPrefab;

        // ReSharper disable once InconsistentNaming
        public static GameManager instance;
        
        private readonly string _startScene = "LauncherSc";
        
        private void Awake()
        {
            if (instance != null) Destroy(this.gameObject);
            else instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                Debug.Log(" *** GM Start ... player is connected? " + PhotonNetwork.IsConnected);
                return;
            }
            if (playerPrefab == null) return;
            var randomPoint = Random.Range(-20, 20);
            var v3Inst = new Vector3(randomPoint, 0, randomPoint);

            PhotonNetwork.Instantiate(playerPrefab.name, v3Inst, Quaternion.identity);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("GM OnJoinedRoom... "+PhotonNetwork.NickName+" has joined to "+PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("GM OnPlayerEnteredRoom ... New player..." + newPlayer.NickName + " has joined to: " + PhotonNetwork.CurrentRoom.Name );
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("GM OnPlayerLeftRoom() ... player {0}", other.NickName + "has left the room"); // seen when other disconnects
            if (!PhotonNetwork.IsMasterClient) return;
            
            Debug.LogFormat("---GM OnPlayerLeftRoom ...player IsMasterClient ? ... {0}",
                PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            
            //PhotonNetwork.LoadLevel(_leftRoomLevelName);
        }
        // TODO check out how to load a scene using OnLeftRoom without getting unloaded additional scenes in hierarchy (Launcher scene)
       // public override void OnLeftRoom() => SceneManager.LoadScene(_leftRoomLevelName);
       // TODO: If I use line below, MainSc will show as "loading MainSc" in hierarchy - Unity
       public override void OnLeftRoom()
       {
           Debug.Log("GM POV OnLeftRoom ... Player " + PhotonNetwork.NickName + " has left the room");

            // If we use SceneManager.LoadScene("LauncherSc");
            // Unity will try to load a scene after quitting or exiting Play mode
            // showing "Loading LaunchSc" or "LaunchSc not loaded"
            PhotonNetwork.LoadLevel(_startScene);
            
        }

        public void LeaveRoom()
        {
            //QuitButtonClicked = true;
            PhotonNetwork.LeaveRoom();
            Debug.Log(">>> Quit button has been clicked");
        }
    }
}