<?php
    session_start();
    if (!isset($_SESSION["LOGGED_IN"])) $_SESSION["LOGGED_IN"] = false;
?>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Quản lý Bán Sữa</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f5f5f5;
            color: #333;
        }
        
        .header {
            background-color: #34495E;
            color: white;
            padding: 15px 20px;
            display: flex;
            justify-content: space-between;
        }
        
        .logo {
            font-size: 18px;
            font-weight: bold;
        }
        
        .auth-links a {
            color: white;
            text-decoration: none;
            margin-left: 15px;
        }
        
        .container {
            max-width: 800px;
            margin: 40px auto;
            padding: 20px;
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }
        
        h1 {
            color: #34495E;
            text-align: center;
            margin-bottom: 30px;
        }

        .note {
            background-color: #EBF5FB;
            padding: 15px;
            margin: 20px 0;
            border-left: 4px solid #3498DB;
        }
        
        .exercises {
            margin-top: 30px;
        }
        
        .exercise-links {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 10px;
            margin-top: 15px;
        }
        
        .exercise-links a {
            display: block;
            padding: 10px;
            background-color: #f8f9fa;
            border: 1px solid #ddd;
            text-decoration: none;
            color: #34495E;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="header">
        <div class="logo">Quản lý Bán Sữa</div>
        <div class="auth-links">
            <?php
                if ($_SESSION["LOGGED_IN"]){
                    echo "<a>".$_SESSION["USER"]."</a>";
                    echo "<a href='logout.php'>Đăng xuất</a>";
                }
                else{
                    echo "<a href='login.php'>Đăng nhập</a>";
                    echo "<a href='register.php'>Đăng ký</a>";
                }
            ?>
        </div>
    </div>
    
    <div class="container">
        <div class="exercises">
            <div class="note">
                <strong>Tài khoản mặc định:</strong><br>
                Tên đăng nhập: admin<br>
                Mật khẩu: admin
            </div>
            <div class="exercise-links">
                <a href="bai2.3.php">Bài 2.3</a>
                <a href="bai2.4.php">Bài 2.4</a>
                <a href="bai2.5.php">Bài 2.5</a>
                <a href="bai2.6.php">Bài 2.6</a>
                <a href="bai2.7.php">Bài 2.7</a>
                <a href="bai2.8.php">Bài 2.8</a>
                <a href="bai2.9.php">Bài 2.9</a>
                <a href="bai2.10.php">Bài 2.10</a>
                <a href="bai2.11.php">Bài 2.11</a>
                <a href="thong_tin_khach_hang.php">Bài 2.12</a>
            </div>
        </div>
    </div>
    <div align='center'>
        <a href="../index.html">Trang chủ</a>
    </div>
</body>
</html>