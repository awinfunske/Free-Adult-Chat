/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;
using Photon.Pun;
using System.Collections;

namespace TPCEngine.Network
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerNetworkInstance : MonoBehaviour, IPunObservable
    {
        private const string REMOTELAYER = "Remote Body";

        [Range(0, 10)] [SerializeField] private float syncSmooth = 0.1f;
        [SerializeField] private float timeBeforeRespawn;
        [SerializeField] private Behaviour[] components;

        private Vector3 playerPos;
        private Quaternion playerRot;
        private PhotonView photonView;
        private IHealth health;
        private RuntimeMenuManager runtimeMenuManager;

        protected virtual void Awake()
        {
            photonView = GetComponent<PhotonView>();
            health = GetComponent<IHealth>();
            runtimeMenuManager = FindObjectOfType<RuntimeMenuManager>();
            if (photonView.IsMine)
            {
                RuntimeMenuManager runtimeMenuManager = FindObjectOfType<RuntimeMenuManager>();
                
                EnableOwn(components);
                if (!runtimeMenuManager)
                {
                    GetComponent<TPCharacter>().SetCamera(runtimeMenuManager.CameraInstance);
                    GetComponent<TPCharacter>().GetInverseKinematics().LookTarget = runtimeMenuManager.CameraInstance.GetComponentInChildren<Transform>(true);
                    runtimeMenuManager.CameraInstance.GetComponent<FPCameraPostProcessing>()._CharacterHealth = GetComponent<CharacterHealth>();

                }
                gameObject.AddComponent<StandaloneController>();
            }

        }

        protected virtual void Update()
        {
            if (!photonView.IsMine)
            {
                transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime * syncSmooth);
                transform.rotation = Quaternion.Lerp(transform.rotation, playerRot, Time.deltaTime * syncSmooth);
            }
            else
            {
                if (!health.IsAlive)
                    StartCoroutine(RemovePlayer(timeBeforeRespawn));
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                playerPos = (Vector3)stream.ReceiveNext();
                playerRot = (Quaternion)stream.ReceiveNext();
            }
        }

        public virtual void EnableOwn(Behaviour[] components)
        {
            for (int i = 0; i < components.Length; i++)
                components[i].enabled = true;
        }

        public IEnumerator RemovePlayer(float time)
        {
            yield return new WaitForSeconds(time);
            runtimeMenuManager.RuntimeMenu.mainWindow.gameObject.SetActive(true);
            runtimeMenuManager.FreeCamera.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}