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
        private ExamplesUI m_Example;
        public ExamplesUI Example
        {
            set { m_Example = value; }
            get { return m_Example; }
        }


        public override void Show()
        {
            m_Example.Hide();
            base.Show();

        }

        public override void Hide()
        {
            m_Example.Hide();
            base.Hide();
        }

    }
}
