<?php
    require("connect.php");

    $columnPerRow = 5;
    $rowsPerPage = 2;
    $rowCount = 0;

    if (!isset($_GET['page'])) $_GET['page'] = 1;

    $offset = ($_GET["page"] - 1) * $columnPerRow * $rowsPerPage;

    $sql = "select * 
    from (sua 
    inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua)
    inner join loai_sua on sua.Ma_loai_sua = loai_sua.Ma_loai_sua LIMIT " . $offset . ", " . $columnPerRow * $rowsPerPage;
    
    $result = mysqli_query($conn, $sql);

    
    echo "<table align='center' border=1 style='border-collapse:collapse'>";
    echo "<head><th colspan='" . $columnPerRow . "'>THÔNG TIN CÁC SẢN PHẨM</th></head>";
    if (mysqli_num_rows($result) !== 0){
        while ($data = mysqli_fetch_array($result)){
            if ($rowCount % $columnPerRow === 0) echo "<tr>";
            echo "<td style='text-align:center'>";
            echo "<a href='" . "bai2.7-chitiet.php?Ma_sua=" . $data["Ma_sua"] . "'>" . $data["Ten_sua"] . "</a><br/>";
            echo $data["Trong_luong"] . " gr - "
                .$data["Don_gia"] . " VND</p>";
            echo "<img class='image' src='images/" . $data["Hinh"] . "'>";
            echo "</td>";

            if ($rowCount % $columnPerRow === $columnPerRow - 1) echo "</tr>";

            $rowCount++;
        }
    }
    echo "</table>";

    $res = mysqli_query($conn, 
    "select * 
    from (sua 
    inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua)
    inner join loai_sua on sua.Ma_loai_sua = loai_sua.Ma_loai_sua"
    );

    $maxPage = ceil(mysqli_num_rows($res) / ($columnPerRow * $rowsPerPage));

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