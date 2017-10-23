using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    #region DataModel

    [System.Serializable]
    public class Dialog
    {
        [SerializeField]
        private string m_Title;
        public string Title
        {
            set { m_Title = value; }
            get { return m_Title; }
        }

        [SerializeField]
        private string m_Audio;
        public string Audio
        {
            set { m_Audio = value; }
            get { return m_Audio; }
        }

        [SerializeField]
        private List<string> m_Kanji= new List<string>();
        public List<string> Kanji
        {
            set { m_Kanji = value; }
            get { return m_Kanji; }
        }
        [SerializeField]
        private List<string> m_Hiragana= new List<string>();
        public List<string> Hiragana
        {
            set { m_Hiragana = value; }
            get { return m_Hiragana; }
        }
        [SerializeField]
        private List<string> m_Romanji= new List<string>();
        public List<string> Romanji
        {
            set { m_Romanji = value; }
            get { return m_Romanji; }
        }
        [SerializeField]
        private List<string> m_English= new List<string>();
        public List<string> English
        {
            set { m_English = value; }
            get { return m_English; }
        }

        [SerializeField]
        private List<string> m_Vocabulary= new List<string>();
        public List<string> Vocabulary
        {
            set { m_Vocabulary = value; }
            get { return m_Vocabulary; }
        }


        [SerializeField]
        private string m_Description;
        public string Description
        {
            set { m_Description = value; }
            get { return m_Description; }
        }


    }

    #endregion DataModel

    public class DialogControl : Base
    {
        [SerializeField]
        private string m_DataPath = "Data/Dialogs/";
        [SerializeField]
        private List<string> m_ListDialogs= new List<string>();

        [SerializeField]
        private List<Dialog>  m_DialogSet;

        public override void Init()
        {
            base.Init();


            // Load the data
            m_DialogSet = new List<Dialog>();
            for (int i = 0; i < m_ListDialogs.Count; i++)
            {
                string path = m_DataPath + m_ListDialogs[i];
                string json = Utility.LoadJSONResource(path);
                if (json != "")
                {
                    Dialog data = JsonMapper.ToObject<Dialog>(json);
                    m_DialogSet.Add(data);

                }
            }
        }
    }
}
