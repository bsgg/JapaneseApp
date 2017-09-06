package com.cubenary.jpworld;

import android.content.Context;
import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Bea on 04/09/2017.
 */

public class DataManager
{
    private static final DataManager ourInstance = new DataManager();

    public static DataManager getInstance() {
        return ourInstance;
    }


    private ArrayList<Alphabet> hiraganaAlphabet;

    public ArrayList<Alphabet> GetHiraganaData()
    {
        return hiraganaAlphabet;
    }

    private DataManager()
    {
        hiraganaAlphabet = new ArrayList<Alphabet>();
    }

    public void InitHiraganaData(Context ctx, String filename)
    {
        String hiraganaStr = ReadFromJSONFile(ctx, filename);
        if (!hiraganaStr.isEmpty())
        {
            try
            {
                JSONObject jsonObj = new JSONObject(hiraganaStr);
                JSONArray lHiragana = jsonObj.getJSONArray("Hiragana");
                for (int i = 0; i < lHiragana.length(); i++)
                {
                    Alphabet alp = new Alphabet();

                    JSONObject hiRow = lHiragana.getJSONObject(i);
                    alp.SetTitle(hiRow.getString("Title"));

                    JSONArray hChar = hiRow.getJSONArray("HiraganaChar");
                    for (int c = 0; c < hChar.length(); c++)
                    {
                        Log.e("JP", "[MAINBSGG] hChar: " + hChar.getString(c));
                        alp.GetHiragana().add(hChar.getString(c));
                    }

                    JSONArray jChar = hiRow.getJSONArray("RomanjiChar");
                    for (int c = 0; c < jChar.length(); c++)
                    {
                        Log.e("JP", "[MAINBSGG] hChar: " + jChar.getString(c));
                        alp.GetRomanji().add(hChar.getString(c));
                    }

                    hiraganaAlphabet.add(alp);
                }


            } catch (JSONException ex) {
                ex.printStackTrace();
                Log.e("", "Exception: " + ex.toString());
            }
        }
    }

    private String ReadFromJSONFile(Context ctx, String filename)
    {
        String json = null;
        try
        {
            InputStream is = ctx.getAssets().open(filename);
            int size = is.available();
            byte[] buffer = new byte[size];
            is.read(buffer);
            is.close();
            json = new String(buffer, "UTF-8");

        } catch (IOException ex)
        {
            ex.printStackTrace();
            return null;
        }
        return json;
    }
}
