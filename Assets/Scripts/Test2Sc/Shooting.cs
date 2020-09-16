using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Test2Sc
{
    public class Shooting : MonoBehaviour
    {
        [SerializeField]
        Camera fpsCamera;

        public float fireRate = 0.1f;
        float fireTimer;


        // Update is called once per frame
        void Update()
        {

            if (fireTimer < fireRate)
            {
                fireTimer += Time.deltaTime;
            }


            if (Input.GetButton("Fire1") && fireTimer > fireRate)
            {

                //Reset fireTimer
                fireTimer = 0.0f;


                RaycastHit _hit;
                Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));


                if (Physics.Raycast(ray, out _hit, 100))
                {

                    Debug.Log(_hit.collider.gameObject.name);

                    if (_hit.collider.gameObject.CompareTag("Player") && !_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("ShootingTakeDamage", RpcTarget.AllBuffered, 5);
                    }


                }


            }


        }
    }
}

// TODO checkout why if I shoot a player, and healthbar show red in a part, when returning to room it shows full
//Received RPC "ShootingTakeDamage" for viewID 2001 but this PhotonView does not exist! Was remote PV. Remote called. By: #01 'Unity' Maybe GO was destroyed but RPC not cleaned up.
//UnityEngine.DebugLogHandler:Internal_Log(LogType, LogOption, String, Object)
//UnityEngine.DebugLogHandler:LogFormat(LogType, Object, String, Object[])
//UnityEngine.Logger:Log(LogType, Object)
//UnityEngine.Debug:LogWarning(Object)
//Photon.Pun.PhotonNetwork:ExecuteRpc(Hashtable, Player)(at C: \Users\Hernando\Unity\Projects\Prototype - Hversion1\Assets\Photon\PhotonUnityNetworking\Code\PhotonNetworkPart.cs:396)
//Photon.Pun.PhotonNetwork:OnEvent(EventData)(at C: \Users\Hernando\Unity\Projects\Prototype - Hversion1\Assets\Photon\PhotonUnityNetworking\Code\PhotonNetworkPart.cs:2156)
//Photon.Realtime.LoadBalancingClient:OnEvent(EventData)(at C: \Users\Hernando\Unity\Projects\Prototype - Hversion1\Assets\Photon\PhotonRealtime\Code\LoadBalancingClient.cs:3142)
//ExitGames.Client.Photon.PeerBase:DeserializeMessageAndCallback(StreamBuffer)
//ExitGames.Client.Photon.EnetPeer:DispatchIncomingCommands()
//ExitGames.Client.Photon.PhotonPeer:DispatchIncomingCommands()
//Photon.Pun.PhotonHandler:Dispatch()(at C: \Users\Hernando\Unity\Projects\Prototype - Hversion1\Assets\Photon\PhotonUnityNetworking\Code\PhotonHandler.cs:208)
//Photon.Pun.PhotonHandler:FixedUpdate()(at C: \Users\Hernando\Unity\Projects\Prototype - Hversion1\Assets\Photon\PhotonUnityNetworking\Code\PhotonHandler.cs:142)