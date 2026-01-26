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
        function taoMaTran($m, $n){
            $ma_tran = array();
            for ($i = 0; $i < $m; $i++) {
                $hang = array();
                for ($j = 0; $j < $n; $j++) {
                    $hang[] = rand(0, 20);
                }
                $ma_tran[] = $hang;
            }
            return $ma_tran;
        }

        function xuatMaTran($ma_tran){
            $ket_qua = "";
            foreach ($ma_tran as $hang) {
                $ket_qua = $ket_qua . implode(",", $hang) . "\n";
            }
            return $ket_qua;
        }

        function xuatMaTranChanLe($ma_tran){
            $ket_qua = "";
            foreach ($ma_tran as $i => $hang) {
                if ($i % 2 == 1) continue;
                $hang_chan_le = array();
                foreach ($hang as $j => $so) {
                    if ($j % 2 == 0) continue;
                    $hang_chan_le[] = $so;
                }
                $ket_qua = $ket_qua . implode(",", $hang_chan_le) . "\n";
            }
            return $ket_qua;
        }

        function tinhTong($ma_tran){
            $tong = 0;
            foreach($ma_tran as $hang) {
                foreach($hang as $so) {
                    if ($so % 10 === 0) $tong += $so;
                }
            }
            return $tong;
        }

        $so_dong = "";
        $so_cot = "";
        $ma_tran_hien_thi = "";
        $ma_tran_chan_le = "";
        $tong = "";

        if (isset($_POST["so_dong"]) && isset($_POST["so_cot"])){
            $so_dong = trim($_POST["so_dong"]);
            $so_cot = trim($_POST["so_cot"]);
            if (!empty($so_dong) && !empty($so_cot)){
                $so_dong = intval($so_dong);
                $so_cot = intval($so_cot);
                if ($so_dong > 0 && $so_cot > 0){
                    $ma_tran = taoMaTran($so_dong, $so_cot);
                    $ma_tran_hien_thi = xuatMaTran($ma_tran);
                    $ma_tran_chan_le = xuatMaTranChanLe($ma_tran);
                    $tong = tinhTong($ma_tran);
                } else {
                    $ma_tran_hien_thi = "Số dòng và số cột phải là số nguyên dương!";
                }
            }
        }
    ?>
    <div id="container">
        <form action="" method="post" id="form">
            <div id="title">
                <h1>Phát sinh mảng và tính toán</h1>
            </div>
            <table>
                <tr>
                    <td>Số dòng:</td>
                    <td><input type="text" name="so_dong" id="so_dong" value="<?php echo $so_dong; ?>"></td>
                </tr>
                <tr>
                    <td>Số cột:</td>
                    <td><input type="text" name="so_cot" id="so_cot" value="<?php echo $so_cot; ?>"></td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="submit" name="submit" id="submit" value="Phát sinh ma trận"></td>
                </tr>
                <tr>
                    <td>Ma trận:</td>
                    <td><textarea name="ma_tran" id="ma_tran" readonly><?php echo $ma_tran_hien_thi; ?></textarea></td>
                </tr>
                <tr>
                    <td>Ma trận dòng chẵn cột lẻ:</td>
                    <td><textarea name="ma_tran_chan_le" id="ma_tran_chan_le" readonly><?php echo $ma_tran_chan_le; ?></textarea></td>
                </tr>
                <tr>
                    <td>Tổng:</td>
                    <td><input type="text" name="tong" id="tong" value="<?php echo $tong; ?>" readonly></td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><a href="../../index.html">Home</a></td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>