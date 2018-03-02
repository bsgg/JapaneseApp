using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JapaneseApp
{
    #region DataModel

    [Serializable]
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
        private List<string> m_Description;
        public List<string> Description
        {
            set { m_Description = value; }
            get { return m_Description; }
        }
    }

    #endregion DataModel  

    [Serializable]
    public class DialogObject
    {
        public string Name;
        public AudioClip Audio;
        public Dialog Data;
    }

    [Serializable]
    public class DialogSet
    {
        public string Title;

        public DialogControl.ECategory Category;

        public List<DialogObject> DialogDictionary;

        //public List<Dialog> ListDialogData;

        //public List<AudioClip> AudioClips;

        /*public DialogSet()
        {
            DialogDictionary = new List<DialogObject>();
        }*/

       /* public AudioClip GetAudio(string name)
        {
            for (int i = 0; i < DialogDictionary.Count; i++)
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
        }*/
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

        [SerializeField] private List<DialogObject> m_DialogData;
        [SerializeField] private List<AudioClip> m_AudioClipList;

        public override IEnumerator Initialize()
        {

            m_CategoriesUI.Hide();
            m_DialogUI.Hide();


            m_DialogSet = new List<DialogSet>();

            m_AudioClipList = new List<AudioClip>();
            m_DialogData = new List<DialogObject>();

            for (int i = 0; i < AppController.Instance.Launcher.DialogIndexData.Data.Count; i++)
            {
               
                string json = AppController.Instance.Launcher.DialogIndexData.Data[i].Data;
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        //Dialog auxDialog = JsonUtility.FromJson<Dialog>(json);
                        DialogObject obj = new DialogObject();
                        obj.Data = JsonMapper.ToObject<Dialog>(json);
                        obj.Name = obj.Data.Title;

                        Debug.Log("<color=blue>" + "[DialogControl.Initialize] Converted: " + obj.Data.Title + "</color>");

                        m_DialogData.Add(obj);                        

                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[DialogControl.Initialize] Exception at: " + AppController.Instance.Launcher.DialogIndexData.Data[i].Title + " Data: " + json + " " + e.ToString());
                    }

                    

                }
                else
                {
                    Debug.LogError("[DialogControl.Initialize] Unable to parse " + AppController.Instance.Launcher.DialogIndexData.Data[i].URL + " Data is null or empty ");
                }
            }

            for (int i=0; i<m_DialogData.Count; i++)
            {
                string audio = m_DialogData[i].Data.Audio;

                // Download audio
                AudioClip clip = null;
                yield return AppController.Instance.Launcher.LoadAudio("Audio", audio, ".mp3", (result) => clip = result);

                if (clip != null)
                {
                    Debug.Log("<color=blue>" + "[DialogControl.Initialize] Clipe loaded : " + clip.length + " " + clip.name + "</color>");
                    m_DialogData[i].Audio = clip;

                }
                else
                {
                    Debug.Log("<color=blue>" + "[DialogControl.Initialize] Clip NULL : " + audio + "</color>");
                }

            }
           // yield break;

            // Load the data
            /*for (int i = 0; i < m_DialogSet.Count; i++)
            {
                string category = m_DialogSet[i].Category.ToString();

                for (int d = 0; d< m_DialogSet[i].DialogDictionary.Count; d++)
                {
                    string dialogName = m_DialogSet[i].DialogDictionary[d].Name;

                    string path = m_DataPath + category + "\\" + dialogName;
                    string json = Utility.LoadJSONResource(path);
                    try
                    {
                        if (!string.IsNullOrEmpty(json))
                        {
                            m_DialogSet[i].DialogDictionary[d].Data = JsonMapper.ToObject<Dialog>(json);
                            
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
            }*/

            yield break;
        }

        private void SetDialog(int id)
        {
            if ((id < 0) || (id >= m_DialogSet[(int) m_SelectedCategory].DialogDictionary.Count)) return;
            m_SelectedDialogID = id;

            // Set clip        
            AudioClip clip =  m_DialogSet[(int)m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Audio;
            if (clip != null)
            {
                m_AudioSource.clip = clip;
            }

            m_DialogUI.Title = m_DialogSet[(int) m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Title;

            SwitchToKanji();
        }

        public override void Back()
        {
            base.Back();

            m_AudioSource.Stop();

            // Check if category ui is visible
            if (m_CategoriesUI.Visible)
            {  
                // Check if subcategories is shown
                if (m_IsSubcategory)
                {
                    // Set categories
                    SetCategories();
                    m_CategoriesUI.ScrollMenu.OnItemPress -= OnSubCategoriesPress;
                    m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
                    m_CategoriesUI.Show();

                    m_IsSubcategory = false;

                }else
                {
                    // Hide categories
                    m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
                    m_CategoriesUI.Hide();

                    Hide();
                    // Back to main menu (App Controller)
                    AppController.Instance.ShowMainMenu();
                }
            }
            else // Dialog visible
            {
                m_DialogUI.Hide();
                // // Show sub categories
                m_IsSubcategory = true;
                SetSubcategories();
                m_CategoriesUI.ScrollMenu.OnItemPress += OnSubCategoriesPress;
                m_CategoriesUI.Show();
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

            m_IsSubcategory = true;
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;

            m_SelectedCategory = (ECategory) id;

            
            SetSubcategories();

            m_CategoriesUI.Show();
            m_CategoriesUI.ScrollMenu.OnItemPress += OnSubCategoriesPress;
        }

        private void SetSubcategories()
        {
            // Set subcateogry
            List<string> categories = new List<string>();

            for (int i = 0; i < m_DialogSet[(int) m_SelectedCategory].DialogDictionary.Count; i++)
            {
                string dialogName = m_DialogSet[(int)m_SelectedCategory].DialogDictionary[i].Data.Title;
                categories.Add(dialogName);
            }
            m_CategoriesUI.ScrollMenu.InitScroll(categories);
        }


        private void OnSubCategoriesPress(int id, int x, int y)
        {
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnSubCategoriesPress;
            m_CategoriesUI.Hide();

            m_IsSubcategory = true;
            // Set dialog
            SetDialog(id);
            m_DialogUI.Show();
        }



        private void SwitchToKanji()
        {
            m_DialogUI.Subtitle = "Kanji";

            string dialog = "\n\n";
            int nElements = m_DialogSet[(int)m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Kanji.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Kanji[i];

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
            int nElements =  m_DialogSet[(int)m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Kana.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Kana[i];

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
            int nElements =  m_DialogSet[(int)m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Romaji.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Romaji[i];

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
            int nElements =  m_DialogSet[(int)m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.English.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.English[i];

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
            int nElements =  m_DialogSet[(int)m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Vocabulary.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Vocabulary[i];

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

            //string dialog = "\n\n" +  m_DialogSet[(int)m_SelectedCategory].ListDialog[m_SelectedDialogID].Description;
            string dialog = "\n\n";

            int nElements =  m_DialogSet[(int)m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Description.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[(int) m_SelectedCategory].DialogDictionary[m_SelectedDialogID].Data.Description[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

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
