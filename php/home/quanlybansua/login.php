<?php
        $username = "";
        $password = "";
        $valid = true;
        $status = "";

        if (isset($_POST["submit"])){
            if (isset($_POST["username"])){
                $username = trim($_POST["username"]);
                if ($username === "" || strpos($username, " ")) $valid = false;
            }
            else $valid = false;

            if (isset($_POST["password"])){
                $password = trim($_POST["password"]);
                if ($password === "" || strpos($password, " ")) $valid = false;
            }
            else $valid = false;

            if ($valid){
                require("connect.php");

                $sql = "select * from user where username = '" . $username. "' and password = '". $password. "'";
                $res = mysqli_query($conn, $sql);

                if (mysqli_num_rows($res)){
                    $status = "<p style='color:green'>Đăng nhập thành công</p>";
                    session_start();
                    $_SESSION["LOGGED_IN"] = true;
                    $_SESSION["USER"] = $username;
                    header('Location: index.php');
                    exit;
                }
                else $status = "<p style='color:red'>Đăng nhập thất bại</p>";
                mysqli_close($conn);
            }
            else $status = "<p style='color:red'>Không hợp lệ</p>";
        }
    ?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="css/hehe.css">
</head>
<body>
    <form action="" method="post">
        <h3>ĐĂNG NHẬP</h3>
        <input type="text" name="username" id="username" placeholder="Tên đăng nhập" value="<?php if (isset($username)) echo $username; ?>">
        <input type="text" name="password" id="password" placeholder="Mật khẩu" value="<?php if (isset($password)) echo $password; ?>">
        <input type="submit" name="submit" value="Đăng nhập">
        <div><a href="register.php">Đăng ký</a></div>
        <div><?php echo $status ?></div>
    </form>
</body>
</html>