<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN">
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>Thông tin sữa</title>
	<style>
		.gender-icon{
			width: 20px;
			height: 20px;
		}
		th{
			color: brown;
		}
		td{
			text-align: center;
		}
	</style>
</head>
<body>
<?php
	require("connect.php");

	$sql = 'select * from khach_hang';
	$result = mysqli_query($conn, $sql);

	echo "<p align='center'><font size='5' color='blue'> THÔNG TIN KHÁCH HÀNG</font></p>";
	echo "<table align='center' width='700' border='1' cellpadding='2' cellspacing='2' style='border-collapse:collapse'>";
	echo '<tr><th>STT</th>
			<th>Mã KH</th>
			<th>Tên khách hàng</th>
			<th>Giới tính</th>
			<th>Địa chỉ</th>
			<th>Số điện thoại</th></tr>';
	
	function mau($doi_mau = false){
		$mau = array("#FEDFC0", "#ffffff");
		static $index_mau = 0;
		if (!$doi_mau) return $mau[$index_mau];
		else{
			$index_mau++;
			if ($index_mau === count($mau)) $index_mau = 0;
		}
	}

	if (mysqli_num_rows($result) !== 0)
	{
		$stt = 1;
		while ($rows=mysqli_fetch_row($result))
		{
			echo "<tr style='background-color:" . mau() . "'>";
			echo "<td>$stt</td>";
			echo "<td>$rows[0]</td>";
			echo "<td>$rows[1]</td>";
			if ($rows[2] === 0) echo "<td><img src='images/male.png' class='gender-icon'/></td>";
			else echo "<td><img src='images/female.png' class='gender-icon'/></td>";
			echo "<td>$rows[3]</td>";
			echo "<td>$rows[4]</td>";
			echo "</tr>";
			if ($stt % 3 === 0) mau(doi_mau:true);
			$stt++;
		}
	}
	echo"</table>";
	echo "<div style='text-align:center'><a href='index.php'>Quay lại</a><div>";
	mysqli_close($conn);
?>
</body>
</html>