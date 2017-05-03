using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public struct DrillUnit
    {
        public HiraganaDrill.ETYPEEDRILL TypeQuestion;
        public int IndexQuestion;

        public HiraganaDrill.ETYPEEDRILL TypeAnswer;
        //public int IndexAnswer;
        
    }
    public class HiraganaDrill : MonoBehaviour
    {
        public enum ETYPEEDRILL { H_SYMBOL, H_ROMANJI, H_WORD, H_WORDROMANJI, H_WORDMEANING };

        private HiraganaData m_HiraganaData;

        private const int m_NumberDrills = 20;
        private DrillUnit[] m_Drills = new DrillUnit[m_NumberDrills];

        public void Init(HiraganaData data, int idData)
        {
            int nUnits = m_NumberDrills / 2;

            int startDrill = 0;

            // Hiragana symbol questions
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                m_Drills[i].TypeQuestion = ETYPEEDRILL.H_SYMBOL;
                m_Drills[i].IndexQuestion = Random.Range(0, data.Hiragana[idData].HiraganaChar.Count);
                m_Drills[i].TypeAnswer = ETYPEEDRILL.H_ROMANJI;
            }
            startDrill += nUnits;

            // Hiragana romanji questions
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                m_Drills[i].TypeQuestion = ETYPEEDRILL.H_ROMANJI;
                m_Drills[i].IndexQuestion = Random.Range(0, data.Hiragana[idData].RomanjiChar.Count);
                m_Drills[i].TypeAnswer = ETYPEEDRILL.H_SYMBOL;
            }

            // Words


        }
    }
}
