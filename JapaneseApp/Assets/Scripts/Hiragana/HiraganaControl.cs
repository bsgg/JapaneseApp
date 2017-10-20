using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace JapaneseApp
{
    #region DataModel
    /*[System.Serializable]
    public class ABC
    {
        [SerializeField]
        private List<string> m_Hiragana;
        public List<string> Hiragana
        {
            get { return m_Hiragana; }
            set { m_Hiragana = value; }
        }

        [SerializeField]
        private List<string> m_Romanji;
        public List<string> Romanji
        {
            get { return m_Romanji; }
            set { m_Romanji = value; }
        }

        [SerializeField]
        private SentencesExamples m_SentencesExamples = new SentencesExamples();
        public SentencesExamples SentencesExamples
        {
            set { m_SentencesExamples = value; }
            get { return m_SentencesExamples; }
        }
    }*/

    [System.Serializable]
    public class HiraganaData
    {
        /*[SerializeField]
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }*/

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

        // Array (Row, Col) Hiragana Char        
        //private string[,] m_HiraganaChar;
        //private string[,] m_RomanjiChar;

        public override void Init()
        {
            base.Init();


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
                string h = m_HiraganaSet.Data[i].Hiragana;
                string e = m_HiraganaSet.Data[i].Romanji;

                string[] splitH = h.Split('_');
                string[] splitE = e.Split('_');

                // Both must have 5 elements
                if ((splitH != null) && (splitE != null) && (splitH.Length >= 5) && (splitE.Length >= 5))
                {
                    for (int j= 0; j< 5; j++)
                    {
                        m_HiraganaSet.HiraganaChar[i, j] = splitH[j];
                        m_HiraganaSet.RomanjiChar[i, j] = splitE[j];

                        int id = 5 * i + j;
                        string text = m_HiraganaSet.RomanjiChar[i, j]+ " : " + m_HiraganaSet.HiraganaChar[i, j];
                        m_ListButtonText[id].Initialize(text, id, i, j, OnItemButtonPress);
                    }
                }
                else
                {
                    Debug.Log("<color=cyan>" + "Wrong Format: " + h + " - " + e +"</color>");
                }
            }

           

            // Set table








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

        public void OnItemButtonPress(int id, int x, int y)
        {
            Debug.Log("Item: " + id + " (" + x + "," + y + ") m_ListButtonText[id].TextButton" + m_ListButtonText[id].TextButton);

            Debug.Log("H:" + m_HiraganaSet.HiraganaChar[x, y]  + ", R: " + m_HiraganaSet.RomanjiChar[x, y]);            

        }

    }
}
