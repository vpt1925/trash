<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="css/hehe.css">
</head>
<body>
    <?php
        $ketqua = "";
        if (isset($_POST["op"])){
            $op = trim($_POST["op"]);
            if (empty($op)) $ketqua = "Lỗi";
        }
        if (isset($_POST["num1"])){
            $num1 = trim($_POST["num1"]);
            if (empty($num1) || !is_numeric($num1)) $ketqua = "Lỗi";
            else $num1 = (float) $num1;
        }
        if (isset($_POST["num2"])){
            $num2 = trim($_POST["num2"]);
            if (empty($num2) || !is_numeric($num2)) $ketqua = "Lỗi";
            else $num2 = (float) $num2;
        }
        if ($ketqua == ""){
            switch ($op){
                case "+": $ketqua = $num1 + $num2; break;
                case "-": $ketqua = $num1 - $num2; break;
                case "*": $ketqua = $num1 * $num2; break;
                case "/":
                    if ($num2 == 0) $ketqua = "Lỗi";
                    else $ketqua = $num1 / $num2; break;
                default: $ketqua = "Lỗi";
            }
        }
    ?>
    <div id="container_">
        <form action="" method="post">
            <table>
                <thead>
                    <th>PHÉP TÍNH TRÊN HAI SỐ</th>
                </thead>
                <tr>
                    <td>Chọn phép tính:</td>
                    <td><?php if (isset($op)) echo $op; ?> </td>
                </tr>
                <tr>
                    <td>Số thứ nhất</td>
                    <td><?php if (isset($num1)) echo $num1 ?> </td>
                </tr>
                <tr>
                    <td>Số thứ hai</td>
                    <td><?php if (isset($num2)) echo $num2 ?> </td>
                </tr>
                <tr>
                    <td>Kết quả:</td>
                    <td><?php echo $ketqua ?></td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="pheptinh.php">Quay lại</td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>