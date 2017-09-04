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


    Button btnTestLayout;
    Button btnBackMainLayout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        //mHiraganaSymbolTxt = (TextView) findViewById(R.id.hiraganaSymbolTxt) ;
       // mRomanjiTxt = (TextView) findViewById(R.id.romanjiTxt) ;


        //btnTestLayout = (Button) findViewById(R.id.btnTestLayout);
        btnBackMainLayout = (Button) findViewById(R.id.btnBackLayout);

        GridView gridview = (GridView) findViewById(R.id.gridview);


        ArrayList<String> myStringArray =  new ArrayList<String>();
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
        gridview.setAdapter(new TestAdapter(DataManager.getInstance().GetHiraganaData()));



       // contactList = new ArrayList<>();






        //mHiraganaSymbolTxt.setText(DataManager.getInstance().GetHiraganaData().get(0).GetHiragana().get(0));


        //lv = (ListView) findViewById(R.layout.list_item);

        //String jsonStr =loadJSONFromAsset();

       // Log.e("", "Response from url: " + jsonStr);


       /* if (jsonStr != null)
        {
            try
            {
               //ArrayList<HashMap<String, String>> studentList = new ArrayList<HashMap<String, String>>();


                JSONObject jsonObj = new JSONObject(jsonStr);

                JSONArray lHiragana = jsonObj.getJSONArray("Hiragana");

                Log.e("", "[MAINBSGG] NumberStudents: " + lHiragana.length());

                //for (int i = 0; i < lHiragana.length(); i++)
                {
                    JSONObject c = lHiragana.getJSONObject(0);

                    String title = c.getString("Title");

                    mRomanjiTxt.setText(title);

                    Log.e("", "[MAINBSGG] title: " + title);

                    JSONArray hChar = c.getJSONArray("HiraganaChar");

                    Log.e("", "[MAINBSGG] hChar elements: " + hChar.length());

                    for (int h = 0; h < hChar.length(); h++)
                    {

                        mHiraganaSymbolTxt.setText(hChar.getString(h));
                       Log.e("", "[MAINBSGG] hChar: " + hChar.getString(h));


                    }

                    JSONArray rChar = c.getJSONArray("RomanjiChar");
                    for (int h = 0; h < hChar.length(); h++)
                    {

                        mRomanjiTxt.setText(rChar.getString(h));
                        Log.e("", "[MAINBSGG] rChar: " + rChar.getString(h));


                    }


                   /* String id = c.getString(TAG_ID);
                    String name = c.getString(TAG_NAME);
                    String email = c.getString(TAG_EMAIL);
                    String address = c.getString(TAG_ADDRESS);
                    String gender = c.getString(TAG_GENDER);

                    // Phone node is JSON Object
                    JSONObject phone = c.getJSONObject(TAG_PHONE);
                    String mobile = phone.getString(TAG_PHONE_MOBILE);
                    String home = phone.getString(TAG_PHONE_HOME);

                    // tmp hashmap for single student
                    HashMap<String, String> student = new HashMap<String, String>();

                    // adding each child node to HashMap key => value
                    student.put(TAG_ID, id);
                    student.put(TAG_NAME, name);
                    student.put(TAG_EMAIL, email);
                    student.put(TAG_PHONE_MOBILE, mobile);

                    // adding student to students list
                    studentList.add(student);*/

              /*  }




            } catch (JSONException ex)
            {
                ex.printStackTrace();
                Log.e("", "Exception: " + ex.toString());
            }
        }
*/









       /* if (jsonStr != null)
        {
            try
            {
                ArrayList<HashMap<String, String>> studentList = new ArrayList<HashMap<String, String>>();


                JSONObject jsonObj = new JSONObject(jsonStr);

                JSONArray lStudents = jsonObj.getJSONArray(TAG_STUDENTINFO);

                Log.e("", "[MAINBSGG] NumberStudents: " + lStudents.length());

                for (int i = 0; i < lStudents.length(); i++)
                {
                    JSONObject c = lStudents.getJSONObject(i);

                    String id = c.getString(TAG_ID);
                    String name = c.getString(TAG_NAME);
                    String email = c.getString(TAG_EMAIL);
                    String address = c.getString(TAG_ADDRESS);
                    String gender = c.getString(TAG_GENDER);

                    // Phone node is JSON Object
                    JSONObject phone = c.getJSONObject(TAG_PHONE);
                    String mobile = phone.getString(TAG_PHONE_MOBILE);
                    String home = phone.getString(TAG_PHONE_HOME);

                    // tmp hashmap for single student
                    HashMap<String, String> student = new HashMap<String, String>();

                    // adding each child node to HashMap key => value
                    student.put(TAG_ID, id);
                    student.put(TAG_NAME, name);
                    student.put(TAG_EMAIL, email);
                    student.put(TAG_PHONE_MOBILE, mobile);

                    // adding student to students list
                    studentList.add(student);

                }




            } catch (JSONException ex)
            {
                ex.printStackTrace();
                Log.e("", "Exception: " + ex.toString());
            }
        }*/

    }

    public void OnTestLayout(View view)
    {
        // click handling code
        setContentView(R.layout.second_layout);
    }

    public void OnBackLayout(View view)
    {
        // click handling code
        setContentView(R.layout.activity_main);
    }

    public String loadJSONFromAsset()
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
    }




}

