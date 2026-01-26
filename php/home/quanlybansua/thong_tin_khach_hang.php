<?php
    session_start();
    require("connect.php");

    $sql = "select * from khach_hang";
    $result = mysqli_query($conn, $sql);
    echo "<table align='center' border=1 style='border-collapse:collapse'>";
    echo '<head>
        <th>Mã KH</th>
        <th>Tên khách hàng</th>
        <th>Giới tính</th>
        <th>Địa chỉ</th>
        <th>Số điện thoại</th>
        <th>Email</th>
        <th></th>
        <th></th>
        </head>';
    while ($row = mysqli_fetch_array($result)){
        echo "<tr>";
        echo "<td>".$row["Ma_khach_hang"]."</td>";
        echo "<td>".$row["Ten_khach_hang"]."</td>";
        if ($row["Phai"]) echo "<td>Nữ</td>";
        else echo "<td>Nam</td>";
        echo "<td>".$row["Dia_chi"]."</td>";
        echo "<td>".$row["Dien_thoai"]."</td>";
        echo "<td>".$row["Email"]."</td>";
        echo "<td><a href='bai2.12_edit.php?Ma_khach_hang=".$row["Ma_khach_hang"]."'>Sửa</a></td>";
        echo "<td><a href='bai2.12_delete.php?Ma_khach_hang=".$row["Ma_khach_hang"]."'>Xóa</a></td>";
        echo "</tr>";
    }
    echo "<tr><td align='center' colspan='8'><a href='index.php'>Quay lại</a></td></tr>";
    echo "</table>";
    mysqli_close($conn);
?>