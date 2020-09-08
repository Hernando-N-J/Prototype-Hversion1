using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TestSc2PlayerController : MonoBehaviourPun
{
    public int speed = 10;

    Color syncColor = Color.grey;

    private void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.tag = "Local Player";
            Debug.Log("Tag is:" + gameObject.tag);
        }

        Debug.Log("Start playerPrefab photonview.IsMine = ** " + photonView.IsMine);
        Debug.Log("Start PlyrPrefab isConnected = ** " + PhotonNetwork.IsConnected);

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Move();

            gameObject.GetComponent<Renderer>().material.color = syncColor;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                syncColor = gameObject.GetComponent<Renderer>().material.color = Color.magenta;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                syncColor = gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            }
        }
    }

    public void Move()
    {
        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.position += Movement * speed * Time.deltaTime;
    }
}

 
  



