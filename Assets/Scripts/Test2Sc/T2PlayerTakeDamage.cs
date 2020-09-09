using UnityEngine;
using Photon.Pun;

public class T2PlayerTakeDamage : MonoBehaviour
{
    public int health;
    public int startHealth = 100;

    private void Start()
    {
        health = startHealth;
    }

    [PunRPC]
    public void TakeDamage(int _d)
    {
        health -= _d;
        Debug.Log(" *** the player with tag: " + gameObject.tag + " \nhas a health value of: " + health);
        gameObject.GetComponent<Renderer>().material.color = Color.blue;

        if(health < 80)
            gameObject.GetComponent<Renderer>().material.color = Color.green;

        if (health < 60)
            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
    }

   


}
