using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace JapaneseApp
{
    public class ExamplesUI : UIBase
    {
        public delegate void ExampleAction();
        public ExampleAction OnNextExampleEvent;

        [Header("ExamplesU Text")]
        [SerializeField]
        private Text m_Sentence;
        public string Sentence
        {
            set { m_Sentence.text = value; }
            get { return m_Sentence.text; }
        }

        // Sentence with kanji symbols
        private string m_KanjiExample;
        public string KanjiExample
        {
            set { m_KanjiExample = value; }
            get { return m_KanjiExample; }
        }

        // Sentence with only hiragana symbols
        private string m_KanaExample;
        public string KanaExample
        {
            set { m_KanaExample = value; }
            get { return m_KanaExample; }
        }

        private bool m_ToggleToKanjiExample = true;


        [SerializeField]
        private Text m_Romaji;
        public string Romaji
        {
            set { m_Romaji.text = value; }
            get { return m_Romaji.text; }
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

        [Header("ExamplesU Buttons")]
        [SerializeField]
        private IconBtn m_NextBtn;
        public IconBtn NextBtn
        {
            get { return m_NextBtn; }
        }

        [SerializeField]
        private IconBtn m_SoundBtn;
        public IconBtn SoundBtn
        {
            get { return m_SoundBtn; }
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
            if (string.IsNullOrEmpty(m_KanaExample) || string.IsNullOrEmpty(m_KanjiExample)) return;

            if (m_ToggleToKanjiExample)
            {
                m_Sentence.text = m_KanjiExample;
                m_ToggleToKanjiExample = false;

            } else
            {
                m_Sentence.text = m_KanaExample;
                m_ToggleToKanjiExample = true;
            }
        }

        public void OnNextExamplePress()
        {
            if (OnNextExampleEvent != null)
            {
                OnNextExampleEvent();
            }
        }

        public void OnSoundPlay()
        {
            string debug = "[ExamplesUI.OnSoundPlay]";

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                debug += " Call SpeechFlush, Romaji: " + m_Romaji.text;

                EasyTTSUtil.SpeechFlush(m_Romaji.text);
            }

            AppController.Instance.DebugUI.Log0 = debug;
        }

        #endregion Handles

    }
}
