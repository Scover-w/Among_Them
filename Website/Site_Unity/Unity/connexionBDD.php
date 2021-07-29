<?php

/* Auteur : Kyllian Marie-Magdelaine
 * Créer le : 18/05/2020
 * Description : Fichier permettant d'avoir accès à la base de données. (Fichier a inclure avec le fonction "include" de php)
 * Modifié par : Kyllian Marie-Magdelaine
 * Le : 27/05/2020
 * */
include "parametre.inc.php";
try {
    $bdd = new PDO('mysql:host='.$host.';dbname='.$dbname, $username, $password);
    $bdd->exec("SET CHARACTER SET utf8");
} catch (Exception $e) {
    die('Erreur' . $e->getMessage());
}
?>
