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
    }

    public class HiraganaDrill
    {
        public enum ETYPEEDRILL { H_SYMBOL, H_ROMANJI, H_WORD, H_WORDROMANJI, H_WORDMEANING };

        private HiraganaData m_HiraganaData;

        private const int m_NumberDrills = 20;
        private List<DrillUnit> m_Drills = new List<DrillUnit>();

        public HiraganaDrill(HiraganaData data, int idData)
        {
            int nUnits = m_NumberDrills / 5;

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
            startDrill += nUnits;

            // Words
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                m_Drills[i].TypeQuestion = ETYPEEDRILL.H_WORD;
                m_Drills[i].IndexQuestion = Random.Range(0, data.Hiragana[idData].Vocabulary.Hiragana.Count);

                // Select randomly between the other two options
                int chance = Random.Range(0, 100);
                if (chance <= 50)
                {
                    m_Drills[i].TypeAnswer = ETYPEEDRILL.H_WORDMEANING;
                }else
                {
                    m_Drills[i].TypeAnswer = ETYPEEDRILL.H_WORDROMANJI;
                }                
            }
            startDrill += nUnits;

            // Words
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                m_Drills[i].TypeQuestion = ETYPEEDRILL.H_WORDMEANING;
                m_Drills[i].IndexQuestion = Random.Range(0, data.Hiragana[idData].Vocabulary.Meaning.Count);
                m_Drills[i].TypeAnswer = ETYPEEDRILL.H_WORD;
                
            }
            startDrill += nUnits;

            // Words
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                m_Drills[i].TypeQuestion = ETYPEEDRILL.H_WORDROMANJI;
                m_Drills[i].IndexQuestion = Random.Range(0, data.Hiragana[idData].Vocabulary.Romanji.Count);
                m_Drills[i].TypeAnswer = ETYPEEDRILL.H_WORD;

            }

            // Suffle drills
            //DrillUnit[] RandomDrills = Utility.Shuffle(m_Drills);


        }
    }
}
