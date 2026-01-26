<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
</head>
<body>
    <?php
        $n = rand(0, 1000);
        echo "so nguyen: $n<br/>";
        if ($n%3==0 && $n%5==0) echo "$n la boi cua 3 va 5<br/>";
        else echo "$n khong phai la boi cua 3 va 5<br/>";
        echo "cac so chinh phuong nho hon $n: ";
        for ($i=0; $i<$n; $i++){
            if ((pow($i, 2)) < $n) echo pow($i, 2) . ", ";
            else break;
        }
        $count = 0;
        $_n = $n;
        while ($_n != 0){
            $count++;
            $_n = intdiv($_n, 10);
        }
        echo "<br/>$n co $count chu so";
    ?>
    <a href="../../index.html">Home</a>
</body>
</html>