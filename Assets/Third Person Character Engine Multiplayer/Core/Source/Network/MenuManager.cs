/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace TPCEngine.Network
{
    #region [Network Menu Datas]
    /// <summary>
    /// Sign up UI elements
    /// </summary>
    [Serializable]
    public struct SignUpData
    {
        public RectTransform signUpWindow;
        public InputField login;
        public InputField password;
    }

    /// <summary>
    /// Lobby UI elements
    /// </summary>
    [Serializable]
    public struct LobbyData
    {
        public RectTransform lobbyWindow;
        public RectTransform createRoomWindow;
        public RectTransform joinRoomWindow;
        public RectTransform roomWindow;
    }

    /// <summary>
    /// Room settings UI elements
    /// </summary>
    [Serializable]
    public struct RoomData
    {
        public InputField roomName;
        public Dropdown levelID;
        public InputField teamA;
        public InputField teamB;
        public InputField maxPlayer;
        public InputField minPlayer;
        public InputField playerTTL;
        public InputField roomTTL;
        public InputField expectedUsers;
        public Toggle isPrivateRoom;
        public Toggle isVisibleRoom;
        public Toggle publishUserID;
    }

    /// <summary>
    /// Runtime room UI Elements
    /// </summary>
    [Serializable]
    public struct RuntimeRoomData
    {
        public Text roomName;
        public Text teamA;
        public Text teamB;
        public Text messageArea;
        public Button startGame;
    }

    /// <summary>
    /// Display message window.
    ///    Used to call messages to the screen.
    /// </summary>
    [Serializable]
    public struct DisplayMessageData
    {
        public RectTransform displayMessageWindow;
        public Text message;
    }
    #endregion

    /// <summary>
    /// Menu manager, this class contains all menu data elements (sing up, lobbly and ect.).
    /// </summary>
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        #region [Edit in Inspector Components]
        [Header("---Templates---")]
        [SerializeField] private GameObject matchTemplate;
        [SerializeField] private GameObject playerTemplate;
        [SerializeField] private GameObject masterPlayerTemplate;

        [Header("---Contents---")]
        [SerializeField] private RectTransform roomListContent;
        [SerializeField] private RectTransform teamAListContent;
        [SerializeField] private RectTransform teamBListContent;

        [Header("---Datas---")]
        [SerializeField] private SignUpData signUpData;
        [SerializeField] private LobbyData lobbyData;
        [SerializeField] private RoomData roomData;
        [SerializeField] private RuntimeRoomData runtimeRoomData;
        [SerializeField] private DisplayMessageData displayMessage;
        #endregion

        #region [Require Components]
        private string joinRoomName;
        private short codeMessage = -1;
        private IEnumerator displayMessageCoroutine;
        private Hashtable _PhotonHashtable;
        #endregion

        #region [Functions]
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        protected virtual void Start()
        {
            ActivateWindow("SingUp");

            //Init Hashtable
            _PhotonHashtable = new Hashtable();
            _PhotonHashtable.Add("Level ID", null);
            _PhotonHashtable.Add("Team A Name", null);
            _PhotonHashtable.Add("Team B Name", null);
            _PhotonHashtable.Add("Team A Players", null);
            _PhotonHashtable.Add("Team B Players", null);
            _PhotonHashtable.Add("Chat Cache", null);
        }

        /// <summary>
        /// Called after the connection to the master is established and authenticated but only when PhotonNetwork.autoJoinLobby is false.
        /// </summary>
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        /// <summary>
        /// Called on entering a lobby on the Master Server.
        /// </summary>
        public override void OnJoinedLobby()
        {
            ActivateWindow("Lobby");
        }

        /// <summary>
        /// Called when this client created a room and entered it.
        /// </summary>
        public override void OnJoinedRoom()
        {
            ActivateWindow("Lobby.Room");

            if (PhotonNetwork.IsMasterClient)
            {
                TeamManager.Instance.AddPlayer(PhotonNetwork.LocalPlayer);
                _PhotonHashtable["Team A Name"] = TeamManager.Instance.TeamA.Name;
                _PhotonHashtable["Team B Name"] = TeamManager.Instance.TeamB.Name;
                _PhotonHashtable["Level ID"] = roomData.levelID.options[roomData.levelID.value].text;
                PhotonNetwork.CurrentRoom.SetCustomProperties(_PhotonHashtable);

                runtimeRoomData.startGame.enabled = true;
                runtimeRoomData.startGame.transform.GetChild(0).GetComponent<Text>().text = "Start Game";
            }
            else
            {
                runtimeRoomData.startGame.enabled = false;
                runtimeRoomData.startGame.transform.GetChild(0).GetComponent<Text>().text = "Start Game (Only Master)";
            }

            //Load last chat messages
            runtimeRoomData.messageArea.text = PhotonNetwork.CurrentRoom.CustomProperties["Chat Cache"] as string;

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
            runtimeRoomData.roomName.text = PhotonNetwork.CurrentRoom.Name;
            runtimeRoomData.teamA.text = TeamManager.Instance.TeamA.Name;
            runtimeRoomData.teamB.text = TeamManager.Instance.TeamB.Name;
            GeneratePlayerUIList();
        }

        /// <summary>
        /// Called when a remote player entered the room. 
        /// This Player is already added to the playerlist.
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            TeamManager.Instance.AddPlayer(newPlayer);

            SendMessageInRoom("New player \"" + newPlayer.NickName + "\" conneted in the room");

            if (PhotonNetwork.IsMasterClient)
            {
                _PhotonHashtable["Team A Players"] = TeamManager.Instance.TeamA.Players.ToArray();
                _PhotonHashtable["Team B Players"] = TeamManager.Instance.TeamB.Players.ToArray();
                PhotonNetwork.CurrentRoom.SetCustomProperties(_PhotonHashtable);
            }


            int minPlayerCount = int.Parse(roomData.minPlayer.text);
            int maxPlayerCount = int.Parse(roomData.maxPlayer.text);
            runtimeRoomData.startGame.enabled = (PhotonNetwork.PlayerList.Length >= minPlayerCount);

            if (PhotonNetwork.PlayerList.Length == maxPlayerCount)
                PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        /// <summary>
        /// Called for any update of the room-listing while in a lobby (InLobby) on the Master Server.
        /// </summary>
        /// <param name="roomList"></param>
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            //Clear room list for update
            int rooms = roomListContent.childCount;
            for (int i = 0; i < rooms; i++)
            {
                Destroy(roomListContent.GetChild(i).gameObject);
            }

            //Add new rooms
            for (int i = 0; i < roomList.Count; i++)
            {
                GameObject room = Instantiate(matchTemplate, roomListContent);
                room.name = roomList[i].Name;
                room.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                room.transform.FindChild("Match Name").GetComponent<Text>().text = roomList[i].Name;
                room.transform.FindChild("Players").GetComponent<Text>().text = roomList[i].PlayerCount + "/" + roomList[i].MaxPlayers;
                room.transform.FindChild("Ping").GetComponent<Text>().text = Random.Range(0, 100).ToString();
                room.transform.GetComponent<Button>().onClick.AddListener(() => OnSetJoinRoomName(room.transform.FindChild("Match Name").GetComponent<Text>()));
            }
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
        /// Called after leaving a lobby.
        /// </summary>
        public override void OnLeftLobby()
        {
            ActivateWindow("SignUp");
        }

        /// <summary>
        /// Called when the local user/client left a room.
        /// </summary>
        public override void OnLeftRoom()
        {
            if (TeamManager.Instance.TeamA.Players.Contains(PhotonNetwork.LocalPlayer))
                TeamManager.Instance.TeamA.RemovePlayer(PhotonNetwork.LocalPlayer);
            else if (TeamManager.Instance.TeamA.Players.Contains(PhotonNetwork.LocalPlayer))
                TeamManager.Instance.TeamA.Players.Remove(PhotonNetwork.LocalPlayer);

            ActivateWindow("Lobby");
        }

        /// <summary>
        /// Called when a CreateRoom() call failed. Optional parameters provide ErrorCode and message.
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            ActivateWindow("Lobby");
            DisplayMessageOnScreen(message + "\n" + "Code: " + returnCode);
        }

        /// <summary>
        /// Called when a JoinRoom() call failed. Optional parameters provide ErrorCode and message.
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            ActivateWindow("Lobby");
            DisplayMessageOnScreen(message + "\n" + "Code: " + returnCode);
        }

        /// <summary>
        /// Open specific window
        /// </summary>
        /// <param name="window"></param>
        public void ActivateWindow(string window)
        {
            switch (window)
            {
                case "SignUp":
                    signUpData.signUpWindow.gameObject.SetActive(true);
                    lobbyData.lobbyWindow.gameObject.SetActive(false);
                    lobbyData.roomWindow.gameObject.SetActive(false);
                    break;
                case "Lobby":
                    lobbyData.lobbyWindow.gameObject.SetActive(true);
                    signUpData.signUpWindow.gameObject.SetActive(false);
                    lobbyData.roomWindow.gameObject.SetActive(false);
                    break;
                case "Lobby.CreateRoom":
                    lobbyData.createRoomWindow.gameObject.SetActive(true);
                    lobbyData.joinRoomWindow.gameObject.SetActive(false);
                    lobbyData.roomWindow.gameObject.SetActive(false);
                    break;
                case "Lobby.JoinRoom":
                    lobbyData.joinRoomWindow.gameObject.SetActive(true);
                    lobbyData.createRoomWindow.gameObject.SetActive(false);
                    lobbyData.roomWindow.gameObject.SetActive(false);
                    break;
                case "Lobby.Room":
                    lobbyData.roomWindow.gameObject.SetActive(true);
                    lobbyData.lobbyWindow.gameObject.SetActive(false);
                    lobbyData.joinRoomWindow.gameObject.SetActive(false);
                    lobbyData.createRoomWindow.gameObject.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// Generate random nick name.
        ///     Default type: (Player ####) where is #### it's range from 1000 to 9999.
        /// </summary>
        /// <returns></returns>
        public string GenerateNickName()
        {
            return "Player " + Random.Range(1000, 9999);
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

        /// <summary>
        /// Display messages to the screen.
        /// </summary>
        /// <param name="message">Display message</param>
        /// <param name="time">Display time in seconds</param>
        public void DisplayMessageOnScreen(string message, float time = 3.0f)
        {
            displayMessage.message.text = message;
            if (displayMessageCoroutine != null)
                StopCoroutine(displayMessageCoroutine);
            displayMessageCoroutine = DisplayMessageOnScreenCoroutine(time);
            StartCoroutine(DisplayMessageOnScreenCoroutine(time));
        }

        /// <summary>
        /// DisplayMessageOnScreen coroutine
        /// </summary>
        /// <param name="time">Display time in seconds</param>
        /// <returns></returns>
        protected virtual IEnumerator DisplayMessageOnScreenCoroutine(float time)
        {
            displayMessage.displayMessageWindow.gameObject.SetActive(true);
            yield return new WaitForSeconds(time);
            displayMessage.displayMessageWindow.gameObject.SetActive(false);
            displayMessage.message.text = "None.";
            yield break;
        }
        #endregion

        #region [PunRPC Callbacks]
        /// <summary>
        /// RPC Attribute
        /// Set message in room message area
        /// Automatically translates to a new line.
        /// </summary>
        /// <param name="message"></param>
        [PunRPC]
        public void SendMessageInRoom(string message)
        {
            runtimeRoomData.messageArea.text += message + "\n";
        }
        #endregion

        #region [UI Callbacks]
        /// <summary>
        /// Connent to Photon
        /// </summary>
        public virtual void OnConnentToPhoton()
        {
            PhotonNetwork.GameVersion = Network.GameVersion;
            PhotonNetwork.NickName = (!string.IsNullOrEmpty(signUpData.login.text)) ? signUpData.login.text : GenerateNickName();
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// Create room with specific room options.
        /// </summary>
        public virtual void OnCreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = (byte)int.Parse(roomData.maxPlayer.text),
                IsOpen = roomData.isPrivateRoom.enabled,
                IsVisible = roomData.isVisibleRoom,
                PlayerTtl = int.Parse(roomData.playerTTL.text),
                EmptyRoomTtl = int.Parse(roomData.roomTTL.text),
                PublishUserId = roomData.publishUserID.enabled
            };

            string roomName = (!string.IsNullOrEmpty(roomData.roomName.text)) ? roomData.roomName.text : "UnNamedRoom - " + Random.Range(0, 9999);
            string[] expectedUsers = (roomData.expectedUsers.text != "") ? roomData.expectedUsers.text.Split(',') : null;

            TeamManager.Instance.TeamA = new Team(roomData.teamA.text, new List<Player>());
            TeamManager.Instance.TeamB = new Team(roomData.teamB.text, new List<Player>());

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default, expectedUsers);
        }

        /// <summary>
        /// Join in specific room.
        /// </summary>
        public virtual void OnJoinRoom()
        {
            PhotonNetwork.JoinRoom(joinRoomName);
        }

        /// <summary>
        /// Set name for joining room.
        /// </summary>
        /// <param name="joinRoomName"></param>
        public virtual void OnSetJoinRoomName(Text joinRoomName)
        {
            this.joinRoomName = joinRoomName.text;
        }

        /// <summary>
        /// Joins any available room of the currently used lobby and fails if none is available.
        /// </summary>
        public virtual void OnQuickMatch()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        /// <summary>
        /// Load game level.
        /// </summary>
        public virtual void OnLoadLevel()
        {
            string levelID = PhotonNetwork.CurrentRoom.CustomProperties["Level ID"] as string;
            PhotonNetwork.LoadLevel(levelID);
        }

        /// <summary>
        /// Leave the current room and return to the Master Server where you can join or create rooms (see remarks).
        /// </summary>
        public virtual void OnLeaveRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        /// <summary>
        /// Makes this client disconnect from the photon server.
        /// </summary>
        public virtual void OnLeaveLobby()
        {
            PhotonNetwork.Disconnect();
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
            _PhotonHashtable["Chat Cache"] = runtimeRoomData.messageArea.text;
            PhotonNetwork.CurrentRoom.SetCustomProperties(_PhotonHashtable);
        }

        /// <summary>
        /// Simulate click on first room from room list
        /// </summary>
        public void ChooseFirstRoom()
        {
            if (roomListContent.childCount == 0)
                return;

            Button button = roomListContent.GetChild(0).GetComponent<Button>();
            if (button != null)
                button.onClick.Invoke();
        }
        #endregion

        #region [Properties]
        public SignUpData _SignUpData
        {
            get
            {
                return signUpData;
            }

            set
            {
                signUpData = value;
            }
        }

        public LobbyData _LobbyData
        {
            get
            {
                return lobbyData;
            }

            set
            {
                lobbyData = value;
            }
        }

        public RoomData _RoomData
        {
            get
            {
                return roomData;
            }

            set
            {
                roomData = value;
            }
        }

        public RuntimeRoomData _RuntimeRoomData
        {
            get
            {
                return runtimeRoomData;
            }

            set
            {
                runtimeRoomData = value;
            }
        }

        public DisplayMessageData _DisplayMessage
        {
            get
            {
                return displayMessage;
            }

            set
            {
                displayMessage = value;
            }
        }
        #endregion
    }
}