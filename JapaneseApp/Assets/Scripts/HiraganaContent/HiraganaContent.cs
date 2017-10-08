using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class HiraganaContent : UIBase
    {
        public enum ETYPEMENU { NONE, MAINMENU, HIRAGANATABLE, DRILL };
        private ETYPEMENU m_CurrentMenu;

        private string m_PathData = "Data/Hiragana";

        private HiraganaData m_HiraganaData;

        public HiraganaData HiraganaData
        {
            get { return m_HiraganaData; }
            set { m_HiraganaData = value; }
        }
        
        // Main menu with lessons
        [SerializeField] private HiraganaScrollMenu m_MainScrollMenu;

        // Hiragana table
        [SerializeField] private HiraganaTable m_HiraganaTable;

        // Exercises
        [SerializeField]
        private HiraganaDrillContent m_HiraganaDrill;
        private HiraganaDrill m_CurrentDrill;


        public override void Init()
        {
            base.Init();
            m_CurrentMenu = ETYPEMENU.NONE;
            m_HiraganaTable.Hide();
            m_MainScrollMenu.Hide();

            // Init Hiragana data an menu
            m_HiraganaData = new HiraganaData();
            string jsonActionsString = Utility.LoadJSONResource(m_PathData);
            if (jsonActionsString != "")
            {
                m_HiraganaData = JsonMapper.ToObject<HiraganaData>(jsonActionsString);
            }

            List<string> lTitle = new List<string>();
            for (int i = 0; i < m_HiraganaData.Hiragana.Count; i++)
            {
                HiraganaAlphabet ha = m_HiraganaData.Hiragana[i];
                lTitle.Add(ha.Title);
            }
            m_MainScrollMenu.InitScrollMenu(lTitle);
            m_MainScrollMenu.ScrollMenu.OnButtonPress += OnButtonMenuPress;
            

        }
        public override void Show()
        {
            base.Show();
            m_MainScrollMenu.Show();
            m_CurrentMenu = ETYPEMENU.MAINMENU;
        }

        public override void Finish()
        {
            base.Finish();
            m_HiraganaTable.Hide();
            m_MainScrollMenu.Hide();
        }

        public override void Back()
        {
            base.Back();

            switch (m_CurrentMenu)
            {
                case ETYPEMENU.NONE:
                    Finish();
                    break;
                case ETYPEMENU.MAINMENU:
                    m_HiraganaTable.Hide();
                    m_MainScrollMenu.Hide();
                break;
                case ETYPEMENU.HIRAGANATABLE:
                    m_HiraganaTable.Hide();
                    m_MainScrollMenu.Show();
                break;
                case ETYPEMENU.DRILL:
                    //m_HiraganaTable.Hide();
                    //m_MainScrollMenu.Show();
                break;
            }

        }

        private void OnButtonMenuPress(int id)
        {
            if ((m_HiraganaData != null) && (m_HiraganaData.Hiragana != null) && (id < m_HiraganaData.Hiragana.Count))
            {
                m_HiraganaTable.Initialize(m_HiraganaData.Hiragana[id]);
                // Generate drill
                m_CurrentDrill = new HiraganaDrill(m_HiraganaData, id);

                m_HiraganaDrill.HiraganaData = m_HiraganaData.Hiragana[id];

                m_MainScrollMenu.Hide();
                m_HiraganaTable.Show();
                m_CurrentMenu = ETYPEMENU.HIRAGANATABLE;
            }
        }


        public void OnDrillPress()
        {
            m_CurrentMenu = ETYPEMENU.DRILL;

            m_HiraganaTable.Hide();

            m_HiraganaDrill.Drill = m_CurrentDrill;
            m_HiraganaDrill.Show();
        }

    }
}
