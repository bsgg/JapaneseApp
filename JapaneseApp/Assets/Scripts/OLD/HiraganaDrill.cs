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

        private int m_NumberDrills = 20;
        private int m_NumberSections = 5;

        private List<DrillUnit> m_Drills = new List<DrillUnit>();
        public List<DrillUnit> Drills
        {
            get { return m_Drills; }
            set { m_Drills = value; }
        }

        public HiraganaDrill(HiraganaData2 data, int idData)
        {
            List<DrillUnit> listDrills = new List<DrillUnit>();
            int nUnits = m_NumberDrills / m_NumberSections;

            int startDrill = 0;

            // Hiragana symbol questions
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                DrillUnit drill = new DrillUnit();
                drill.TypeQuestion = ETYPEEDRILL.H_SYMBOL;
                drill.IndexQuestion = Random.Range(0, data.Hiragana[idData].HiraganaChar.Count);
                drill.TypeAnswer = ETYPEEDRILL.H_ROMANJI;

                listDrills.Add(drill);
            }
            startDrill += nUnits;

            // Hiragana romanji questions
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                DrillUnit drill = new DrillUnit();
                drill.TypeQuestion = ETYPEEDRILL.H_ROMANJI;
                drill.IndexQuestion = Random.Range(0, data.Hiragana[idData].RomanjiChar.Count);
                drill.TypeAnswer = ETYPEEDRILL.H_SYMBOL;

                listDrills.Add(drill);
            }
            startDrill += nUnits;

            // Words
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                DrillUnit drill = new DrillUnit();
                drill.TypeQuestion = ETYPEEDRILL.H_WORD;
                drill.IndexQuestion = Random.Range(0, data.Hiragana[idData].Vocabulary.Hiragana.Count);

                // Select randomly between the other two options
                int chance = Random.Range(0, 100);
                if (chance <= 50)
                {
                    drill.TypeAnswer = ETYPEEDRILL.H_WORDMEANING;
                }else
                {
                    drill.TypeAnswer = ETYPEEDRILL.H_WORDROMANJI;
                }
                listDrills.Add(drill);
            }
            startDrill += nUnits;

            // Words
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                DrillUnit drill = new DrillUnit();
                drill.TypeQuestion = ETYPEEDRILL.H_WORDMEANING;
                drill.IndexQuestion = Random.Range(0, data.Hiragana[idData].Vocabulary.Meaning.Count);
                drill.TypeAnswer = ETYPEEDRILL.H_WORD;
                listDrills.Add(drill);
            }
            startDrill += nUnits;

            // Words
            for (int i = startDrill; i < (startDrill + nUnits); i++)
            {
                DrillUnit drill = new DrillUnit();
                drill.TypeQuestion = ETYPEEDRILL.H_WORDROMANJI;
                drill.IndexQuestion = Random.Range(0, data.Hiragana[idData].Vocabulary.Romanji.Count);
                drill.TypeAnswer = ETYPEEDRILL.H_WORD;
                listDrills.Add(drill);
            }

            // Suffle drills
            m_Drills = Utility.Shuffle(listDrills);

        }
    }
}
