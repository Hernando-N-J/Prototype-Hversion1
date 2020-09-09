using UnityEngine;
using Photon.Pun;
using TMPro;

public class PxlPlayerSetup : MonoBehaviourPunCallbacks
{

    [SerializeField]
    GameObject FPSCamera;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {

        if (photonView.IsMine)
        {

            transform.GetComponent<MoveController>().enabled = true;
            FPSCamera.GetComponent<Camera>().enabled = true;


        }
        else
        {
            transform.GetComponent<MoveController>().enabled = false;
            FPSCamera.GetComponent<Camera>().enabled = false;

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
