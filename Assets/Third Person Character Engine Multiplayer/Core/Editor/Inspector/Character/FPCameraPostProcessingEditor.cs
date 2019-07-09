/* ================================================================
   ---------------------------------------------------
   Project   :    TPC Engine
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEditor;

namespace TPCEngine.Editor
{
    [CustomEditor(typeof(FPCameraPostProcessing))]
    [CanEditMultipleObjects]
    public class FPCameraPostProcessingEditor : TPCEditor
    {
        public override string HeaderName
        {
            get
            {
                return "FPCamera Post Processing";
            }
        }
    }
}