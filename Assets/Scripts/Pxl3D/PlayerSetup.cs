using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Pxl3D
{
    public class PlayerSetup : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject fpsCamera;
        [SerializeField] private TextMeshProUGUI playerNameText;

        // Start is called before the first frame update
        void Start()
        {

            if (photonView.IsMine)
            {

                transform.GetComponent<MoveController>().enabled = true;
                fpsCamera.GetComponent<Camera>().enabled = true;


            }
            else
            {
                transform.GetComponent<MoveController>().enabled = false;
                fpsCamera.GetComponent<Camera>().enabled = false;

            }


            SetPlayerUI();

        }

        void SetPlayerUI()
        {
            if (playerNameText!=null)
            {
                playerNameText.text = photonView.Owner.NickName;

            }
        }

    
    }
}
