<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN">
<?php
   if(isset($_FILES['image'])!=NULL){
      $errors= array();
      $file_name = $_FILES['image']['name'];
      $file_size =$_FILES['image']['size'];
      $file_tmp =$_FILES['image']['tmp_name'];
      $file_type=$_FILES['image']['type'];
      $file_ext=@strtolower(end(explode('.',$_FILES['image']['name'])));
      $expensions= array("jpeg","jpg","png");
      
      if(in_array($file_ext,$expensions)=== false){
         $errors[]="Don't accept image files with this extension, please choose JPEG or PNG.";
      }
      if($file_size > 2097152){
         $errors[]='File size should be 2MB';
		}
      if(empty($errors)==true){
         $path = __DIR__ . "\\images\\" . $file_name;
         move_uploaded_file($file_tmp, $path);
         echo $path;
         echo "Upload File successfully!!!";
      }
      else{
         print_r($errors);
      }
   }
?>
<html>
   <body>
      
      <form action="" method="POST" enctype="multipart/form-data">
         <input type="file" name="image" />
         <input type="submit"/>
			
         <ul>
            <li>File name: <?php echo $_FILES['image']['name'];  ?>
            <li>File size   : <?php echo $_FILES['image']['size'];  ?>
            <li>File type    : <?php echo $_FILES['image']['type'] ?>
            <li><img src=<?php echo "images/" . $file_name; ?>  alt="hehe"></li>
         </ul>
			<a href="uploadForm.html">Quay lại</a>
      </form>
      
   </body>
</html>