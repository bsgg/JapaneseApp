using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class DialogUI : UIBase
    {
        [Header("DialogUI")]

        [SerializeField]
        private Text m_Title;
        public string Title
        {
            set { m_Title.text = value; }
            get { return m_Title.text; }
        }

        // Sentence with kanji symbols
        private string m_Dialog;
        public string Dialog
        {
            set { m_Dialog = value; }
            get { return m_Dialog; }
        }
    }
}
