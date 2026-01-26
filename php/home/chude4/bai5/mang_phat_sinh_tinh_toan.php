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
        function taoMang($n){
            $mang = array();
            for ($i = 0; $i < $n; $i++) $mang[] = rand(0, 20);
            return $mang;
        }

        function xuatMang($mang){
            return implode(",", $mang);
        }

        function tinhTong($mang){
            $tong = 0;
            foreach($mang as $so) $tong += $so;
            return $tong;
        }
        
        function timMax($mang){
            $max = $mang[0];
            foreach($mang as $so) if ($so > $max) $max = $so;
            return $max;
        }

        function timMin($mang){
            $min = $mang[0];
            foreach($mang as $so) if ($so < $min) $min = $so;
            return $min;
        }

        $so_phan_tu = "";
        $mang = "";
        $mang_hien_thi = "";
        $tong_mang = "";
        $gtln = "";
        $gtnn = "";

        if (isset($_POST["so_phan_tu"])){
            $so_phan_tu = trim($_POST["so_phan_tu"]);
            if (!empty($so_phan_tu)){
                $so_phan_tu = intval($so_phan_tu);
                if ($so_phan_tu > 0){
                    $mang = taoMang($so_phan_tu);
                    $mang_hien_thi = xuatMang($mang);
                    $tong_mang = tinhTong($mang);
                    $gtln = timMax($mang);
                    $gtnn = timMin($mang);
                }
            }
        }
    ?>
    <div id="container">
        <form action="" method="post" id="form">
            <div id="title">
                <h1>Phát sinh mảng và tính toán</h1>
            </div>
            <table>
                <tr>
                    <td>Nhập số phần tử:</td>
                    <td><input type="text" name="so_phan_tu" id="so_phan_tu" value="<?php echo $so_phan_tu; ?>"></td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="submit" name="submit" id="submit" value="Phát sinh và tính toán"></td>
                </tr>
                <tr>
                    <td><label for="mang_so">Mảng</label></td>
                    <td><input type="text" name="mang_so" id="mang_so" value="<?php echo $mang_hien_thi; ?>" readonly></td>
                </tr>
                <tr>
                    <td><label for="gtln">GTLN (Max) trong mảng:</label></td>
                    <td><input type="text" name="gtln" id="gtln" value="<?php echo $gtln; ?>" readonly></td>
                </tr>
                <tr>
                    <td><label for="gtnn">GTNN (Min) trong mảng:</label></td>
                    <td><input type="text" name="gtnn" id="gtnn" value="<?php echo $gtnn; ?>" readonly></td>
                </tr>
                <tr>
                    <td><label for="tong_mang">Tổng mảng:</label></td>
                    <td><input type="text" name="tong_mang" id="tong_mang" value="<?php echo $tong_mang; ?>" readonly></td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><a href="../../index.html">Home</a></td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>