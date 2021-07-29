<?php
include "connexionBDD.php";
include_once "tools.php";
$user = clean(isset($_POST['user'])? $_POST['user']: null);
$pwd = clean(isset($_POST['pwd'])? $_POST['pwd']: null);
$pwdc = clean(isset($_POST['pwdc'])? $_POST['pwdc']: null);

if (!$user)
{
    header("Location: register.php?fail=1");
    return;
}

if (!$pwd)
{
    header("Location: register.php?fail=2");
    return;
}

if (strcmp($pwd, $pwdc) != 0)
{
    header("Location: register.php?fail=5");
    return;
}

$sql_get_user = "SELECT * FROM user WHERE name = ".$bdd->quote($user);

$req_get_id = $bdd->prepare($sql_get_user);

if ($req_get_id->execute())
{
    if ($req_get_id->rowCount() > 0)
    {
        header("Location: register.php?fail=3");
        return;
    }
}


$pwd = hash("md5",$pwd);

$req = "INSERT INTO `user` (`name`, `password`) VALUES ( \"" . $user . "\" , \"" . $pwd . "\" );";

$req_prep = $bdd->prepare($req);

if ($req_prep->execute())
{
    //success
    $sql_get_success = "SELECT * FROM success";

    $req_get_success = $bdd->query($sql_get_success);

    //user_id
    $sql_get_id = "SELECT * FROM user WHERE name = ".$bdd->quote($user);

    $req_get_id = $bdd->query($sql_get_id);

    $user_id = $req_get_id->fetch()['id_user'];

    $i = 0;
    $j = 0;

    while ($data = $req_get_success->fetch())
    {
        $i++;
        $success_id = $data['id_success'];
        $sql_insert_sbu = "INSERT INTO `succes_by_user`(`id_user`, `id_success`, `advancement`) VALUES (?,?,0)";

        $req_sbu = $bdd->prepare($sql_insert_sbu);

        if ($req_sbu->execute(array($user_id, $success_id)))
        {
            $j++;
            
        }
    }


    if($i == $j)
    {
        header("Location: index.php");
        session_start();
        $_SESSION['username'] = $user;
        $_SESSION['user_id'] = $user_id;
        session_write_close();
        return;
    }
    else
    {
        header("Location: register.php?fail=3");
        return;
    }

}else{
    header("Location: register.php?fail=4");
    return;
}