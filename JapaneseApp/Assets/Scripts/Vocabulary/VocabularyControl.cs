using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    [System.Serializable]
    public class VocabularyData
    {
        public string Name = "";
        public VocabularyControl.ECategory Category;
        public string DataPath = "";
        public WordData WordData;

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
        public enum ECategory { NONE = -1, ANIMALS, PROFESIONS, NUMBERS, NUM };

        [SerializeField]
        private List<VocabularyData> m_VocabularySet;


        [SerializeField]
        private VocabularyUI m_VocabularyUI;
        [SerializeField]
        CategoriesUI m_CategoriesUI;


        private WordVocabulary m_CurrentWord;
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

            List<string> categories =  new List<string>();

            for (int i= 0; i< m_VocabularySet.Count; i++)
            {
                m_VocabularySet[i].WordData = new WordData();

                string json = Utility.LoadJSONResource( m_VocabularySet[i].DataPath);
                if (json != "")
                {
                    m_VocabularySet[i].WordData = JsonMapper.ToObject<WordData>(json);
                }

                categories.Add(m_VocabularySet[i].Name);

            }

            m_CategoriesUI.ScrollMenu.InitScroll(categories);
            m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;            

        }
       
        public void ShowCategories()
        {
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


            Debug.Log("OnCategoryPress: " + id + " : " + m_CurrentCategory.ToString());

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
                m_CurrentWord = m_VocabularySet[(int)m_CurrentCategory].WordData.GetRandomWord();
            }else
            {
                m_CurrentWord = m_VocabularySet[(int)m_CurrentCategory].WordData.GetWordById(m_CurrentWordID);
            }
            if (m_CurrentWord != null)
            {
                // Set word
                m_VocabularyUI.Word = m_CurrentWord.Word;
                m_VocabularyUI.English = m_CurrentWord.Meaning;
                m_VocabularyUI.Hiragana = m_CurrentWord.Hiragana + " : " + m_CurrentWord.Romanji;

                if (!string.IsNullOrEmpty(m_CurrentWord.SpriteID))
                {
                    m_VocabularyUI.Picture.sprite = m_VocabularySet[(int)m_CurrentCategory].SpriteByKey(m_CurrentWord.SpriteID);
                    m_VocabularyUI.Picture.preserveAspect = true;
                    m_VocabularyUI.PictureObject.gameObject.SetActive(true);
                }
                else
                {
                    m_VocabularyUI.PictureObject.gameObject.SetActive(false);
                }

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
                m_CurrentWordID %= m_VocabularySet[(int)m_CurrentCategory].WordData.Data.Count;

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

        #endregion Handles
    }

}
