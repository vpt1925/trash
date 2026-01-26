<?php
    require("connect.php");
    if (isset($_POST["submit"])){
        session_start();
        $error = array();

        $sql = "select * from sua where Ma_sua = '".$_POST["Ma_sua"]."'";
        $result = mysqli_query($conn, $sql);

        $file_name = $_FILES['Hinh_anh']['name'];
        $file_size = $_FILES['Hinh_anh']['size'];
        $file_tmp = $_FILES['Hinh_anh']['tmp_name'];
        $file_type = $_FILES['Hinh_anh']['type'];
        $file_ext = @strtolower(end(explode('.', $_FILES['Hinh_anh']['name'])));
        $expensions = array("jpeg","jpg","png");

        if (!$_SESSION["LOGGED_IN"]) $error[] = "Chưa đăng nhập!";
        if (mysqli_num_rows($result) !== 0) $error[] = "Mã sữa đã tồn tại";
        if (!is_numeric($_POST["Trong_luong"])) $error[] = "Trọng lượng phải là số";
        if (!is_numeric($_POST["Don_gia"])) $error[] = "Đơn giá phải là số";
        if (in_array($file_ext, $expensions) === false) $error[] = "Ảnh phải thuộc định dạng jpg, jpeg hoặc png";
        if ($file_size > 2097152) $error[] = "Kích cỡ ảnh không vượt quá 2 MB";

        if (empty($error) === true){
            $sql = "insert into sua values ("
            ."'".$_POST["Ma_sua"]."'".","
            ."'".$_POST["Ten_sua"]."'".","
            ."'".$_POST["Ma_hang_sua"]."'".","
            ."'".$_POST["Ma_loai_sua"]."'".","
            ."'".$_POST["Trong_luong"]."'".","
            ."'".$_POST["Don_gia"]."'".","
            ."'".$_POST["Thanh_phan_dinh_duong"]."'".","
            ."'".$_POST["Loi_ich"]."'".","
            ."'".$file_name."'".")";

            move_uploaded_file($file_tmp, __DIR__ . "\\images\\" . $file_name);

            $result = mysqli_query($conn, $sql);
        }
    }
?>
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
        form{
            display: flex;
            justify-content: center;
            align-items: center;
        }
        table{
            text-align: justify;
        }
    </style>
</head>
<body>
    <form action="" method="POST" enctype="multipart/form-data">
        <table>
            <thead>
                <th colspan="2">THÊM SỮA MỚI</th>
            </thead>
            <tr>
                <td>Mã sữa</td>
                <td><input type="text" name="Ma_sua" value="<?php if (isset($_POST["Ma_sua"])) echo $_POST["Ma_sua"] ?>"></td>
            </tr>
            <tr>
                <td>Tên sữa</td>
                <td><input type="text" name="Ten_sua" value="<?php if (isset($_POST["Ten_sua"])) echo $_POST["Ten_sua"] ?>"></td>
            </tr>
            <tr>
                <td>Hãng sữa</td>
                <td>
                    <select name="Ma_hang_sua">
                    <?php
                        if (!isset($_POST["Ma_hang_sua"])) $_POST["Ma_hang_sua"] = "";
                        $sql = "select * from hang_sua";
                        $result = mysqli_query($conn, $sql);
                        while($res = mysqli_fetch_array($result)){
                            if ($res["Ma_hang_sua"] === $_POST["Ma_hang_sua"])
                                echo "<option value='" . $res["Ma_hang_sua"] . "' selected>" . $res["Ten_hang_sua"] . "</option>";
                            else
                                echo "<option value='" . $res["Ma_hang_sua"] . "'>" . $res["Ten_hang_sua"] . "</option>"; 
                        }
                    ?>
                </select>
                </td>
            </tr>
            <tr>
                <td>Loại sữa</td>
                <td>
                    <select name="Ma_loai_sua">
                        <?php
                            if (!isset($_POST["Ma_loai_sua"])) $_POST["Ma_loai_sua"] = "";
                            $sql = "select * from loai_sua";
                            $result = mysqli_query($conn, $sql);
                            while($res = mysqli_fetch_array($result)){
                                if ($res["Ma_loai_sua"] === $_POST["Ma_loai_sua"])
                                    echo "<option value='" . $res["Ma_loai_sua"] . "' selected>" . $res["Ten_loai"] . "</option>"; 
                                else
                                    echo "<option value='" . $res["Ma_loai_sua"] . "'>" . $res["Ten_loai"] . "</option>";
                            }
                        ?>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Trọng lượng</td>
                <td><input type="text" name="Trong_luong" value="<?php if (isset($_POST["Trong_luong"])) echo $_POST["Trong_luong"]; ?>"> (gr hoặc ml)</td>
            </tr>
            <tr>
                <td>Đơn giá</td>
                <td><input type="text" name="Don_gia" value="<?php if (isset($_POST["Don_gia"])) echo $_POST["Don_gia"]; ?>"> (VND)</td>
            </tr>
            <tr>
                <td>Thành phần dinh dưỡng</td>
                <td><textarea rows="3" cols="50" name="Thanh_phan_dinh_duong"><?php if (isset($_POST["Thanh_phan_dinh_duong"])) echo $_POST["Thanh_phan_dinh_duong"]; ?></textarea></td>
            </tr>
            <tr>
                <td>Lợi ích</td>
                <td><textarea rows="3" cols="50" name="Loi_ich"><?php if (isset($_POST["Loi_ich"])) echo $_POST["Loi_ich"]; ?></textarea></td>
            </tr>
            <tr>
                <td>Hình ảnh</td>
                <td>
                    <input type="text" name="Hinh_anh_" disabled value="<?php if (isset($file_name)) echo $file_name; ?>">
                    <input type="file" name="Hinh_anh">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="submit" name="submit" value="Thêm mới">    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<?php
    if (isset($_POST["submit"])){
        if (empty($error) === true){
            if (mysqli_num_rows($result) !== 0){
                echo "Đã thêm sản phẩm thành công";
            }
        }
        else print_r($error);
    }
    echo "<div style='text-align:center'><a href='index.php'>Quay lại</a><div>";
    mysqli_close($conn);
?>
