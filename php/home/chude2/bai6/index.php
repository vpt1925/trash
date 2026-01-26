<?php
    if (isset($_POST["n"])){
        if ($_POST["n"] !== ""){
            function taoMang($n){
                $a = array();
                for ($i = 0; $i < $n; $i++){
                    $a[$i] = rand(-100, 100);
                }
                return $a;
            }
            function inMang(&$a, $n){
                echo "[ ";
                for ($i = 0; $i < $n; $i++){
                    echo $a[$i] . " ";
                }
                echo " ]<br/>";
            }

            //
            $n = $_POST["n"];
            echo "n = $n<br/>";

            //
            $a = taoMang($n);
            echo "a = ";
            inMang($a, $n);

            //
            sort($a);
            echo "Mang sau khi sap xep: a = ";
            inMang($a, $n);
        }
    }
    ?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
</head>
<body>
    <form action="index.php" method="post">
        <input type="text" name="n">
        <input type="submit" name="submit">
    </form>
    <a href="../../index.html">Home</a>
</body>
</html>