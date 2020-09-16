using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Test2Sc
{
    public class TakeDamage : MonoBehaviourPun
    {
        [SerializeField] private Image healthBar;
       
        // ReSharper disable once InconsistentNaming
        private float health = 100;
        public float startHealth = 100;


        private void Start()
        {
            if (photonView.IsMine)
            {
                health = startHealth;
                healthBar.fillAmount = health / startHealth;
            }
        }


        [PunRPC]
        public void ChangeColor(int _d)
        {
            health -= _d;
            Debug.Log(" *** the player with tag: " + gameObject.tag + " \nhas a health value of: " + health);
            gameObject.GetComponent<Renderer>().material.color = Color.blue;

            if(health < 80)
                gameObject.GetComponent<Renderer>().material.color = Color.green;

            if (health < 60)
                gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }


        [PunRPC]
        public void ShootingTakeDamage(int damage)
        {
            health -= damage; 
            
            Debug.Log(health);

            healthBar.fillAmount = health / startHealth;

            if (health <= 0f) Die();
        }

        private void Die()
        {
            if (photonView.IsMine) GameManager.instance.LeaveRoom();
        }
    }
}
