<?php
    session_start();
    $_SESSION["LOGGED_IN"] = false;
    unset($_SESSION["USER"]);
    session_destroy();
    header("Location: index.php");
    exit;
?>