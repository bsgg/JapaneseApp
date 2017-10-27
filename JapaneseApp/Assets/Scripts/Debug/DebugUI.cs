using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class DebugUI : UIBase
    {
        [SerializeField]
        private Text m_Log0;
        public string Log0
        {
            set { m_Log0.text = value; }
            get { return m_Log0.text; }
        }
    }
}
