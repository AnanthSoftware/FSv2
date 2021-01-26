<?php
    error_reporting(0);

    $message = array();
    
    session_start();
    if(empty($_SESSION)) {header("Location: index.php?login=expried");exit;}
    if(!$_SESSION['regno']) {header("Location: index.php?login=expried");exit;}
    
    $regno = $_SESSION['regno'];
    include('conf.php');
    
    $query=mysqli_query($con,"select * from student where regno='$regno'");
    $num_rows=mysqli_num_rows($query);
    if ($num_rows>0) {
        $row = mysqli_fetch_array($query);
        $_SESSION['name'] = $row['name'];
        $message = array("ok","Welcome, {$_SESSION['name']}");
    } else {
        header("Location: index.php?invalid=login");
        exit;
    }
    
    if($_GET) {
        if($_GET['form']) {
            if($_GET['form'] == "disabled") {
                $message = array("error","This service is disabled by class advisor.");
            }
            if($_GET['form'] == "donecollege") {
                $message = array("alert","Your college & department form already received.");
            }
            if($_GET['form'] == "donefaculty") {
                $message = array("alert","Your faculty evaluation form already received.");
            }
        }
    }
    
    if($_SESSION['flag'] == "logedin") {
        $y = date("Y") - (2000 + substr($regno,4,2));
        if(date("m") < 7) {
            $s = $y * 2;
        } else {
            $y = $y + 1;
            $s = $y * 2 - 1;
        }
        switch ($y) {
            case 1: $year = "I"; break;
            case 2: $year = "II"; break;
            case 3: $year = "III"; break;
            case 4: $year = "IV"; break;
        }
        switch ($s) {
            case 1: $sem = "I"; break;
            case 2: $sem = "II"; break;
            case 3: $sem = "III"; break;
            case 4: $sem = "IV"; break;
            case 5: $sem = "V"; break;
            case 6: $sem = "VI"; break;
            case 7: $sem = "VII"; break;
            case 8: $sem = "VIII"; break;
        }
        
        $b = substr($regno,6,3);
        switch($b) {
            case 103: $branch = "CIVIL"; break;
            case 104: $branch = "CSE"; break;
            case 105: $branch = "EEE"; break;
            case 106: $branch = "ECE"; break;
            case 114: $branch = "MECH"; break;
            case 205: $branch = "IT"; break;
            case 631: $branch = "MBA"; break;
            case 621: $branch = "MCA"; break;
        }
        
        $classcode = substr($regno,0,9);
        
        $query = mysqli_query($con,"select * from subject where classcode='$classcode' and semester='$sem'");
        if (mysqli_num_rows($query) > 0) {
            $i = 0;
            $count = 0;
            while($row = mysqli_fetch_assoc($query)) {
                $sub[$i] = $row["subject"];
                if($sub[$i] != "") 
                    $count++;
                $i++;
            }
            $_SESSION['sub1'] = $sub[0];
            $_SESSION['sub2'] = $sub[1];
            $_SESSION['sub3'] = $sub[2];
            $_SESSION['sub4'] = $sub[3];
            $_SESSION['sub5'] = $sub[4];
            $_SESSION['sub6'] = $sub[5];
            $_SESSION['sub7'] = $sub[6];
            $_SESSION['sub8'] = $sub[7];
            $_SESSION['sub9'] = $sub[8];
            $_SESSION['sub10'] = $sub[9];
            $_SESSION['count'] = $count;
        }
        $_SESSION['year'] = $year;
        $_SESSION['sem'] = $sem;
        $_SESSION['branch'] = $branch;
        $_SESSION['classcode'] = $classcode;
    }
    
    if (isset($_POST["upload"])) {
        $fileinfo = @getimagesize($_FILES["file"]["tmp_name"]);
        $width = $fileinfo[0];
        $height = $fileinfo[1];
        for($i = $width; $i > 1; $i--) {
            if(($height % $i) == 0 && ($width % $i) == 0) {
                $height = $height / $i;
                $width = $width / $i;
            }
        }
        $ratio = "$height:$width";
        
        $allow = array("png","jpg","jpeg");
        $extension = pathinfo($_FILES["file"]["name"], PATHINFO_EXTENSION);
        
        if(!file_exists($_FILES["file"]["tmp_name"])) {
            $message = array("alert","Choose image file to upload.");
        } elseif(!in_array($extension, $allow)) {
            $message = array("alert","Upload valid images. Only PNG and JPEG are allowed.");
        } else if ($ratio != "4:3") {
            $message = array("alert","$value Image dimension should be 4:3 ratio.");
        } else if (($_FILES["file"]["size"] > 2000000)) {
            $message = array("error","Image size exceeds 2MB.");
        } else {
            $target = "profiles/" . md5($_SESSION['regno']) . '.' . $extension;
            if (move_uploaded_file($_FILES["file"]["tmp_name"], $target)) {
                $message = array("ok","Profile photo uploaded successfully.");
            } else {
                $message = array("error","Problem in uploading image files.");
            }
        }
    }
?>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Student Home</title>
    <meta name="theme-color" content="#3CB372">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="author" content="Ananth B">
    <meta name="description" content="Feedback System of MPNMJ Engineering Collage">
    <meta name="robots" content="noindex, nofollow">
    <link rel="icon shortcut" href="logo.png">
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Roboto&family=Sen&display=swap');
        body{margin:0;padding:0;min-width:256px;background:#FBFBFB} span{color:#FF2222} a{text-decoration:none}
        table{width:100%;display:table} .row2,.row3,.row4{float:left;display:table-cell}
        .row2{width:50%}.row3{width:30%} .row4{width:25%}
        .card{overflow:hidden;margin:2.5%;padding:0;background:#F4F4F4;box-shadow:0 2px 5px 0 rgba(0,0,0,0.1),0 2px 10px 0 rgba(0,0,0,0.08)}
        .box{overflow:hidden;margin:0;padding:2%} marquee{font-family:"Roboto",sans-serif;font-size:16px;color:#222}
        .item{float:left;padding:10px;margin:0 10px 10px 0;height:100px;width:100px;color:#FBFBFB;background:#00BBFF}
        .item p{margin-top:20px;text-align:center;color:white;font-size:16px;font-weight:bold} .item:hover{background: #555}
        h2,h4,p,.items{font-family:"Sen",sans-serif} p{font-size:18px;color:#222}
        h2{height:28px;margin:0;padding:10px 2%;font-size:24px;color:#FBFBFB;background:#3CB372}
        .profile{width:80%;float:left} .logout{width:4%;background:#FF4444;float:left} .reload{width:4%;float:left}
        h4{margin-top:0;margin-bottom:10px;padding:0;color:#555;font-size:20px}
        button{cursor:pointer;font-size:14px;color:white;background:#3CB372}
        button:hover,button:active,button:focus{background:#30B060} form input{display:none}
        .upload{width:130px;text-align:center;border:1px solid #BBB;background:#EEE;color:#222;font-family:"Sen",sans-serif;display:inline-block;padding:6px 9px;cursor:pointer}
        .icon{height:22px;width:22px;fill:white;position:relative;top:2px}
        .message{padding:12px 5% 10px 5%;color:white} .message p {margin:0;color:white} .bar{height:2px}
        .ok{background:#3CB372} .alert{background:#FFCC00} .error{background:#FF2222}
        .accordion{background:#EEE!important;color:#444;cursor:pointer;padding:12px;width:100%;border:none;text-align:left;outline:none;font-family:"Sen",sans-serif;font-size:16px;transition:0.4s}
        .active,.accordion:hover{background:#CCC!important}
        .accordion:after {content:"\002B";color:#666;font-size:16px;font-weight:bold;float:right;margin-left:10px}
        .active:after {content:"\2212"}
        .panel{padding:10px 12px;display:none;background:#F8F8F8;font-family:"Sen",sans-serif;overflow:hidden}
        @media only screen and (max-width: 640px) {
            .row2,.row3,.row4{width:100%;display:block;}
            .card{margin:5%} .box{padding:5%}
            h2 {padding:10px 4%}.profile{width:60%} .logout{width:8%} .reload{width:8%}
        }
    </style>
</head>
<body <?php if(!empty($message)) echo "onload='message()'"?>>
    <svg width="0" height="0" style="display:none">
        <symbol xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" id="form">
            <path d="M3 1.01c-1.108 0-2 .892-2 2v10c0 1.108.892 2 2 2h10c1.108 0 2-.892 2-2v-10c0-1.108-.892-2-2-2H3zM4 3h8v2H4V3zm0 4h8v2H4V7zm0 4h5v2H4v-2z"></path>
        </symbol>
        <symbol xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" id="info">
            <path d="M8 1a7 7 0 00-7 7 7 7 0 007 7 7 7 0 007-7 7 7 0 00-7-7zm0 2.75A1.25 1.25 0 019.25 5 1.25 1.25 0 018 6.25 1.25 1.25 0 016.75 5 1.25 1.25 0 018 3.75zM7 7h2v5H7V7z"></path>
        </symbol>
        <symbol xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" id="exit">
            <path d="M 3,1 C 3,1 2,1 2,2 V 14 C 2,15 3,15 3,15 H 13 C 14,15 14,14 14,14 V 9.75 L 12,11 V 13 H 4 V 3 H 12 V 5 L 14,6.25 V 2 C 14,1 13,1 13,1 Z M 10,5 V 7 H 6 V 9 H 10 V 11 L 14.5,8 Z"></path>
        </symbol>
        <symbol xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" id="package">
            <path d="M3 1c-.554 0-.825.475-1 1L1 5v8c0 .554.446 1 1 1h12c.554 0 1-.446 1-1V5l-1-3c-.175-.526-.446-1-1-1zm.67 2h8.66l.334 1h-9.33zM7 6h2v3h1.75L8 12 5.25 9H7z"></path>
        </symbol>
        <symbol xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" id="reload">
            <path d="M 8 1 A 7 7 0 0 0 1 8 A 7 7 0 0 0 8 15 A 7 7 0 0 0 14.701 10 L 12.58 10 A 5 5 0 0 1 8 13 A 5 5 0 0 1 3 8 A 5 5 0 0 1 8 3 A 5 5 0 0 1 11.529 4.4707 L 9 7 L 15 7 L 15 1 L 12.947 3.0527 A 7 7 0 0 0 8 1 z"></path>
        </symbol>
        <symbol xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" id="services">
            <path d="M6.25 1l-.154 1.844a5.5 5.5 0 00-1.608.93l-1.676-.79-1.75 3.032 1.522 1.056A5.5 5.5 0 002.5 8a5.5 5.5 0 00.08.932L1.062 9.984l1.75 3.032 1.672-.787a5.5 5.5 0 001.612.923l.15 1.85h3.5l.154-1.844a5.5 5.5 0 001.608-.93l1.676.79 1.75-3.032-1.522-1.056a5.5 5.5 0 00.084-.928 5.5 5.5 0 00-.08-.932l1.518-1.052-1.75-3.032-1.672.787A5.5 5.5 0 009.9 2.85L9.75 1h-3.5zM8 6a2 2 0 012 2 2 2 0 01-2 2 2 2 0 01-2-2 2 2 0 012-2z"></path>
        </symbol>
        <symbol xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" id="user">
            <path d="M11 5a3 3 0 01-3 3 3 3 0 01-3-3 3 3 0 013-3 3 3 0 013 3zM8 9c-6 0-6 4-6 4v1h12v-1s0-4-6-4z"></path>
        </symbol>
    </svg>
    
    <?php if(!empty($message)) echo 
        "<div id='message' class='card'>
            <div class='message ".$message[0]."'><p>".$message[1]."</p></div>
            <div id='bar' class='bar ".$message[0]."'></div>
        </div>";
    ?>
    <div id="profile" class="card">
        <div>
            <div style="clear: both">
                <h2 class="profile"><svg class="icon"><use xlink:href="#user"></use></svg> Profile</h2>
                <a href="home.php"><h2 title="Reload" class="reload"><svg class="icon" style="float:right;top:4px;height:20px;width:20px"><use xlink:href="#reload"></use></svg></h2></a>
                <a href="index.php?logout=true"><h2 title="Logout" class="logout"><svg class="icon" style="float:right;top:4px;height:20px;width:20px"><use xlink:href="#exit"></use></svg></h2></a>
            </div>
        </div>
        <div class="table box">
            <div class="row2">
                <img height="200" width="150" <?php 
                    $file = "profiles/" . md5($_SESSION['regno']) . '.';
                    if(file_exists($file."png")) echo "src='".$file."png'";
                    if(file_exists($file."jpg")) echo "src='".$file."jpg'";
                    if(file_exists($file."jpeg")) echo "src='".$file."jpeg'";
                ?>>
                <form style="margin:10px 0 0 0" action="home.php" method="post" enctype="multipart/form-data">
                    <label class="upload">
                        <input type="file" name="file" onchange="autosubmit()">Upload Photo
                    </label>
                    <input id="submit" type="submit" name="upload">
                </form>
            </div>
            <div class="row2">
                <p>Name: <?php echo $_SESSION['name']?></p>
                <p>Register number: <?php echo $_SESSION['regno']?></p>
                <p>Year: <?php echo $_SESSION['year']?></p>
                <p>Semester: <?php echo $_SESSION['sem']?></p>
                <p style="margin-bottom:0">Branch: <?php echo $_SESSION['branch']?></p>
            </div>
        </div>
    </div>
    <div id="info" class="card">
        <h2><svg class="icon"><use xlink:href="#info"></use></svg> Informations</h2>
        <div class="box">
            <marquee behavior="scroll" direction="left" scrollamount="2">No informations/events</marquee>
        </div>
    </div>
    <div id="materials" class="card">
        <h2><svg class="icon"><use xlink:href="#package"></use></svg> Materials</h2>
        <ul class="box">
            <?php
                $first = "<button class='accordion'>";
                $last = "</button><div class='panel'><a href='#'><span>Not available</span></a></div>";
                if($_SESSION['sub1'] != "") echo "$first{$_SESSION['sub1']}$last";
                if($_SESSION['sub2'] != "") echo "$first{$_SESSION['sub2']}$last";
                if($_SESSION['sub3'] != "") echo "$first{$_SESSION['sub3']}$last";
                if($_SESSION['sub4'] != "") echo "$first{$_SESSION['sub4']}$last";
                if($_SESSION['sub5'] != "") echo "$first{$_SESSION['sub5']}$last";
                if($_SESSION['sub6'] != "") echo "$first{$_SESSION['sub6']}$last";
                if($_SESSION['sub7'] != "") echo "$first{$_SESSION['sub7']}$last";
                if($_SESSION['sub8'] != "") echo "$first{$_SESSION['sub8']}$last";
                if($_SESSION['sub9'] != "") echo "$first{$_SESSION['sub9']}$last";
                if($_SESSION['sub10'] != "") echo "$first{$_SESSION['sub10']}$last";
            ?>
        </ul>
    </div>
    <div id="services" class="card">
        <h2><svg class="icon"><use xlink:href="#services"></use></svg> Services</h2>
        <div class="box">
            <h4><svg class="icon" style="fill:#555;height:18px;width:18px"><use xlink:href="#form"></use></svg> Feedback Forms</h4>
            <div class="items">
                <a class="item" href="form.php?form=college"><p>College & Department form</p></a>
                <a class="item" href="form.php?form=faculty"><p>Faculty evaluation form</p></a>
            </div>
        </div>
    </div>
    
    <script>
        var acc=document.getElementsByClassName("accordion"); var i;
        for (i=0;i<acc.length;i++) {
            acc[i].addEventListener("click", function() {this.classList.toggle("active");var panel=this.nextElementSibling;
                if(panel.style.display==="block"){panel.style.display="none";}else{panel.style.display="block";}
            });
        }
        function autosubmit(){document.getElementById("submit").click();}
        function message(){move();setTimeout(closemessage,5200);}
        function closemessage(){document.getElementById("message").style.display="none";}
        function move(){var bar=document.getElementById("bar");var width=1;var id=setInterval(frame,50);
            function frame(){if(width>=100){clearInterval(id);}else{width++;bar.style.width=width+'%';}}
        }
    </script>
</body>
</html>