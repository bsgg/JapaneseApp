using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class ButtonText : MonoBehaviour
    {
        public delegate void ButtonAction(int index, int x, int y);
        public ButtonAction OnButtonPress;


        [SerializeField]  private Text       m_TextButton;
        public string TextButton
        {
            get { return m_TextButton.text; }
            set { m_TextButton.text = value; }
        }

        private int m_ID;
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        private int m_X;
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }
        private int m_Y;
        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        [SerializeField] private Button  m_ButtonComponent;
        public Button ButtonComponent
        {
            get { return m_ButtonComponent; }
            set { m_ButtonComponent = value; }
        } 

        public void OnPress()
        {
            if (OnButtonPress != null)
            {
                OnButtonPress(m_ID,m_X,m_Y);
            }
        }

    }
}
