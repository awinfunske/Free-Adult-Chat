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

namespace TPCEngine.Editor
{
	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(TPCamera))]
	[CanEditMultipleObjects]
	public class TPCameraEditor : TPCEditor
	{
        public override string HeaderName
        {
            get
            {
                return "Third Person Camera";
            }
        }
    }
}