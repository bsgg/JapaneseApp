using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    #region DataModel

    [System.Serializable]
    public class SentencesExamples
    {
        [SerializeField]
        private List<string> m_Sentence= new List<string>();
        public List<string> Sentence
        {
            set { m_Sentence = value; }
            get { return m_Sentence; }
        }
        public string GetSentence(int index)
        {
            if ((m_Sentence != null) && (index >= 0) && (index < m_Sentence.Count))
            {
                return m_Sentence[index];
            }
            return string.Empty;
        }

        [SerializeField]
        private List<string> m_Kana = new List<string>();
        public List<string> Kana
        {
            set { m_Kana = value; }
            get { return m_Kana; }
        }
        public string GetKana(int index)
        {
            if ((m_Kana != null) && (index >= 0) && (index < m_Kana.Count))
            {
                return m_Kana[index];
            }
            return string.Empty;
        }

        [SerializeField]
        private List<string> m_Kanjis= new List<string>();
        public List<string> Kanjis
        {
            set { m_Kanjis = value; }
            get { return m_Kanjis; }
        }

        public string GetKanjis(int index)
        {
            if ((m_Kanjis != null) && (index >=0 ) && (index < m_Kanjis.Count))
            {
                string[] aKanjis = m_Kanjis[index].Split('|');
                if (aKanjis.Length >= 1)
                {
                    string kanjis = "";
                    for (int i = 0; i < aKanjis.Length; i++)
                    {
                        kanjis += aKanjis[i];
                        if (i < (aKanjis.Length - 1))
                        {
                            kanjis += "\n";
                        }
                    }
                    return kanjis;
                }
            }
            return string.Empty;            
        }

        [SerializeField]
        private List<string> m_Romaji= new List<string>();
        public List<string> Romaji
        {
            set { m_Romaji = value; }
            get { return m_Romaji; }
        }

        public string GetRomanji(int index)
        {
            if ((m_Romaji != null) && (index >= 0) && (index < m_Romaji.Count))
            {
                return m_Romaji[index];
            }
            return string.Empty;
        }

        [SerializeField]
        private List<string> m_English = new List<string>();
        public List<string> English
        {
            set { m_English = value; }
            get { return m_English; }
        }

        public string GetEnglish(int index)
        {
            if ((m_English != null) && (index >= 0) && (index < m_English.Count))
            {
                return m_English[index];
            }
            return string.Empty;
        }
    }

    
    [System.Serializable]
    public class VWord
    {
        [SerializeField]
        private string m_Meaning;
        public string Meaning
        {
            set { m_Meaning = value; }
            get { return m_Meaning; }
        }

        [SerializeField]
        private string m_Word;
        public string Word
        {
            set { m_Word = value; }
            get { return m_Word; }
        }

        [SerializeField]
        private string m_Kana;
        public string Kana
        {
            set { m_Kana = value; }
            get { return m_Kana; }
        }

        [SerializeField]
        private string m_Romaji;
        public string Romaji
        {
            set { m_Romaji = value; }
            get { return m_Romaji; }
        }

        [SerializeField]
        private string m_SpriteID;
        public string SpriteID
        {
            set { m_SpriteID = value; }
            get { return m_SpriteID; }
        }

        [SerializeField]
        private SentencesExamples m_SentencesExamples = new SentencesExamples();
        public SentencesExamples SentencesExamples
        {
            set { m_SentencesExamples = value; }
            get { return m_SentencesExamples; }
        }
    }

    [System.Serializable]
    public class WordData
    {
        [SerializeField]
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [SerializeField]
        private List<VWord> m_Data = new List<VWord>();
        public List<VWord> Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        public VWord GetRandomWord()
        {
            if (m_Data != null)
            {
                int iRand = UnityEngine.Random.Range(0, m_Data.Count);
                return m_Data[iRand];
            }

            return null;
        }

        public int GetRandomWordID()
        {
            if (m_Data != null)
            {
                 return UnityEngine.Random.Range(0, m_Data.Count);
            }

            return -1;
        }

        public VWord GetWordById(int id)
        {
            if ((m_Data != null) && (id >= 0) && (id < m_Data.Count))
            {
                return m_Data[id];
            }

            return null;
        }
    }


    #endregion DataModel

    [System.Serializable]
    public class SpritesData
    {
        public VocabularyControl.ECategory Category;        

        [SerializeField]  public List<Sprite> Sprites;
        public Sprite Sprite(string id)
        {
            for (int i = 0; i < Sprites.Count; i++)
            {
                Debug.Log("Sprite: " + Sprites[i].name);
                if (Sprites[i].name.Equals(id))
                {
                    return Sprites[i];
                }
            }

            return null;
        }
    }

    public class VocabularyControl : Base
    {
        public enum EMenu { NONE = -1, Category, WordDay, RandomWord };
        public enum ECategory { NONE = -1, Animals,  Places, Technology, Profesions, Actions, Home, Food, Numbers, Dates, Family, Objects, Misc, NUM };

        [SerializeField] private string m_DataPath = "Data/Vocabulary/";

        [SerializeField]
        private List<SpritesData> m_SpriteSet;

        [SerializeField]
        private List<WordData> m_VocabularySet;

        [Header("UI")]
        [SerializeField] private VocabularyUI m_VocabularyUI;
        [SerializeField] private CategoriesUI m_CategoriesUI;
        [SerializeField] private ExamplesUI m_ExamplesUI;

       // private VWord m_SelectedWord;
        private int m_SelectedExampleID;
        private int m_SelectedWordID;      
        private ECategory m_SelectedCategory;

        private EMenu m_Menu;

        [SerializeField]
        private Color m_EnableBtnColor;

        [SerializeField]
        private Color m_DisableBtnColor;

        public override void Init()
        {
            base.Init();
           
            m_VocabularyUI.Hide();
            m_ExamplesUI.Hide();
            m_CategoriesUI.Hide();

            // Load the data
            m_VocabularySet = new List<WordData>();
            for (int i = 0; i < (int) ECategory.NUM; i++)
            {
                string category = ((ECategory)i).ToString();
                string path = m_DataPath + category;
                string json = Utility.LoadJSONResource(path);
                if (json != "")
                {
                    WordData data = JsonMapper.ToObject<WordData>(json);
                    data.Name = category;
                    m_VocabularySet.Add(data);

                }
            }
        }        

        #region Navigation
       
        public override void Back()
        {
            base.Back();

            // Always hide sprite
            if (m_VocabularyUI.Sprite.Visible)
            {
                m_VocabularyUI.Sprite.Hide();
            }

            // If example is visible, hide it, otherwise check type menu
            if (m_ExamplesUI.Visible)
            {
                m_ExamplesUI.OnMenuItemEvent -= OnExampleMenuItem;
                m_ExamplesUI.Hide();
            }else
            {
                switch (m_Menu)
                {
                    case EMenu.Category:
                        // If category visible, back to main menu app controller, otherwise, hide vocabulary and show categories menu
                        if (m_CategoriesUI.Visible)
                        {
                            AppController.Instance.ShowMainMenu();
                            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
                            m_CategoriesUI.Hide();
                            // Back to main menu (App Controller)
                        }
                        else
                        {
                            SelectMenu(EMenu.Category);
                        }

                        break;
                    case EMenu.RandomWord:
                    case EMenu.WordDay:
                        // Back to main menu (App Controller)
                        Hide();
                        AppController.Instance.ShowMainMenu();

                        break;
                }

                if (m_ExamplesUI.Visible)
                {
                    m_ExamplesUI.Hide();
                }
            }
        }

        public void SelectMenu(EMenu menu)
        {
            m_Menu = menu;

            m_ExamplesUI.Hide();
            m_VocabularyUI.Hide();
            m_CategoriesUI.Hide();


            switch (m_Menu)
            {
                case EMenu.Category:

                    SetCategories();
                    m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
                    m_CategoriesUI.Show();

                break;

                case EMenu.RandomWord:
                    SetWord();
                    m_VocabularyUI.Show();
                break;
                case EMenu.WordDay:

                    CheckNewDayWord();

                    SetWord();
                    //SetRandomWord();
                    m_VocabularyUI.Show();
                break;
            }

            Show();
        }        

        public void OnCategoryPress(int id, int x, int y)
        {
            Debug.Log("[VocabularyControl] OnCategoryPress");
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;

            m_SelectedCategory = (ECategory)id;
            m_SelectedWordID = 0;

            SetWord();

            m_ExamplesUI.Hide();
            m_VocabularyUI.Show();
            m_CategoriesUI.Hide();
        }

        #endregion Navigation

        #region SetData

        public Sprite GetSprite(ECategory category, string key)
        {
            for (int i = 0; i < m_SpriteSet.Count; i++)
            {
                if (m_SpriteSet[i].Category == category)
                {
                    return m_SpriteSet[i].Sprite(key);
                }
            }

            return null;
        }

        private void SetCategories()
        {
            List<string> categories =  new List<string>();

            for (int i = 0; i < (int) ECategory.NUM; i++)
            {
                categories.Add(((ECategory) i).ToString());
            }

            m_CategoriesUI.ScrollMenu.InitScroll(categories);
        }

        private void SetWord()
        {
            if ((m_SelectedCategory <= ECategory.NONE) || (m_SelectedExampleID <= 0)) return;

            // Check number of words for this category
            if (m_Menu == EMenu.WordDay)
            {
                m_VocabularyUI.NextWordBtn.Enable(false, m_DisableBtnColor);
            }
            else
            {
                if (m_VocabularySet[(int)m_SelectedCategory].Data.Count > 0)
                {
                    m_VocabularyUI.NextWordBtn.Enable(true, m_EnableBtnColor);
                }
                else
                {
                    m_VocabularyUI.NextWordBtn.Enable(false, m_DisableBtnColor);
                }
            }
            VWord word = m_VocabularySet[(int)m_SelectedCategory].GetWordById(m_SelectedWordID);  

            if (word != null)
            {
                // Set word
                m_VocabularyUI.Word = word.Word;
                m_VocabularyUI.English = word.Meaning;
                m_VocabularyUI.Kana = word.Kana + " : " + word.Romaji;

                // Set sprite
                m_VocabularyUI.SpriteBtn.Enable(false, m_DisableBtnColor);
                if (!string.IsNullOrEmpty(word.SpriteID))
                {
                    Sprite sprite = GetSprite(m_SelectedCategory, word.SpriteID);
                    if (sprite != null)
                    {
                        m_VocabularyUI.Sprite.SpriteObject = sprite;

                        m_VocabularyUI.SpriteBtn.Enable(true, m_EnableBtnColor);
                    }
                }
                
                m_VocabularyUI.Sprite.Hide();


                // Set sentence
                if ((word.SentencesExamples != null) && (word.SentencesExamples.Sentence.Count > 0))
                {
                    if (word.SentencesExamples.Sentence.Count > 1)
                    {
                        m_ExamplesUI.NextBtn.Enable(true, m_EnableBtnColor);
                    }
                    else
                    {
                        m_ExamplesUI.NextBtn.Enable(false, m_DisableBtnColor);
                    }


                    m_VocabularyUI.ExampleBtn.Enable(true, m_EnableBtnColor);

                    // Set sentence
                    m_SelectedExampleID = 0;
                    SetExample(m_SelectedExampleID);
                }
                else
                {
                    m_ExamplesUI.NextBtn.Enable(false, m_DisableBtnColor);
                    m_VocabularyUI.ExampleBtn.Enable(false, m_DisableBtnColor);


                }

            }
            else
            {
                Debug.Log("<color=cyan> No Current Word </color>");
            }
        }


        private ECategory GetRandomCategory()
        {
            int rCategory = UnityEngine.Random.Range(0, (int)ECategory.NUM);
            return (ECategory)rCategory;
        } 
        

        private void SetExample(int index)
        {
            if ((m_SelectedCategory <= ECategory.NONE) || (m_SelectedExampleID <= 0)) return;
            // Set sentence
            VWord word = m_VocabularySet[(int)m_SelectedCategory].GetWordById(m_SelectedWordID);
            m_ExamplesUI.Sentence = word.SentencesExamples.GetSentence(index);
            m_ExamplesUI.English = word.SentencesExamples.GetEnglish(index);
            m_ExamplesUI.Kanjis = word.SentencesExamples.GetKanjis(index);
        }
        #endregion SetData

        #region MenuButtons

        public void OnExamplesBtn()
        {
            m_ExamplesUI.OnMenuItemEvent += OnExampleMenuItem;
            m_ExamplesUI.Show();
        }

        public void OnSpriteBtn()
        {
            if (m_VocabularyUI.Sprite.Visible)
            {
                m_VocabularyUI.Word = m_VocabularySet[(int)m_SelectedCategory].Data[m_SelectedWordID].Word;
                m_VocabularyUI.Sprite.Hide();
            }else
            {
                m_VocabularyUI.Word = "";
                m_VocabularyUI.Sprite.Show();
            }            
        }

        public void OnNextWordBtn()
        {
            if ((m_SelectedCategory <= ECategory.NONE) || (m_SelectedExampleID <= 0)) return;
            /*if (m_Menu == EMenu.RandomWord)
            {
                SetRandomWord();
            }
            else
            {*/
            // Increase current word id
            m_SelectedWordID++;
            m_SelectedWordID %= m_VocabularySet[(int)m_SelectedCategory].Data.Count;

            SetWord();
            //}
        }

        private void OnExampleMenuItem(AppController.EMenu id)
        {
            VWord word = m_VocabularySet[(int)m_SelectedCategory].GetWordById(m_SelectedWordID);

            switch (id)
            {
                case AppController.EMenu.NEXT:
                    // Set next sentence
                    m_SelectedExampleID++;
                    m_SelectedExampleID %= word.SentencesExamples.Sentence.Count;

                    SetExample(m_SelectedExampleID);
                break;

                case AppController.EMenu.KANJI:
                    m_ExamplesUI.Sentence = word.SentencesExamples.Sentence[m_SelectedExampleID];
                    break;
                case AppController.EMenu.KANA:
                    m_ExamplesUI.Sentence = word.SentencesExamples.Kana[m_SelectedExampleID];
                    break;
                case AppController.EMenu.ROMAJI:
                    m_ExamplesUI.Sentence = word.SentencesExamples.Romaji[m_SelectedExampleID];
                break;
            }
        }

        #endregion MenuButtons


        #region WordOfDay

        public void CheckNewDayWord()
        {
            PlayerPrefs.DeleteAll();

            string keyLastDay = "LastDayWordDate";
            string keyLastCategory = "LastCategoryDayWord";
            string keyLastWord = "LastWordDayWord";

            if (PlayerPrefs.HasKey(keyLastDay))
            {
                // Check date
                string dateV = PlayerPrefs.GetString(keyLastDay);

                DateTime lastTime = Convert.ToDateTime(dateV);
                DateTime current = DateTime.Now;                

                // Check if last time 
                TimeSpan elapsed = current - lastTime;

                // Select new word
                if (elapsed.TotalDays >= 1)
                {
                    // Update time
                    PlayerPrefs.SetString(keyLastDay, current.ToString());

                    // New category New word
                    m_SelectedCategory = GetRandomCategory();
                    SetKey(keyLastCategory, (int)m_SelectedCategory);

                    // Set new random word
                    m_SelectedWordID = m_VocabularySet[(int)m_SelectedCategory].GetRandomWordID();
                    SetKey(keyLastWord, m_SelectedWordID);
                }
                else
                {
                    // Get last category and last word
                    m_SelectedCategory = (ECategory)GetKey(keyLastCategory);
                    m_SelectedWordID = GetKey(keyLastWord);
                }
                

            }
            else
            { 
                // First time set current Time, Get random category and word
                string dateTime = DateTime.Now.ToString();
                PlayerPrefs.SetString(keyLastDay, dateTime);

                // New category New word
                m_SelectedCategory = GetRandomCategory();
                SetKey(keyLastCategory, (int)m_SelectedCategory);

                // Set new random word
                m_SelectedWordID = m_VocabularySet[(int)m_SelectedCategory].GetRandomWordID();
                SetKey(keyLastWord, m_SelectedWordID);

                PlayerPrefs.Save();
            }
        }


        public int GetKey(string key, int defaultValue = -1)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                // Create key
                SetKey(key, defaultValue);
                return -1;
            }
        }

        public void SetKey(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }


        #endregion WordOfDay

    }

}
