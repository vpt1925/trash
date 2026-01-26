<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
</head>
<body>
    <form action="index.php" method="post">
        <input type="text" name="a" placeholder="Nhap a">
        <input type="text" name="b" placeholder="Nhap b">
        <input type="submit" value="Tinh">
    </form>
    <?php
        const PI = 3.1415926;
        if (isset($_POST["a"], $_POST["b"])){
            $a = $_POST["a"];
            $b = $_POST["b"];
            $C = (float) 0;
            $S = (float) 0;
            if (($a<1 || 5<$a) || ($b<10 || 100<$b)) echo "<br/>a phai thuoc [1, 5] va b phai thuoc [10, 100]";
            else{
                switch ($a){
                    case "1":   $S = PI * $b *$b;
                                $C = 2 * PI * $b; break;
                    case "2":   $S = $b * $b;
                                $C = 4 * $b; break;
                    case "3":   $S = $b * $b * sqrt(3) / 4; 
                                $C = 3 * $b; break;
                    default:    $S = $a * $b;
                                $C = 2 * ($a + $b);
                }
                echo "<br/>Chu vi: $C<br/>Dien tich: $S";
            }
        }
    ?>
    <a href="../../index.html">Home</a>
</body>
</html>