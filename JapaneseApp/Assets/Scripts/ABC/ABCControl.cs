using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace JapaneseApp
{
    #region DataModel

    [System.Serializable]
    public class ABCData
    {
        [SerializeField]
        private List<VWord> m_Data = new List<VWord>();
        public List<VWord> Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        private string[,] m_SymbolChar;
        public string[,] SymbolChar
        {
            get { return m_SymbolChar; }
            set { m_SymbolChar = value; }
        }
        private string[,] m_RomanjiChar;
        public string[,] RomanjiChar
        {
            get { return m_RomanjiChar; }
            set { m_RomanjiChar = value; }
        }
    }

    #endregion DataModel

    public class ABCControl : Base
    {
        [SerializeField]
        private ABCUI m_ABCCharUI;          

        [SerializeField]
        private ButtonText[] m_ListButtonText;

        [SerializeField]
        private string m_DataPath = "Data/Hiragana/";

        [SerializeField]
        private ABCData m_ABCSet;        

        private VWord m_SelectedABC;
        private int m_SelectedExample;

        public override void Init()
        {
            base.Init();            

            // Load the data
            m_ABCSet = new ABCData();
           
            string path = m_DataPath;
            string json = Utility.LoadJSONResource(path);
            if (json != "")
            {
                m_ABCSet = JsonMapper.ToObject<ABCData>(json);

            }

            m_ListButtonText = m_ABCCharUI.ScrollContent.GetComponentsInChildren<ButtonText>();
            for (int i = 0; i < m_ListButtonText.Length; i++)
            {
                m_ListButtonText[i].TextButton = "";
            }

            m_ABCSet.SymbolChar = new string[m_ABCSet.Data.Count, 5];
            m_ABCSet.RomanjiChar = new string[m_ABCSet.Data.Count, 5];

            int lastButtonId = -1;
            for (int i=0; i< m_ABCSet.Data.Count; i++)
            {
                string[] splitH = m_ABCSet.Data[i].Kana.Split('_');
                string[] splitE = m_ABCSet.Data[i].Romaji.Split('_');

                // Both must have 5 elements
                if ((splitH != null) && (splitE != null) && (splitH.Length >= 5) && (splitE.Length >= 5))
                {
                    for (int j= 0; j< 5; j++)
                    {
                        string h = splitH[j];
                        string r = splitE[j];

                        m_ABCSet.SymbolChar[i, j] = h;
                        m_ABCSet.RomanjiChar[i, j] = r;

                        int id = 5 * i + j;
                        lastButtonId = id;
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
                    Debug.Log("<color=cyan>" + "Wrong Format: " + m_ABCSet.Data[i].Kana + " - " + m_ABCSet.Data[i].Romaji + "</color>");
                }
            }

            // Disable rest of the buttons
            for (int i = (lastButtonId+1); i < m_ListButtonText.Length; i++)
            {
                m_ListButtonText[i].TextButton = "";
                m_ListButtonText[i].ButtonComponent.enabled = false;
                Color32 cButton = m_ListButtonText[i].ButtonComponent.targetGraphic.color;
                cButton.a = 0;
                m_ListButtonText[i].ButtonComponent.targetGraphic.color = cButton;
            }


            // Set table

            m_ABCCharUI.Init();

            m_ABCCharUI.Hide();
        }

        public override void Back()
        {
            base.Back();

            if (m_ABCCharUI.ExampleUI.Visible)
            {
                m_ABCCharUI.ExampleUI.Hide();
            }else
            {
                Hide();
                AppController.Instance.ShowMainMenu();
            }
        }

        public override void Show()
        {
            base.Show();

            m_ABCCharUI.Show();
        }

        public override void Hide()
        {
            base.Hide();
            m_ABCCharUI.Hide();
        }

        public void OnItemButtonPress(int id, int x, int y)
        {
            Debug.Log("Item: " + id + " (" + x + "," + y + ") m_ListButtonText[id].TextButton" + m_ListButtonText[id].TextButton);

            Debug.Log("H:" + m_ABCSet.SymbolChar[x, y]  + ", R: " + m_ABCSet.RomanjiChar[x, y]);

            


            if (m_ABCSet == null)
            {
                Debug.Log("<color=cyan> SetExample,  m_CurrentHiragana null </color>");
                return;
            }

            // X has the current hiragana (ROW in the json)
            if ((x >= m_ABCSet.Data.Count) || (x < 0))
            {
                Debug.Log("<color=cyan> SetExample, Index out of boundaries </color>");
                return;
            }

            m_SelectedABC = m_ABCSet.Data[x];

            /*if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                EasyTTSUtil.SpeechFlush(m_ABCSet.RomanjiChar[x, y]);
            }*/
            
            //SetExample(0);
            //m_ABCCharUI.ExampleUI.Show();
        }
               

        private void SetExample(int index)
        {
            if (m_SelectedABC == null)
            {
                Debug.Log("<color=cyan> SetExample,  m_CurrentHiragana null </color>");
                return;
            }

            if ((index >= m_SelectedABC.SentencesExamples.Sentence.Count) || (index < 0))
            {
                Debug.Log("<color=cyan> SetExample, Index out of boundaries </color>");
                return;
            }

            m_SelectedExample = index;

            // Set sentence
            string examples = string.Empty;
            for (int i= 0; i< m_SelectedABC.SentencesExamples.Sentence.Count; i++ )
            {
                examples += " - " + m_SelectedABC.SentencesExamples.Sentence[i] + " (" + m_SelectedABC.SentencesExamples.Romaji[i] + ") = " + m_SelectedABC.SentencesExamples.English[i];
                m_ABCCharUI.ExampleUI.HiraganaExample += m_SelectedABC.SentencesExamples.Romaji[i] + " ";
                if (i < (m_SelectedABC.SentencesExamples.Sentence.Count -1))
                {
                    examples += "\n";
                }
            }
            m_ABCCharUI.ExampleUI.Example = examples;         
        }


        public void OnNextExample()
        {
            Debug.Log("<color=cyan> HIRAGANA CONTROL OnNextExample </color>");

            if (m_SelectedABC == null)
            {
                Debug.Log("<color=cyan> No Current Word </color>");
                return;
            }


            // Set next sentence
            m_SelectedExample++;
            m_SelectedExample %= m_SelectedABC.SentencesExamples.Sentence.Count;

            SetExample(m_SelectedExample);
        }


    }
}
