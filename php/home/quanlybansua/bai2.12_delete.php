<?php
    session_start();
    if (isset($_GET["Ma_khach_hang"])){
        $Ma_khach_hang = trim($_GET["Ma_khach_hang"]);
    }
    require("connect.php");
    $sql = "select * from khach_hang where Ma_khach_hang = '" . $Ma_khach_hang . "'";
    $result = mysqli_query($conn, $sql);
    $row = mysqli_fetch_array($result);

    $Ten_khach_hang = $row["Ten_khach_hang"];
    $Phai = $row["Phai"];
    $Dia_chi = $row["Dia_chi"];
    $Dien_thoai = $row["Dien_thoai"];
    $Email = $row["Email"];

    if (isset($_POST["submit"])){
        $error = array();
        if (!$_SESSION["LOGGED_IN"]) $error[] = "Chưa đăng nhập";
        
        if (empty($error) === true){
            $sql = "delete from khach_hang where Ma_khach_hang = '" . $Ma_khach_hang . "'";
            $result = mysqli_query($conn, $sql);
            if ($result){
                echo "Xóa thành công khách hàng";
                header("Location: thong_tin_khach_hang.php");
                exit;
            }
            else{
                echo "Lỗi";
            }
        }
        else print_r(($error));
    }
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
    <form action="" method="post">
        <table align="center">
            <tr>
                <td>Mã KH</td>
                <td><input type="text" name="Ma_khach_hang" value="<?php if (isset($Ma_khach_hang)) echo $Ma_khach_hang; ?>" disabled></td>
            </tr>
            <tr>
                <td>Tên khách hàng</td>
                <td><input type="text" name="Ten_khach_hang" value="<?php if (isset($Ten_khach_hang)) echo $Ten_khach_hang; ?>" disabled></td>
            </tr>
            <tr>
                <td>Giới tính</td>
                <td><input type="text" name="Phai" value="<?php if (isset($Phai)) echo $Phai; ?>" disabled></td>
            </tr>
            <tr>
                <td>Địa chỉ</td>
                <td><input type="text" name="Dia_chi" value="<?php if (isset($Dia_chi)) echo $Dia_chi; ?>" disabled></td>
            </tr>
            <tr>
                <td>Số điện thoại</td>
                <td><input type="text" name="Dien_thoai" value="<?php if (isset($Dien_thoai)) echo $Dien_thoai; ?>" disabled></td>
            </tr>
            <tr>
                <td>Email</td>
                <td><input type="text" name="Email" value="<?php if (isset($Email)) echo $Email; ?>" disabled></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="submit" name="submit" value="Xóa">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center"><a href="thong_tin_khach_hang.php">Quay lại</a></td>
            </tr>
        </table>
    </form>
</body>
</html>