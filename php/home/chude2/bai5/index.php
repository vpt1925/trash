<?php
    if (isset($_POST["m"], $_POST["n"])){
        if ($_POST["m"] !== "" && $_POST["n"] !== ""){
            function taoMaTran($m, $n){
                $matrix = array();
                for ($i = 0; $i < $m; $i++){
                    for ($j = 0; $j < $n; $j++){
                        $matrix[$i][$j] = rand(-100, 100);
                    }
                }
                return $matrix;
            }

            function inMaTran(&$matrix, $m, $n){
                echo "<table border='1'>";
                for ($i = 0; $i < $m; $i++){
                    echo "<tr>";
                    for ($j = 0; $j < $n; $j++){
                        echo "<td>". $matrix[$i][$j] . "</td>";
                    }
                    echo "</tr>";
                }
                echo "</table>";
            }

            function inMaTranChanLe(&$matrix, $m, $n){
                echo "<table border='1'>";
                for ($i = 0; $i < $m; $i++){
                    echo "<tr>";
                    for ($j = 0; $j < $n; $j++){
                        if (($i+1) % 2 === 0 && ($j+1) % 2 === 1){
                            echo "<td>". $matrix[$i][$j] . "</td>";
                        }
                    }
                    echo "</tr>";
                }
            echo "</table>";
            }

            function laSoNguyenTo(&$so){
                if ($so <= 1) return false;
                $so_uoc = 0;
                for ($i=2; $i<=sqrt($so); $i++){
                    if ($so % $i === 0){
                        $so_uoc += 1;
                    }
                }
                return ($so_uoc === 0);
            }

            function inMangSoNguyenTo(&$matrix, $m, $n){
                for ($i = 0; $i < $m; $i++){
                    for ($j = 0; $j < $n; $j++){ 
                        if (laSoNguyenTo($matrix[$i][$j])){
                            echo $matrix[$i][$j] . ", ";
                        }
                    }
                }
                echo "<br/>";
            }

            function ghiMaTranVaoFile($path, &$matrix, $m, $n){
                $file = fopen($path, "w");
                for ($i = 0; $i < $m; $i++){
                    for ($j = 0; $j < $n; $j++){ 
                        fputs($file, $matrix[$i][$j] . " ");
                    }
                fputs($file, "\n");
                }
                fclose($file);
            }

            function docMaTranTuFile($path){
                $matrix = array();
                $i = 0;
                $file = fopen($path, "r");
                while (!feof(($file))){
                    $matrix[$i++] = array_slice(explode(" ", fgets($file)), 0, -1);
                }
                return $matrix;
            }

            //
            $m = $_POST["m"];
            $n = $_POST["n"];
            echo "(m, n) = ($m, $n)<br/>";

            //
            $matrix = taoMaTran($m, $n);
            echo "Ma tran<br/>";
            inMaTran($matrix, $m, $n);

            //
            echo "Ma tran hang chan, cot le:<br/>";
            inMaTranChanLe($matrix, $m, $n);

            //
            echo "Phan tu la so nguyen to: ";
            inMangSoNguyenTo($matrix, $m, $n);

            //
            echo "ghi ma tran...<br/>";
            ghiMaTranVaoFile("matrix.txt", $matrix, $m, $n);

            //
            echo "Kiem tra ma tran trong file<br/>";
            $matrix_test = docMaTranTuFile("matrix.txt");
            inMaTran($matrix_test, $m, $n);
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
        <input type="text" name="m" />
        <input type="text" name="n" />
        <input type="submit" value="Enter" />
    </form>
    <a href="../../index.html">Home</a>
</body>
</html>