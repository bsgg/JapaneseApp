package com.cubenary.jpworld;

import android.app.ListActivity;
import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.GridView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.TextView;
import android.view.View;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;

public class MainActivity extends AppCompatActivity
{

    // URL to get contacts JSON
    //private static String url = "https://raw.githubusercontent.com/mobilesiri/JSON-Parsing-in-Android/master/index.html";
    private static String url = "http://beatrizcv.com/TestJSON/testjson.html";

    // JSON Node names
    private static final String TAG_STUDENTINFO = "studentsinfo";
    private static final String TAG_ID = "id";
    private static final String TAG_NAME = "name";
    private static final String TAG_EMAIL = "email";
    private static final String TAG_ADDRESS = "address";
    private static final String TAG_GENDER = "gender";
    private static final String TAG_PHONE = "phone";
    private static final String TAG_PHONE_MOBILE = "mobile";
    private static final String TAG_PHONE_HOME = "home";

    ArrayList<HashMap<String, String>> contactList;
    private ListView lv;

    private TextView mHiraganaSymbolTxt;
    private TextView mRomanjiTxt;

    private GridView gridviewtest;

    Button btnTestLayout;
    Button btnBackMainLayout;

    View HiraganaLayout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        mHiraganaSymbolTxt = (TextView) findViewById(R.id.hiraganaSymbolTxt) ;
        mRomanjiTxt = (TextView) findViewById(R.id.romanjiTxt) ;


        btnTestLayout = (Button) findViewById(R.id.btnHiraganaMenu);
        btnBackMainLayout = (Button) findViewById(R.id.btnBackLayout);




        //ArrayList<String> myStringArray =  new ArrayList<String>();
        /*ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                gridview, myStringArray);*/


       // gridview.setAdapter(new ImageAdapter(this));


       /* gridview.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View v,
                                    int position, long id)
            {
                /*Toast.makeText(this, "" + position,
                        Toast.LENGTH_SHORT).show();*/
          /*  }
        });*/

        DataManager.getInstance().InitHiraganaData(this,"Hiragana.json");


       /* ArrayList<String> items = new ArrayList<String>();
        items.add("1 , Hello11 , Hello12");
        items.add("2 , Hello21 , Hello22");*/
        HiraganaLayout= getLayoutInflater().inflate(R.layout.hiragana_layout, null);
        if (HiraganaLayout == null) {

            Log.e("JP", "inflatedView NULL");
        }else
        {
            Log.e("JP", "inflatedView not null ");

            TextView testTitle = (TextView) HiraganaLayout.findViewById(R.id.txtTitle) ;
            testTitle.setText("TEST TITLE");
        }

        gridviewtest= (GridView) HiraganaLayout.findViewById(R.id.gridview);
           if (gridviewtest != null)
           {
               Log.e("JP", "[BSGG] grid is not null fILLING WITH INFORMATION ");
               gridviewtest.setAdapter(new TestAdapter(DataManager.getInstance().GetHiraganaData()));
              // gridviewtest.setOnItemClickListener(  OnHiraganaElementClick());
           }else
           {
               Log.e("JP", "[BSGG] grid is null ");
           }



       // contactList = new ArrayList<>();






        //mHiraganaSymbolTxt.setText(DataManager.getInstance().GetHiraganaData().get(0).GetHiragana().get(0));


        //lv = (ListView) findViewById(R.layout.list_item);

        //String jsonStr =loadJSONFromAsset();

       // Log.e("", "Response from url: " + jsonStr);












    }

    public void OnTestLayout(View view)
    {
        // click handling code
        //setContentView(R.layout.hiragana_layout);
        setContentView(HiraganaLayout);


        /*TextView testTitle = (TextView) HiraganaLayout.findViewById(R.id.txtTitle) ;
        testTitle.setText("TEST TITLE");*/


        /*if (gridviewtest != null)
        {
            Log.e("JP", "[BSGG] grid is not null fILLING WITH INFORMATION ");
            gridviewtest.setAdapter(new TestAdapter(DataManager.getInstance().GetHiraganaData()));
        }else
        {
            Log.e("JP", "[BSGG] grid is null ");
        }*/
    }

    public void OnBackLayout(View view)
    {
        // click handling code
        setContentView(R.layout.activity_main);
    }

    public void OnHiraganaElementClick(AdapterView<?> parent, View v, int position, long id)
    {
        Log.e("JP", "[OnHiraganaElementClick] " + position + " id: " + id);
    }

   /*public String loadJSONFromAsset()
    {
        String json = null;
        try
        {
            InputStream is = this.getAssets().open("Hiragana.json");
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
    }*/




}

