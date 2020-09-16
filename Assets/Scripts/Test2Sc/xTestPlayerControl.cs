using Photon.Pun;
using UnityEngine;

namespace Test2Sc
{
    public class xTestPlayerControl : MonoBehaviourPun
    {
        public int speed = 10;

        private Color _myColor;

        private void Start()
        { 
            if (photonView.IsMine)
            {
                var go = gameObject;
                go.tag = "Local Player";
                Debug.Log(" The GO tag is: " + go.tag);
            }

            Debug.Log("Start playerPrefab photonview.IsMine = ** " + photonView.IsMine);
            Debug.Log("Start PlyrPrefab isConnected = ** " + PhotonNetwork.IsConnected);

            _myColor = GetComponent<Renderer>().material.color;
        }

        void Update()
        {
            if (!photonView.IsMine) return;

            Move();

            _myColor = Color.gray;
            if (Input.GetKeyDown(KeyCode.Q)) _myColor = Color.magenta;
            if (Input.GetKeyDown(KeyCode.E)) _myColor = Color.cyan;
        }

        private void Move()
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //transform.position += move * speed * Time.deltaTime;
            transform.position += move * (speed * Time.deltaTime);
        }
    }
}

 
  



