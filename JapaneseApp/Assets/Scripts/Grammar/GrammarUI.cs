using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace JapaneseApp
{
    public class GrammarUI : UIBase
    {
        [Header("GrammarUI")]

        [SerializeField]
        private Text m_Title;
        public string Title
        {
            set { m_Title.text = value; }
            get { return m_Title.text; }
        }


        [SerializeField]
        private Text m_Description;
        public string Description
        {
            set { m_Description.text = value; }
            get { return m_Description.text; }
        }

        [SerializeField]
        private IconBtn m_ExampleBtn;
        public IconBtn ExampleBtn
        {
            get { return m_ExampleBtn; }
        }

        [SerializeField]
        private IconBtn m_NextBtn;
        public IconBtn NextBtn
        {
            get { return m_NextBtn; }
        }

    }
}
