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
        private string m_KanjiExample;
        public string KanjiExample
        {
            set { m_KanjiExample = value; }
            get { return m_KanjiExample; }
        }

        [SerializeField]
        private string m_HiraganaExample;
        public string HiraganaExample
        {
            set { m_HiraganaExample = value; }
            get { return m_HiraganaExample; }
        }

        private bool m_ToggleToKanjiExample = true;


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

        public override void Show()
        {
            base.Show();

            m_Sentence.text = m_KanjiExample;
            m_ToggleToKanjiExample = false;
        }

        public override void Hide()
        {
            base.Hide();
            m_Sentence.text = m_KanjiExample;
            m_ToggleToKanjiExample = false;
        }

        #region Handles

        public void OnSentencePress()
        {
            if (m_ToggleToKanjiExample)
            {
                m_Sentence.text = m_KanjiExample;
                m_ToggleToKanjiExample = false;

            } else
            {
                m_Sentence.text = m_HiraganaExample;
                m_ToggleToKanjiExample = true;
            }
        }

        public void OnJapanesePlay()
        {

        }

        public void OnEnglishPlay()
        {

        }

        #endregion Handles

    }
}
