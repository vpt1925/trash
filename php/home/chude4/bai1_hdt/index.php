<?php
    abstract class Nguoi{
        public $ho_ten;
        public $dia_chi;
        public $gioi_tinh;
    }

    class SinhVien extends Nguoi{
        public $lop_hoc;
        public $nganh_hoc;
        public function __construct($ht, $dc, $gt, $lh, $nh){
            $this->ho_ten = $ht;
            $this->dia_chi = $dc;
            $this->gioi_tinh = $gt;
            $this->lop_hoc = $lh;
            $this->nganh_hoc = $nh;
        }
        public function tinhDiemThuong(){
            switch($this->nganh_hoc){
                case "CNTT": $dt = 1; break;
                case "KT": $dt = 1.5; break;
                default: $dt = 0;
            }
            return $dt;
        }
    }

    class GiangVien extends Nguoi{
        const TRINH_DO = ["Cử nhân", "Thạc sĩ", "Tiến sĩ"]; 
        const LUONG_CO_BAN = 1500000;
        public $trinh_do;
        public function __construct($ht, $dc, $gt, $td)
        {
            $this->ho_ten = $ht;
            $this->dia_chi = $dc;
            $this->gioi_tinh = $gt;
            if (!in_array($td, self::TRINH_DO)) throw new InvalidArgumentException("khônng hợp lệ");
            $this->trinh_do = $td;
        }
        public function tinhLuong(){
            switch($this->trinh_do){
                case "Cử nhân": $luong = self::LUONG_CO_BAN * 2.34; break;
                case "Thạc sĩ": $luong = self::LUONG_CO_BAN * 3.67; break;
                case "Tiến sĩ": $luong = self::LUONG_CO_BAN * 5.66; break;
                default: $luong = -1;
            }
            return $luong;
        }
    }

    function fetch($var){
        if (isset($_POST[$var])){
            $tmp = trim($_POST[$var]);
            return empty($tmp) ? "" : $tmp;
        }
        else return "";
    }

    $ho_ten = fetch("ho_ten");
    $dia_chi = fetch("dia_chi");
    $gioi_tinh = fetch("gioi_tinh");
    $lop_hoc = fetch("lop_hoc");
    $nganh_hoc = fetch("nganh_hoc");
    $trinh_do = fetch("trinh_do");
    $chuc_vu = fetch("chuc_vu");

    if (isset($_POST["submit"])){
        if (isset($chuc_vu)){
            if ($chuc_vu === "sv"){
                $sv = new SinhVien($ho_ten, $dia_chi, $gioi_tinh, $lop_hoc, $nganh_hoc);
                $diem_thuong = $sv->tinhDiemThuong();
                $thong_bao = "Họ tên: $ho_ten<br/>
                    Địa chỉ: $dia_chi<br/>
                    Giới tính: $gioi_tinh<br/>
                    Lớp học: $lop_hoc<br/>
                    Ngành học: $nganh_hoc<br/>
                    Điểm thưởng: $diem_thuong<br/>";
            }
            else if ($chuc_vu === "gv"){
                $gv = new GiangVien($ho_ten, $dia_chi, $gioi_tinh, $trinh_do);
                $luong = $gv->tinhLuong();
                $thong_bao = "Họ tên: $ho_ten<br/>
                    Địa chỉ: $dia_chi<br/>
                    Giới tính: $gioi_tinh<br/>
                    Lương: $luong<br/>";
            }
        }
        
    }
?>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
    <link rel="stylesheet" href="css/hehe.css">
</head>
<body>
    <form action="" method="post">
        <div class="form-group">
            <label for="ho_ten">Họ tên</label>
            <input type="text" id="ho_ten" name="ho_ten" value="<?php if (isset($ho_ten)) echo $ho_ten; ?>">
        </div>
        
        <div class="form-group">
            <label for=dia_chi>Địa chỉ</label>
            <input type="text" id="dia_chi" name="dia_chi" value="<?php if (isset($dia_chi)) echo $dia_chi; ?>">
        </div>
        
        <div class="form-group">
            <label for="gioi_tinh">Giới tính</label>
            <input type="text" id="gioi_tinh" name="gioi_tinh" value="<?php if (isset($gioi_tinh)) echo $gioi_tinh; ?>">
        </div>
        
        
        <div class="form-group">
            <label>
                <input type="radio" name="chuc_vu" value="sv" checked <?php if (isset($chuc_vu)) if ($chuc_vu === "sv") echo "checked='checked'"; ?>> Sinh viên
            </label>
            <label>
                <input type="radio" name="chuc_vu" value="gv" <?php if (isset($chuc_vu)) if ($chuc_vu === "gv") echo "checked='checked'"; ?>> Giảng viên
            </label>
        </div>
        
        <div class="form-group chuc_vu-sv">
            <label for="lop_hoc">Lớp học</label>
            <input type="text" id="lop_hoc" name="lop_hoc" value="<?php if (isset($lop_hoc)) echo $lop_hoc; ?>">
        </div>
        
        <div class="form-group chuc_vu-sv">
            <label for="nganh_hoc">Ngành học</label>
            <input type="text" id="nganh_hoc" name="nganh_hoc" value="<?php if (isset($nganh_hoc)) echo $nganh_hoc; ?>">
        </div>
        
        <div class="form-group chuc_vu-gv hidden">
            <label for="trinh_do">Trình độ</label>
            <input type="text" id="trinh_do" name="trinh_do" value="<?php if (isset($trinh_do)) echo $trinh_do; ?>">
        </div>
        
        <input type="submit" name="submit" value="Tính">

        <div id="thong_bao"><?php if (isset($thong_bao)) echo $thong_bao; ?></div>
        <a href="../../index.html">Home</a>
    </form>

    <script src="js/hehe.js"></script>
</body>
</html>