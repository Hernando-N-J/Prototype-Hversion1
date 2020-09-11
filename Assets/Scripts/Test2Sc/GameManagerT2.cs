using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO Create a code to activate the Sphere collider only if the playerGO has a "Enemy" tag

namespace Test2Sc
{
    public class PGameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject playerPrefab;

        public static PGameManager Instance;

        private const string SceneIfLeftRoom = "LauncherSc";
        private string _levelName = "TestSc2";
        private string _leftRoomLevelName = "LauncherSc";

        private void Awake()
        {
            if (Instance != null) Destroy(this.gameObject);
            else Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (!PhotonNetwork.IsConnected) return;
            if (playerPrefab == null) return;
            var randomPoint = Random.Range(-20, 20);
            var v3Inst = new Vector3(randomPoint, 0, randomPoint);

            PhotonNetwork.Instantiate(playerPrefab.name, v3Inst, Quaternion.identity);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            // not seen if you're the player connecting
            Debug.LogFormat("--- OnPlayerEnteredRoom() The new player's nickname is => {0}", other.NickName);
            Debug.Log("-- OnPlayerEnteredRoom() Number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);

            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("-- --- OnPlayerEnteredRoom() -- Client " + PhotonNetwork.NickName + " *** is not master client... Synchronizing scene: " + _levelName);
                return;
            }
            
            //We only load a level if we are the first player,
            //else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            
            // called before OnPlayerLeftRoom
            Debug.Log("-- OnPlayerEnteredRoom()  Player " + PhotonNetwork.NickName + " *** is master client? => " +  PhotonNetwork.IsMasterClient + " ... Loading level: " + _levelName);
            
            LoadMatchScene();
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
            if (!PhotonNetwork.IsMasterClient) return;
            
            Debug.LogFormat("--- OnPlayerLeftRoom IsMasterClient {0}",
                PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            
            PhotonNetwork.LoadLevel(_leftRoomLevelName);
           
            //LoadMatchScene();
        }
        
        private void LoadMatchScene()
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("--- LoadMatchScene ... Loading level => " + _levelName);
            
            PhotonNetwork.LoadLevel(_levelName);
        }
        
        public override void OnLeftRoom() => SceneManager.LoadScene(SceneIfLeftRoom);

        public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    }
}