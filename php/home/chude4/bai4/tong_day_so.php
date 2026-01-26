<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="css/hehe.css">
</head>
<body>
    <?php
        $day_so = "";
        $tong = "";
        if (isset($_POST["day_so"])){
            $day_so = trim($_POST["day_so"]);
            if (!empty($day_so)){
                $mang_so = explode(",", $day_so);
                $tong = array_sum($mang_so);
            }
        }
    ?>
    <div id="container">
        <form action="" method="post" id="form">
            <div id="title">
                <h1>Nhập và tính trên dãy số</h1>
            </div>
            <div id="content">
                <div id="nhap">
                    <label for="day_so">Nhập dãy số: </label>
                    <input type="text" name="day_so" id="day_so" value="<?php echo $day_so; ?>">
                </div>
                <div id="tinh">
                    <input type="submit" name="submit" id="submit" value="Tính">
                </div>
                <div id="ket_qua">
                    <label for="tong">Tổng dãy số: </label>
                    <input type="text" name="tong" id="tong" value="<?php echo $tong; ?>" readonly>
                </div>
                <a href="../../index.html">Home</a>
            </div>
        </form>
    </div>
</body>
</html>