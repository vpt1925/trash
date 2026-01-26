<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
    <link rel="stylesheet" href="css/hehe.css">
</head>
<body>
    <div id="container_">
        <form action="ketquapheptinh.php" method="post">
            <table id="table_">
                <tr>
                    <th colspan="2">PHÉP TÍNH TRÊN HAI SỐ</th>
                </tr>
                <tr>
                    <td>Chọn phép tính:</td>
                    <td>
                        <input type="radio" name="op" value="+"> Cộng
                        <input type="radio" name="op" value="-"> Trừ
                        <input type="radio" name="op" value="*"> Nhân
                        <input type="radio" name="op" value="/"> Chia
                    </td>
                </tr>
                <tr>
                    <td>Số thứ nhất</td>
                    <td><input type="text" name="num1" id="num1_"></td>
                </tr>
                <tr>
                    <td>Số thứ hai</td>
                    <td><input type="text" name="num2" id="num2_"></td>
                </tr>
                <tr>
                    <td colspan="2">
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