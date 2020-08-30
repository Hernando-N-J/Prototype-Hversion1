using UnityEngine;
using Photon.Pun;

namespace com.compA.gameA
{
    public class CameraWork : MonoBehaviourPun
    {
        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float camDistance = 7.0f;

        [Tooltip("The height we want the camera to be above the target")]
        [SerializeField]
        private float camHeight = 3.0f;

        [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
        [SerializeField]
        private Vector3 centerOffset = Vector3.zero;

        [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
        [SerializeField]
        private bool followOnStart = false;

        [Tooltip("The Smoothing for the camera to follow the target")]
        [SerializeField]
        private float smoothSpeed = 0.125f;

        private Transform cameraTransform;  // cached transform of the target

        private bool isFollowing;  // flag to reconnect if lost target or switched camera 

        private Vector3 cameraOffset = Vector3.zero;  // Cache for camera offset

        private void Start()
        {
            if (followOnStart) OnStartFollowing();
        }

        private void LateUpdate()
        {
            // The transform target may not destroy on level load,
            // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
            if (cameraTransform == null && isFollowing) OnStartFollowing();

            if (isFollowing) Follow();
        }

        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            
            // we don't smooth anything, we go straight to the right camera shot
            Cut();
        }

        private void Cut()
        {
            cameraOffset.z = -camDistance;
            cameraOffset.y = camHeight;

            cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

            cameraTransform.LookAt(this.transform.position + centerOffset);
        }

        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        private void Follow()
        {
            cameraOffset.z = -camDistance;
            cameraOffset.y = camHeight;

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

            cameraTransform.LookAt(this.transform.position + centerOffset);
        }
    }
}
