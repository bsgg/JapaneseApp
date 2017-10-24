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
        private Text m_Subtitle;
        public string Subtitle
        {
            set { m_Subtitle.text = value; }
            get { return m_Subtitle.text; }
        }


        [Header("Scroll Dialog")]
        [SerializeField]
        private Text m_ScrollText;
        public string ScrollText
        {
            set { m_ScrollText.text = value; }
            get { return m_ScrollText.text; }
        }

        [SerializeField] private ScrollRect m_DialogScrollRect;
        [SerializeField] private RectTransform  m_ContentDialogScroll;

        public void SetScrollDialog(string text)
        {
            m_ScrollText.text = text;

            m_ContentDialogScroll.sizeDelta = new Vector2(m_ContentDialogScroll.sizeDelta.x, m_ScrollText.preferredHeight);

            m_DialogScrollRect.verticalNormalizedPosition = 1.0f;
        }
    }
}
