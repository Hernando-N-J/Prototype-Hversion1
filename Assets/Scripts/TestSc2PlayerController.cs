using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class TestSc2PlayerController: MonoBehaviourPun
{
    public int speed = 10;

    public int n;

    private void Start()
    {
        Debug.Log("Start playerPrefab photonview.IsMine = ** " + photonView.IsMine);
        Debug.Log("Start PlyrPrefab isConnected = ** " + PhotonNetwork.IsConnected);
    }

    void Update()
    {
         if(photonView.IsMine) Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Sphere Collider OnTriggerEnter");
            other.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void Move()
    {
        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.position += Movement * speed * Time.deltaTime;
    }

    [PunRPC]
    public void SetCubeColor(int playersCount)
    {
        if (playersCount == 1) this.GetComponent<Renderer>().material.color = Color.yellow;
        else if (playersCount == 2) this.GetComponent<Renderer>().material.color = Color.blue;
        else if (playersCount == 3) this.GetComponent<Renderer>().material.color = Color.green;
        else this.GetComponent<Renderer>().material.color = Color.red;
    }

    public void SendCubeColor(int pCount)
    {
        this.photonView.RPC("SetCubeColor", RpcTarget.All, pCount);
    }
}


