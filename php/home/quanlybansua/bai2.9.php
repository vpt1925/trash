<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <style>
        body{
            width: 100%;
            min-height: 100vh;
            padding: 0;
            margin: 0;
            box-sizing: border-box;
            text-align: center;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 12pt;
        }
        input{
            padding: 5px;
            margin: 5px;
            border-radius: 10px;
        }
        input[type="text"]{
            width: 300px;
        }
        input[type="submit"]{
            width: 80px;
        }
    </style>
</head>
<body>
    <h3>TÌM KIẾM THÔNG TIN SỮA</h3>
    <form action="" method="POST">
        <input type="text" name="Ten_sua" placeholder="Nhập tên sữa" value="<?php if(isset($_POST["Ten_sua"])) echo $_POST["Ten_sua"] ?>">
        <input type="submit" name="submit" value="Tìm kiếm">
    </form>
</body>
</html>

<?php
    if (isset($_POST["submit"])){
        $error = array();
        if (isset($_POST["Ten_sua"])){
            $Ten_sua = trim($_POST["Ten_sua"]);
            if (!preg_match("/^[\p{L}0-9\s]+$/u", $Ten_sua)) $_POST["Ten_sua"] = "";
            else $error[] = "Trường tên sữa chứa ký tự đặc biệt";
        }
        else $_POST["Ten_sua"] = "";

        require("connect.php");

        $sql = "select * 
        from (sua 
        inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua)
        inner join loai_sua on sua.Ma_loai_sua = loai_sua.Ma_loai_sua WHERE sua.Ten_sua LIKE '%" . $_POST["Ten_sua"] . "%'";
        
        $result = mysqli_query($conn, $sql);

        if (mysqli_num_rows($result) !== 0){
            echo "<h5>Tìm thấy " . mysqli_num_rows($result) . " sản phẩm</h5><br/>";
            echo "<table align='center' border=1 style='border-collapse:collapse'>";
            while ($row = mysqli_fetch_array($result)){
                    echo "<tr><td colspan=2 style='font-size:20pt'>" . $row["Ten_sua"] . " - " . $row["Ten_hang_sua"] . "<td></tr>";
                    echo "<tr>";
                        echo "<td><img src='images/" . $row["Hinh"] . "' alt='anh_sua'></td>";
                        echo "<td align='justify'>"
                                ."<p>
                                <i>Thành phần dinh dưỡng:</i>
                                <br/>"
                                .$row["TP_Dinh_Duong"]
                                ."<br/>"
                                ."<i>Lợi ích:</i>
                                <br/>"
                                .$row["Loi_ich"]
                                ."<br/><br/>"
                                ."<b><i>Trọng lượng:</i></b> "
                                .$row["Trong_luong"] . " gr - "
                                ."<b><i>Đơn giá:</i></b> "
                                .$row["Don_gia"] . " VND</p>"
                            ."</p>"
                            ."</td>";
                    echo "</tr>";
                }
            echo "</table>";
        }
        else echo "<h5>Không tìm thấy sản phẩm</h5>";

        mysqli_close($conn);
    }
    echo "<div style='text-align:center'><a href='index.php'>Quay lại</a><div>";
?>