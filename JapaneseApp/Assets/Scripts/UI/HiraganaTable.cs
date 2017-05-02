using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class HiraganaTable : MonoBehaviour
    {

        [SerializeField] private
        GridLayoutGroup m_HiraganaTable;
        [SerializeField]
        private List<CellText> m_ListHiraganaCells;


        [SerializeField] private   GridLayoutGroup m_VocabularyTable;
        [SerializeField]
        private List<CellText> m_ListVocabularyCells;


        void Start()
        {
            /*int id = 9;

            int row = (id / m_NumberColums);
            int column = (id % m_NumberColums);*/

            // id = column + (row * width)

            m_ListHiraganaCells = new List<CellText>();
            for (int i= 0; i < m_HiraganaTable.transform.childCount; i++)
            {
                m_ListHiraganaCells.Add(m_HiraganaTable.transform.GetChild(i).GetComponent<CellText>());
            }

            m_ListVocabularyCells = new List<CellText>();
            for (int i = 0; i < m_VocabularyTable.transform.childCount; i++)
            {
                m_ListVocabularyCells.Add(m_VocabularyTable.transform.GetChild(i).GetComponent<CellText>());
            }
        }

        public void Initialize(HiraganaAlphabet hAlphabet)
        {
            if (hAlphabet !=null)
            {
                // Initialize hiragana table
                int indexHiragana = 0;
                int indexRomanji = indexHiragana + 1;                
                for (int i=0; i<hAlphabet.HiraganaChar.Count; i++)
                {
                    
                    m_ListHiraganaCells[indexHiragana].SetText(hAlphabet.HiraganaChar[i]);
                    m_ListHiraganaCells[indexRomanji].SetText(hAlphabet.RomanjiChar[i]);

                    indexHiragana += 2;
                    indexRomanji = indexHiragana + 1;
                }

                // Initialize Vocabulary
                indexHiragana = 0;
                indexRomanji = indexHiragana + 1;
                int indexMeaning = indexHiragana + 2;

                for (int i = 0; i < hAlphabet.Vocabulary.Hiragana.Count; i++)
                {
                    m_ListVocabularyCells[indexHiragana].SetText(hAlphabet.Vocabulary.Hiragana[i]);
                    m_ListVocabularyCells[indexRomanji].SetText(hAlphabet.Vocabulary.Romanji[i]);                    
                    m_ListVocabularyCells[indexMeaning].SetText(hAlphabet.Vocabulary.Meaning[i]);

                    indexHiragana += 3;
                    indexRomanji = indexHiragana + 1;
                    indexMeaning = indexHiragana + 2;
                }

            }
        }
       
    }
}
