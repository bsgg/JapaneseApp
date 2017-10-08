using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class VocabularyControl : UIBase
    {
        private string m_AnimalsPathData = "Data/Vocabulary/Animals";

        [SerializeField] private AnimalsData m_AnimalsData;

        public override void Init()
        {
            base.Init();

            // Init Hiragana data an menu
            m_AnimalsData = new AnimalsData();
            string jsonActionsString = Utility.LoadJSONResource(m_AnimalsPathData);
            if (jsonActionsString != "")
            {
                m_AnimalsData = JsonMapper.ToObject<AnimalsData>(jsonActionsString);
            }

        }
    }
}
