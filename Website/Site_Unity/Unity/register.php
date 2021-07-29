<html lang="fr">
    <?php include "header.php"; ?>
    <body>
        <?php include "navbar.php"; ?>
        <div class="w-75 bg-main-div position-relative mx-auto my-2">
            <div class="w-auto bg-header-div border-header-div py-2 px-3">
                S'enregistrer
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
                            echo "Nom d'utilisateur déjà utilisé. Veuillez en choisir un autre";
                            break;
                        case 4:
                            echo "Une erreur est survenue avec la base de données";
                            break;
                        case 5:
                            echo "Veuillez bien réécrire votre mot de passe";
                            break;
                        default:
                            echo "Erreur";
                    }
                }

            ?>
            </p>

            <form action="data_register.php" method="post" class="d-flex flex-column mx-auto w-50 my-2">
                <div class="d-flex flex-row my-2 ">
                    <p class="mb-1" style="width: 30%">Nom d'utilisateur :</p>
                    <input class="bg-transparent w-100 border-bottom-input" type="text" name="user">
                </div>

                <div class="d-flex flex-row my-2">
                    <p class="mb-1" style="width: 30%">Mot de passe :</p>
                    <input class="bg-transparent w-100 border-bottom-input" type="password" name="pwd">
                </div>

                <div class="d-flex flex-row my-2 ">
                    <p class="mb-1" style="width: 30%">Confirmer MDP :</p>
                    <input class="bg-transparent w-100 border-bottom-input" type="password" name="pwdc">
                </div>
                <input class="my-2" type="submit" value="S'enregistrer">
            </form>


        </div>
    </body>

</html>
