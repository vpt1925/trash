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
        require("connect.php");

        $images = array("");
        $rowsPerPage = 5; //số mẩu tin trên mỗi trang, giả sử là 10

        if (!isset($_GET['page'])) $_GET['page'] = 1;
        //vị trí của mẩu tin đầu tiên trên mỗi trang

        $offset = ($_GET['page'] - 1) * $rowsPerPage;

        $sql = "select * 
        from ((sua 
        inner join hang_sua on sua.Ma_hang_sua = hang_sua.Ma_hang_sua)
        inner join loai_sua on sua.Ma_loai_sua = loai_sua.Ma_loai_sua) LIMIT ". $offset . ", " . $rowsPerPage;
        
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
        $req = mysqli_query($conn, 'select * from sua');
        $numRows = mysqli_num_rows($req);
        $maxPage = ceil($numRows/$rowsPerPage);

        //
        echo "<div style='text-align:center; margin-top:20px'>";
        if ($_GET['page'] > 1)
        { 
            echo "<a style='text-decoration:none; color:blue' href = " . $_SERVER['PHP_SELF'] . "?page=1" . "> << </a>";
            echo "<a style='text-decoration:none; color:blue' href=" . $_SERVER['PHP_SELF'] . "?page=" . ($_GET['page']-1) . "> < </a> ";
        }

        for ($i=1 ; $i<=$maxPage ; $i++) //tạo link tương ứng tới các trang
        { 
            if ($i == $_GET['page'])
                echo '<b>'. $i. '</b> '; //trang hiện tại sẽ được bôi đậm
            else 
                echo "<a style='text-decoration:none; color:blue' href=" . $_SERVER['PHP_SELF'] . "?page=" . $i . "> " . $i . " </a> ";
        }
        //gắn thêm nút Next
        if ($_GET['page'] < $maxPage)
        { 
            echo "<a style='text-decoration:none; color:blue' href = " . $_SERVER['PHP_SELF'] . "?page=" . ($_GET['page'] + 1) . "> > </a>";
            echo "<a style='text-decoration:none; color:blue' href = " . $_SERVER['PHP_SELF'] . "?page=" . $maxPage . "> >> </a>";
        }

        // echo "<p style='text-align:center'>Tong so trang la: ".$maxPage . "</p>";
        echo "</div>";
        echo "<div style='text-align:center'><a href='index.php'>Quay lại</a><div>";
        //
        mysqli_close($conn);
?>
</body>
</html>