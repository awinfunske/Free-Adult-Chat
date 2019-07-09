/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TPCEngine.Utility;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace TPCEngine.Network
{
    /// <summary>
    /// Player information UI elements
    /// </summary>
    public struct PlayerInfo
    {
        public Text name;
        public Text killCount;
        public Text deathCount;
        public Text score;
        public Text ping;
    }

    [System.Serializable]
    public struct RuntimeMenu
    {
        public RectTransform mainWindow;
        public RectTransform staticsWindow;
    }

    public class RuntimeMenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] Transform freeCamera;
        [SerializeField] private RuntimeMenu runtimeMenu;
        [SerializeField] private Transform playerInstance;
        [SerializeField] private TPCamera cameraInstance;
        [SerializeField] private Transform teamA;
        [SerializeField] private Transform teamB;

        [Header("Other UI Components")]
        [SerializeField] private Text teamATitle;
        [SerializeField] private Text teamBTitle;
        [SerializeField] private Text messageArea;

        [Header("---Templates---")]
        [SerializeField] private GameObject playerTemplate;
        [SerializeField] private GameObject masterPlayerTemplate;

        [Header("---Contents---")]
        [SerializeField] private RectTransform teamAListContent;
        [SerializeField] private RectTransform teamBListContent;

        private Hashtable _PhotonHashtable;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        protected virtual void Start()
        {
            runtimeMenu.mainWindow.gameObject.SetActive(true);
            _PhotonHashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            messageArea.text = _PhotonHashtable["Chat Cache"] as string;
        }

        /// <summary>
        /// Called when a remote player entered the room. 
        /// This Player is already added to the playerlist.
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            TeamManager.Instance.AddPlayer(newPlayer);

            //SendMessageInRoom("New player \"" + newPlayer.NickName + "\" conneted in the room");

            if (PhotonNetwork.IsMasterClient)
            {
                _PhotonHashtable["Team A Players"] = TeamManager.Instance.TeamA.Players.ToArray();
                _PhotonHashtable["Team B Players"] = TeamManager.Instance.TeamB.Players.ToArray();
                PhotonNetwork.CurrentRoom.SetCustomProperties(_PhotonHashtable);
            }
        }

        /// <summary>
        /// Called when a room's custom properties changed. The propertiesThatChanged contains all that was set via Room.SetCustomProperties.
        /// </summary>
        /// <param name="propertiesThatChanged"></param>
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                TeamManager.Instance.TeamA.Name = propertiesThatChanged["Team A Name"] as string;
                TeamManager.Instance.TeamB.Name = propertiesThatChanged["Team B Name"] as string;
            }
            Player[] playersA = propertiesThatChanged["Team A Players"] as Player[];
            Player[] playersB = propertiesThatChanged["Team B Players"] as Player[];
            if (playersA != null)
            {
                TeamManager.Instance.TeamA.Players.Clear();
                for (int i = 0; i < playersA.Length; i++)
                {
                    TeamManager.Instance.TeamA.Players.Add(playersA[i]);
                }
            }
            if (playersA != null)
            {
                TeamManager.Instance.TeamB.Players.Clear();
                for (int i = 0; i < playersB.Length; i++)
                {
                    TeamManager.Instance.TeamB.Players.Add(playersB[i]);
                }
            }
            teamATitle.text = TeamManager.Instance.TeamA.Name;
            teamBTitle.text = TeamManager.Instance.TeamB.Name;
            GeneratePlayerUIList();
        }

        /// <summary>
        /// Called when a remote player left the room. This PhotonPlayer is already removed from the playerlist at this time.
        /// </summary>
        /// <param name="otherPlayer"></param>
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                TeamManager.Instance.RemovePlayer(otherPlayer);
                _PhotonHashtable["Team A Players"] = TeamManager.Instance.TeamA.Players.ToArray();
                _PhotonHashtable["Team B Players"] = TeamManager.Instance.TeamB.Players.ToArray();
                PhotonNetwork.CurrentRoom.SetCustomProperties(_PhotonHashtable);
            }
        }


        /// <summary>
        /// Add new player on UI List
        /// </summary>
        /// <param name="player"></param>
        public virtual void AddPlayerOnUIList(Player player)
        {
            string team = "None";

            if (TeamManager.Instance.TeamA.Players.Contains(player))
                team = "A";
            else if (TeamManager.Instance.TeamB.Players.Contains(player))
                team = "B";

            GameObject playerUI = Instantiate((PhotonNetwork.IsMasterClient) ? masterPlayerTemplate : playerTemplate);
            playerUI.transform.SetParent((team == "A") ? teamAListContent : teamBListContent);
            playerUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            playerUI.name = player.NickName;
            playerUI.transform.FindChild("Name").GetComponent<Text>().text = player.NickName;
            if (PhotonNetwork.LocalPlayer == player)
                playerUI.transform.FindChild("Name").GetComponent<Text>().fontStyle = FontStyle.Bold;
            playerUI.transform.FindChild("Ping").GetComponent<Text>().text = Random.Range(0, 100).ToString();
            if (PhotonNetwork.IsMasterClient)
                playerUI.transform.FindChild("Kick").GetComponent<Button>().onClick.AddListener(() => TeamManager.Instance.DisconnectPlayer(player));
        }

        /// <summary>
        /// Generate player UI lists by Team Manager instance 
        /// </summary>
        public virtual void GeneratePlayerUIList()
        {
            //Clear team A list before update
            for (int i = 0; i < teamAListContent.childCount; i++)
                Destroy(teamAListContent.GetChild(i).gameObject);

            //Clear team B list before update
            for (int i = 0; i < teamBListContent.childCount; i++)
                Destroy(teamBListContent.GetChild(i).gameObject);

            //Generate team A list
            for (int i = 0; i < TeamManager.Instance.TeamA.Players.Count; i++)
                AddPlayerOnUIList(TeamManager.Instance.TeamA.Players[i]);

            //Generate team B list
            for (int i = 0; i < TeamManager.Instance.TeamB.Players.Count; i++)
                AddPlayerOnUIList(TeamManager.Instance.TeamB.Players[i]);
        }

        [PunRPC]
        public void SendMessageInRoom(string message)
        {
            messageArea.text += message + "\n";
        }

        #region [UI CallBacks]
        public void SpawnPlayer()
        {
            int teamID = TeamManager.Instance.ContainsPlayer(PhotonNetwork.LocalPlayer);
            GameObject player;
            switch (teamID)
            {
                case 1:
                    player = PhotonNetwork.Instantiate(playerInstance.name, teamA.position.RandomPositionInCircle(2), Quaternion.identity);
                    cameraInstance.Target = player.transform;
                    cameraInstance.gameObject.SetActive(true);
                    freeCamera.gameObject.SetActive(false);
                    runtimeMenu.mainWindow.gameObject.SetActive(false);
                    break;
                case 2:
                    player = PhotonNetwork.Instantiate(playerInstance.name, teamB.position.RandomPositionInCircle(2), Quaternion.identity);
                    cameraInstance.Target = player.transform;
                    cameraInstance.gameObject.SetActive(true);
                    freeCamera.gameObject.SetActive(false);
                    runtimeMenu.mainWindow.gameObject.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// RPC Set message in room message area
        /// Automatically translates to a new line.
        /// </summary>
        /// <param name="name"></param>
        public void OnSendMessageInRoom(string message)
        {
            string _ClientMessagePrefix = PhotonNetwork.NickName + ": ";
            string _MasterMessagePrefix = "(Master)" + PhotonNetwork.NickName + ": ";
            message = (!PhotonNetwork.IsMasterClient) ? _ClientMessagePrefix + message : _MasterMessagePrefix + message;
            photonView.RPC("SendMessageInRoom", RpcTarget.All, message);
            _PhotonHashtable["Chat Cache"] = messageArea.text;
            PhotonNetwork.CurrentRoom.SetCustomProperties(_PhotonHashtable);
        }
        #endregion

        public RuntimeMenu RuntimeMenu
        {
            get
            {
                return runtimeMenu;
            }

            set
            {
                runtimeMenu = value;
            }
        }

        public Transform FreeCamera
        {
            get
            {
                return freeCamera;
            }

            set
            {
                freeCamera = value;
            }
        }

        public TPCamera CameraInstance
        {
            get
            {
                return cameraInstance;
            }

            set
            {
                cameraInstance = value;
            }
        }
    }
}