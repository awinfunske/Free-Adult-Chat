/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;

namespace TPCEngine.Editor
{
	[CustomEditor(typeof(TPGrabSystem))]
	[CanEditMultipleObjects]
	public class TPGrabSystemEditor : TPCEditor
	{
        public override string HeaderName
        {
            get
            {
                return "TPGrab System";
            }
        }
    }
}