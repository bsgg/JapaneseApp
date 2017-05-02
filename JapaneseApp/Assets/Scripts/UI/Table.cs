using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class Table : MonoBehaviour
    {

        [SerializeField]
        private
        GridLayoutGroup m_Table;
        // Use this for initialization

        [SerializeField]
        private int m_NumberColums = 0;

        private List<CellText> m_ListCells;

        

        void Start()
        {
            /*int id = 9;

            int row = (id / m_NumberColums);
            int column = (id % m_NumberColums);*/

            // id = column + (row * width)

            m_ListCells = new List<CellText>();
            for (int i= 0; i < transform.childCount; i++)
            {
                m_ListCells.Add(transform.GetChild(i).GetComponent<CellText>());
            }
        }

        public void InitializeTable(HiraganaAlphabet hAlphabet)
        {
            if (hAlphabet !=null)
            {
                int indexH = 1;
                int indexR = 0;
                for (int i=0; i<hAlphabet.HiraganaChar.Count; i++)
                {
                    string hChar = hAlphabet.HiraganaChar[i];
                    string rChar = hAlphabet.RomanjiChar[i];

                    m_ListCells[indexR].SetText(rChar);
                    m_ListCells[indexH].SetText(hChar);

                    indexR++;
                    indexH++;
                }
            }
        }
       
    }
}
