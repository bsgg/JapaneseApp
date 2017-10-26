using LitJson;
using System.Collections;
using System.Collections.Generic;
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
    public class AudioData
    {
        [SerializeField]
        public List<AudioClip> AudioClips;
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
        [SerializeField]
        private string m_DataPath = "Data/Dialogs/";
        [SerializeField]
        private List<string> m_ListDialogs= new List<string>();

        [SerializeField]
        private AudioData m_AudioDataSet;
        private int m_SelectedAudioClipID;

        [SerializeField]
        private List<Dialog>  m_DialogSet;
        private int m_SelectedDialogID;

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
            m_DialogSet = new List<Dialog>();
            for (int i = 0; i < m_ListDialogs.Count; i++)
            {
                string path = m_DataPath + m_ListDialogs[i];
                string json = Utility.LoadJSONResource(path);
                if (json != "")
                {
                    Dialog data = JsonMapper.ToObject<Dialog>(json);
                    m_DialogSet.Add(data);

                }
            }
                 
        }

        private void SetDialog(int id)
        {
            if ((id < 0) || (id >= m_DialogSet.Count)) return;
            m_SelectedDialogID = id;

            // Set clip            
            AudioClip clip = m_AudioDataSet.GetAudio(m_DialogSet[m_SelectedDialogID].Audio);
            if (clip != null)
            {
                m_AudioSource.clip = clip;
            }

            m_DialogUI.Title = m_DialogSet[m_SelectedDialogID].Title;

            OnChangeToKanji();
        }

        public override void Back()
        {
            base.Back();

            m_AudioSource.Stop();

            // Check if category is visible
            if (m_CategoriesUI.Visible)
            {
                m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
                m_CategoriesUI.Hide();

                Hide();
                // Back to main menu (App Controller)
                AppController.Instance.ShowMainMenu();
            }
            else
            {
                Hide();
                // Show categories
                SetCategories();
                m_CategoriesUI.Show();
                m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
            }
        }

        public override void Show()
        {
            base.Show();

            SetCategories();
            m_CategoriesUI.Show();

            m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
        }

        public override void Hide()
        {
            base.Hide();
            m_DialogUI.Hide();
        }

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
            m_CategoriesUI.Hide();

            // Set dialog
            SetDialog(id);
            m_DialogUI.Show();
        }


        public void OnChangeToKanji()
        {
            m_DialogUI.Subtitle = "Hiragana-Kanji";

            string dialog = "\n\n";
            int nElements = m_DialogSet[m_SelectedDialogID].Kanji.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[m_SelectedDialogID].Kanji[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }


        public void OnChangeToHiragana()
        {
            m_DialogUI.Subtitle = "Hiragana";

            string dialog = "\n\n";
            int nElements = m_DialogSet[m_SelectedDialogID].Kana.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[m_SelectedDialogID].Kana[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }

        public void OnChangeToRomaji()
        {
            m_DialogUI.Subtitle = "Romaji";

            string dialog = "\n\n";
            int nElements = m_DialogSet[m_SelectedDialogID].Romaji.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[m_SelectedDialogID].Romaji[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }

        public void OnVocabulary()
        {
            m_DialogUI.Subtitle = "Vocabulary";
            string dialog = "\n\n";
            int nElements = m_DialogSet[m_SelectedDialogID].Vocabulary.Count;
            for (int i = 0; i < nElements; i++)
            {
                dialog += m_DialogSet[m_SelectedDialogID].Vocabulary[i];

                if (i < (nElements - 1))
                {
                    dialog += "\n\n";
                }
            }

            m_DialogUI.SetScrollDialog(dialog);
        }

        public void OnDescription()
        {
            m_DialogUI.Subtitle = "Description";

            string dialog = "\n\n" + m_DialogSet[m_SelectedDialogID].Description;

            m_DialogUI.SetScrollDialog(dialog);
        }

        public void OnSound()
        {
            m_AudioSource.Stop();
            m_AudioSource.Play();
        }
    }
}
