using UnityEngine;
using Photon.Pun;
//using Photon.Realtime;


public class TestSc2 : MonoBehaviourPun
{
    public TestScs2a t;

    private int speed = 10;

    private void Start()
    {
        Debug.Log("Start playerPrefab photonview.IsMine = ** " + photonView.IsMine);
        Debug.Log("Start PlyrPrefab isConnected = ** " + PhotonNetwork.IsConnected);
    }

    void Update()
    {
         if(photonView.IsMine) Move();
    }
    
    public void Move()
    {
        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.position += Movement * speed * Time.deltaTime;
    }

    //public void showInfo()
    //{
    //    Debug.Log("photonview.IsMine = ** " + photonView.IsMine);
    //    t.InstantiatePlayer();
    //}
}
