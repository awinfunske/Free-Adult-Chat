/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using Photon.Realtime;
using System.Collections.Generic;

namespace TPCEngine.Network
{
    /// <summary>
    /// Entity of the team.
    /// </summary>
    public class Team : ITeamCallbacks
    {
        private string name;
        private List<Player> players;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="players"></param>
        public Team(string name, List<Player> players)
        {
            this.name = name;
            if (players != null)
                this.players = players;

        }

        #region [Functions]
        /// <summary>
        /// Add new player in the team.
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        /// <summary>
        /// Get player from this team.
        /// If not found returns empty null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Player GetPlayer(string name)
        {
            for (int i = 0; i < players.Count; i++)
                if (players[i].NickName == name)
                    return players[i];
            return null;
        }

        /// <summary>
        /// Remove player from team by incstance.
        /// </summary>
        /// <param name="player"></param>
        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }

        /// <summary>
        /// Remove player from team by name.
        /// </summary>
        /// <param name="name"></param>
        public void RemovePlayer(string name)
        {
            for (int i = 0; i < players.Count; i++)
                if (players[i].NickName == name)
                    players.Remove(players[i]);
        }
        #endregion

        #region [Properties]
        /// <summary>
        /// Team name
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Player count in this team
        /// </summary>
        public int PlayersCount { get { return players.Count; } }

        public List<Player> Players
        {
            get
            {
                return players;
            }

            set
            {
                players = value;
            }
        }
        #endregion
    }
}