/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEditor;
using TPCEngine.UI;

namespace TPCEngine.Editor
{
    [CustomEditor(typeof(HUDManager))]
    public class HUDManagerEditor : TPCEditor
    {
        public override string HeaderName
        {
            get
            {
                return "HUD Manager";
            }
        }
    }
}