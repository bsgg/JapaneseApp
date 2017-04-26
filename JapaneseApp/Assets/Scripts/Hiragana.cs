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

		private string m_Hiragana;
		public string Hiragana
		{
			set { m_Hiragana = value; }
			get { return m_Hiragana; }
		}

		private string m_Romanji;
		public string Romanji
		{
			set { m_Romanji = value; }
			get { return m_Romanji; }
		}
	}

	public class HiraganaData
	{
		private List<HiraganaAlphabet> m_HiraganaAlphabet;
		public List<HiraganaAlphabet> HiraganaAlphabet
		{
			get { return m_HiraganaAlphabet; }
			set { m_HiraganaAlphabet = value; }
		}

		public HiraganaData()
		{
			m_HiraganaAlphabet = new List<HiraganaAlphabet>();
		}
	}
}

