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
        public enum ECategory { NONE = -1, ANIMALS, PROFESIONS,  NUM };


        [SerializeField]
        private List<VocabularyData> m_DataSet;
        

        [SerializeField]
        private VocabularyUI m_VocabularyUI;


        private WordVocabulary m_CurrentWord;
        private int m_ICurrentExample;

        public override void Init()
        {
            base.Init();

            m_VocabularyUI.Hide();
            m_VocabularyUI.Example.Hide();

            for (int i= 0; i< m_DataSet.Count; i++)
            {
                m_DataSet[i].WordData = new WordData();

                string json = Utility.LoadJSONResource( m_DataSet[i].DataPath);
                if (json != "")
                {
                    m_DataSet[i].WordData = JsonMapper.ToObject<WordData>(json);
                }
            }

            // Init vocabulary data
           /* m_AnimalsData = new AnimalsData();
            string jsonActionsString = Utility.LoadJSONResource(m_AnimalsPathData);
            if (jsonActionsString != "")
            {
                m_AnimalsData = JsonMapper.ToObject<AnimalsData>(jsonActionsString);
            }


            m_PlacesData = new PlacesData();
            jsonActionsString = Utility.LoadJSONResource(m_PlacesPathData);
            if (jsonActionsString != "")
            {
                m_PlacesData = JsonMapper.ToObject<PlacesData>(jsonActionsString);
            }

            m_ProfesionsData = new ProfesionsData();
            jsonActionsString = Utility.LoadJSONResource(m_ProfesionsPathData);
            if (jsonActionsString != "")
            {
                m_ProfesionsData = JsonMapper.ToObject<ProfesionsData>(jsonActionsString);
            }*/
            

        }

        private int m_CurrentDataID;

        private void SetRandomWord()
        {
            // Get random word from a category
            m_CurrentDataID = Random.Range(0,(int)ECategory.NUM);
            m_CurrentWord = m_DataSet[m_CurrentDataID].WordData.GetRandomWord();
           

            if (m_CurrentWord != null)
            {
                // Set word
                m_VocabularyUI.Word = m_CurrentWord.Word;
                m_VocabularyUI.English = m_CurrentWord.Meaning;
                m_VocabularyUI.Hiragana = m_CurrentWord.Hiragana + " : " + m_CurrentWord.Romanji;

                if (!string.IsNullOrEmpty(m_CurrentWord.SpriteID))
                {
                    m_VocabularyUI.Picture.sprite = m_DataSet[m_CurrentDataID].SpriteByKey(m_CurrentWord.SpriteID);
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

        public override void Show()
        {
            SetRandomWord();

            base.Show();

            m_VocabularyUI.Example.Hide();
            m_VocabularyUI.Show();
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
            SetRandomWord();
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
