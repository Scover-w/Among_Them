<?php

include "connexionBDD.php";
include_once "tools.php";
$user = clean(isset($_POST['user_id'])? $_POST['user_id']: null);


$sql_success_for_user = "SELECT `succes_by_user`.`id_success`, `succes_by_user`.`advancement`, success.name_success, success.objectif_success FROM `succes_by_user` JOIN success ON succes_by_user.id_success = success.id_success WHERE id_user = ".$user;

$req_success = $bdd->prepare($sql_success_for_user);

if ($req_success->execute())
{
    while ($data = $req_success->fetch())
    {
        echo $data['id_success'].",".$data['name_success'].",".$data['advancement'].",".$data['objectif_success'].";";
    }
}else
{
    echo var_dump($req_success->errorInfo());
}
