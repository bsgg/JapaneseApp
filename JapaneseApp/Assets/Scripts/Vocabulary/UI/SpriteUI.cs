using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JapaneseApp
{
    public class SpriteUI : UIBase
    {
        [SerializeField]
        private Image m_SpriteObject;
        public Sprite SpriteObject
        {
            set
            {
                m_SpriteObject.sprite = value;
                m_SpriteObject.preserveAspect = true;
            }
            get { return m_SpriteObject.sprite; }
        }
    }
}
