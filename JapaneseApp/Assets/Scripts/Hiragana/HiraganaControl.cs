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

            if (m_ListButtonText != null)
            {
                for (int i = 0; i< m_ListButtonText.Length; i++)
                {
                    m_ListButtonText[i].Initialize(i, OnItemButtonPress);
                }
            }

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

        public void OnItemButtonPress(int id)
        {
            Debug.Log("Item: " + id + "  m_ListButtonText[id].TextButton" + m_ListButtonText[id].TextButton);


        }

    }
}
