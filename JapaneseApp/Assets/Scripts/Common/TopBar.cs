using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class TopBar : UIBase
    {
        [SerializeField]
        private Text m_Title;

        public string Title
        {
            get { return m_Title.text; }
            set { m_Title.text = value; }
        }

        [SerializeField]
        private GameObject m_CloseBtn;
        public GameObject CloseBtn
        {
            get { return m_CloseBtn; }
            set { m_CloseBtn = value; }
        }
    }
}
