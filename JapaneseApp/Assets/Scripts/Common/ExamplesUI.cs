using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class ExamplesUI : UIBase
    {
        [Header("ExamplesUI")]

        [SerializeField]
        private Text m_Sentence;
        public string Sentence
        {
            set { m_Sentence.text = value; }
            get { return m_Sentence.text; }
        }

        [SerializeField]
        private Text m_Romanji;
        public string Romanji
        {
            set { m_Romanji.text = value; }
            get { return m_Romanji.text; }
        }

        [SerializeField]
        private Text m_English;
        public string English
        {
            set { m_English.text = value; }
            get { return m_English.text; }
        }

        [SerializeField]
        private Text m_Kanji;
        public string Kanji
        {
            set { m_Kanji.text = value; }
            get { return m_Kanji.text; }
        }


        [SerializeField]
        private Button m_NextSentenceBtn;
        public Button NextSentenceBtn
        {
            set { m_NextSentenceBtn = value; }
            get { return m_NextSentenceBtn; }
        }

        public void ActiveNextSentenceBtn(bool active)
        {
            m_NextSentenceBtn.gameObject.SetActive(active);
        }


        #region Handles

        public void OnJapanesePlay()
        {

        }

        public void OnEnglishPlay()
        {

        }

        #endregion Handles

    }
}
