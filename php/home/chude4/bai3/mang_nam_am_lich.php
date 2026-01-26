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
        
        function chuyen_duong_sang_am_lich($nam){
            $mang_can = array("Quý","Giáp","Ất","Bính","Đinh","Mậu","Kỷ","Canh","Tân","Nhâm");
            $mang_chi = array("Hợi","Tý","Sửu","Dần","Mão","Thìn","Tỵ","Ngọ","Mùi","Thân","Dậu","Tuất");
            $mang_hinh = array("hoi.jpg","ty_chuot.jpg","suu.jpg","dan.jpg","mao.jpg","thin.jpg","ty_ran.jpg","ngo.jpg","mui.jpg","than.jpg","dau.jpg","tuat.jpg");
            $nam = $nam - 3;
            $can = $nam % 10;
            $chi = $nam % 12;
            $nam_am_lich = $mang_can[$can] . " " . $mang_chi[$chi];
            $hinh_anh = "<img src='images/" . $mang_hinh[$chi] . "' alt='anh thay the'>";
            return array("nam_am_lich" => $nam_am_lich, "hinh_anh" => $hinh_anh);
        }

        $nam_am = "";
        $hinh_anh = "";

        if (isset($_POST["nam_duong"])){
            $nam_duong = trim($_POST["nam_duong"]);
            if (!empty($nam_duong)){
                $nam_duong = intval($nam_duong);
                $ket_qua = chuyen_duong_sang_am_lich($nam_duong);
                $nam_am = $ket_qua["nam_am_lich"];
                $hinh_anh = $ket_qua["hinh_anh"];
            }
        }
    ?>
    <div id="container">
        <form action="" method="post">
            <div id="title">
                <h1>Tính năm âm lịch</h1>
            </div>
            <div id="content">
                <div id="duong">
                    <label for="">Năm dương lịch</label>
                    <input type="text" name="nam_duong" id="nam_duong" value="<?php if (isset($nam_duong)) echo $nam_duong; ?>">
                </div>
                <div id="chuyen">
                    <input type="submit" name="submit" id="submit" value="=>">
                </div>
                <div id="am">
                    <label for="">Năm âm lịch</label>
                    <input type="text" name="nam_am" id="nam_am" value="<?php if (isset($nam_am)) echo $nam_am; ?>">
                </div>
            </div>
            <div id="hinh_anh">
                    <?php if (isset($hinh_anh)) echo $hinh_anh; ?>
            </div>
            <a href="../../index.html">Home</a>
        </form>
    </div>
</body>
</html>