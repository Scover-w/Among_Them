<div class="sticky top-0 w-100 bg-navbar d-flex flex-row">
    <div class="d-flex flex-row w-50">
        <a class="btn text-white border-0" href="index.php">Accueil</a>
        <a class="btn text-white" href="rank.php">Classement</a>
    </div>
    <div class="d-flex flex-row justify-content-end w-50">
        <?php
            include_once "tools.php";
            if (clean(isset($_SESSION['username'])))
            {
                echo "<a class=\"btn text-white\" href=\"profil.php\">".clean($_SESSION['username'])."</a>
                      <a class=\"btn text-white\" href=\"deconnexion.php\"><i class=\"fas fa-sign-out-alt\"></i></a>";
            }
            else
            {
                echo "<a class=\"btn text-white\" href=\"login.php\">Connexion</a>
                      <a class=\"btn text-white\" href=\"register.php\">S'enregistrer</a>";
            }
        ?>
    </div>

</div>
