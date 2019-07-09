/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using Photon.Realtime;

namespace TPCEngine.Network
{
    /// <summary>
    /// ITeamCallbacks interface, that contains required functions for Team entity.
    /// </summary>
    public interface ITeamCallbacks
    {
        /// <summary>
        /// Add new player in the team.
        /// </summary>
        /// <param name="player"></param>
        void AddPlayer(Player player);

        /// <summary>
        /// Get player from this team.
        /// If not found returns empty null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Player GetPlayer(string name);

        /// <summary>
        /// Remove player from team by incstance.
        /// </summary>
        /// <param name="player"></param>
        void RemovePlayer(Player player);

        /// <summary>
        /// Remove player from team by name.
        /// </summary>
        /// <param name="name"></param>
        void RemovePlayer(string name);
    }
}