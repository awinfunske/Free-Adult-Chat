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
    [InitializeOnLoad]
    public class QuickStart : EditorWindow
    {
        private const string SHOWED_ON_START_KEY = "QS_Window_ShowedOnStart";

        private Vector2 scrollPos;

        private GUIStyle titleStyle;
        private GUIStyle paragraphStyle;
        private GUIStyle defaultTextStyle;
        private GUIStyle linkStyle;
        private GUIStyle pathsStyle;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static QuickStart()
        {
            EditorApplication.delayCall += () =>
            {
                if (!ShowedOnStart)
                    Open();
            };
        }

        [MenuItem("Help/TPC Engine Quick Start", false, 0)]
        public static void Open()
        {
            QuickStart qsWindow = (QuickStart)GetWindow(typeof(QuickStart), true, "TPC Engine");
            Vector2 QSWindowSize = new Vector2(600, 500);
            qsWindow.minSize = new Vector2(QSWindowSize.x, QSWindowSize.y);
            qsWindow.maxSize = new Vector2(QSWindowSize.x, QSWindowSize.y);
            qsWindow.position = new Rect(
                (Screen.currentResolution.width / 2) - (QSWindowSize.x / 2),
                (Screen.currentResolution.height / 2) - (QSWindowSize.y / 2),
                QSWindowSize.x,
                QSWindowSize.y);
            qsWindow.Show();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            InitializeGUIStyles();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events. 
        /// If the MonoBehaviour's enabled property is set to false, OnGUI() will not be called.
        /// </summary>
        protected virtual void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            GUILayout.Space(5);

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Third Person Character Engine\nQuick Start", titleStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(5);
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            GUILayout.Space(5);

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            ParagrapLabel("1: Tags & Layers");
            DefaultLabel("Check your tags for this go to the tab:", defaultTextStyle);
            DefaultLabel("Edit -> Project Settings -> Tags & Layers", pathsStyle);
            DefaultLabel("Create the missing tags:\n   [Player]", defaultTextStyle);
            DefaultLabel("", defaultTextStyle);
            DefaultLabel("Create the missing layers:\n   [8 - Player]", defaultTextStyle);
            GUILayout.Space(5);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            ParagrapLabel("2: Input");
            DefaultLabel("Setup your input settings for this go to the tab:", defaultTextStyle);
            DefaultLabel("Edit -> Project Settings -> Input", pathsStyle);
            DefaultLabel("Add inputs :   [Crouch] [Grab] [Sprint] [Menu]", defaultTextStyle);
            GUILayout.Space(5);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            ParagrapLabel("3: TPC Engine Preferences");
            DefaultLabel("Setup preferences for this go to the tab:", defaultTextStyle);
            DefaultLabel("Edit -> Project Settings -> Preferences", pathsStyle);
            DefaultLabel("Make sure that, paths and color filled correctly.\nIf it completely new project, recommended reset settings.\nFor this press on \"Reset\" buttons.", defaultTextStyle);
            GUILayout.Space(5);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            ParagrapLabel("Well done!");

            GUILayout.BeginHorizontal();
            DefaultLabel("For get full informations about \"How to use\" TPC Engine see - ", defaultTextStyle);
            if (GUILayout.Button("Documentation", linkStyle))
                Documentation.Open();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DefaultLabel("To keep abreast of all the new news, follow us on - ", defaultTextStyle);
            if (GUILayout.Button("Twitter", linkStyle))
                Application.OpenURL("https://twitter.com/InfiniteDawnTS");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DefaultLabel("If you have any questions you can ask them in the - ", defaultTextStyle);
            if (GUILayout.Button("Official Thread", linkStyle))
                Application.OpenURL("https://forum.unity.com/threads/third-person-character-engine-official-thread.545533/");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Сomplete the setup!"))
            {
                this.Close();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// OnDestroy will only be called on game objects that have previously been active.
        /// </summary>
        protected virtual void OnDestroy()
        {
            EditorPrefs.SetBool(SHOWED_ON_START_KEY, true);
        }

        /// <summary>
        /// QuickStart window already showed on start
        /// </summary>
        public static bool ShowedOnStart
        {
            get
            {
                return EditorPrefs.GetBool(SHOWED_ON_START_KEY);
            }
        }

        /// <summary>
        /// Default label for QuickStart window. 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="style"></param>
        public virtual void DefaultLabel(string label, GUIStyle style)
        {
            GUILayout.Space(3);
            GUILayout.Label(label, style);
        }

        /// <summary>
        /// Paragrap label for QuickStart window. 
        /// </summary>
        /// <param name="label"></param>
        public virtual void ParagrapLabel(string label)
        {
            GUILayout.Space(5);
            GUILayout.Label(label, paragraphStyle);
            GUILayout.Space(5);
        }

        /// <summary>
        /// Initialize GUI styles for QuickStart window.
        /// Styles: titleStyle, paragraphStyle, pathsStyle, defaultTextStyle and linkStyle.
        /// </summary>
        public virtual void InitializeGUIStyles()
        {
            titleStyle = new GUIStyle()
            {
                fontSize = 21,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            paragraphStyle = new GUIStyle()
            {
                fontSize = 17,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };

            pathsStyle = new GUIStyle()
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Italic
            };

            defaultTextStyle = new GUIStyle()
            {
                fontSize = 12
            };

            linkStyle = new GUIStyle()
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold
            };
            linkStyle.normal.textColor = Color.blue;
        }

        public GUIStyle TitleStyle
        {
            get
            {
                return titleStyle;
            }

            set
            {
                titleStyle = value;
            }
        }

        public GUIStyle ParagraphStyle
        {
            get
            {
                return paragraphStyle;
            }

            set
            {
                paragraphStyle = value;
            }
        }

        public GUIStyle DefaultTextStyle
        {
            get
            {
                return defaultTextStyle;
            }

            set
            {
                defaultTextStyle = value;
            }
        }

        public GUIStyle LinkStyle
        {
            get
            {
                return linkStyle;
            }

            set
            {
                linkStyle = value;
            }
        }

        public GUIStyle PathsStyle
        {
            get
            {
                return pathsStyle;
            }

            set
            {
                pathsStyle = value;
            }
        }
    }
}