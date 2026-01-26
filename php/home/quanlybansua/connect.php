<?php
    const HOST = "localhost";
    const USERNAME = "root";
    const PASSWORD = "";
    const DB = "qlbansua";
    $conn = mysqli_connect (HOST, USERNAME, PASSWORD, DB) or die ('Could not connect to MySQL: ' . mysqli_connect_error());
    mysqli_set_charset($conn, 'UTF8');
?>