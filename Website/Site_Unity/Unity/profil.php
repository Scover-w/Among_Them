<?php
    session_start();
?>
<html lang="fr">
    <?php include "header.php"; ?>
    <body>
        <?php include "navbar.php"; ?>

        <?php


        $req_record = "SELECT time FROM run_history WHERE user_id = ? ORDER BY time ASC LIMIT 1 ";
        $q_record = $bdd->prepare($req_record);
        $isExecute = $q_record->execute(array($_SESSION['user_id']));

        $nbTime = $q_record->rowCount();

        if ($isExecute)
        {
            $data_record = $q_record->fetch();
            $record = $data_record[0];
        }

        $req_fav_plat = "SELECT platform, COUNT(*) FROM run_history GROUP BY platform ORDER BY COUNT(*) ASC LIMIT 1";
        $q_fav_plat = $bdd->prepare($req_fav_plat);
        $isExecute = $q_fav_plat->execute();

        if ($isExecute)
        {
            $data_fav_plat = $q_fav_plat->fetch();
            $fav_plat = $data_fav_plat[0];

            //Plateform
            $req_platform = "SELECT name_platform FROM platform WHERE id_platform = ?";

            $q_platform = $bdd->prepare($req_platform);

            $isExecute = $q_platform->execute(array($fav_plat));

            $platform = "ERROR";

            if ($isExecute){
                $fav_plat_name = $q_platform->fetch()[0];
            }

            //
        }

        ?>
        
        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3">
                PROFIL
            </div>

            <p>&nbsp;&nbsp;Identifiant : <?php echo $_SESSION['username']?></p>

            <?php 
                if($nbTime > 0)
                {
            ?> 
                    <p>&nbsp;&nbsp;Record : <?php echo date_format(date_create($record), "i's''" ) ?></p>
                    <p>&nbsp;&nbsp;Plateforme favorite : <?php echo $fav_plat_name?></p>
            <?php
                }
                else
                {
                    ?><p>&nbsp;&nbsp;Veuillez terminer une partie pour voir vos statistiques !</p><?php
                }
            ?>

        </div>

        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3 ">
                HISTORIQUE
            </div>
            <div class="overflow-auto">
                <table class="w-100 text-center ">
                    <tr>
                        <td class="w-25 ">
                            Rank
                        </td>
                        <td class="w-25">
                            Time
                        </td>
                        <td class="w-25">
                            Platform
                        </td>
                        <td class="w-25">
                            Date
                        </td>
                    </tr>
                    <?php
                    $req_profil = "SELECT user_id, time, platform, date FROM run_history WHERE user_id = ? ORDER BY time ASC";

                    $q_profil = $bdd->prepare($req_profil);

                    $isExecute = $q_profil->execute(array($_SESSION['user_id']));

                    $counter = 1;

                    while ($data_rank = $q_profil->fetch())
                    {
                        //Date
                        $date = date_format(date_create($data_rank[3]), "d/m/Y à H:i" );

                        //Plateform
                        $req_platform = "SELECT name_platform FROM platform WHERE id_platform = ?";

                        $q_platform = $bdd->prepare($req_platform);

                        $isExecute = $q_platform->execute(array($data_rank[2]));

                        $platform = "ERROR";

                        if ($isExecute){
                            $platform = $q_platform->fetch()[0];
                        }

                        //
                        $class_number = "";
                        switch ($counter)
                        {
                            case 1:
                                $class_number = "top1";
                                break;
                            case 2:
                                $class_number = "top2";
                                break;
                            case 3:
                                $class_number = "top3";
                                break;
                            default:
                                $class_number = "hors_top3";
                                break;
                        }

                        echo "
                    <tr class=\"my-5\">
                        <td class='".$class_number."'>
                            ".$counter."
                        </td>
                        <td>
                            ".date_format(date_create($data_rank[1]), "i's''" )."
                        </td>
                        <td>
                            ".$platform."
                        </td>
                        <td>
                            ".$date."
                        </td>
                    </tr>";

                        $counter++;
                    }


                    ?>

                </table>
            </div>



        </div>

        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3">
                SUCCES
            </div>

            <div class="grid mt-2">
                <?php
                    $sql_success_for_user = "SELECT * FROM `succes_by_user` JOIN success ON succes_by_user.id_success = success.id_success WHERE id_user =".$_SESSION['user_id'];

                    $req_success = $bdd->prepare($sql_success_for_user);

                    if ($req_success->execute())
                    {
                        while ($data = $req_success->fetch())
                        {
                            $class_success = "";

                            if ($data['advancement'] >= $data['objectif_success'])
                            {
                                $class_success = "success";
                            }

                            echo "<div class=\"grid-item ".$class_success." \">
                                    <p>".$data['name_success']." - ".$data['advancement']."/".$data['objectif_success']."</p>
                                    <p>".$data['desc_success']."</p>
                                </div>";
                        }
                    }

                ?>

            </div>

        </div>
    </body>

</html>

<?php
session_write_close();
?>