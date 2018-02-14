using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    #region DataModel
    [System.Serializable]
    public class GrammarSection
    {
        [SerializeField]
        private string m_Title;
        public string Title
        {
            set { m_Title = value; }
            get { return m_Title; }
        }

        [SerializeField]
        private List<string> m_Description;
        public List<string> Description
        {
            set { m_Description = value; }
            get { return m_Description; }
        }
        
        [SerializeField]
        private SentencesExamples m_SentencesExamples = new SentencesExamples();
        public SentencesExamples SentencesExamples
        {
            set { m_SentencesExamples = value; }
            get { return m_SentencesExamples; }
        }
    }

    [Serializable]
    public class GrammarData
    {
        public string Title;
        public string FileName;
        public List<GrammarSection> Data;

        public GrammarData()
        {
            Data = new List<GrammarSection>();
        }

    }

    #endregion DataModel

    public class GrammarControl : Base
    {
        [SerializeField]
        private string m_DataPath = "Data/Grammar/";

        [SerializeField]
        private List<GrammarData> m_GrammarSet;

        [Header("UI")]
        [SerializeField] private GrammarUI m_GrammarUI;
        [SerializeField] private CategoriesUI m_CategoriesUI;
        [SerializeField] private ExamplesUI m_ExampleUI;

        private int m_SelectedGrammar;
        private int m_SelectedCategory;
        private int m_SelectedExample;

        [SerializeField]
        private Color m_EnableBtnColor;

        [SerializeField]
        private Color m_DisableBtnColor;

        public override IEnumerator Initialize()
        {
           

            m_GrammarUI.Hide();
            m_ExampleUI.Hide();
            m_CategoriesUI.Hide();

            //m_GrammarSet = new List<GrammarData>();
            for (int i = 0; i < m_GrammarSet.Count; i++)
            {
                GrammarSection grammar = new GrammarSection();

                // string category = ((ECategory)i).ToString();
                string fileName = m_GrammarSet[i].FileName;
                string title = m_GrammarSet[i].Title;
                string path = m_DataPath + fileName;
                string json = Utility.LoadJSONResource(path);

                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        m_GrammarSet[i] = JsonMapper.ToObject<GrammarData>(json);
                        m_GrammarSet[i].FileName = fileName;
                        m_GrammarSet[i].Title = title;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[GrammarControl.Init] Bad Format JSON File: " + path);
                    }
                }
                else
                {
                    Debug.Log("[GrammarControl.Init] JSON not found: " + path);
                }
            }

            m_SelectedGrammar = 0;

            yield break;
        }


        public override void Hide()
        {
            m_CategoriesUI.Hide();
            m_ExampleUI.Hide();
            base.Hide();
        }

        public override void Show()
        {
            m_CategoriesUI.Hide();
            m_ExampleUI.Hide();
            base.Show();
        }


        public override void Back()
        {
            base.Back();

            // If example is visible, hide it, otherwise check type menu
            if (m_ExampleUI.Visible)
            {
                m_ExampleUI.OnMenuItemEvent -= OnExampleMenuItem;
                m_ExampleUI.Hide();
            }
            else if (m_CategoriesUI.Visible)
            {
                m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
                m_CategoriesUI.Hide();
                Hide();
                AppController.Instance.ShowMainMenu();
            }
            else
            {
                m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
                m_CategoriesUI.Show();
            }
        }       

       
        public void ShowCategories()
        {
            Show();

            List<string> categories =  new List<string>();

            for (int i = 0; i < m_GrammarSet.Count; i++)
            {
                categories.Add(m_GrammarSet[i].Title);
            }

            m_CategoriesUI.ScrollMenu.InitScroll(categories);

            m_ExampleUI.Hide();
            m_GrammarUI.Hide();

            m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
            m_CategoriesUI.Show();
        }


        public void OnCategoryPress(int id, int x, int y)
        {
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;

            Debug.Log("[GrammarControl] OnCategoryPress");

            m_SelectedCategory = id;
            m_SelectedGrammar = 0;

            SetGrammarByCategory();

            m_ExampleUI.Hide();
            m_GrammarUI.Show();
            m_CategoriesUI.Hide();

        }


        private void SetGrammarByCategory()
        {
            // Set Grammar info
            GrammarSection grammar = m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar];
            m_GrammarUI.Title = grammar.Title;

            if (grammar.Description != null)
            {
                string desc = "";

                for (int i=0; i< grammar.Description.Count; i++ )
                {
                    desc += "\n\n" + grammar.Description[i];
                }
                m_GrammarUI.Description = desc;
            }

            // Set number of grammar for this category
            if (m_GrammarSet[m_SelectedCategory].Data.Count > 1)
            {
                m_GrammarUI.NextBtn.Enable(true, m_EnableBtnColor);
            }else
            {
                m_GrammarUI.NextBtn.Enable(false, m_DisableBtnColor);
            }


            // Set examples
            if ((grammar.SentencesExamples != null) && (grammar.SentencesExamples.Sentence.Count > 0))
            {
                if (grammar.SentencesExamples.Sentence.Count > 1)
                {
                    m_ExampleUI.NextBtn.Enable(true, m_EnableBtnColor);
                }
                else
                {
                    m_ExampleUI.NextBtn.Enable(false, m_DisableBtnColor);
                }

                m_GrammarUI.ExampleBtn.Enable(true, m_EnableBtnColor);

                // Set sentence
                m_SelectedExample = 0;
                    
                SetExample(m_SelectedExample);

            }
            else
            {
                m_GrammarUI.ExampleBtn.Enable(false, m_DisableBtnColor);
                m_ExampleUI.NextBtn.Enable(false, m_DisableBtnColor);
            }
        }

        private void SetExample(int index)
        {
            GrammarSection grammar = m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar];

            // Set sentence
            m_ExampleUI.Sentence = grammar.SentencesExamples.GetSentence(index);            
            m_ExampleUI.English = grammar.SentencesExamples.GetEnglish(index);
            m_ExampleUI.Kanjis = grammar.SentencesExamples.GetKanjis(index);
        }

        #region MenuButtons

        public void OnExamplesBtn()
        {
            m_ExampleUI.OnMenuItemEvent += OnExampleMenuItem;
            m_ExampleUI.Show();
        }

        public void OnDescriptionBtn()
        {
            m_GrammarUI.Description = "";
            if (m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar].Description != null)
            {
                string desc = "";

                for (int i = 0; i < m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar].Description.Count; i++)
                {
                    desc += m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar].Description[i] + "\n\n";
                }
                m_GrammarUI.Description = desc;
            }

        }

        public void OnNextGrammarBtn()
        {
            // Increase grammar ID
            m_SelectedGrammar++;
            m_SelectedGrammar %=  m_GrammarSet[m_SelectedCategory].Data.Count;

            SetGrammarByCategory();
        }

        private void OnExampleMenuItem(AppController.EMenu id)
        {
            Debug.Log("<color=cyan> [GrammarControl.OnExampleMenuItem] id: " + id.ToString() + "</color>");

            switch (id)
            {
                case AppController.EMenu.NEXT:
                    // Set next sentence
                    m_SelectedExample++;
                    m_SelectedExample %= m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar].SentencesExamples.Sentence.Count;

                    SetExample(m_SelectedExample);
                break;

                case AppController.EMenu.KANJI:
                    m_ExampleUI.Sentence = m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar].SentencesExamples.Sentence[m_SelectedExample];
                break;
                case AppController.EMenu.KANA:
                    m_ExampleUI.Sentence = m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar].SentencesExamples.Kana[m_SelectedExample];
                    break;
                case AppController.EMenu.ROMAJI:
                    m_ExampleUI.Sentence = m_GrammarSet[m_SelectedCategory].Data[m_SelectedGrammar].SentencesExamples.Romaji[m_SelectedExample];
                break;
            }
        }

        #endregion MenuButtons
    }
}
