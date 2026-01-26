<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
    <link rel="stylesheet" href="css/hehe.css">
</head>
<body>
    <?php
        $cost = "";
        if (isset($_POST["name"])){
            $name = $_POST["name"];
        }
        else $name = "";

        if (isset($_POST["old_index"])){
            $old_index = trim($_POST["old_index"]);
            if (!empty($old_index)) $old_index = (float) $old_index;
            else $cost = "Lỗi";
        }
        else $old_index = 0;

        if (isset($_POST["new_index"])){
            $new_index = $_POST["new_index"];
            if (!empty($new_index)) $new_index = (float) $new_index;
            else $cost = "Lỗi";
        }
        else $new_index = 0;

        if (isset($_POST["unit_price"])){
            $unit_price = $_POST["unit_price"];
            if (!empty($unit_price)) $unit_price = (float) $unit_price;
            else $cost = "Lỗi";
        }
        else $unit_price = 0;

        if ($cost == "" && $new_index >= $old_index){
            $cost = ($new_index - $old_index) * $unit_price;
        }
        else $cost = "Lỗi";
    ?>
    <div id="container_">
        <form action="" method="post">
            <table id="table_">
                <thead>
                    <th>THANH TOÁN TIỀN ĐIỆN</th>
                </thead>
                <tr>
                    <td>Tên chủ hộ:</td>
                    <td><input type="text" name="name" id="name_"></td>
                </tr>
                <tr>
                    <td>Chỉ số cũ:</td>
                    <td><input type="text" name="old_index" id="old_index_"> (kW)</td>
                </tr>
                <tr>
                    <td>Chỉ số mới:</td>
                    <td><input type="text" name="new_index" id="new_index_"> (kW)</td>
                </tr>
                <tr>
                    <td>Đơn giá:</td>
                    <td><input type="text" name="unit_price" id="unit_price_"> (VND)</td>
                </tr>
                <tr>
                    <td>Số tiền thanh toán:</td>
                    <td><input type="text" name="cost" id="cost_" disabled="disabled" value="<?php echo $cost ?>" > (VND)</td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <input type="submit" name="submit" id="submit_">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><a href="../../index.html">Home</a></td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>