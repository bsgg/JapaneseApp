﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class GrammarUI : UIBase
    {
        [Header("GrammarUI")]

        [SerializeField]
        private ExamplesUI m_Example;
        public ExamplesUI Example
        {
            set { m_Example = value; }
            get { return m_Example; }
        }

        [SerializeField]
        private Text m_Title;
        public string Title
        {
            set { m_Title.text = value; }
            get { return m_Title.text; }
        }


        [SerializeField]
        private Text m_Description;
        public string Description
        {
            set { m_Description.text = value; }
            get { return m_Description.text; }
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