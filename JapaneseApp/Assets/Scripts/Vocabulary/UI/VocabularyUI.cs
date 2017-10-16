﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class VocabularyUI : UIBase
    {
        [Header("VocabularyUI")]

        [SerializeField]
        private ExamplesUI m_Example;
        public ExamplesUI Example
        {
            set { m_Example = value; }
            get { return m_Example; }
        }

        [SerializeField]
        private Text m_English;
        public string English
        {
            set { m_English.text = value; }
            get { return m_English.text; }
        }


        [SerializeField]
        private Text m_Word;
        public string Word
        {
            set { m_Word.text = value; }
            get { return m_Word.text; }
        }


        [SerializeField]
        private SpriteUI m_Sprite;
        public SpriteUI Sprite
        {
            set { m_Sprite = value; }
            get { return m_Sprite; }
        }
        

        [SerializeField]
        private Text m_Hiragana;
        public string Hiragana
        {
            set { m_Hiragana.text = value; }
            get { return m_Hiragana.text; }
        }

        [SerializeField]
        private GameObject m_ExampleButton;
        public GameObject ExampleButton
        {
            set { m_ExampleButton = value; }
            get { return m_ExampleButton; }
        }

        public override void Show()
        {
            base.Show();

            m_Example.Hide();

        }
    }
}
