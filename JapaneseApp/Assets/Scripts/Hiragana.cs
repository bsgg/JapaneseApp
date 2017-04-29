using System.Collections.Generic;

namespace JapaneseApp
{
    


    public class HiraganaAlphabet
	{
		private string m_Title;
		public string Title
		{
			set { m_Title = value; }
			get { return m_Title; }
		}

		private List<string> m_HiraganaChar;
		public List<string> HiraganaChar
        {
			set { m_HiraganaChar = value; }
			get { return m_HiraganaChar; }
		}

		private List<string> m_RomanjiChar;
		public List<string> RomanjiChar
        {
			set { m_RomanjiChar = value; }
			get { return m_RomanjiChar; }
		}

        private Vocabulary m_Vocabulary;
        public Vocabulary Vocabulary
        {
            set { m_Vocabulary = value; }
            get { return m_Vocabulary; }
        }
    }


    public class Vocabulary
    {

        private List<string> m_Hiragana;
        public List<string> Hiragana
        {
            set { m_Hiragana = value; }
            get { return m_Hiragana; }
        }

        private List<string> m_Romanji;
        public List<string> Romanji
        {
            set { m_Romanji = value; }
            get { return m_Romanji; }
        }

        private List<string> m_Meaning;
        public List<string> Meaning
        {
            set { m_Meaning = value; }
            get { return m_Meaning; }
        }
    }


    public class HiraganaData
	{
        private List<HiraganaAlphabet> m_Hiragana;
		public List<HiraganaAlphabet> Hiragana
		{
			get { return m_Hiragana; }
			set { m_Hiragana = value; }
		}

		public HiraganaData()
		{
            m_Hiragana = new List<HiraganaAlphabet>();
		}
    }

}

