﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace JapaneseApp
{
    #region DataModel

    [Serializable]
    public class SentencesExamples
    {
        public List<string> Sentence;
        public List<string> Kana;
        public List<string> Kanjis;
        public List<string> Romaji;
        public List<string> English;

        public string GetSentence(int index)
        {
            if ((Sentence != null) && (index >= 0) && (index < Sentence.Count))
            {
                return Sentence[index];
            }
            return string.Empty;
        }

        public string GetKana(int index)
        {
            if ((Kana != null) && (index >= 0) && (index < Kana.Count))
            {
                return Kana[index];
            }
            return string.Empty;
        }

        public string GetKanjis(int index)
        {
            if ((Kanjis != null) && (index >= 0) && (index < Kanjis.Count))
            {
                string[] aKanjis = Kanjis[index].Split('|');
                if (aKanjis.Length >= 1)
                {
                    string kanjis = "";
                    for (int i = 0; i < aKanjis.Length; i++)
                    {
                        kanjis += aKanjis[i];
                        if (i < (aKanjis.Length - 1))
                        {
                            kanjis += "\n";
                        }
                    }
                    return kanjis;
                }
            }
            return string.Empty;
        }


        public string GetRomanji(int index)
        {
            if ((Romaji != null) && (index >= 0) && (index < Romaji.Count))
            {
                return Romaji[index];
            }
            return string.Empty;
        }                

        public string GetEnglish(int index)
        {
            if ((English != null) && (index >= 0) && (index < English.Count))
            {
                return English[index];
            }
            return string.Empty;
        }
    }

    [Serializable]
    public class VWord
    {
        public string Word;
        public string Kana;
        public string Meaning;
        public string Romaji;
        public string SpriteID;
        public Sprite SpriteObj = null;
        public SentencesExamples SentencesExamples;
        
    }

    [Serializable]
    public class WordData
    {
        //public string Name;
        public List<VWord> Data;        

        public VWord GetRandomWord()
        {
            if (Data != null)
            {
                int iRand = UnityEngine.Random.Range(0, Data.Count);
                return Data[iRand];
            }

            return null;
        }

        public int GetRandomWordID()
        {
            if (Data != null)
            {
                return UnityEngine.Random.Range(0, Data.Count);
            }

            return -1;
        }

        public VWord GetWordById(int id)
        {
            if ((Data != null) && (id >= 0) && (id < Data.Count))
            {
                return Data[id];
            }

            return null;
        }
    }
    

    [Serializable]
    public class VocabularyData
    {
        public string Category;
        public string FileName;

        public WordData WordSet;        
    }

    #endregion DataModel

    public class VocabularyControl : Base
    {
        public enum EMenu { NONE = -1, Category, WordDay, RandomWord };
       
        [SerializeField]
        private List<VocabularyData> m_VocabularyData;

        [Header("UI")]
        [SerializeField] private VocabularyUI m_VocabularyUI;
        [SerializeField] private CategoriesUI m_CategoriesUI;
        [SerializeField] private ExamplesUI m_ExamplesUI;

       // private VWord m_SelectedWord;
        private int m_SelectedExampleID;
        private int m_SelectedWordID;      
        private int m_SelectedCategory;
        private EMenu m_Menu;

        [SerializeField]
        private Color m_EnableBtnColor;

        [SerializeField]
        private Color m_DisableBtnColor;

        public override IEnumerator Initialize()
        {           
            m_VocabularyUI.Hide();
            m_ExamplesUI.Hide();
            m_CategoriesUI.Hide();

            m_VocabularyData = new List<VocabularyData>();
            for (int i = 0; i < AppController.Instance.Launcher.VocabularyIndexData.Data.Count; i++)
            {               
                VocabularyData aux = new VocabularyData();
                aux.Category = AppController.Instance.Launcher.VocabularyIndexData.Data[i].Title;
                string json = AppController.Instance.Launcher.VocabularyIndexData.Data[i].Data;
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        aux.WordSet = JsonUtility.FromJson<WordData>(json);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[VocabularyControl.Init] Exception at: " + AppController.Instance.Launcher.VocabularyIndexData.Data[i].URL + " " + e.ToString());
                    }

                    for (int iW = 0; iW< aux.WordSet.Data.Count; iW++)
                    {
                        string spriteId = aux.WordSet.Data[iW].SpriteID;
                        if (!string.IsNullOrEmpty(spriteId))
                        {
                            Texture2D texture = null;
                            yield return AppController.Instance.Launcher.LoadPicture("Pictures",spriteId, (result) => texture = result);

                            if (texture != null)
                            {
                                Rect rec = new Rect(0, 0, texture.width, texture.height);

                                aux.WordSet.Data[iW].SpriteObj = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);                              
                                
                            }
                        }
                            
                    }
                    m_VocabularyData.Add(aux);                                            

                }
                else
                {
                    Debug.Log("[VocabularyControl.Init] JSON not found: " + AppController.Instance.Launcher.VocabularyIndexData.Data[i].URL);
                }
            }
            
        }        

        #region Navigation
       
        public override void Back()
        {
            base.Back();

            // Always hide sprite
            if (m_VocabularyUI.Sprite.Visible)
            {
                m_VocabularyUI.Sprite.Hide();
            }

            // If example is visible, hide it, otherwise check type menu
            if (m_ExamplesUI.Visible)
            {
                m_ExamplesUI.OnMenuItemEvent -= OnExampleMenuItem;
                m_ExamplesUI.Hide();
            }else
            {
                switch (m_Menu)
                {
                    case EMenu.Category:
                        // If category visible, back to main menu app controller, otherwise, hide vocabulary and show categories menu
                        if (m_CategoriesUI.Visible)
                        {
                            AppController.Instance.ShowMainMenu();
                            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;
                            m_CategoriesUI.Hide();
                            // Back to main menu (App Controller)
                        }
                        else
                        {
                            SelectMenu(EMenu.Category);
                        }

                        break;
                    case EMenu.RandomWord:
                    case EMenu.WordDay:
                        // Back to main menu (App Controller)
                        Hide();
                        AppController.Instance.ShowMainMenu();

                        break;
                }

                if (m_ExamplesUI.Visible)
                {
                    m_ExamplesUI.Hide();
                }
            }
        }

        public void SelectMenu(EMenu menu)
        {
            m_Menu = menu;

            m_ExamplesUI.Hide();
            m_VocabularyUI.Hide();
            m_CategoriesUI.Hide();


            switch (m_Menu)
            {
                case EMenu.Category:

                    SetCategories();
                    m_CategoriesUI.ScrollMenu.OnItemPress += OnCategoryPress;
                    m_CategoriesUI.Show();

                break;

                case EMenu.RandomWord:
                    SetWord();
                    m_VocabularyUI.Show();
                break;
                case EMenu.WordDay:

                    CheckNewDayWord();

                    SetWord();
                    //SetRandomWord();
                    m_VocabularyUI.Show();
                break;
            }

            Show();
        }        

        public void OnCategoryPress(int id, int x, int y)
        {
            m_CategoriesUI.ScrollMenu.OnItemPress -= OnCategoryPress;

            m_SelectedCategory = id;
            m_SelectedWordID = 0;

            SetWord();

            m_ExamplesUI.Hide();
            m_VocabularyUI.Show();
            m_CategoriesUI.Hide();
        }

        #endregion Navigation

        #region SetData
        
        private void SetCategories()
        {
            List<string> categories =  new List<string>();

            for (int i = 0; i < m_VocabularyData.Count; i++)
            {
                categories.Add(m_VocabularyData[i].Category);
            }

            m_CategoriesUI.ScrollMenu.InitScroll(categories);
        }

        private void SetWord()
        {
            // Out of bondaries
            if ((m_SelectedCategory < 0) || (m_SelectedCategory >= m_VocabularyData.Count))
            {
                Debug.LogError("[VocabularyControl.SetWord] SelectedCategory " + m_SelectedCategory + " Out of range");
                return;
            }

            if ((m_SelectedWordID < 0) || (m_SelectedWordID >= m_VocabularyData[m_SelectedCategory].WordSet.Data.Count))
            {
                Debug.LogError("[VocabularyControl.SetWord] m_SelectedWordID " + m_SelectedWordID + " Out of range");
                return;
            }

            // Check number of words for this category
            if (m_Menu == EMenu.WordDay)
            {
                m_VocabularyUI.NextWordBtn.Enable(false, m_DisableBtnColor);
            }
            else
            {
                if (m_VocabularyData[m_SelectedCategory].WordSet.Data.Count > 0)
                {
                    m_VocabularyUI.NextWordBtn.Enable(true, m_EnableBtnColor);
                }
                else
                {
                    m_VocabularyUI.NextWordBtn.Enable(false, m_DisableBtnColor);
                }
            }
            VWord word = m_VocabularyData[m_SelectedCategory].WordSet.GetWordById(m_SelectedWordID);  

            if (word != null)
            {
                // Set word
                m_VocabularyUI.Word = word.Word;
                m_VocabularyUI.English = word.Meaning;
                m_VocabularyUI.Kana = word.Kana + " : " + word.Romaji;

                // Set sprite
                m_VocabularyUI.SpriteBtn.Enable(false, m_DisableBtnColor);
                if (word.SpriteObj != null)
                {
                    m_VocabularyUI.Sprite.SpriteObject = word.SpriteObj;

                    m_VocabularyUI.SpriteBtn.Enable(true, m_EnableBtnColor);                    
                }
                
                m_VocabularyUI.Sprite.Hide();


                // Set sentence
                if ((word.SentencesExamples != null) && (word.SentencesExamples.Sentence.Count > 0))
                {
                    if (word.SentencesExamples.Sentence.Count > 1)
                    {
                        m_ExamplesUI.NextBtn.Enable(true, m_EnableBtnColor);
                    }
                    else
                    {
                        m_ExamplesUI.NextBtn.Enable(false, m_DisableBtnColor);
                    }


                    m_VocabularyUI.ExampleBtn.Enable(true, m_EnableBtnColor);

                    // Set sentence
                    m_SelectedExampleID = 0;
                    SetExample(m_SelectedExampleID);
                }
                else
                {
                    m_ExamplesUI.NextBtn.Enable(false, m_DisableBtnColor);
                    m_VocabularyUI.ExampleBtn.Enable(false, m_DisableBtnColor);


                }

            }
            else
            {
                Debug.Log("<color=cyan> No Current Word </color>");
            }
        }


        private int GetRandomCategory()
        {
            int rCategory = UnityEngine.Random.Range(0, m_VocabularyData.Count);
            return rCategory;
        } 
        

        private void SetExample(int index)
        {
            if ((index < 0) || (index >= m_VocabularyData.Count))
            {
                Debug.LogError("[VocabularyControl.SetExample] index " + index + " Out of range");
                return;
            }

            // Set sentence
            VWord word = m_VocabularyData[m_SelectedCategory].WordSet.GetWordById(m_SelectedWordID);
            m_ExamplesUI.Sentence = word.SentencesExamples.GetSentence(index);
            m_ExamplesUI.English = word.SentencesExamples.GetEnglish(index);
            m_ExamplesUI.Kanjis = word.SentencesExamples.GetKanjis(index);
        }
        #endregion SetData

        #region MenuButtons

        public void OnExamplesBtn()
        {
            m_ExamplesUI.OnMenuItemEvent += OnExampleMenuItem;
            m_ExamplesUI.Show();
        }

        public void OnSpriteBtn()
        {
            if (m_VocabularyUI.Sprite.Visible)
            {
                m_VocabularyUI.Word = m_VocabularyData[m_SelectedCategory].WordSet.Data[m_SelectedWordID].Word;
                m_VocabularyUI.Sprite.Hide();
            }else
            {
                m_VocabularyUI.Word = "";
                m_VocabularyUI.Sprite.Show();
            }            
        }

        public void OnNextWordBtn()
        {
            
            // Increase current word id
            m_SelectedWordID++;
            m_SelectedWordID %= m_VocabularyData[m_SelectedCategory].WordSet.Data.Count;

            SetWord();
        }

        private void OnExampleMenuItem(AppController.EMenu id)
        {
            VWord word = m_VocabularyData[m_SelectedCategory].WordSet.GetWordById(m_SelectedWordID);

            switch (id)
            {
                case AppController.EMenu.NEXT:
                    // Set next sentence
                    m_SelectedExampleID++;
                    m_SelectedExampleID %= word.SentencesExamples.Sentence.Count;

                    SetExample(m_SelectedExampleID);
                break;

                case AppController.EMenu.KANJI:
                    m_ExamplesUI.Sentence = word.SentencesExamples.Sentence[m_SelectedExampleID];
                    break;
                case AppController.EMenu.KANA:
                    m_ExamplesUI.Sentence = word.SentencesExamples.Kana[m_SelectedExampleID];
                    break;
                case AppController.EMenu.ROMAJI:
                    m_ExamplesUI.Sentence = word.SentencesExamples.Romaji[m_SelectedExampleID];
                break;
            }
        }

        #endregion MenuButtons

        #region WordOfDay

        public void CheckNewDayWord()
        {
            //PlayerPrefs.DeleteAll();

            string keyLastDay = "LastDayWordDate";
            string keyLastCategory = "LastCategoryDayWord";
            string keyLastWord = "LastWordDayWord";

            if (PlayerPrefs.HasKey(keyLastDay))
            {
                // Check date
                string dateV = PlayerPrefs.GetString(keyLastDay);

                DateTime lastTime = Convert.ToDateTime(dateV);
                DateTime current = DateTime.Now;                

                // Check if last time 
                TimeSpan elapsed = current - lastTime;

                // Select new word
                if (elapsed.TotalDays >= 1)
                {
                    // Update time
                    PlayerPrefs.SetString(keyLastDay, current.ToString());

                    // New category New word
                    m_SelectedCategory = GetRandomCategory();
                    SetKey(keyLastCategory, (int)m_SelectedCategory);

                    // Set new random word
                    m_SelectedWordID = m_VocabularyData[m_SelectedCategory].WordSet.GetRandomWordID();
                    SetKey(keyLastWord, m_SelectedWordID);
                }
                else
                {
                    // Get last category and last word
                    m_SelectedCategory = GetKey(keyLastCategory);
                    m_SelectedWordID = GetKey(keyLastWord);
                }
                

            }
            else
            { 
                // First time set current Time, Get random category and word
                string dateTime = DateTime.Now.ToString();
                PlayerPrefs.SetString(keyLastDay, dateTime);

                // New category New word
                m_SelectedCategory = GetRandomCategory();
                SetKey(keyLastCategory, (int)m_SelectedCategory);

                // Set new random word
                m_SelectedWordID = m_VocabularyData[m_SelectedCategory].WordSet.GetRandomWordID();
                SetKey(keyLastWord, m_SelectedWordID);

                PlayerPrefs.Save();
            }
        }


        public int GetKey(string key, int defaultValue = -1)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                // Create key
                SetKey(key, defaultValue);
                return -1;
            }
        }

        public void SetKey(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }


        #endregion WordOfDay

    }

}
