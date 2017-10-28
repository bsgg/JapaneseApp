using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace JapaneseApp
{
    public class ExamplesUI : UIBase
    { 
        public delegate void ExampleAction(AppController.EMenu id);
        public ExampleAction OnMenuItemEvent;

        [Header("ExamplesU Text")]
        [SerializeField]
        private Text m_Sentence;
        public string Sentence
        {
            set { m_Sentence.text = value; }
            get { return m_Sentence.text; }
        }

        [SerializeField]
        private Text m_English;
        public string English
        {
            set { m_English.text = value; }
            get { return m_English.text; }
        }

        [SerializeField]
        private Text m_Kanjis;
        public string Kanjis
        {
            set { m_Kanjis.text = value; }
            get { return m_Kanjis.text; }
        }

        [Header("ExamplesU Buttons")]
        [SerializeField]
        private IconBtn m_NextBtn;
        public IconBtn NextBtn
        {
            get { return m_NextBtn; }
        }

        [SerializeField]
        private IconBtn m_KanaBtn;
        public IconBtn KanaBtn
        {
            get { return m_KanaBtn; }
        }

        [SerializeField]
        private IconBtn m_KajiBtn;
        public IconBtn KajiBtn
        {
            get { return m_KajiBtn; }
        }

        [SerializeField]
        private IconBtn m_RomajiBtn;
        public IconBtn RomajiBtn
        {
            get { return m_RomajiBtn; }
        }
        

        #region Handles        

        public void OnMenuItem(int id)
        {
            if (OnMenuItemEvent != null)
            {
                OnMenuItemEvent((AppController.EMenu)id);
            }
        }


        #endregion Handles

    }
}
