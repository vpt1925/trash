<?php
    require("connect.php");

    $sql = "select * from sua inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua where sua.Ma_sua = '" . $_GET["Ma_sua"] . "'";
    $result = mysqli_query($conn, $sql);
    if ($row = mysqli_fetch_array($result)){
        echo "<div align='center'>";
        echo "<table border=1>";
        echo "<thead><th colspan=2 style='font-size:20pt'>" . $row["Ten_sua"] . " - " . $row["Ten_hang_sua"] . "<th></thead>";
        echo "<tr>" . "<td><img src='images/" . $row["Hinh"] . "' alt='anh_sua'></td>";
        echo "<td>"
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
        ."</p>" . "</td>";
        echo "</tr>";
        echo "<tr><td align='center'><a style='text-decoration:none' href='bai2.7.php?page=1'>Quay lại</a></td></tr>";
        echo "</table></div>";
    }
?>