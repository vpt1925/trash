<?php
    require("connect.php");

    $rowsPerPage = 3;

    if (!isset($_GET['page'])) $_GET['page'] = 1;

    $offset = ($_GET["page"] - 1) * $rowsPerPage;

    $sql = "select * 
    from (sua 
    inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua)
    inner join loai_sua on sua.Ma_loai_sua = loai_sua.Ma_loai_sua LIMIT " . $offset . ", " . $rowsPerPage;
    
    $result = mysqli_query($conn, $sql);

     
    echo "<table align='center' border=1 style='border-collapse:collapse'>";
    echo "<head><th colspan=2>THÔNG TIN CHI TIẾT CÁC LOẠI SỮA</th></head>";
    if (mysqli_num_rows($result) !== 0){
        while ($row = mysqli_fetch_array($result)){
            echo "<tr><td colspan=2 style='font-size:20pt'>" . $row["Ten_sua"] . " - " . $row["Ten_hang_sua"] . "<td></tr>";
            echo "<tr>";
                echo "<td><img src='images/" . $row["Hinh"] . "' alt='anh_sua'></td>";
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
                        ."</p>"
                    ."</td>";
            echo "</tr>";
        }
    }
    echo "</table>";

    $res = mysqli_query($conn, 
    "select * 
    from (sua 
    inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua)
    inner join loai_sua on sua.Ma_loai_sua = loai_sua.Ma_loai_sua"
    );

    $maxPage = ceil(mysqli_num_rows($res) / $rowsPerPage);

    echo "<div style='text-align:center'>";

    if ($_GET["page"] > 1){
        echo "<a style='text-decoration:none; color:blue' href = " . $_SERVER['PHP_SELF'] . "?page=1" . "> << </a>";
        echo "<a style='text-decoration:none; color:blue' href=" . $_SERVER['PHP_SELF'] . "?page=" . ($_GET['page'] - 1) . "> < </a> "; 
    }
    for ($i = 1; $i <= $maxPage; $i++){
        if ($i == $_GET["page"]) echo "<b> " . $i . " </b>";
        else echo "<a style='text-decoration:none' href='" . $_SERVER["PHP_SELF"] . "?page=" . $i . "'> " . $i . " </a>";
    }

    if ($_GET["page"] < $maxPage){
        echo "<a style='text-decoration:none; color:blue' href = " . $_SERVER['PHP_SELF'] . "?page=" . ($_GET["page"] + 1) . "> > </a>";
        echo "<a style='text-decoration:none; color:blue' href=" . $_SERVER['PHP_SELF'] . "?page=" . ($maxPage) . "> >> </a> "; 
    }
    echo "</div>";
    echo "<div style='text-align:center'><a href='index.php'>Quay lại</a><div>";
    mysqli_close($conn);
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
</head>
<body>
    
</body>
</html>