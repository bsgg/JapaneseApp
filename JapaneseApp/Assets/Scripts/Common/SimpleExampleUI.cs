using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class SimpleExampleUI : UIBase
    {

        [SerializeField]
        private string m_HiraganaExample;
        public string HiraganaExample
        {
            set { m_HiraganaExample = value; }
            get { return m_HiraganaExample; }
        }

        [SerializeField]
        private Text m_Example;
        public string Example
        {
            set { m_Example.text = value; }
            get { return m_Example.text; }
        }

        public void OnSoundPlay()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                EasyTTSUtil.SpeechFlush(m_HiraganaExample);
            }
        }

    }
}
