<html lang="fr">
    <?php include "header.php"; ?>
    <body>
        <?php include "navbar.php"; ?>
        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3">
                Se connecter
            </div>

            <p class="text-center text-danger">
                <?php
                include_once "tools.php";
                if (clean(isset($_GET['fail'])))
                {
                    switch (clean($_GET['fail']))
                    {
                        case 1:
                            echo "Veuillez entrer un nom d'utilisateur";
                            break;
                        case 2:
                            echo "Veuillez entrer un mot de passe";
                            break;
                        case 3:
                            echo "Le compte n'existe pas. <a href='register.php'>Veuillez vous inscrire ici.</a>";
                            break;
                        case 4:
                            echo "Une erreur est survenue avec la base de données";
                            break;
                        default:
                            echo "Erreur";
                    }
                }

                ?>
            </p>

            <form action="data_login.php" method="post" class="d-flex flex-column mx-auto w-50 my-2">
                <div class="d-flex flex-row my-2 ">
                    <p class="mb-1" style="width: 30%">Nom d'utilisateur :</p>
                    <input class="bg-transparent w-100 border-bottom-input" type="text" name="user">
                </div>

                <div class="d-flex flex-row my-2">
                    <p class="mb-1" style="width: 30%">Mot de passe :</p>
                    <input class="bg-transparent w-100 border-bottom-input" type="password" name="pwd">
                </div>
                <input class="my-2" type="submit" value="Se connecter">
            </form>


        </div>
    </body>

</html>
