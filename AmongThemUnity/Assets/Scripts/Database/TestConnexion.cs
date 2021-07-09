using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestConnexion : MonoBehaviour
{
    public TMP_InputField login;
    public TMP_InputField password;
    public TMP_Text btnLoginText;

    public TMP_Text statusText;
    public Image statusIndicator;

    private bool status = false;

    public bool onMenu;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("idUser") && onMenu)
        {
            statusText.text = PlayerPrefs.GetString("nameUser");
            statusIndicator.color = Color.green;
            login.text = "";
            password.text = "";
            login.gameObject.SetActive(false);
            password.gameObject.SetActive(false);
            btnLoginText.text = "Log off";
            status = true;
        }
    }

    void Upload()
    {
        
    }

    public void Click()
    {
        if (status)
        {
            StartCoroutine(LogOff());
            return;
        }
        StartCoroutine(GetDataLogin());
    }

    public IEnumerator SendData(string time, int platform)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        WWWForm form = new WWWForm();
        form.AddField("user_id", PlayerPrefs.GetString("idUser"));
        form.AddField("time", time);
        form.AddField("platform", platform);
        form.AddField("date", date);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity/test.php", form))
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

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity/test_login.php", form))
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
                statusText.text = data[1];
                PlayerPrefs.SetString("idUser", data[0]);
                PlayerPrefs.SetString("nameUser", data[1]);
                statusIndicator.color = Color.green;
                login.text = "";
                password.text = "";
                login.gameObject.SetActive(false);
                password.gameObject.SetActive(false);
                btnLoginText.text = "Log off";
                status = true;
                Debug.Log(PlayerPrefs.GetString("idUser"));
            }
        }
    }

    public IEnumerator LogOff()
    {
        statusText.text = "Disconneted";
        statusIndicator.color = Color.red;
        login.gameObject.SetActive(true);
        password.gameObject.SetActive(true);
        btnLoginText.text = "Login";
        status = false;
        PlayerPrefs.DeleteKey("idUser");
        PlayerPrefs.DeleteKey("nameUser");
        Debug.Log(PlayerPrefs.GetString("idUser"));
        yield return null;
    }
}
