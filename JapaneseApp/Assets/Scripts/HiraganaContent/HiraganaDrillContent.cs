using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class HiraganaDrillContent : UIBase
    {
        [SerializeField]
        private Text m_DrillTitle;

        [SerializeField]
        private Text m_Question;

        [SerializeField]
        private Text m_Time;

        [SerializeField]
        private Text[] m_Options;

        private HiraganaDrill m_Drill;
        public HiraganaDrill Drill
        {
            set { m_Drill = value; }
        }

        private HiraganaAlphabet m_HiraganaData;
        public HiraganaAlphabet HiraganaData
        {
            set { m_HiraganaData = value; }
        }

        private int m_CurrentQuestion;
        private int m_CorrectAnswer;
        private int[] m_Answers;

        public override void Show()
        {
            base.Show();

            m_CurrentQuestion = 0;
            m_DrillTitle.text = "Hiragana Drill (" + m_CurrentQuestion + "/" + m_Drill.Drills.Count.ToString() + ")";

            // Set first question
            int iQuestion = m_Drill.Drills[m_CurrentQuestion].IndexQuestion;
            m_CorrectAnswer = iQuestion;

            string question = "";           


            switch (m_Drill.Drills[m_CurrentQuestion].TypeQuestion)
            {
                case HiraganaDrill.ETYPEEDRILL.H_SYMBOL:
                    question = m_HiraganaData.HiraganaChar[iQuestion];
                   
                break;
            }

            // Create index array of answers and suffle them
            m_Answers = new int[] { 0, 1, 2, 3, 4 };
            Utility.Shuffle(m_Answers);

            switch (m_Drill.Drills[m_CurrentQuestion].TypeAnswer)
            {
                case HiraganaDrill.ETYPEEDRILL.H_ROMANJI:

                    // Fill button asnwers
                    for (int i = 0; i < m_Options.Length; i++)
                    {
                        m_Options[i].text = m_HiraganaData.RomanjiChar[m_Answers[i]];
                    }

                    // Fill array answers
                    /*for (int i=0; i< m_HiraganaData.RomanjiChar.Count;i++)
                    {
                        //m_Answers[i] = m_HiraganaData.RomanjiChar[i];
                    }  */



                    break;
            }

           

            m_Question.text =  "<color=#5bd3de>Select the correct answer for: </color>\n <color =#c9e8ff>" + question + "</color>";
            
            



            //m_Question.text = m_Drill.Drills[m_CurrentQuestion].IndexQuestion;


            //StartCoroutine(TimeRoutine(30));
        }

        

        private IEnumerator TimeRoutine(int totalSeconds)
        {
            m_Time.text = "<color=#5bd3de>Time:   </color><color=#c9e8ff>" + totalSeconds.ToString() + "</color>";

            while (totalSeconds > 0)
            {
                
                yield return new WaitForSeconds(1.0f);
                totalSeconds -= 1;

                m_Time.text = "<color=#5bd3de>Time:   </color><color=#c9e8ff>" + totalSeconds.ToString() + "</color>";
            }
        }

        public void OnOptionPress(int id)
        {
            Debug.Log("OnOptionPress " + id + " m_CorrectAnswer: " + m_CorrectAnswer);

            Debug.Log("Option " + m_Options[id].text + " m_CorrectAnswer: " + m_HiraganaData.RomanjiChar[m_CorrectAnswer]);
           

        }


    }
}
