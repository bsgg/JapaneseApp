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

        [SerializeField]
        private Text m_Dialog;
        public string Dialog
        {
            set { m_Dialog.text = value; }
            get { return m_Dialog.text; }
        }
    }
}
