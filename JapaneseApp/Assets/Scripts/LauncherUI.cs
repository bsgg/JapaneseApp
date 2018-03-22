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

        

        [SerializeField] private GameObject m_ProgressBar;
        public GameObject ProgressBar
        {
            get { return m_ProgressBar; }
        }

        [SerializeField] private Image m_ProgressValue;
        public float ProgressValue
        {
            set { m_ProgressValue.fillAmount = value; }
            get { return m_ProgressValue.fillAmount; }
        }

        [SerializeField]
        private Text m_ContentText;
        public string ContentText
        {
            set { m_ContentText.text = value; }
            get { return m_ContentText.text; }
        }

        [SerializeField] private Button[] m_MenuButtons;
        public void ActiveButtons(bool active)
        {
            for (int i=0; i < m_MenuButtons.Length; i++)
            {
                m_MenuButtons[i].interactable = active;
            }
        }

    }
}