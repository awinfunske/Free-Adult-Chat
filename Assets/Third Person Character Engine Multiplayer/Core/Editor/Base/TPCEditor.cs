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
    /// <summary>
    /// Base inspector editor class for TPC Engine components.
    /// </summary>
    public class TPCEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Custom Inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BeginBackground();
            Header(HeaderName);
            BeginBox();
            BeforeBaseGUI();
            base.OnInspectorGUI();
            AfterBaseGUI();
            EndBox();
            EndBackground();
        }

        /// <summary>
        /// Called before OnInspectorGUI().
        /// </summary>
        public virtual void BeforeBaseGUI()
        {

        }

        /// <summary>
        /// Called after OnInspectorGUI().
        /// </summary>
        public virtual void AfterBaseGUI()
        {

        }

        /// <summary>
        /// Header name.
        /// </summary>
        public virtual string HeaderName
        {
            get
            {
                return GetType().Name;
            }
        }

        /// <summary>
        /// Header GUI.
        /// </summary>
        /// <param name="message"></param>
        public virtual void Header(string message)
        {
            GUI.color = Color.white;
            GUILayout.BeginVertical(GUI.skin.button);
            EditorGUILayout.LabelField(message, HeaderText);
            GUILayout.EndVertical();
        }
        
        /// <summary>
        /// Begin background GUI.
        /// </summary>
        public virtual void BeginBackground()
        {
            GUILayout.Space(7);
            GUI.color = TPCEPreferences.EditorData.BackgroundColor;
            GUILayout.BeginVertical(GUI.skin.window, GUILayout.Height(1));
        }
        
        /// <summary>
        /// End background GUI.
        /// </summary>
        public virtual void EndBackground()
        {
            GUILayout.EndVertical();
            GUILayout.Space(7);
        }

        /// <summary>
        /// Begin box GUI.
        /// </summary>
        public virtual void BeginBox()
        {
            GUI.color = TPCEPreferences.EditorData.BoxColor;
            GUILayout.BeginVertical(GUI.skin.button);
            GUILayout.Space(7);
        }

        /// <summary>
        /// End box GUI.
        /// </summary>
        public virtual void EndBox()
        {
            GUILayout.Space(7);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Header text GUI style.
        /// </summary>
        /// <returns></returns>
        public virtual GUIStyle HeaderText
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

		/// <summary>
        /// Mini title GUI style.
        /// </summary>
        /// <returns></returns>
        public virtual GUIStyle MiniTitleText
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 11;
				style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }
    }
}