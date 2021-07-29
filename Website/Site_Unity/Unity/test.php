<?php
include "connexionBDD.php";

$user = isset($_POST['user_id'])? $_POST['user_id']: null;
$time = isset($_POST['time'])? $_POST['time']: null;
$platform = isset($_POST['platform'])? $_POST['platform']: null;
$date = isset($_POST['date'])? $_POST['date']: null;


$suc1 = "UPDATE `succes_by_user` SET `advancement`= advancement + 1 WHERE advancement < (SELECT success.objectif_success FROM success WHERE success.id_success = succes_by_user.id_success AND success.id_success = 1) AND id_user = ?";

$req_suc1 = $bdd->prepare($suc1);

$req_suc1->execute(array($user));

$req = "INSERT INTO `run_history` (`run_id`, `user_id`, `time`, `platform`, `date`) VALUES (NULL, ?, ?, ?, ?);";

$req_prep = $bdd->prepare($req);

if ($req_prep->execute(array($user, $bdd->quote($time), $platform, $bdd->quote($date))))
{


    $sql_rank_check = "SELECT * FROM ranking WHERE user = ".$user;
    $req_rank_check = $bdd->query($sql_rank_check);
    if ($req_rank_check->rowCount() > 0)
    {
        $data = $req_rank_check->fetch();

        $time1 = DateTime::createFromFormat("H:i:s",$data['time']);
        $time2 = DateTime::createFromFormat("H:i:s",$time);
        $time3 = DateTime::createFromFormat("H:i:s","00:09:58");


        if ($time1->diff($time3)->format("%R") == "-"){
            $suc2 = "UPDATE `succes_by_user` SET `advancement`= advancement + 1 WHERE advancement < (SELECT success.objectif_success FROM success WHERE success.id_success = succes_by_user.id_success AND success.id_success = 3) AND id_user = ?";


            $req_suc2 = $bdd->prepare($suc2);

            $req_suc2->execute(array($user));

        }

        echo $time1->diff($time2)->format("%R");

        if ($time1->diff($time2)->format("%R") == "-")
        {
            $sql_rank_update = "UPDATE `ranking` SET `time`= ? WHERE user = ?";

            $req_rank_update = $bdd->prepare($sql_rank_update);

            if ($req_rank_update->execute(array($time, $user)))
            {
                http_response_code(201);
            }else
            {
                http_response_code(401);
            }
        }
    }else
    {
        $sql_rank_insert = "INSERT INTO `ranking`(`id_rank`, `user`, `time`, `platform`) VALUES (null,?,?,?)";

        $req_rank_insert = $bdd->prepare($sql_rank_insert);

        if ($req_rank_insert->execute( array($user, $time,$platform)))
        {
            http_response_code(202);
        }else
        {
            http_response_code(402);
        }
    }

}else{
    echo "merde alors";
    http_response_code(404);
}