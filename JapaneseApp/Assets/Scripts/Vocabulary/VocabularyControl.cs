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
        private List<string> m_Hiragana = new List<string>();
        public List<string> Hiragana
        {
            set { m_Hiragana = value; }
            get { return m_Hiragana; }
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
        public enum ECategory { NONE = -1, Animals,  Places, Technology, Profesions, Actions, Home, Numbers, Dates, Misc, NUM };

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
        //private bool m_SelectRandomWord = false;
        private ECategory m_CurrentCategory;


        private EMenu m_Menu;

        [SerializeField]
        private Color m_EnableBtnColor;

        [SerializeField]
        private Color m_DisableBtnColor;

        public override void Init()
        {
            base.Init();
           
            m_VocabularyUI.Hide();
            m_VocabularyUI.Example.Hide();
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

        public override void Show()
        {
            m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
            m_VocabularyUI.Example.NextSentenceBtn.onClick.AddListener(() => OnNextExample());
            base.Show();           

        }

        public override void Hide()
        {
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
            m_VocabularyUI.Example.NextSentenceBtn.onClick.RemoveAllListeners();

            base.Hide();
        }

        public override void Back()
        {
            base.Back();

            // Always hide sprite
            if (m_VocabularyUI.Sprite.Visible)
            {
                m_VocabularyUI.Sprite.Hide();
            }

            // If example is visible, hide it, otherwise check type menu
            if (m_VocabularyUI.Example.Visible)
            {
                m_VocabularyUI.Example.Hide();
            }else
            {
                switch (m_Menu)
                {
                    case EMenu.Category:
                        // If category visible, back to main menu app controller, otherwise, hide vocabulary and show categories menu
                        if (m_CategoriesUI.Visible)
                        {
                            // Back to main menu (App Controller)
                            Hide();
                            AppController.Instance.ShowMainMenu();
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

                if (m_VocabularyUI.Example.Visible)
                {
                    m_VocabularyUI.Example.Hide();
                }
            }
        }

        public void SelectMenu(EMenu menu)
        {
            m_Menu = menu;
                        
            m_VocabularyUI.Example.Hide();
            m_VocabularyUI.Hide();
            m_CategoriesUI.Hide();


            switch (m_Menu)
            {
                case EMenu.Category:

                    SetCategories();
                    m_CategoriesUI.Show();

                break;

                case EMenu.RandomWord:
                    SetRandomWord();
                    m_VocabularyUI.Show();
                break;
                case EMenu.WordDay:
                    // TODO DO LOGIC FOR DIFFERENT WORD EVERYDAY
                    SetRandomWord();
                    m_VocabularyUI.Show();
                break;
            }

            Show();
        }        

        public void OnCategoryPress(int id, int x, int y)
        {
            
            m_CurrentCategory = (ECategory)id;
            m_CurrentWordID = 0;

            SetWordByCategory();

            Show();

            m_VocabularyUI.Example.Hide();
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

        private void SetWordByCategory()
        {

            // Check number of words for this category
            if (m_VocabularySet[(int) m_CurrentCategory].Data.Count > 0)
            {
                m_VocabularyUI.NextWordBtn.Enable(true);
                m_VocabularyUI.NextWordBtn.SetColor(m_EnableBtnColor);
            }else
            {
                m_VocabularyUI.NextWordBtn.Enable(false);
                m_VocabularyUI.NextWordBtn.SetColor(m_DisableBtnColor);
            }

            if (m_Menu == EMenu.RandomWord)
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


                // Set sprite
                m_VocabularyUI.SpriteBtn.Enable(false);
                m_VocabularyUI.SpriteBtn.SetColor(m_DisableBtnColor);
                if (!string.IsNullOrEmpty(m_CurrentWord.SpriteID))
                {
                    Sprite sprite = GetSprite(m_CurrentCategory,m_CurrentWord.SpriteID);
                    if (sprite != null)
                    {
                        m_VocabularyUI.Sprite.SpriteObject = sprite;

                        m_VocabularyUI.SpriteBtn.Enable(true);
                        m_VocabularyUI.SpriteBtn.SetColor(m_EnableBtnColor);
                    }
                }
                
                m_VocabularyUI.Sprite.Hide();


                // Set sentence
                if ((m_CurrentWord.SentencesExamples != null) && (m_CurrentWord.SentencesExamples.Sentence.Count > 0))
                {
                    if (m_CurrentWord.SentencesExamples.Sentence.Count > 1)
                    {
                        m_VocabularyUI.Example.ActiveNextSentenceBtn(true);
                    }
                    else
                    {
                        m_VocabularyUI.Example.ActiveNextSentenceBtn(false);
                    }


                    m_VocabularyUI.ExampleBtn.Enable(true);
                    m_VocabularyUI.ExampleBtn.SetColor(m_EnableBtnColor);

                    // Set sentence
                    m_ICurrentExample = 0;
                    SetExample(m_ICurrentExample);
                }
                else
                {
                    m_VocabularyUI.Example.ActiveNextSentenceBtn(false);
                    m_VocabularyUI.ExampleBtn.Enable(false);
                    m_VocabularyUI.ExampleBtn.SetColor(m_DisableBtnColor);

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
            m_VocabularyUI.Example.KanjiExample = m_CurrentWord.SentencesExamples.Sentence[index];
            if ((m_CurrentWord.SentencesExamples.Hiragana != null) && ((m_CurrentWord.SentencesExamples.Hiragana.Count > 0)))
            {
                m_VocabularyUI.Example.HiraganaExample = m_CurrentWord.SentencesExamples.Hiragana[index];
            }
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
        #endregion SetData

        #region MenuButtons

        public void OnSoundBtn()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                EasyTTSUtil.SpeechFlush(m_CurrentWord.Hiragana);
            }
        }

        public void OnExamplesBtn()
        {
            m_VocabularyUI.Example.Show();
        }

        public void OnSpriteBtn()
        {
            if (m_VocabularyUI.Sprite.Visible)
            {
                m_VocabularyUI.Sprite.Hide();
            }else
            {
                m_VocabularyUI.Sprite.Show();
            }            
        }

        public void OnNextWordBtn()
        {
            if (m_Menu == EMenu.RandomWord)
            {
                SetRandomWord();
            }
            else
            {
                // Increase current word id
                m_CurrentWordID++;
                m_CurrentWordID %= m_VocabularySet[(int) m_CurrentCategory].Data.Count;

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

        #endregion MenuButtons
    }

}
