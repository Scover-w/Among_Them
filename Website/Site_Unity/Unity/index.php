<?php 
    session_start();
?>
<html lang="fr">
    <?php include "header.php"; ?>
    <body>
        <?php include "navbar.php"; ?>

        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3">
                PRESENTATION
            </div>

            <video controls width="100%">
                <source src="Videos/cinematic.mp4"
                        type="video/mp4">
            </video>

        </div>

        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3">
                PUBLICITE
            </div>
            <img class="w-100" src="Images/pub_at.png">

        </div>

    </body>

</html>
