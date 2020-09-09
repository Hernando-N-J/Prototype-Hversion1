using UnityEngine;
using Photon.Pun;

public class T2DetectEnemies : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z) && !photonView.IsMine)
            {
                other.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 10);
                
            }
        }
    }

    // PhotonNetwork.Destroy  --- DestroyPlayerObjects


    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player") && !photonView.IsMine)
    //    {
    //        other.GetComponent<Renderer>().material.color = Color.red;

    //        if (Input.GetKeyDown(KeyCode.Z))
    //            other.GetComponent<Renderer>().material.color = Color.blue;

    //        if (Input.GetKeyDown(KeyCode.X))
    //            other.GetComponent<Renderer>().material.color = Color.yellow;

    //        if (Input.GetKeyDown(KeyCode.C)) other.gameObject.SetActive(false);

    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<Renderer>().material.color = Color.yellow;
    }


}
