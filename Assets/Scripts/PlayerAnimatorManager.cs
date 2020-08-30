using UnityEngine;
using Photon.Pun;

namespace com.compA.gameA
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        [SerializeField] private float _directionDampTime = 0.25f;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            if(!_animator) Debug.LogError("PAM missing Animator comp");
        }

        private void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                Debug.Log("photonview.IsMine == false, IsConnected == true");
            }

            AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            // Check if running
            if(animStateInfo.IsName("BaseLayer.Run"))
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    _animator.SetTrigger("Jump");
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            
            if (v < 0) v = 0;

            _animator.SetFloat("Speed", h * h + v * v);
            _animator.SetFloat("Direction", h, _directionDampTime, Time.deltaTime);
        }



    }
}
