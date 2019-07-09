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
using Photon.Realtime;
using System.Collections.Generic;
using TPCEngine.Utility;

namespace TPCEngine.Network
{
    public class TeamManager : Singleton<TeamManager>
    {
        #region [Require Components]
        private Team teamA = new Team("None", new List<Player>());
        private Team teamB = new Team("None", new List<Player>());
        #endregion

        #region [Functions]
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Add player in the team.
        /// Automatically distributes the player.
        /// </summary>
        /// <param name="player"></param>
        public virtual void AddPlayer(Player player)
        {
            if (teamA.PlayersCount == teamB.PlayersCount)
            {
                int teamNumber = Random.Range(0, 2);

                if (teamNumber == 0)
                    teamA.AddPlayer(player);
                else if (teamNumber == 1)
                    teamB.AddPlayer(player);
            }
            else if (teamB.PlayersCount > teamA.PlayersCount)
            {
                teamA.AddPlayer(player);
            }
            else if (teamA.PlayersCount > teamB.PlayersCount)
            {
                teamB.AddPlayer(player);
            }
        }

        /// <summary>
        /// Disconnect player from room and remove from team.
        /// Can called only on master client.
        /// </summary>
        /// <param name="player"></param>
        public virtual void DisconnectPlayer(Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Only master(admin) can remove player from the room.");
                return;
            }

            if (teamA.Players.Contains(player))
                teamA.RemovePlayer(player);
            else if (teamB.Players.Contains(player))
                teamB.Players.Remove(player);

            PhotonNetwork.CloseConnection(player);
        }

        public virtual int ContainsPlayer(Player player)
        {
            if (teamA.Players.Contains(player))
                return 1;
            else if (teamB.Players.Contains(player))
                return 2;
            return 0;
        }

        /// <summary>
        /// Disconnect player from room and remove from team.
        /// Can called only on master client.
        /// </summary>
        /// <param name="player"></param>
        public virtual void RemovePlayer(Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Only master(admin) can remove player from the room.");
                return;
            }

            if (teamA.Players.Contains(player))
                teamA.RemovePlayer(player);
            else if (teamB.Players.Contains(player))
                teamB.Players.Remove(player);
        }
        #endregion

        #region [Properties]
        /// <summary>
        /// Get team A
        /// </summary>
        public Team TeamA
        {
            get
            {
                return teamA;
            }

            set
            {
                teamA = value;
            }
        }

        /// <summary>
        /// Get team B
        /// </summary>
        public Team TeamB
        {
            get
            {
                return teamB;
            }

            set
            {
                teamB = value;
            }
        }
        #endregion
    }
}