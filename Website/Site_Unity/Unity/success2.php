<?php
include "connexionBDD.php";
include_once "tools.php";

$user = clean(isset($_POST['user_id'])? $_POST['user_id']: null);
$password = clean(isset($_POST['password'])? $_POST['password']: null);


$pwd = hash("md5",$password);

$req = "SELECT * FROM user WHERE id_user = ? AND password = ?;";

$req_prep = $bdd->prepare($req);

if ($req_prep->execute(array($user,$pwd)))
{
    if ($req_prep->rowCount() != 1)
    {
        http_response_code(404);
        return;
    }
}

$suc1 = "UPDATE `succes_by_user` SET `advancement`= advancement + 1 WHERE advancement < (SELECT success.objectif_success FROM success WHERE success.id_success = succes_by_user.id_success AND success.id_success = 2) AND id_user = ?";

$req_suc1 = $bdd->prepare($suc1);

if ($req_suc1->execute(array($user)))
{
    http_response_code(200);
    return;
}else{
    http_response_code(400);
    return;
}