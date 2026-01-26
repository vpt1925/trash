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
        if (isset($_POST["Ten_khach_hang"])){
            $Ten_khach_hang = trim($_POST["Ten_khach_hang"]);
            if (empty($Ten_khach_hang)) $error[] = "Tên KH không được trống";
        }
        if (isset($_POST["Phai"])){
            $Phai = trim($_POST["Phai"]);
            if ($Phai === "") $error[] = "Giới tính KH không được trống";
            if ($Phai != 0 && $Phai != 1) $error[] = "Giới tính KH phải là 1 (Nữ) hoặc 0 (Nam)";
        }
        if (isset($_POST["Dia_chi"])){
            $Dia_chi = trim($_POST["Dia_chi"]);
            if (empty($Dia_chi)) $error[] = "Địa chỉ KH không được trống";
        }
        if (isset($_POST["Dien_thoai"])){
            $Dien_thoai = trim($_POST["Dien_thoai"]);
            if (empty($Dien_thoai)) $error[] = "Số điện thoại KH không được trống";
        }
        if (isset($_POST["Email"])){
            $Email = trim($_POST["Email"]);
            if (empty($Email)) $error[] = "Email KH không được trống";
        }

        if (empty($error) === true){
            $sql = "select * from khach_hang where Ma_khach_hang = '" . $Ma_khach_hang . "'";
            $result = mysqli_query($conn, $sql);
            if (mysqli_num_rows($result) === 0){
                echo "Lỗi";
            }
            else{
                $sql = "update khach_hang"
                        ." set Ten_khach_hang = '" . $Ten_khach_hang . "'"
                        .", Phai = '" . $Phai . "'"
                        .", Dia_chi = '" . $Dia_chi . "'"
                        .", Dien_thoai = '" . $Dien_thoai . "'"
                        .", Email = '" . $Email . "'"
                        ." where Ma_khach_hang = '". $Ma_khach_hang . "'";
                $result = mysqli_query($conn, $sql);
                if ($result){
                    header("Location: thong_tin_khach_hang.php");
                    exit;
                }
                else echo "Lỗi";
            }
        }
        else print_r($error);
        mysqli_close($conn);
    }
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
                <td><input type="text" name="Ten_khach_hang" value="<?php if (isset($Ten_khach_hang)) echo $Ten_khach_hang; ?>"></td>
            </tr>
            <tr>
                <td>Giới tính</td>
                <td><input type="text" name="Phai" value="<?php if (isset($Phai)) echo $Phai; ?>"></td>
            </tr>
            <tr>
                <td>Địa chỉ</td>
                <td><input type="text" name="Dia_chi" value="<?php if (isset($Dia_chi)) echo $Dia_chi; ?>"></td>
            </tr>
            <tr>
                <td>Số điện thoại</td>
                <td><input type="text" name="Dien_thoai" value="<?php if (isset($Dien_thoai)) echo $Dien_thoai; ?>"></td>
            </tr>
            <tr>
                <td>Email</td>
                <td><input type="text" name="Email" value="<?php if (isset($Email)) echo $Email; ?>"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="submit" name="submit" value="Sửa">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center"><a href="thong_tin_khach_hang.php">Quay lại</a></td>
            </tr>
        </table>
    </form>
</body>
</html>