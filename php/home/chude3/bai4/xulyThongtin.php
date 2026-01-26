<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>XuLy</title>
    <link rel="stylesheet" href="css/hehe.css">
</head>
<body>
    <?php
        function print_custom($prop){
            if (isset($_POST[$prop])) return $_POST[$prop];
            return "";
        }
        echo "<p>Bạn đã đăng nhập thành công, dưới đấy là những thông tin bạn nhập:<br/>";
        echo "Họ tên: " . print_custom("name"). "<br/>";
        echo "Giới tính: " . print_custom("gender") . "<br/>";
        echo "Địa chỉ: " . print_custom("address") . "<br/>";
        echo "Điện thoại: " . print_custom("phone_number") . "<br/>";
        echo "Quốc tịch: " . print_custom("nationally") . "<br/>";
        echo "Môn học: " . print_custom("sub1") . " " . print_custom("sub2") . " " . print_custom("sub3") . " " . print_custom("sub4");
        echo "ghi chú: " . print_custom("comment") . "</p>";
    ?>
    <a href="nhapThongtin.html">Quay lại</a>
</body>
</html>