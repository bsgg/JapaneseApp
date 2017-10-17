using LitJson;
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

        [SerializeField]
        private List<string> m_Kanjis= new List<string>();
        public List<string> Kanjis
        {
            set { m_Kanjis = value; }
            get { return m_Kanjis; }
        }

        [SerializeField]
        private List<string> m_Romanji= new List<string>();
        public List<string> Romanji
        {
            set { m_Romanji = value; }
            get { return m_Romanji; }
        }

        [SerializeField]
        private List<string> m_English = new List<string>();
        public List<string> English
        {
            set { m_English = value; }
            get { return m_English; }
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
        private string m_Hiragana;
        public string Hiragana
        {
            set { m_Hiragana = value; }
            get { return m_Hiragana; }
        }

        [SerializeField]
        private string m_Romanji;
        public string Romanji
        {
            set { m_Romanji = value; }
            get { return m_Romanji; }
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
                int iRand = Random.Range(0, m_Data.Count);
                return m_Data[iRand];
            }

            return null;
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
        public Sprite SpriteByKey(string key)
        {
            for (int i = 0; i < Sprites.Count; i++)
            {
                Debug.Log("Sprite: " + Sprites[i].name);
                if (Sprites[i].name.Equals(key))
                {
                    return Sprites[i];
                }
            }

            return null;
        }
    }

    public class VocabularyControl : Base
    {
        public enum ECategory { NONE = -1, Animals,  Places, Technology, Profesions, Actions, Numbers, Dates, Misc, NUM };

        [SerializeField] private string m_DataPath = "Data/Vocabulary/";

        [SerializeField]
        private List<SpritesData> m_SpriteSet;

        [SerializeField]
        private List<WordData> m_VocabularySet;

        [Header("UI")]
        [SerializeField] private VocabularyUI m_VocabularyUI;
        [SerializeField] private CategoriesUI m_CategoriesUI;


        private VWord m_CurrentWord;
        private int m_ICurrentExample;
        private int m_CurrentWordID = 0;
        private bool m_SelectRandomWord = false;

        private ECategory m_CurrentCategory;
        

        public override void Init()
        {
            base.Init();

            m_VocabularyUI.Hide();
            m_VocabularyUI.Example.Hide();
            m_CategoriesUI.Hide();

            for (int i = 0; i < (int) ECategory.NUM; i++)
            {
                m_VocabularySet[i] = new WordData();

                string category = ((ECategory)i).ToString();
                string path = m_DataPath + category;
                string json = Utility.LoadJSONResource(path);
                if (json != "")
                {
                    m_VocabularySet[i] = JsonMapper.ToObject<WordData>(json);
                    m_VocabularySet[i].Name = category;
                }
            }
        }

        private void SetCategories()
        {
            List<string> categories =  new List<string>();

            for (int i = 0; i < (int) ECategory.NUM; i++)
            {
                categories.Add(((ECategory) i).ToString());
            }

            m_CategoriesUI.ScrollMenu.InitScroll(categories);
            m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
        }

        public override void Hide()
        {
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
            base.Hide();
        }

        public void ShowCategories()
        {
            SetCategories();
            Show();

            m_VocabularyUI.Example.Hide();
            m_VocabularyUI.Hide();
            m_CategoriesUI.Show();

            m_SelectRandomWord = false;
        }

        public void ShowRandomWord()
        {
            SetRandomWord();
            Show();

            m_VocabularyUI.Example.Hide();
            m_VocabularyUI.Show();
            m_CategoriesUI.Hide();

            m_SelectRandomWord = true;
        }


        public void ShowWordDay()
        {
            // TODO DO LOGIC FOR DIFFERENT WORD EVERYDAY
            SetRandomWord();
            Show();

            m_VocabularyUI.Example.Hide();
            m_VocabularyUI.Show();
            m_CategoriesUI.Hide();
        }

        public void OnCategoryPress(int id)
        {
            
            m_CurrentCategory = (ECategory)id;
            m_CurrentWordID = 0;

            SetWordByCategory();

            Show();

            m_VocabularyUI.Example.Hide();
            m_VocabularyUI.Show();
            m_CategoriesUI.Hide();
        }

        private void SetWordByCategory()
        {

            if (m_SelectRandomWord)
            {
                m_CurrentWord = m_VocabularySet[(int)m_CurrentCategory].GetRandomWord();
            }else
            {
                m_CurrentWord = m_VocabularySet[(int)m_CurrentCategory].GetWordById(m_CurrentWordID);
            }
            if (m_CurrentWord != null)
            {
                // Set word
                m_VocabularyUI.Word = m_CurrentWord.Word;
                m_VocabularyUI.English = m_CurrentWord.Meaning;
                m_VocabularyUI.Hiragana = m_CurrentWord.Hiragana + " : " + m_CurrentWord.Romanji;

                Debug.Log("SPRITE ID: " + m_CurrentWord.SpriteID);
                if (!string.IsNullOrEmpty(m_CurrentWord.SpriteID))
                {
                    m_VocabularyUI.Sprite.SpriteObject = m_SpriteSet[(int)m_CurrentCategory].SpriteByKey(m_CurrentWord.SpriteID);

                }
                
                m_VocabularyUI.Sprite.Hide();

                // Set sentence
                if ((m_CurrentWord.SentencesExamples != null) && (m_CurrentWord.SentencesExamples.Sentence.Count > 0))
                {

                    if (m_CurrentWord.SentencesExamples.Sentence.Count > 1)
                    {
                        m_VocabularyUI.Example.NextSentenceButton.SetActive(true);
                    }
                    else
                    {
                        m_VocabularyUI.Example.NextSentenceButton.SetActive(false);
                    }


                    m_VocabularyUI.ExampleButton.SetActive(true);

                    // Set sentence
                    m_ICurrentExample = 0;
                    SetExample(m_ICurrentExample);

                }
                else
                {
                    m_VocabularyUI.ExampleButton.SetActive(false);
                    m_VocabularyUI.Example.NextSentenceButton.SetActive(false);


                }

            }
            else
            {
                Debug.Log("<color=cyan> No Current Word </color>");
            }
        }


        private void SetRandomWord()
        {
            // Get random word from a category
            int rCategory = Random.Range(0, (int)ECategory.NUM);
            m_CurrentCategory = (ECategory)rCategory;
            m_SelectRandomWord = true;

            SetWordByCategory();
        }       


        private void SetExample(int index)
        {
            if ((index < 0) || (index >= m_CurrentWord.SentencesExamples.Sentence.Count))
            {
                Debug.Log("<color=cyan> SetExample, Index out of boundaries </color>");
                return;
            }
           
            // Set sentence
            m_VocabularyUI.Example.Sentence = m_CurrentWord.SentencesExamples.Sentence[index];
            m_VocabularyUI.Example.Romanji = m_CurrentWord.SentencesExamples.Romanji[index];
            m_VocabularyUI.Example.English = m_CurrentWord.SentencesExamples.English[index];


            string[] aKanjis = m_CurrentWord.SentencesExamples.Kanjis[index].Split('|');
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
                m_VocabularyUI.Example.Kanji = kanjis;
            } 
           
        }


        #region Handles

        public void OnClose()
        {
            if (m_VocabularyUI.Example.Visible)
            {
                m_VocabularyUI.Example.Hide();
            }

            if (m_VocabularyUI.Sprite.Visible)
            {
                m_VocabularyUI.Sprite.Hide();
            }
        }

        public void OnNextWord()
        {
            if (m_SelectRandomWord)
            {
                SetRandomWord();
            }
            else
            {
                // Increase current word id
                m_CurrentWordID++;
                m_CurrentWordID %= m_VocabularySet[(int)m_CurrentCategory].Data.Count;

                SetWordByCategory();
            }
            
        }

        public void OnNextExample()
        {
            if (m_CurrentWord == null)
            {
                Debug.Log("<color=cyan> No Current Word </color>");
                return;
            }


            // Set next sentence
            m_ICurrentExample++;
            m_ICurrentExample %= m_CurrentWord.SentencesExamples.Sentence.Count;

            SetExample(m_ICurrentExample);

        }

        public void OnSoundPlay()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                EasyTTSUtil.SpeechFlush(m_CurrentWord.Hiragana);
            }
        }



        #endregion Handles
    }

}
