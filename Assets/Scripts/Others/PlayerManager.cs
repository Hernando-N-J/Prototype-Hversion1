using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace com.compA.gameA
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private GameObject beams;
        [SerializeField] private GameObject PlayerUiPrefab;

        [Tooltip("Check if this instance is the Local Player")]
        public static GameObject localPlayerInstance;
       
        public float Health = 1f;

        private bool IsFiring;
      
        void Awake()
        {
            if (photonView.IsMine) PlayerManager.localPlayerInstance = this.gameObject;

            // Keep the instance when doing level synchronization
            DontDestroyOnLoad(this.gameObject);

            if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }
        }

       void Start()
        {
            if (PlayerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(PlayerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }

            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

/// <summary>
/// MonoBehaviour method called on GameObject by Unity on every frame.
/// </summary>
private void Update()
        {
            if(photonView.IsMine) ProcessInputs();

            // trigger Beams active state
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }

            if (Health <= 0f) GameManager.GameManagerInstance.LeaveRoom();
        }

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring) IsFiring = true;
            }
                

            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring) IsFiring = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine) 
                Debug.Log("It is not the local player");
            
        
            if (!other.name.Contains("Beam"))
                Debug.Log("other.name.Contains != 'Beam'");

            Health -= 0.1f;
        }

        private void OnTriggerStay(Collider other)
        {
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine)
                Debug.Log("It is not the local player");

            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
                Debug.Log("other.name.Contains != 'Beam'");

            Health -= 0.1f * Time.deltaTime;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(IsFiring); // Send our data to others
                stream.SendNext(Health);
            }
            else
            {
                this.IsFiring = (bool)stream.ReceiveNext(); // Receive data from network player
                this.Health = (float)stream.ReceiveNext();
            }
        }

        private void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadScMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }

        private void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
