using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class HiraganaScrollMenu : UIBase
    {
        [SerializeField] private ScrollPanelUI m_ScrollHiraganaMenu;

        public ScrollPanelUI ScrollMenu
        {
            get { return m_ScrollHiraganaMenu; }
            set { m_ScrollHiraganaMenu = value; }
        }

        public void InitScrollMenu(List<string> lTitle)
        {
            m_ScrollHiraganaMenu.InitScroll(lTitle);
        }
    }
}
