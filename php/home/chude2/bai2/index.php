<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
</head>
<body>
    <?php
        for ($i = 1; $i <= 10; $i++){
            echo "Bang cuu chuong $i:<br/>";
            for ($j = 1; $j <= 10; $j++){
                $kq = $i * $j;
                echo "      $i * $j = $kq<br/>";
            }
        } 
    ?>
    <a href="../../index.html">Home</a>
</body>
</html>