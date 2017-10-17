using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    [System.Serializable]
    public class GrammarSection
    {
        [SerializeField]
        private string m_Title;
        public string Title
        {
            set { m_Title = value; }
            get { return m_Title; }
        }

        [SerializeField]
        private string m_Description;
        public string Description
        {
            set { m_Description = value; }
            get { return m_Description; }
        }

        [SerializeField]
        private string m_Vocabulary;
        public string Vocabulary
        {
            set { m_Vocabulary = value; }
            get { return m_Vocabulary; }
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
    public class GrammarData
    {
        [SerializeField]
        private List<GrammarSection> m_Data = new List<GrammarSection>();
        public List<GrammarSection> Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
    }

}
