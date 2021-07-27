using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConnexionManager
{
    public static string idUser = "";
    public static string username = "";
    public static string password = "";
    private static bool isConnected = false;

    public static string IDUser
    {
        get => idUser;
        set => idUser = value;
    }

    public static string Username
    {
        get => username;
        set => username = value;
    }
    
    public static string Password
    {
        get => password;
        set => password = value;
    }

    public static bool IsConnected
    {
        get => isConnected;
        set => isConnected = value;
    }

}
