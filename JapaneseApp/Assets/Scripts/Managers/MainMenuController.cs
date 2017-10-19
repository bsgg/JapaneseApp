using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class MainMenuController : Base
    {
        [SerializeField] private UIBase m_MainMenu;
        

        public override void Show()
        {
            base.Show();
            m_MainMenu.Show();
        }

        public override void Hide()
        {
            base.Hide();
            m_MainMenu.Hide();
        }

        public override void Finish()
        {
            base.Finish();

            m_MainMenu.Hide();
        }
    }
}
