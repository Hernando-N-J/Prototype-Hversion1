using UnityEngine;
using Photon.Pun;

public class PlayerNameInputManager : MonoBehaviour
{
    public void SetPlayerName(string playername)
    {

        if (string.IsNullOrEmpty(playername))
        {
            Debug.Log("player nama is empty");
            return;
        }

        PhotonNetwork.NickName = playername;





    }
}
