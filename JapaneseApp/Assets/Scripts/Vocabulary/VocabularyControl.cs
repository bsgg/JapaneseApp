using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class VocabularyControl : Base
    {
        private string m_AnimalsPathData = "Data/Vocabulary/Animals";

        [SerializeField] private AnimalsData m_AnimalsData;

        private string m_PlacesPathData = "Data/Vocabulary/Places";

        [SerializeField]
        private PlacesData m_PlacesData;

        [SerializeField]
        private VocabularyUI m_VocabularyUI;


        private WordVocabulary m_CurrentWord;
        private int m_ICurrentExample;

        public override void Init()
        {
            base.Init();

            m_VocabularyUI.Hide();
            m_VocabularyUI.Example.Hide();

            // Init vocabulary data
            m_AnimalsData = new AnimalsData();
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

        }

        private void SetRandomWord()
        {
            int iSection = Random.Range(0,2);
            switch (iSection)
            {
                case 0:
                    m_CurrentWord = m_AnimalsData.GetRandomWord();
                    break;
                case 1:
                    m_CurrentWord = m_PlacesData.GetRandomWord();
                    break;
            }


            if (m_CurrentWord != null)
            {
                // Set word
                m_VocabularyUI.Word = m_CurrentWord.Word;
                m_VocabularyUI.English = m_CurrentWord.Meaning;
                m_VocabularyUI.Hiragana = m_CurrentWord.Hiragana + " : " + m_CurrentWord.Romanji;


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
