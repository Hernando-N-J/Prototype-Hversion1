using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.compA.gameA
{
    /// <summary>
    /// Game manager.
    /// Connects and watches Photon Status, Instantiates Player
    /// Deals with quiting the room and the game
    /// Deals with level loading (outside the in room synchronization)
    /// It is a prefab because the game requires several scenes and this script will be reused
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager GameManagerInstance;

        public GameObject playerPrefab;

        private void Start()
        {
            // Replacing if (playerPrefab == null) 
            // Instantiate only if PlayerManager doesn't have a reference to any local player instance
            if (PlayerManager.localPlayerInstance == null)
            {
                // Debug.LogError("Put the playerPrefab");
                Debug.LogFormat("Instantiating local player from {0}", SceneManagerHelper.ActiveSceneName);
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load from {0}", SceneManagerHelper.ActiveSceneName);

            }

            GameManagerInstance = this;
        }

        private void LoadArena()
        {
            // Check if this client is Master Client
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork: client is not Master client");
            }

            Debug.LogFormat("-- Loading level: {0}", PhotonNetwork.CurrentRoom.Name);

            /* We use PhotonNetwork.LoadLevel() to load the level we want, we don't use Unity directly, because we want to rely on Photon to load this level on all connected clients in the room, since we've enabled PhotonNetwork.AutomaticallySyncScene for this Game. 
              Can be called only by Master Client
              However, we'll call LoadArena() ONLY if we are the MasterClient using PhotonNetwork.IsMasterClient.
             Method checks out if is or isn't MasterClient, but anyway it continues till here.*/
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        /// <summary>
        /// Player's connection -  Entering room
        /// </summary>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("-- OnPlayerEnteredRoom(): {0}", newPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("-- OnPlayerEnteredRoom IsMasterClient = {0}", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("-- OnPlayerLeftRoom: {0}", otherPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("-- OnPlayerLeftRoom IsMasterClient: {0}", PhotonNetwork.IsMasterClient);
                
                LoadArena();
            }
        }

        public override void OnLeftRoom()
        {
            Debug.Log("--GM POV OnLeftRoom called by PUN");
            Debug.Log("-- Loading scene 0");
            // If not in a game, show Launcher scene
            SceneManager.LoadScene(0);
            Debug.Log("-- Scene 0 called");
            //TODO: Why SceneManager? Change it to Photon LoadLevel
            //Result: the same, Launcher scene appears Loading or not loaded

            //PhotonNetwork.LoadLevel(0);
            Debug.Log("-- Leaving POV OnLeftRoom");
        }

        public void LeaveRoom()
        {
            Debug.Log("-- GM LeaveRoom");
            // The player leaves the room and returns to master server where he can create or join into another room
            // TTL: Time To Live ... for an actor in a room
            PhotonNetwork.LeaveRoom();
            Debug.Log("-- GM LeaveRoom/PN.LeaveRoom");
        }

    }
}
