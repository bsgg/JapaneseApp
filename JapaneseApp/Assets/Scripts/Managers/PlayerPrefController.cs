using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JapaneseApp
{
    public class PlayerPrefController
    {
        private string GetGUID()
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


        /*public int GetLastDayWordCategory(int newCategory = -1)
        {
            string key = "LastDayWordCategory";
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {

            }
        }*/

        public bool CheckDayWordDate()
        {
            // Get category and word from last time


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
        

    }
}
