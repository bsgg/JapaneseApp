package com.cubenary.jpworld;

import java.util.ArrayList;


/**
 * Created by Bea on 04/09/2017.
 */

public class Alphabet
{
    private String title;
    public String GetTitle()
    {
        return title;
    }
    public void SetTitle(String inTitle)
    {
        title = inTitle;
    }

    private ArrayList<String> hiraganaChar;
    public ArrayList<String> GetHiragana()
    {
        return hiraganaChar;
    }
    public void SetHiragana(ArrayList<String> inList)
    {
        hiraganaChar = inList;
    }

    private ArrayList<String> romanjiChar;
    public ArrayList<String> GetRomanji()
    {
        return romanjiChar;
    }
    public void SetRomanji(ArrayList<String> inList)
    {
        romanjiChar = inList;
    }

    public Alphabet()
    {
        hiraganaChar = new ArrayList<String>();
        romanjiChar = new ArrayList<String>();
    }


}
