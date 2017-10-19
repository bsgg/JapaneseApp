using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class HiraganaControl : Base
    {
        [SerializeField]
        private HiraganaUI m_HiraganaUI;

        public override void Init()
        {
            base.Init();

            m_HiraganaUI.Init();

            m_HiraganaUI.Hide();
        }

        public override void Back()
        {
            base.Back();
        }

        public override void Show()
        {
            base.Show();
            m_HiraganaUI.Show();
        }

        public override void Hide()
        {
            base.Hide();
            m_HiraganaUI.Hide();
        }

    }
}
