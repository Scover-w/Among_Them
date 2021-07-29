<?php
    session_start();
?>
<html lang="fr">
    <?php include "header.php"; ?>
    <body>
        <?php include "navbar.php"; ?>
        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3">
                CLASSEMENT
            </div>

            <?php
            $req_rank = "SELECT DISTINCT user, time, platform FROM ranking ORDER BY time ASC LIMIT 3";

            $q_ranking = $bdd->prepare($req_rank);

            $isExecute = $q_ranking->execute();

            $top3 = $q_ranking->fetchAll();

            ?>
            <div class="flex flex-row align-items-end mx-auto w-auto mt-2">
                <div style="width: 12.5%"></div>
                <div class="bg-dark border-top-podium-second w-25 flex flex-column justify-content-center align-items-center top2" style="height: 150px">
                    <p class="my-1">#2</p>
                    <p class="my-1"><?php
                        $req_user = "SELECT name FROM user WHERE id_user = ?";

                        $q_user = $bdd->prepare($req_user);

                        $name = "--";

                        if (isset($top3[1][0]) && $q_user->execute(array($top3[1][0]))){
                            $name = $q_user->fetch()[0];
                        }

                        echo $name;
                        ?></p>
                    <p class="my-1"><?php echo isset($top3[1][1]) ? date_format(date_create($top3[1][1]), "i's''" ): "--'--''" ?></p>
                    <p class="my-1"><?php
                        $req_platform = "SELECT name_platform FROM platform WHERE id_platform = ?";

                        $q_platform = $bdd->prepare($req_platform);

                        $platform = "--";

                        if (isset($top3[1][2]) && $q_platform->execute(array($top3[1][2]))){
                            $platform = $q_platform->fetch()[0];
                        }

                        echo $platform;
                        ?></p>
                </div>
                <div class="bg-dark border-top-podium-first w-25 flex flex-column justify-content-center align-items-center top1" style="height: 200px">
                    <p class="my-1">#1</p>
                    <p class="my-1"><?php
                        $req_user = "SELECT name FROM user WHERE id_user = ?";

                        $q_user = $bdd->prepare($req_user);

                        $name = "--";

                        if (isset($top3[0][0]) && $q_user->execute(array($top3[0][0]))){
                            $name = $q_user->fetch()[0];
                        }

                        echo $name;
                        ?></p>
                    <p class="my-1"><?php echo isset($top3[0][1]) ? date_format(date_create($top3[0][1]), "i's''" ): "--'--''" ?></p>
                    <p class="my-1"><?php
                        $req_platform = "SELECT name_platform FROM platform WHERE id_platform = ?";

                        $q_platform = $bdd->prepare($req_platform);

                        $platform = "--";

                        if (isset($top3[0][2]) && $q_platform->execute(array($top3[0][2]))){
                            $platform = $q_platform->fetch()[0];
                        }

                        echo $platform;
                        ?></p>
                </div>
                <div class="bg-dark border-top-podium-third w-25 flex flex-column justify-content-center align-items-center top3" style="height: 120px">
                    <p class="my-1">#3</p>
                    <p class="my-1"><?php
                        $req_user = "SELECT name FROM user WHERE id_user = ?";

                        $q_user = $bdd->prepare($req_user);

                        $name = "--";

                        if (isset($top3[2][0]) && $q_user->execute(array($top3[2][0]))){
                            $name = $q_user->fetch()[0];
                        }

                        echo $name;
                        ?></p>
                    <p class="my-1"><?php echo isset($top3[2][1]) ? date_format(date_create($top3[2][1]), "i's''" ): "--'--''" ?></p>
                    <p class="my-1"><?php
                        $req_platform = "SELECT name_platform FROM platform WHERE id_platform = ?";

                        $q_platform = $bdd->prepare($req_platform);

                        $platform = "--";

                        if (isset($top3[2][2]) && $q_platform->execute(array($top3[2][2]))){
                            $platform = $q_platform->fetch()[0];
                        }

                        echo $platform;
                        ?></p>
                </div>
                <div style="width: 12.5%"></div>
            </div>

            <table class="w-100 text-center">
                <tr>
                    <td class="w-25 ">
                        Rank
                    </td>
                    <td class="w-25">
                        Name
                    </td>
                    <td class="w-25">
                        Time
                    </td>
                    <td class="w-25">
                        Platform
                    </td>
                </tr>
                <?php
                $req_rank = "SELECT DISTINCT user, time, platform FROM ranking ORDER BY time ASC LIMIT 50 OFFSET 3";

                $q_ranking = $bdd->prepare($req_rank);

                $isExecute = $q_ranking->execute();

                $counter = 4;

                while ($data_rank = $q_ranking->fetch())
                {
                    //Username
                    $req_user = "SELECT name FROM user WHERE id_user = ?";

                    $q_user = $bdd->prepare($req_user);

                    $isExecute = $q_user->execute(array($data_rank[0]));

                    $name = "ERROR";

                    if ($isExecute){
                        $name = $q_user->fetch()[0];
                    }

                    //

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
                            ".$name."
                        </td>
                        <td>
                            ".date_format(date_create($data_rank[1]), "i's''" )."
                        </td>
                        <td>
                            ".$platform."
                        </td>
                    </tr>";

                    $counter++;
                }


                ?>

            </table>

        </div>
    </body>

</html>
<?php
    session_write_close();
?>