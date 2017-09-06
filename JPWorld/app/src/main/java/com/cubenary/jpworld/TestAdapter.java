package com.cubenary.jpworld;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;

/**
 * Created by Bea on 04/09/2017.
 */

public class TestAdapter extends BaseAdapter
{
    private ArrayList<Alphabet> mItems;
    private int mCount;

    public TestAdapter(ArrayList<Alphabet> items)
    {

        //mCount = items.size() * 3;
        mCount = items.size();
        mItems = items;

        // for small size of items it's ok to do it here, sync way
        for (Alphabet item : items)
        {

            Log.e("JP", "[TestAdapter.TestAdapter] " + item.GetTitle());

            // get separate string parts, divided by ,
            /*final String[] parts = item.split(",");

            // remove spaces from parts
            for (String part : parts) {
                part.replace(" ", "");
                mItems.add(part);
            }*/
        }
    }

    @Override
    public int getCount() {
        return mCount;
    }

    @Override
    public Object getItem(final int position) {
        return mItems.get(position);
    }

    @Override
    public long getItemId(final int position) {
        return position;
    }

    @Override
    public View getView(final int position, final View convertView, final ViewGroup parent) {

        View view = convertView;

        if (view == null)
        {
            view = LayoutInflater.from(parent.getContext()).inflate(android.R.layout.simple_list_item_1, parent, false);
        }

        //Button button = (Button) view.findViewById(android.R.id.grid_item);

        final TextView text = (TextView) view.findViewById(android.R.id.text1);

        //text.setText(mItems.get(position));

        //text.setText(mItems.get(position).GetTitle());

        String titleH = DataManager.getInstance().GetHiraganaData().get(position).GetTitle();
        Log.e("JP", "[TestAdapter.getView] " + titleH);

        text.setText( titleH);
        return view;
    }

}
