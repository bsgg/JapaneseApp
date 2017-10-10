using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    [System.Serializable]
    public class Vocabulary
    {
        [SerializeField]
        private List<string> m_Hiragana;
        public List<string> Hiragana
        {
            set { m_Hiragana = value; }
            get { return m_Hiragana; }
        }

        [SerializeField]
        private List<string> m_Romanji;
        public List<string> Romanji
        {
            set { m_Romanji = value; }
            get { return m_Romanji; }
        }

        [SerializeField]
        private List<string> m_Meaning;
        public List<string> Meaning
        {
            set { m_Meaning = value; }
            get { return m_Meaning; }
        }
    }
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
    public class WordVocabulary
    {
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
        private string m_Meaning;
        public string Meaning
        {
            set { m_Meaning = value; }
            get { return m_Meaning; }
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
    public class AnimalsData
    {
        [SerializeField]
        private List<WordVocabulary> m_Animals = new List<WordVocabulary>();
        public List<WordVocabulary> Animals
        {
            get { return m_Animals; }
            set { m_Animals = value; }
        }


        public WordVocabulary GetRandomWord()
        {
            if (m_Animals != null)
            {
                int iRand = Random.Range(0, m_Animals.Count);
                return m_Animals[iRand];
            }

            return null;
        }
    }

    [System.Serializable]
    public class PlacesData
    {
        [SerializeField]
        private List<WordVocabulary> m_Places = new List<WordVocabulary>();
        public List<WordVocabulary> Places
        {
            get { return m_Places; }
            set { m_Places = value; }
        }


        public WordVocabulary GetRandomWord()
        {
            if (m_Places != null)
            {
                int iRand = Random.Range(0, m_Places.Count);
                return m_Places[iRand];
            }

            return null;
        }
    }

    [System.Serializable]
    public class ProfesionsData
    {
        [SerializeField]
        private List<WordVocabulary> m_Profesions = new List<WordVocabulary>();
        public List<WordVocabulary> Profesions
        {
            get { return m_Profesions; }
            set { m_Profesions = value; }
        }


        public WordVocabulary GetRandomWord()
        {
            if (m_Profesions != null)
            {
                int iRand = Random.Range(0, m_Profesions.Count);
                return m_Profesions[iRand];
            }

            return null;
        }
    }

}
