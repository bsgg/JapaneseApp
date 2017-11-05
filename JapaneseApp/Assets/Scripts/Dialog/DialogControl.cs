using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JapaneseApp
{
    #region DataModel

    [System.Serializable]
    public class Dialog
    {
        [SerializeField]
        private string m_Title;
        public string Title
        {
            set { m_Title = value; }
            get { return m_Title; }
        }

        [SerializeField]
        private string m_Audio;
        public string Audio
        {
            set { m_Audio = value; }
            get { return m_Audio; }
        }

        [SerializeField]
        private List<string> m_Kanji= new List<string>();
        public List<string> Kanji
        {
            set { m_Kanji = value; }
            get { return m_Kanji; }
        }
        [SerializeField]
        private List<string> m_Kana= new List<string>();
        public List<string> Kana
        {
            set { m_Kana = value; }
            get { return m_Kana; }
        }
        [SerializeField]
        private List<string> m_Romaji= new List<string>();
        public List<string> Romaji
        {
            set { m_Romaji = value; }
            get { return m_Romaji; }
        }
        [SerializeField]
        private List<string> m_English= new List<string>();
        public List<string> English
        {
            set { m_English = value; }
            get { return m_English; }
        }

        [SerializeField]
        private List<string> m_Vocabulary= new List<string>();
        public List<string> Vocabulary
        {
            set { m_Vocabulary = value; }
            get { return m_Vocabulary; }
        }


        [SerializeField]
        private string m_Description;
        public string Description
        {
            set { m_Description = value; }
            get { return m_Description; }
        }


    }

    #endregion DataModel  

    [System.Serializable]
    public class DialogSet
    {
        public string Title;

        public DialogControl.ECategory Category;

        public List<string> NameDialog;

        public List<Dialog> ListDialog;

        public List<AudioClip> AudioClips;

        public DialogSet()
        {
            ListDialog = new List<Dialog>();
        }

        public AudioClip GetAudio(string name)
        {
            for (int i = 0; i < AudioClips.Count; i++)
            {
                if (AudioClips[i].name.Equals(name))
                {
                    return AudioClips[i];
                }
            }

            return null;
        }

        public AudioClip GetAudio(int id)
        {
            if ((id < 0) || (id >= AudioClips.Count)) return null;
            return AudioClips[id];
        }
    }

    public class DialogControl : Base
    {
        public enum ECategory {  NONE = -1, NHSEasyJapanese = 0, NihongoDojo, NUM };

        [SerializeField]
        private string m_DataPath = "Data/Dialogs/";
        [SerializeField]
        private int m_NumberDialogs;

        [SerializeField] private List<DialogSet> m_DialogSet;
        private int m_SelectedDialogID;
        private int m_SelectedAudioClipID;
        private ECategory m_SelectedCategory = ECategory.NHSEasyJapanese;

        [SerializeField]
        private AudioSource m_AudioSource;

        [Header("UI")]
        [SerializeField] private CategoriesUI m_CategoriesUI;
        [SerializeField] private DialogUI m_DialogUI;

        public override void Init()
        {
            base.Init();

            m_CategoriesUI.Hide();
            m_DialogUI.Hide();

            // Load the data
            for (int i = 0; i < m_DialogSet.Count; i++)
            {               
                for (int d = 0; d< m_DialogSet[i].NameDialog.Count; d++)
                {
                    string nameDialog = m_DialogSet[i].Category.ToString() + "\\" + m_DialogSet[i].NameDialog[d];
                    string path = m_DataPath + nameDialog;
                    string json = Utility.LoadJSONResource(path);
                    try
                    {
                        if (!string.IsNullOrEmpty(json))
                        {
                            Dialog data = JsonMapper.ToObject<Dialog>(json);
                            m_DialogSet[i].ListDialog.Add(data);
                        }
                        else
                        {
                            Debug.Log("[DialogControl.Init] JSON not found: " + path);
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("[DialogControl.Init] Bad Format JSON File: " + path);
                    }                    
                }
            }                 
        }

        private void SetDialog(int id)
        {
            if ((id < 0) || (id >= m_DialogSet.Count)) return;
            m_SelectedDialogID = id;

            // Set clip        
            string nameClip = m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].Audio;
            AudioClip clip =  m_DialogSet[(int) m_SelectedCategory].GetAudio(nameClip);
            if (clip != null)
            {
                m_AudioSource.clip = clip;
            }

            m_DialogUI.Title = m_DialogSet[(int) m_SelectedCategory].ListDialog[m_SelectedDialogID].Title;

            SwitchToKanji();
        }

        public override void Back()
        {
            base.Back();

            m_AudioSource.Stop();

            // Check if category is visible
            if (m_CategoriesUI.Visible)
            {
                if (m_IsSubcategory)
                {
                    m_DialogUI.Hide();

                    SetCategories();
                    m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
                    m_CategoriesUI.Show();

                    m_IsSubcategory = false;

                }
                else
                {
                    m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
                    m_CategoriesUI.Hide();

                    Hide();
                    // Back to main menu (App Controller)
                    AppController.Instance.ShowMainMenu();
                }
               
            }
            else
            {
                Hide();
                // Show categories
                SetSubcategories();
                //SetCategories();
                m_CategoriesUI.Show();
                m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
            }
        }

        public override void Show()
        {
            base.Show();

            m_IsSubcategory = false;
            SetCategories();
            m_CategoriesUI.Show();

            m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
        }

        public override void Hide()
        {
            base.Hide();
            m_DialogUI.Hide();
        }

        private bool m_IsSubcategory = false;

        private void SetCategories()
        {
            List<string> categories = new List<string>();

            for (int i = 0; i < m_DialogSet.Count; i++)
            {
                categories.Add(m_DialogSet[i].Title);
            }

            m_CategoriesUI.ScrollMenu.InitScroll(categories);
        }


        public void OnCategoryPress(int id, int x, int y)
        {
            Debug.Log("[DialogControl] OnCategoryPress");


            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;

            m_SelectedCategory = (ECategory) id;

            m_IsSubcategory = true;
            SetSubcategories();

            m_CategoriesUI.Show();
            m_CategoriesUI.ScrollMenu.OnItemPress += OnSubCategoriesPress;
        }

        private void SetSubcategories()
        {
            // Set subcateogry
            List<string> categories = new List<string>();

            for (int i = 0; i < m_DialogSet[(int) m_SelectedCategory].ListDialog.Count; i++)
            {
                string dialogName = m_DialogSet[(int)m_SelectedCategory].ListDialog[i].Title;
                categories.Add(dialogName);
            }
            m_CategoriesUI.ScrollMenu.InitScroll(categories);
        }


        private void OnSubCategoriesPress(int id, int x, int y)
        {
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnSubCategoriesPress;
            m_CategoriesUI.Hide();

            // Set dialog
            SetDialog(id);
            m_DialogUI.Show();
        }



        private void SwitchToKanji()
        {
            m_DialogUI.Subtitle = "Kanji";

            string dialog = "\n\n";
            int nElements = m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].Kanji.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].ListDialog[m_SelectedDialogID].Kanji[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            

            m_DialogUI.SetScrollDialog(dialog);
        }

        private void SwitchToKana()
        {
            m_DialogUI.Subtitle = "Kana";

            string dialog = "\n\n";
            int nElements =  m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].Kana.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].ListDialog[m_SelectedDialogID].Kana[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }

        private void SwitchToRomaji()
        {
            m_DialogUI.Subtitle = "Romaji";

            string dialog = "\n\n";
            int nElements =  m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].Romaji.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].ListDialog[m_SelectedDialogID].Romaji[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }

        private void SwitchToEnglish()
        {
            m_DialogUI.Subtitle = "English";

            string dialog = "\n\n";
            int nElements =  m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].English.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].ListDialog[m_SelectedDialogID].English[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }

        private void SwitchToVocabulary()
        {
            m_DialogUI.Subtitle = "Vocabulary";
            string dialog = "\n\n";
            int nElements =  m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].Vocabulary.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].ListDialog[m_SelectedDialogID].Vocabulary[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }

        public void SwitchToDescription()
        {
            m_DialogUI.Subtitle = "Description";

            string dialog = "\n\n" +  m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].Description;

            m_DialogUI.SetScrollDialog(dialog);
        }

        public void SwitchToSound()
        {
            m_AudioSource.Stop();
            m_AudioSource.Play();
        }

        public void OnMenuItemPress(int id)
        {
            switch (id)
            {
                case 0:
                    SwitchToKanji();
                break;
                case 1:
                    SwitchToKana();
                break;
                case 2:
                    SwitchToRomaji();
                break;
                case 3:
                    SwitchToEnglish();
                break;
                case 4:
                    SwitchToVocabulary();
                break;
                case 5:
                    SwitchToDescription();
                    break;
                case 6:
                    SwitchToSound();
                break;
            }
        }
    }
}
