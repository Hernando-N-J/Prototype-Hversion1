using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Pxl3D
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputManager : MonoBehaviour
    {
        private const string PlayerCachedName = "Player Cached Name";

        private void Start()
        {
            //var defaultPlayerName = "";

            var inpField = GetComponent<InputField>();

            if (inpField == null) return;
            
            if (!PlayerPrefs.HasKey(PlayerCachedName)) return;
            
            // get the cached player name in PlayerPrefs and show it in input field text box
            var defaultPlayerName = PlayerPrefs.GetString(PlayerCachedName);
                
            inpField.text = defaultPlayerName;
        }

        public void SetDefaultPlayerName(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                Debug.LogWarning("Player name is null or empty");
                return;
            }

            PhotonNetwork.NickName = s;
            
            // Set PlayerCachedName as a PlayerPref key and assign it 
            // the s value
            PlayerPrefs.SetString(PlayerCachedName, s);
        }
    }
}
