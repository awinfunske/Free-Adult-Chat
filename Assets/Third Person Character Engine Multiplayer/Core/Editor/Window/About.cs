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
	public class About : EditorWindow
	{
		private static Vector2 AboutWindowSize = new Vector2(570, 290);
		private static GUIStyle titleGUI = new GUIStyle();
		private static GUIStyle labelGUI = new GUIStyle();
		private static GUIStyle linkGUI = new GUIStyle();

		public static void Open()
		{
			About aboutWindow = (About) GetWindow(typeof(About), true, "Third Person Character Engine");
			aboutWindow.minSize = new Vector2(AboutWindowSize.x, AboutWindowSize.y);
			aboutWindow.maxSize = new Vector2(AboutWindowSize.x, AboutWindowSize.y);
			aboutWindow.position = new Rect(
				(Screen.currentResolution.width / 2) - (AboutWindowSize.x / 2),
				(Screen.currentResolution.height / 2) - (AboutWindowSize.y / 2),
				AboutWindowSize.x,
				AboutWindowSize.y);
			aboutWindow.Show();
		}

		protected virtual void OnEnable()
		{
			InitGUIStyle();
		}

		protected virtual void OnGUI()
		{
			GUILayout.Space(10);
			GUILayout.Label("Third Person Character Engine", titleGUI);
            GUILayout.Space(10);
            GUILayout.BeginVertical("HelpBox");
			GUILayout.BeginHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginVertical();
			GUILayout.Space(2);
			GUILayout.Label("Project:   " + Info.NAME, labelGUI);
			GUILayout.Space(2);
			GUILayout.Label("Publisher: " + Info.PUBLISHER, labelGUI);
			GUILayout.Space(2);
			GUILayout.Label("Author:    " + Info.AUTHOR, labelGUI);
			GUILayout.Space(2);
			GUILayout.Label("Version:   " + Info.VERSION, labelGUI);
			GUILayout.Space(2);
			GUILayout.Label("Release:   " + Info.RELEASE, labelGUI);
			GUILayout.Space(2);
			GUILayout.Label(Info.COPYRIGHT, labelGUI);
			GUILayout.Space(2);
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			GUILayout.Space(7);

            GUILayout.BeginVertical("HelpBox");
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            GUILayout.Space(2);
            GUILayout.Label("Support:", labelGUI);
            GUILayout.Space(7);
            GUILayout.BeginHorizontal();
            GUILayout.Label("For get full informations about TPC Engine see - ", labelGUI);
            if (GUILayout.Button("Documentation", linkGUI))
                Documentation.Open();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("To keep abreast of all the new news, follow us on - ", labelGUI);
            if (GUILayout.Button("Twitter", linkGUI))
                Application.OpenURL("https://twitter.com/InfiniteDawnTS");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("If you have any questions you can ask them in the - ", labelGUI);
            if (GUILayout.Button("Official Thread", linkGUI))
                Application.OpenURL("https://forum.unity.com/threads/third-person-character-engine-official-thread.545533/");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
            GUILayout.EndVertical();
        }

		protected virtual void InitGUIStyle()
		{
			titleGUI.fontStyle = FontStyle.Bold;
			titleGUI.alignment = TextAnchor.UpperCenter;
			titleGUI.fontSize = 21;

			labelGUI.fontStyle = FontStyle.Bold;
			labelGUI.fontSize = 15;

            linkGUI.fontStyle = FontStyle.Bold;
            linkGUI.fontSize = 15;
            linkGUI.normal.textColor = Color.blue;
        }
	}
}