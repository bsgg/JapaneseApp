using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class PlayerPrefController
    {
        public static string GetGUID()
        {
            string guid = string.Empty;
            string key = "GUID";
            if (PlayerPrefs.HasKey(key))
            {
                guid = PlayerPrefs.GetString(key);
            }
            else
            {
                // Set Number badges to 0
                guid = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(key, guid);
                PlayerPrefs.Save();
            }

            return guid;
        }

        #region WordDay
        public static int GetLastDayWordCategory()
        {
            string key = "LastDayWordCategory";
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                // Create key
                UpdateLastDayWordCategory(-1);
                return -1;
            }
        }


        public static void UpdateLastDayWordCategory(int updateCategory)
        {
            string key = "LastDayWordCategory";
            PlayerPrefs.SetInt(key, updateCategory);
            PlayerPrefs.Save();
        }

        public static int GetLastDayWord()
        {
            string key = "LastLastDayWord";
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                // Create key
                UpdateLastDayWord(-1);
                return -1;
            }
        }

        public static void UpdateLastDayWord(int updateWord)
        {
            string key = "LastLastDayWord";
            PlayerPrefs.SetInt(key, updateWord);
            PlayerPrefs.Save();
        }

        public static bool IsNewDayWord()
        {
            string key = "LastDayWordDate";
            bool check = false;
            if (PlayerPrefs.HasKey(key))
            {
                // Check date
                string dateV = PlayerPrefs.GetString(key);

                DateTime lastTime = Convert.ToDateTime(dateV);
                DateTime current = DateTime.Now;

                // Check if last time 
                TimeSpan elapsed = current - lastTime;

                check = (elapsed.TotalDays >= 1);
                PlayerPrefs.SetString(key, current.ToString());

            }
            else
            {
                check = true;
                string dateTime = DateTime.Now.ToString();
                PlayerPrefs.SetString(key, dateTime);
                PlayerPrefs.Save();
            }

            return check;
        }

        #endregion WordDay
    }
}
