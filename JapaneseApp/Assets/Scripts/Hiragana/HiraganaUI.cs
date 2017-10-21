using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace JapaneseApp
{
    public class HiraganaUI : UIBase
    {
        [SerializeField]
        private GameObject m_ScrollContent;
        public GameObject ScrollContent
        {
            get { return m_ScrollContent; }
            set { m_ScrollContent = value; }
        }

        [SerializeField]
        private SimpleExampleUI m_ExampleUI;
        public SimpleExampleUI ExampleUI
        {
            get { return m_ExampleUI; }
            set { m_ExampleUI = value; }
        }

        public override void Show()
        {
            m_ExampleUI.Hide();
            base.Show();
        }

        public override void Hide()
        {
            m_ExampleUI.Hide();
            base.Hide();
        }

    }
}
