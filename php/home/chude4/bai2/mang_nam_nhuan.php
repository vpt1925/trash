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
        function nam_nhuan($n){
            return ($n % 400 === 0) || ($n % 4 === 0 && $n % 100 !== 0);
        }
        $ket_qua = "";
        if (isset($_POST["nam"])){
            $nam = trim($_POST["nam"]);
            if (!empty($nam) && is_numeric(($nam))){
                $nam = intval($nam);
                $mang = array();
                if ($nam >= 2000){
                    for ($i = 2000; $i <= $nam; $i++){
                        if (nam_nhuan($i)) $mang[] = $i;
                    }
                } else {
                    for ($i = $nam; $i <= 2000; $i++){
                        if (nam_nhuan($i)) $mang[] = $i;
                    }
                }
                $ket_qua = implode(" ", $mang);
            }
        }
    ?>
    <div id="container">
        <form action="" method="post" id="form">
            <h2>Tìm năm nhuận</h2>
            <div>
                <label for="nam">Năm:</label>
                <input type="text" name="nam" id="nam" value="<?php if (isset($nam)) echo $nam; ?>">
            </div>
            <textarea rows="2" cols="50" name="mang_nam" id="mang_nam" disabled="disabled"><?php echo $ket_qua; ?></textarea>
            <input type="submit" name="submit" id="submit" value="Tìm năm nhuận">
            <a href="../../index.html">Home</a>
        </form>
    </div>
</body>
</html>