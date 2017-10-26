using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace JapaneseApp
{
    public class VocabularyUI : UIBase
    {
        [Header("VocabularyUI")]

        [SerializeField]
        private Text m_English;
        public string English
        {
            set { m_English.text = value; }
            get { return m_English.text; }
        }


        [SerializeField]
        private Text m_Word;
        public string Word
        {
            set { m_Word.text = value; }
            get { return m_Word.text; }
        }


        [SerializeField]
        private SpriteUI m_Sprite;
        public SpriteUI Sprite
        {
            set { m_Sprite = value; }
            get { return m_Sprite; }
        }
        

        [SerializeField]
        private Text m_Kana;
        public string Kana
        {
            set { m_Kana.text = value; }
            get { return m_Kana.text; }
        }

        [Header("Buttons")]        

        [SerializeField]
        private IconBtn m_ExampleBtn;
        public IconBtn ExampleBtn
        {
            get { return m_ExampleBtn; }
        }


        [SerializeField]
        private IconBtn m_SpriteBtn;
        public IconBtn SpriteBtn
        {
            get { return m_SpriteBtn; }
        }

        [SerializeField]
        private IconBtn m_NextWordBtn;
        public IconBtn NextWordBtn
        {
            get { return m_NextWordBtn; }
        }

        [SerializeField]
        private IconBtn m_SoundBtn;
        public IconBtn SoundBtn
        {
            get { return m_SoundBtn; }
        }
        
    }
}
