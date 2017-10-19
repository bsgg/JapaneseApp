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

        [SerializeField]
        private ButtonText[] m_ListButtonText;

        public override void Init()
        {
            base.Init();

            m_ListButtonText = m_ScrollContent.GetComponents<ButtonText>();

        }

    }
}
