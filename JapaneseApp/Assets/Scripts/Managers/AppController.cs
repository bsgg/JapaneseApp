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
        private HiraganaContent m_HiraganaContent;


        [SerializeField]
        private VocabularyControl m_VocabularyControl;

        [SerializeField]
        private GrammarControl m_GrammarControl;

        [SerializeField]
        private UIBase m_MainMenu;


       /* private HiraganaData m_HiraganaData;

        public HiraganaData HiraganaData
        {
            get { return m_HiraganaData;  }
            set { m_HiraganaData = value; }
        }

        public HiraganaAlphabet GetHiraganaAlphabetById(int id)
        {
            if ((m_HiraganaData != null) && (m_HiraganaData.Hiragana != null) && (id < m_HiraganaData.Hiragana.Count))
            {
               return m_HiraganaData.Hiragana[id];
            }
            return null;
        }

        public void HandleMenu()
        {

        }*/


       /* public void ShowHiraganaTable(int id)
        {

            m_HiraganaTableController.Initialize(GetHiraganaAlphabetById(id));
            m_LessonMenuController.Hide();
            m_HiraganaTableController.Show();
        }
        */

      /*  [SerializeField]
        private LessonMenuController m_LessonMenuController;

        [SerializeField]
        private HiraganaTable m_HiraganaTableController;*/

        void Start ()
        {
            // Initialize EasyTTUTIL
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                
                EasyTTSUtil.Initialize(EasyTTSUtil.Japan);
            }



            m_HiraganaContent.Init();
            //m_HiraganaContent.Show();


            m_VocabularyControl.Init();

            m_GrammarControl.Init();

            m_MainMenu.Show();

           // m_VocabularyControl.Show();


            /*LoadHiraganaData();

           
            m_LessonMenuController.Init();

            m_HiraganaTableController.Init();

            m_LessonMenuController.Show();
            m_HiraganaTableController.Hide();*/

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_HiraganaContent.Back();
            }

        }

        /* public void LoadHiraganaData()
         {
             m_HiraganaData = new HiraganaData();
             string jsonActionsString = Utility.LoadJSONResource("Data/Hiragana");
             if (jsonActionsString != "")
             {
                 m_HiraganaData = JsonMapper.ToObject<HiraganaData>(jsonActionsString);
             }

             List<string> lTitle = new List<string>();
             for (int i= 0; i< m_HiraganaData.Hiragana.Count; i++)
             {
                 HiraganaAlphabet ha = m_HiraganaData.Hiragana[i];
                 lTitle.Add(ha.Title);
             }
             m_LessonMenuController.InitScrollMenu(lTitle);
         }*/


        #region MainMenuHandles

        public void OnVocabularyPress()
        {
            m_MainMenu.Hide();
            m_VocabularyControl.ShowCategories();

        }

        public void OnWordDayPress()
        {
            m_MainMenu.Hide();
            m_VocabularyControl.ShowWordDay();
        }

        public void OnRandomWordPress()
        {
            m_MainMenu.Hide();
            m_VocabularyControl.ShowRandomWord();
        }

        public void OnGrammarPress()
        {
            m_MainMenu.Hide();
            m_GrammarControl.ShowCategories();
        }


        #endregion MainMenuHandles

    }
}
