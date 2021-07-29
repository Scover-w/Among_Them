<?php
include "connexionBDD.php";
include_once "tools.php";

$user = clean(isset($_POST['name'])? $_POST['name']: null);
$pwd = clean(isset($_POST['pwd'])? $_POST['pwd']: null);
if (!$user)
{
    header("Location: login.php?fail=1");
}

if (!$pwd)
{
    header("Location: login.php?fail=2");
}

$pwd = hash("md5",$pwd);

$req = "SELECT * FROM user WHERE name = ? AND password = ?;";

$req_prep = $bdd->prepare($req);

if ($req_prep->execute(array($user,$pwd)))
{
    if ($req_prep->rowCount() == 1){
        http_response_code(200);
        $data = $req_prep->fetch();
        echo $data["id_user"];
    }
    else
    {
        http_response_code(404);
    }

}else{
    http_response_code(404);
}