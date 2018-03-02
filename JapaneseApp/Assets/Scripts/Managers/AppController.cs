using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace JapaneseApp
{
    public class AppController : MonoBehaviour
    {
        #region Instance
        private static AppController m_Instance;
        public static AppController Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (AppController)FindObjectOfType(typeof(AppController));

                    if (m_Instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(AppController) + " is needed in the scene, but there is none.");
                    }
                }
                return m_Instance;
            }
        }
        #endregion Instance
         
        public enum EMenu { NONE = -1, KANJI = 0, KANA = 1, ROMAJI = 2, DESCRIPTION = 3, EXAMPLES = 4, NEXT = 5, SOUND = 6 };


        [Header("Debug")]
        [SerializeField]
        private bool m_EnableDebug = false;

        [SerializeField]
        private DebugUI m_DebugUI;
        public DebugUI DebugUI
        {
            get { return m_DebugUI; }
        }


        [Header("Controls")]

        [SerializeField]
        private LauncherControl m_Launcher;
        public LauncherControl Launcher
        {
            get { return m_Launcher; }
        }

        [SerializeField]
        private VocabularyControl m_Vocabulary;

        [SerializeField]
        private GrammarControl m_Grammar;

        [SerializeField]
        private MainMenuController m_MainMenu;

        [SerializeField]
        private ABCControl m_Hiragana;

        [SerializeField]
        private ABCControl m_Katakana;

        [SerializeField]
        private DialogControl m_Dialog;

        [SerializeField]
        private TopBar m_TopBar;
        private Base m_CurrentControl;

        
        void Start ()
        {
            StartCoroutine(Init());
        }


        private IEnumerator Init()
        {
            if (m_EnableDebug)
            {
                m_DebugUI.Show();
            }
            else
            {
                m_DebugUI.Hide();
            }

            // Hide all
            m_Launcher.Hide();
            m_Vocabulary.Hide();

            m_Grammar.Hide();
            m_Hiragana.Hide();
            m_Katakana.Hide();
            m_MainMenu.Hide();
            m_Dialog.Hide();


            // Hide all
            yield return m_Launcher.Initialize();

            m_Launcher.UpdateProgress("Completed m_Launcher", 1.0f);

           // yield return m_Vocabulary.Initialize();

            //m_Launcher.UpdateProgress("Completed Vocabulary", 1.0f);

            yield return m_Dialog.Initialize();

            m_Launcher.UpdateProgress("Completed m_Dialog", 1.0f);

            yield return new WaitForSeconds(0.5f);

            m_Launcher.Hide();

            ShowMainMenu();

        }

        public void ShowMainMenu()
        {
            m_TopBar.Title = "JPWorld";
            m_TopBar.CloseBtn.SetActive(false);
            m_MainMenu.Show();
            m_CurrentControl = m_MainMenu;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }  
        
        public void Back()
        {
            if (m_CurrentControl == m_MainMenu)
            {
                Application.Quit();
            }
            else
            {
                m_CurrentControl.Back();
            }
        }     

        #region MainMenuHandles

        public void OnVocabularyPress()
        {
            m_TopBar.Title = "Vocabulary";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenu.Hide();
            m_CurrentControl = m_Vocabulary;
            m_Vocabulary.SelectMenu(VocabularyControl.EMenu.Category);

        }

        public void OnWordDayPress()
        {
            m_TopBar.Title = "Vocabulary";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenu.Hide();
            m_CurrentControl = m_Vocabulary;

            m_Vocabulary.SelectMenu(VocabularyControl.EMenu.WordDay);
        }

        public void OnRandomWordPress()
        {
            m_TopBar.Title = "Vocabulary";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenu.Hide();
            m_CurrentControl = m_Vocabulary;
            m_Vocabulary.SelectMenu(VocabularyControl.EMenu.RandomWord);
        }

        public void OnGrammarPress()
        {
            m_TopBar.Title = "Grammar";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenu.Hide();
            m_CurrentControl = m_Grammar;
            m_Grammar.ShowCategories();
        }

        public void OnHiraganaPress()
        {
            m_TopBar.Title = "Hiragana";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenu.Hide();
            m_CurrentControl = m_Hiragana;
            m_Hiragana.Show();
        }

        public void OnKatakanaPress()
        {
            m_TopBar.Title = "Katakana";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenu.Hide();
            m_CurrentControl = m_Katakana;
            m_Katakana.Show();
        }

        public void OnDialogPress()
        {
            m_TopBar.Title = "Dialogs";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenu.Hide();
            m_CurrentControl = m_Dialog;
            m_Dialog.Show();
        }

        #endregion MainMenuHandles

    }
}
