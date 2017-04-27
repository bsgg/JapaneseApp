using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class AppController : MonoBehaviour
    {
        [SerializeField]
        private Text m_TextTest;

        private HiraganaData m_HiraganaData;

        void Start ()
        {
            LoadHiraganaData();
        }

        public void LoadHiraganaData()
        {
            m_HiraganaData = new HiraganaData();
            string jsonActionsString = Utility.LoadJSONResource("Data/Hiragana");
            if (jsonActionsString != "")
            {
                m_HiraganaData = JsonMapper.ToObject<HiraganaData>(jsonActionsString);
            }

            string text = "";
            for (int i= 0; i< m_HiraganaData.Hiragana.Count; i++)
            {
                HiraganaAlphabet ha = m_HiraganaData.Hiragana[i];
                text = ha.Title;
                text += "\n Hiragana |   Romanji ";

                for (int j= 0; j< ha.HiraganaChar.Count; j++)
                {
                    text += "\n" + ha.HiraganaChar[j] + " | " + ha.RomanjiChar[j];
                }

            }
            m_TextTest.text = text;



        }

    }
}
