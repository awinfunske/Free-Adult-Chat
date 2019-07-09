/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;
using TPCEngine.Utility;

namespace TPCEngine
{
    public sealed class SurfaceHelper
    {
        public static Object GetSurfaceType(Collider collider, Vector3 contactPos)
        {
            Terrain terrain = collider.GetComponent<Terrain>();
            PhysicMaterial physicMaterial = collider.GetComponent<Collider>().sharedMaterial;
            if(terrain)
            {
                TerrainTextureDetector terrainTextureDetector = new TerrainTextureDetector(terrain.terrainData);
                Texture2D texture = terrainTextureDetector.GetActiveTexture(contactPos);
                return texture;
            }
            return physicMaterial;
        }
    }
}