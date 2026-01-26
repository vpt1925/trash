<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>home</title>
    <style>
        body{
            width: 100%;
            min-height: 100vh;
        }
        th{
            font-size: 30pt;
            color: red;
        }
        tr{
            text-align: center;
        }
        .image{
            width: auto;
            height: 100px;
        }
    </style>
</head>
<body>
    <?php
        $images = array("");
        
        require("connect.php");

        $sql = "select * 
        from (sua 
        inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua)
        inner join loai_sua on sua.Ma_loai_sua = loai_sua.Ma_loai_sua";
        
        $result = mysqli_query($conn, $sql);
        
        echo "<table align='center' border=1 style='border-collapse:collapse'>";
        echo '<head><th colspan="2">THÔNG TIN CÁC SẢN PHẨM</th></head>';
        if (mysqli_num_rows($result) !== 0){
            while ($data = mysqli_fetch_array($result)){
                echo "<tr>";
                echo "<td><img class='image' src='images/" . $data["Hinh"] . "'></td>";

                echo "<td>";
                echo "<h3>" . $data["Ten_sua"] . "</h3>";
                echo "<p>Nhà sản xuất: " . $data["Ten_hang_sua"]
                    ."<br/>"
                    .$data["Ten_loai"]. " - "
                    .$data["Trong_luong"] . " gr - "
                    .$data["Don_gia"] . " VND</p>";
                echo "</td>";
                echo "</tr>";
            }
        }
        echo "</table>";
        echo "<div style='text-align:center'><a href='index.php'>Quay lại</a><div>";
        mysqli_close($conn);
    ?>
</body>
</html>