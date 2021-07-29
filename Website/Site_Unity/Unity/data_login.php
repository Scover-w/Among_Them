<?php
include "connexionBDD.php";
include_once "tools.php";
$user = clean(isset($_POST['user'])? $_POST['user']: null);
$pwd = clean(isset($_POST['pwd'])? $_POST['pwd']: null);

if (!$user)
{
    header("Location: login.php?fail=1");
    return;
}

if (!$pwd)
{
    header("Location: login.php?fail=2");
    return;
}

$pwd = hash("md5",$pwd);

$req = "SELECT * FROM user WHERE name = ? AND password = ?;";

$req_prep = $bdd->prepare($req);

if ($req_prep->execute(array($user,$pwd)))
{
    if ($req_prep->rowCount() < 1)
    {
        header("Location: login.php?fail=3");
        return;
    }
    session_start();
    $user = $req_prep->fetch();
    $_SESSION['username'] = $user[1];
    $_SESSION['user_id'] = $user[0];
    session_write_close();
    header("Location: index.php");
    
    return;
}else{
    header("Location: login.php?fail=4");
    return;
}