using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class LauncherUI : UIBase
    {


        [SerializeField]
        private Button m_AcceptButton;
        public Button AcceptButton
        {
            set { m_AcceptButton = value; }
            get { return m_AcceptButton; }
        }

        [SerializeField] private Image m_Progress;
        public float Progress
        {
            set { m_Progress.fillAmount = value; }
            get { return m_Progress.fillAmount; }
        }

        [SerializeField]
        private Text m_ContentText;
        public string ContentText
        {
            set { m_ContentText.text = value; }
            get { return m_ContentText.text; }
        }

    }
}