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

       

        public override void Init()
        {
            base.Init();

            

        }

    }
}
