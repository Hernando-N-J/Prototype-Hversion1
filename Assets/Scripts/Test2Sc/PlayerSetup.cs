using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Test2Sc
{
    public class PlayerSetup : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject FPSCamera;

        [SerializeField] TextMeshProUGUI playerNameText;

        private void Start()
        {
            if (photonView.IsMine)
            {
                // Ensure that my tag is different from others
                gameObject.tag = "Local Player";
                transform.GetComponent<MoveController>().enabled = true;
                FPSCamera.GetComponent<Camera>().enabled = true;
            }
            else
            {
                transform.GetComponent<MoveController>().enabled = false;
                
                FPSCamera.GetComponent<Camera>().enabled = false;
                FPSCamera.GetComponent<AudioListener>().enabled = false;
                FPSCamera.SetActive(false);

            }

            SetPlayerUI();
        }

        private void SetPlayerUI()
        {
            if (playerNameText != null) playerNameText.text = photonView.Owner.NickName;

        }
    }
}
