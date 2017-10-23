using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        private VocabularyControl m_VocabularyControl;

        [SerializeField]
        private GrammarControl m_GrammarControl;

        [SerializeField]
        private MainMenuController m_MainMenuController;

        [SerializeField]
        private ABCControl m_HiraganaController;

        [SerializeField]
        private ABCControl m_KatakanaController;

        [SerializeField]
        private DialogControl m_DialogControl;

        [SerializeField]
        private TopBar m_TopBar;
        private Base m_CurrentControl;
        
        void Start ()
        {
            // Initialize EasyTTUTIL
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                
                EasyTTSUtil.Initialize(EasyTTSUtil.Japan);
            }

            m_VocabularyControl.Init();

            m_GrammarControl.Init();

            m_HiraganaController.Init();
            m_KatakanaController.Init();

            m_MainMenuController.Init();
            m_DialogControl.Init();


            ShowMainMenu();

        }

        public void ShowMainMenu()
        {
            m_TopBar.Title = "JPWorld";
            m_TopBar.CloseBtn.SetActive(false);
            m_MainMenuController.Show();
            m_CurrentControl = m_MainMenuController;
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
            if (m_CurrentControl == m_MainMenuController)
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
            m_MainMenuController.Hide();
            m_CurrentControl = m_VocabularyControl;
            m_VocabularyControl.SelectMenu(VocabularyControl.EMenu.Category);

        }

        public void OnWordDayPress()
        {
            m_TopBar.Title = "Vocabulary";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenuController.Hide();
            m_CurrentControl = m_VocabularyControl;
            m_VocabularyControl.SelectMenu(VocabularyControl.EMenu.WordDay);
        }

        public void OnRandomWordPress()
        {
            m_TopBar.Title = "Vocabulary";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenuController.Hide();
            m_CurrentControl = m_VocabularyControl;
            m_VocabularyControl.SelectMenu(VocabularyControl.EMenu.RandomWord);
        }

        public void OnGrammarPress()
        {
            m_TopBar.Title = "Grammar";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenuController.Hide();
            m_CurrentControl = m_GrammarControl;
            m_GrammarControl.ShowCategories();
        }

        public void OnHiraganaPress()
        {
            m_TopBar.Title = "Hiragana";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenuController.Hide();
            m_CurrentControl = m_HiraganaController;
            m_HiraganaController.Show();
        }

        public void OnKatakanaPress()
        {
            m_TopBar.Title = "Katakana";
            m_TopBar.CloseBtn.SetActive(true);
            m_MainMenuController.Hide();
            m_CurrentControl = m_KatakanaController;
            m_KatakanaController.Show();
        }

        #endregion MainMenuHandles

    }
}
