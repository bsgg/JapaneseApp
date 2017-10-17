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


        private int m_SelectedGrammar;
        private ECategory m_SelectedCategory;
        private int m_SelectedExample;

        public override void Init()
        {
            base.Init();

            m_GrammarUI.Hide();
            m_GrammarUI.Example.Hide();
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

        private void SetCategories()
        {
            List<string> categories =  new List<string>();

            for (int i = 0; i < (int) ECategory.NUM; i++)
            {
                categories.Add(((ECategory) i).ToString());
            }


            m_CategoriesUI.ScrollMenu.InitScroll(categories);
            m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
        }

        public override void Hide()
        {
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
            base.Hide();
        }

        public void ShowCategories()
        {
            Show();

            SetCategories();

            m_GrammarUI.Example.Hide();
            m_GrammarUI.Hide();
            m_CategoriesUI.Show();
        }


        public void OnCategoryPress(int id)
        {
            m_SelectedCategory = (ECategory) id;
            m_SelectedGrammar = 0;

            SetGrammarByCategory();

            Show();

            m_GrammarUI.Example.Hide();
            m_GrammarUI.Show();
            m_CategoriesUI.Hide();
        }


        private void SetGrammarByCategory()
        {
            // Set Grammar info
            GrammarSection grammar = m_GrammarSet[(int) m_SelectedCategory].Data[m_SelectedGrammar];
            m_GrammarUI.Title = grammar.Title;
            m_GrammarUI.Description = grammar.Description;           

            // Set examples
            if ((grammar.SentencesExamples != null) && (grammar.SentencesExamples.Sentence.Count > 0))
            {

                if (grammar.SentencesExamples.Sentence.Count > 1)
                {
                    m_GrammarUI.Example.NextSentenceButton.SetActive(true);
                }
                else
                {
                    m_GrammarUI.Example.NextSentenceButton.SetActive(false);
                }

                m_GrammarUI.ExampleButton.SetActive(true);

                // Set sentence
                m_SelectedExample = 0;
                    
                //SetExample(m_SelectedExample);

            }
            else
            {
                m_GrammarUI.ExampleButton.SetActive(false);
                m_GrammarUI.Example.NextSentenceButton.SetActive(false);
            }
        }

        private void SetExample(int index)
        {
            GrammarSection grammar = m_GrammarSet[(int) m_SelectedCategory].Data[m_SelectedGrammar];

            if ((index < 0) || (index >= grammar.SentencesExamples.Sentence.Count))
            {
                Debug.Log("<color=cyan> SetExample, Index out of boundaries </color>");
                return;
            }

            // Set sentence
            m_GrammarUI.Example.Sentence = grammar.SentencesExamples.Sentence[index];
            m_GrammarUI.Example.Romanji = grammar.SentencesExamples.Romanji[index];
            m_GrammarUI.Example.English = grammar.SentencesExamples.English[index];


            string[] aKanjis = grammar.SentencesExamples.Kanjis[index].Split('|');
            if (aKanjis.Length >= 1)
            {
                string kanjis = "";
                for (int i = 0; i < aKanjis.Length; i++)
                {
                    kanjis += aKanjis[i];
                    if (i < (aKanjis.Length - 1))
                    {
                        kanjis += "\n";
                    }
                }
                m_GrammarUI.Example.Kanji = kanjis;
            }

        }


        public void OnNextExample()
        {
            // Set next sentence
            m_SelectedExample ++;
            m_SelectedExample %= m_GrammarSet[(int) m_SelectedCategory].Data[m_SelectedGrammar].SentencesExamples.Sentence.Count;

            SetExample(m_SelectedExample);
        }

    }
}
