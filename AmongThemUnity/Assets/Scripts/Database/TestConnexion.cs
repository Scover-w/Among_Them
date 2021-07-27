using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestConnexion : MonoBehaviour
{
    public TMP_InputField login;
    public TMP_InputField password;
    public TMP_Text btnLoginText;
    public TMP_Text btnLogoffText;
    
    
    public Button testBtn;
    public Button loginBtn;
    public Button logoffBtn;

    public TMP_Text statusCoText;
    public TMP_Text statusDecoText;
    public Image statusIndicator;

    private bool status = false;

    public bool onMenu;
    
    void Start()
    {
        if (onMenu)
        {
            btnLogoffText.text = LanguageManager.Instance().GetTextWithReference("logoff_button");
            
            if (ConnexionManager.IsConnected)
            {
                loginBtn.gameObject.SetActive(false);
                logoffBtn.gameObject.SetActive(true);
                statusCoText.gameObject.SetActive(true);
                statusDecoText.gameObject.SetActive(false);
                testBtn.gameObject.SetActive(true);
            
                statusCoText.text = ConnexionManager.Username;
                statusIndicator.color = Color.green;
            
                login.text = "";
                password.text = "";
                login.gameObject.SetActive(false);
                password.gameObject.SetActive(false);
                status = true;
            }
        }
        
    }

    void Upload()
    {
        
    }
    
    public IEnumerator SendDataTest()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        WWWForm form = new WWWForm();
        form.AddField("user_id", ConnexionManager.IDUser);
        form.AddField("time", "00:05:00");
        form.AddField("platform", "1");
        form.AddField("date", date);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.scover.me/AmongThem/test.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form upload complete! " + www.error);
                Debug.Log(date);
            }
        }
    }

    public void TestClick()
    {
        StartCoroutine(SendDataTest());
    }

    public void ClickLogin()
    {
        
        StartCoroutine(GetDataLogin());
    }

    public void ClickLogOff()
    {
       
        StartCoroutine(LogOff());
          
    }

    public IEnumerator SendData(string time, int platform)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log(date);
        WWWForm form = new WWWForm();
        form.AddField("user_id", ConnexionManager.IDUser);
        form.AddField("time", time);
        form.AddField("platform", platform);
        form.AddField("date", date);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.scover.me/AmongThem/test.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form upload complete! " + www.error);
            }
        }
    }
    
    public IEnumerator GetDataLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", login.text);
        form.AddField("pwd", password.text);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.scover.me/AmongThem/test_login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form upload complete! ");
                Debug.Log(www.downloadHandler.text);
                var data = www.downloadHandler.text.Split(';');
                statusCoText.text = data[1];
                ConnexionManager.IDUser =data[0];
                ConnexionManager.Username = data[1];
                ConnexionManager.IsConnected = true;
                statusIndicator.color = Color.green;
                login.text = "";
                password.text = "";
                login.gameObject.SetActive(false);
                password.gameObject.SetActive(false);
                testBtn.gameObject.SetActive(true);
                //
                loginBtn.gameObject.SetActive(false);
                logoffBtn.gameObject.SetActive(true);
                statusCoText.gameObject.SetActive(true);
                statusDecoText.gameObject.SetActive(false);
                //
                btnLogoffText.text = LanguageManager.Instance().GetTextWithReference("logoff_button");
                status = true;
                //Debug.Log(ConnexionManager.IDUser);
            }
        }
    }

    public IEnumerator LogOff()
    {
        statusIndicator.color = Color.red;
        login.gameObject.SetActive(true);
        password.gameObject.SetActive(true);
        testBtn.gameObject.SetActive(false);
        //
        loginBtn.gameObject.SetActive(true);
        logoffBtn.gameObject.SetActive(false);
        statusCoText.gameObject.SetActive(false);
        statusDecoText.gameObject.SetActive(true);
        //
        btnLoginText.text = LanguageManager.Instance().GetTextWithReference("login_button");
        status = false;
        ConnexionManager.IDUser = "";
        ConnexionManager.Username = "";
        ConnexionManager.IsConnected = false;
        //Debug.Log(PlayerPrefs.GetString("idUser"));
        yield return null;
    }
}
