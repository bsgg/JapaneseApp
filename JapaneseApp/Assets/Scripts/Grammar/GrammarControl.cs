using LitJson;
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
        private string m_Description;
        public string Description
        {
            set { m_Description = value; }
            get { return m_Description; }
        }

        [SerializeField]
        private string m_Vocabulary;
        public string Vocabulary
        {
            set { m_Vocabulary = value; }
            get { return m_Vocabulary; }
        }


        [SerializeField]
        private SentencesExamples m_SentencesExamples = new SentencesExamples();
        public SentencesExamples SentencesExamples
        {
            set { m_SentencesExamples = value; }
            get { return m_SentencesExamples; }
        }
    }

    [System.Serializable]
    public class GrammarData
    {
        [SerializeField]
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [SerializeField]
        private List<GrammarSection> m_Data = new List<GrammarSection>();
        public List<GrammarSection> Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

    }

    #endregion DataModel

    public class GrammarControl : Base
    {
        public enum ECategory { NONE = -1, Numbers, Particles, NUM };

        [SerializeField]
        private string m_DataPath = "Data/Grammar/";

        [SerializeField]
        private List<GrammarData> m_GrammarSet;

        [Header("UI")]
        [SerializeField] private GrammarUI m_GrammarUI;
        [SerializeField] private CategoriesUI m_CategoriesUI;
        [SerializeField] private ExamplesUI m_ExampleUI;

        private int m_SelectedGrammar;
        private ECategory m_SelectedCategory;
        private int m_SelectedExample;

        [SerializeField]
        private Color m_EnableBtnColor;

        [SerializeField]
        private Color m_DisableBtnColor;

        public override void Init()
        {
            base.Init();

            m_GrammarUI.Hide();
            m_ExampleUI.Hide();
            m_CategoriesUI.Hide();


            for (int i = 0; i < (int) ECategory.NUM; i++)
            {
                m_GrammarSet[i] = new GrammarData();

                string category = ((ECategory)i).ToString();

                string path = m_DataPath + category;
                string json = Utility.LoadJSONResource(path);

                if (json != "")
                {
                    m_GrammarSet[i] = JsonMapper.ToObject<GrammarData>(json);
                    m_GrammarSet[i].Name = category;
                }
            }

            m_SelectedGrammar = 0;
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
                m_ExampleUI.OnNextExampleEvent -= OnNextExample;
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

            for (int i = 0; i < (int) ECategory.NUM; i++)
            {
                categories.Add(((ECategory) i).ToString());
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

            m_SelectedCategory = (ECategory) id;
            m_SelectedGrammar = 0;

            SetGrammarByCategory();

            m_ExampleUI.Hide();
            m_GrammarUI.Show();
            m_CategoriesUI.Hide();
        }


        private void SetGrammarByCategory()
        {
            // Set Grammar info
            GrammarSection grammar = m_GrammarSet[(int) m_SelectedCategory].Data[m_SelectedGrammar];
            m_GrammarUI.Title = grammar.Title;
            m_GrammarUI.Description = grammar.Description;           

            // Set number of grammar for this category
            if (m_GrammarSet[(int)m_SelectedCategory].Data.Count > 1)
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
            GrammarSection grammar = m_GrammarSet[(int) m_SelectedCategory].Data[m_SelectedGrammar];

            // Set sentence
            m_ExampleUI.Sentence = grammar.SentencesExamples.GetSentence(index);            
            m_ExampleUI.Romaji = grammar.SentencesExamples.GetRomanji(index);
            m_ExampleUI.English = grammar.SentencesExamples.GetEnglish(index);
            m_ExampleUI.Kanji = grammar.SentencesExamples.GetKanjis(index);

            m_ExampleUI.KanjiExample = grammar.SentencesExamples.GetSentence(index);
            m_ExampleUI.KanaExample = grammar.SentencesExamples.GetKana(index);
        }

        #region MenuButtons

        public void OnExamplesBtn()
        {
            m_ExampleUI.OnNextExampleEvent += OnNextExample;
            m_ExampleUI.Show();
        }

        public void OnNextGrammarBtn()
        {
            // Increase grammar ID
            m_SelectedGrammar++;
            m_SelectedGrammar %=  m_GrammarSet[(int) m_SelectedCategory].Data.Count;

            SetGrammarByCategory();
        }

        public void OnNextExample()
        {
            Debug.Log("<color=cyan> grammar CONTROL OnNextExample </color>");
            // Set next sentence
            m_SelectedExample ++;
            m_SelectedExample %= m_GrammarSet[(int) m_SelectedCategory].Data[m_SelectedGrammar].SentencesExamples.Sentence.Count;

            SetExample(m_SelectedExample);
        }

        #endregion MenuButtons
    }
}
