/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace TPCEngine.Utility
{
    public class TerrainTextureDetector
    {
        private TerrainData terrainData;
        private int alphamapWidth;
        private int alphamapHeight;
        private float[,,] splatmapData;
        private int numTextures;

        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        /// <remarks>
        /// Constructor with TerrainData parameter for detect texture from custom terran.
        /// </remarks>
        /// <param name="terrainData"></param>
        public TerrainTextureDetector(TerrainData terrainData)
        {
            this.terrainData = terrainData;
            alphamapWidth = this.terrainData.alphamapWidth;
            alphamapHeight = this.terrainData.alphamapHeight;

            splatmapData = this.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
            numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        /// <remarks>
        /// Constructor created TerrainTextureDetector with active terrainData.
        /// </remarks>
        public TerrainTextureDetector()
        {
            terrainData = Terrain.activeTerrain.terrainData;
            alphamapWidth = terrainData.alphamapWidth;
            alphamapHeight = terrainData.alphamapHeight;

            splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
            numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
        }

        /// <summary>
        /// Convert world position to splat map coordinate.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition)
        {
            Vector3 splatPosition = new Vector3();
            Terrain ter = Terrain.activeTerrain;
            Vector3 terPosition = ter.transform.position;
            splatPosition.x = ((worldPosition.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
            splatPosition.z = ((worldPosition.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
            return splatPosition;
        }

        /// <summary>
        /// Get active texture id.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Active terrain texture index.</returns>
        public int GetActiveTextureId(Vector3 position)
        {
            Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
            int activeTerrainIndex = 0;
            float largestOpacity = 0.0f;

            for (int i = 0; i < numTextures; i++)
            {
                if (largestOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
                {
                    activeTerrainIndex = i;
                    largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
                }
            }
            return activeTerrainIndex;
        }

        /// <summary>
        /// Get active texture instance. 
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Active texture instance.</returns>
        public Texture2D GetActiveTexture(Vector3 position)
        {
            Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
            int activeTerrainIndex = 0;
            float largestOpacity = 0.0f;

            for (int i = 0; i < numTextures; i++)
            {
                if (largestOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
                {
                    activeTerrainIndex = i;
                    largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
                }
            }
            return terrainData.splatPrototypes[activeTerrainIndex].texture ?? null;
        }
    }
}