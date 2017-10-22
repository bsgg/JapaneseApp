using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace JapaneseApp
{
    #region DataModel

    [System.Serializable]
    public class HiraganaData
    {        

        [SerializeField]
        private List<VWord> m_Data = new List<VWord>();
        public List<VWord> Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        private string[,] m_HiraganaChar;
        public string[,] HiraganaChar
        {
            get { return m_HiraganaChar; }
            set { m_HiraganaChar = value; }
        }
        private string[,] m_RomanjiChar;
        public string[,] RomanjiChar
        {
            get { return m_RomanjiChar; }
            set { m_RomanjiChar = value; }
        }
    }

    #endregion DataModel

    public class HiraganaControl : Base
    {
        [SerializeField]
        private HiraganaUI m_HiraganaUI;
        

        [SerializeField]
        private ButtonText[] m_ListButtonText;

        [SerializeField]
        private string m_DataPath = "Data/Hiragana/Hiragana1";

        [SerializeField]
        private HiraganaData m_HiraganaSet;

        private string m_DataPathKat = "Data/Hiragana/Katakana";
        [SerializeField]
        private HiraganaData m_KatakanaSet;

        private VWord m_SelectedHiragana;
        private int m_SelectedExample;

        public override void Init()
        {
            base.Init();

            // Test Katakana
            m_KatakanaSet = new HiraganaData();
            string path1 = m_DataPathKat;
            string json1 = Utility.LoadJSONResource(path1);
            if (json1 != "")
            {
                m_KatakanaSet = JsonMapper.ToObject<HiraganaData>(json1);

            }


            // Load the data
            m_HiraganaSet = new HiraganaData();
           
            string path = m_DataPath;
            string json = Utility.LoadJSONResource(path);
            if (json != "")
            {
                m_HiraganaSet = JsonMapper.ToObject<HiraganaData>(json);

            }

            m_ListButtonText = m_HiraganaUI.ScrollContent.GetComponentsInChildren<ButtonText>();
            for (int i = 0; i < m_ListButtonText.Length; i++)
            {
                m_ListButtonText[i].TextButton = "";
            }
            

            m_HiraganaSet.HiraganaChar = new string[m_HiraganaSet.Data.Count, 5];
            m_HiraganaSet.RomanjiChar = new string[m_HiraganaSet.Data.Count, 5];

            for (int i=0; i<m_HiraganaSet.Data.Count; i++)
            {

                string[] splitH = m_HiraganaSet.Data[i].Hiragana.Split('_');
                string[] splitE = m_HiraganaSet.Data[i].Romanji.Split('_');

                // Both must have 5 elements
                if ((splitH != null) && (splitE != null) && (splitH.Length >= 5) && (splitE.Length >= 5))
                {
                    for (int j= 0; j< 5; j++)
                    {
                        string h = splitH[j];
                        string r = splitE[j];

                        m_HiraganaSet.HiraganaChar[i, j] = h;
                        m_HiraganaSet.RomanjiChar[i, j] = r;

                        int id = 5 * i + j;
                        if ((h != "-") && (r != "-"))
                        {
                            m_ListButtonText[id].ButtonComponent.enabled = true;
                            Color32 cButton = m_ListButtonText[id].ButtonComponent.targetGraphic.color;
                            cButton.a = 255;
                            m_ListButtonText[id].ButtonComponent.targetGraphic.color = cButton;

                            string text = r + " : " + h;
                            m_ListButtonText[id].TextButton = text;
                            m_ListButtonText[id].ID = id;
                            m_ListButtonText[id].X = i;
                            m_ListButtonText[id].Y = j;
                            m_ListButtonText[id].OnButtonPress += OnItemButtonPress;

                        }
                        else
                        {
                            m_ListButtonText[id].ButtonComponent.enabled = false;
                            Color32 cButton = m_ListButtonText[id].ButtonComponent.targetGraphic.color;
                            cButton.a = 0;
                            m_ListButtonText[id].ButtonComponent.targetGraphic.color = cButton;

                        }
                    }
                }
                else
                {
                    Debug.Log("<color=cyan>" + "Wrong Format: " + m_HiraganaSet.Data[i].Hiragana + " - " + m_HiraganaSet.Data[i].Romanji + "</color>");
                }
            }

            // Set table

            m_HiraganaUI.Init();

            m_HiraganaUI.Hide();
        }

        public override void Back()
        {
            base.Back();

            if (m_HiraganaUI.ExampleUI.Visible)
            {
                m_HiraganaUI.ExampleUI.Hide();
            }else
            {
                Hide();
                AppController.Instance.ShowMainMenu();
            }
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

        public void OnItemButtonPress(int id, int x, int y)
        {
            Debug.Log("Item: " + id + " (" + x + "," + y + ") m_ListButtonText[id].TextButton" + m_ListButtonText[id].TextButton);

            Debug.Log("H:" + m_HiraganaSet.HiraganaChar[x, y]  + ", R: " + m_HiraganaSet.RomanjiChar[x, y]);

            if (m_HiraganaSet == null)
            {
                Debug.Log("<color=cyan> SetExample,  m_CurrentHiragana null </color>");
                return;
            }

            // X has the current hiragana (ROW in the json)
            if ((x >= m_HiraganaSet.Data.Count) || (x < 0))
            {
                Debug.Log("<color=cyan> SetExample, Index out of boundaries </color>");
                return;
            }


            m_SelectedHiragana = m_HiraganaSet.Data[x];
            SetExample(0);
            m_HiraganaUI.ExampleUI.Show();
        }
               

        private void SetExample(int index)
        {
            if (m_SelectedHiragana == null)
            {
                Debug.Log("<color=cyan> SetExample,  m_CurrentHiragana null </color>");
                return;
            }

            if ((index >= m_SelectedHiragana.SentencesExamples.Sentence.Count) || (index < 0))
            {
                Debug.Log("<color=cyan> SetExample, Index out of boundaries </color>");
                return;
            }

            m_SelectedExample = index;

            // Set sentence
            string examples = string.Empty;
            for (int i= 0; i< m_SelectedHiragana.SentencesExamples.Sentence.Count; i++ )
            {
                examples += " - " + m_SelectedHiragana.SentencesExamples.Sentence[i] + " (" + m_SelectedHiragana.SentencesExamples.Romanji[i] + ") = " + m_SelectedHiragana.SentencesExamples.English[i];
                m_HiraganaUI.ExampleUI.HiraganaExample += m_SelectedHiragana.SentencesExamples.Romanji[i] + " ";
                if (i < (m_SelectedHiragana.SentencesExamples.Sentence.Count -1))
                {
                    examples += "\n";
                }
            }
            m_HiraganaUI.ExampleUI.Example = examples;         
        }


        public void OnNextExample()
        {
            Debug.Log("<color=cyan> HIRAGANA CONTROL OnNextExample </color>");

            if (m_SelectedHiragana == null)
            {
                Debug.Log("<color=cyan> No Current Word </color>");
                return;
            }


            // Set next sentence
            m_SelectedExample++;
            m_SelectedExample %= m_SelectedHiragana.SentencesExamples.Sentence.Count;

            SetExample(m_SelectedExample);
        }


    }
}
