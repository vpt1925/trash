<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
</head>
<body>
    <?php
        function ucln($_a, $_b){
            while ($_a !== $_b){
                if ($_a > $_b) $_a = $_a - $_b;
                else $_b = $_b - $_a;
            }
            return $_a;
        }
        function bcnn($_a, $_b){
            return ($_a * $_b) / ucln($_a, $_b);
        }
        function shh($n){
            $tong_uoc = 0;
            for ($i=1; $i<=sqrt($n); $i++){
                if ($n % $i === 0) $tong_uoc += $i;
            }
            return ($tong_uoc === $n);
        }
        //
        $a = rand(1, 1000);
        $b = rand(1, 1000);
        echo "Hai so: $a, $b<br/>";
        echo "Uoc chung lon nhat: " . ucln($a, $b) . "<br/>";
        echo "Boi chung nho nhat: " . bcnn($a, $b) . "<br/>";
        
        if (shh($a)) echo "$a la so hoan hao<br/>";
        else echo "$a khong phai la so hoan hao<br/>";

        if (shh($b)) echo "$b la so hoan hao<br/>";
        else echo "$b khong phai la so hoan hao<br/>";
    ?>
    <a href="../../index.html">Home</a>
</body>
</html>