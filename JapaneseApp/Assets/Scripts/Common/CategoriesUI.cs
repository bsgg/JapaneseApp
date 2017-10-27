using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace JapaneseApp
{
    public class CategoriesUI : UIBase
    {
        [Header("CategoriesUI")]

        [SerializeField]
        private Scroll m_ScrollMenu;
        public Scroll ScrollMenu
        {
            get { return m_ScrollMenu; }
            set { m_ScrollMenu = value; }
        }

        private List<string> m_Categories;
        public List<string> Categories
        {
            get { return m_Categories; }
            set { m_Categories = value; }
        }

    }
}
