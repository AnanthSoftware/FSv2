<?php
    error_reporting(0);
    
    $portal = "enable"; // Used to enable/disable student portal
    
    $message = array();

    if($_GET) {
        if($_GET['login']) {
            if($_GET['login'] == "expried") {
                setcookie("regno", "", time()-3600);
                header("Location: index.php");
                exit;
            }
        }
        if($_GET['logout']) {
            if($_GET['logout'] == "true") {
                setcookie("regno", "", time()-3600);
                header("Location: index.php");
                exit;
            }
        }
        
        if($_GET['stop']) {
            if($_GET['stop'] == "true") {
                $message = array("error","Student portal closed temporarily, Try some moment later!");
            }
        }
        
        if($_GET['invalid']) {
            if($_GET['invalid'] == "rno") {
                $message = array("error","Register number should be a number.");
            }
            if($_GET['invalid'] == "dob") {
                $message = array("error","DOB in the form of DD/MM/YYYY <br> Eg,01/01/2000.");
            }
        }
    }
    
    if($_POST) {
        if($portal == "enable") {
            if($_POST['rno']) {
                $rno = check_input($_POST['rno']);
                $dob = check_input($_POST['dob']);
                
                if (!preg_match("/^[0-9]*$/" , $rno)) {
                    header("Location: index.php?invalid=rno");
                    exit;
                }
	            if (!preg_match("/^([0-9]{1,2})\\/([0-9]{1,2})\\/([0-9]{4})$/" , $dob)) {
                 header("Location: index.php?invalid=dob");
                    exit;
	            }
             
                include('conf.php');
                $query=mysqli_query($con,"select * from student where regno='$rno' && dob='$dob'");
                $num_rows=mysqli_num_rows($query);
                if ($num_rows>0) {
                    session_id($rno);
                    session_start();
                    $_SESSION['flag'] = "logedin";
                    $_SESSION['regno'] = $rno;
                    setcookie("regno", $rno , time()+365.25*24*60*60);
    	            header("Location: home.php");
    	         exit;
             } else {
                    $message = array("error","Login Failed!");
                }
                mysqli_close($con);
            } else {
                $message = array("alert","Something went wrong!");
            }
        } else {
            header("Location: index.php?stop=true");
        }
    }

    if(isset($_COOKIE["regno"])){
        if($portal == "enable") {
            if(strlen($_COOKIE["regno"]) == 12) {
                $rno = $_COOKIE["regno"];
                session_id($rno);
                session_start();
                $_SESSION['flag'] = "logedin";
                $_SESSION['regno'] = $rno;
    	        header("Location: home.php");
    	        exit;
            }
        } else {
            header("Location: index.php?stop=true");
    	    setcookie("regno", "", time()-3600);
        }
    }

    function check_input($data) {
        $data = trim($data);
        $data = stripslashes($data);
        $data = htmlspecialchars($data);
        return $data;
    }
?>


<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Student Portal</title>
    <meta name="theme-color" content="#3CB372">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="author" content="Ananth B">
    <meta name="description" content="Feedback System of MPNMJ Engineering Collage">
    <meta name="robots" content="noindex, nofollow">
    <link rel="icon shortcut" href="logo.png">
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Roboto&family=Sen&display=swap');
        body{margin:0;padding:0;min-width:256px;background:#FBFBFB} span{color:#FF2222}
        h2,h4,p{font-family:"Sen",sans-serif} h2{padding:16px;margin:0 0 10px 0;color:#FBFBFB;background:
            <?php if($portal == "enable") echo "#3CB372"; else echo "#FF2222"; ?>} 
        p,h4{font-size:18px}
        input,button{border:0;outline:none} input{font-family:"Roboto",sans-serif}
        input{padding:10px 5%;margin:0 0 15px 0;width:50%;font-size:16px;background:#E4E4E4}
        button{padding:10px 5%;margin:0 0 35px 0;width:60%;cursor:pointer;font-size:16px;color:white;background:#3CB372}
        button:hover,button:active,button:focus{background:#30B060}
        .card{margin:50px 30%;background:#F4F4F4;box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.1), 0 5px 5px 0 rgba(0, 0, 0, 0.1)}
        .message{padding:12px 5% 10px 5%;color:white} .message p {margin:0} .bar{height:2px}
        .ok{background:#3CB372} .alert{background:#FFCC00} .error{background:#FF2222}
        .about{display:none} .about p{margin:8px 0;padding:0 16px;color:#111} .about h4{margin:10px 0} a{color:#00AAFF;text-decoration:none}
        @media only screen and (max-width: 640px) {.card{margin:5%} input{width:70%} button{width:80%}}
    </style>
</head>
<body onload="about();<?php if(!empty($message)) echo "message();"?>">
    <div>
        <?php if(!empty($message)) echo 
                "<div id='message' class='card'>
                    <div class='message ".$message[0]."'><p>".$message[1]."</p></div>
                    <div id='bar' class='bar  ".$message[0]."'></div>
                </div>";
        ?>
        <center>
            <form class="card" method="post" action="index.php">
                <h2 onclick="about()">Student Login</h2>
                <p class="text">Register number <span>*</span></p>
                <input type="text" name="rno" maxlength=12 placeholder="7317XXXXXXXX" onclick="this.value='7317'" required><br>
                <p class="text">Date of birth <span>*</span></p>
                <input type="text" name="dob" maxlength=10 placeholder="DD/MM/YYYY" required><br><br>
                <button id="login" type="submit"><b>LOGIN</b></button><br>
                <div class="about" id="about">
                    <h2 style="padding:10px 0">Version: 3.0</h2>
                    <h4>Changelog</h4>
                    <div style="text-align:left;padding:0 10px 0 40px">
                        <p><b>3.0</b><br>&emsp; - Add section</p>
                        <p><b>2.0</b><br>&emsp; - Improve design</p>
                        <p>&emsp; - Enhance with algorithms</p>
                        <p><b>1.0</b><br>&emsp; - Initial version</p>
                    </div><br>
                    <h4 style="margin-top:20px">Developed by</h4>
                    <p>Ananth B</p>
                    <p>Final year, CSE</p>
                    <p>Website : <a href="http://ananthsoft.org">ananthsoft.org</a></p>
                    <p>Email : <a href="mailto:ananthatstar@gmail.com">ananthatstar@gmail.com</a></p>
                    <p>Source code available at <a href="https://github.com/ananthatstar/projects">github.com/ananthatstar/projects</a></p><br>
                </div>
            </form>
        </center>
    </div>
    
    <script>
        function message(){move();setTimeout(closemessage,5200);}
        function closemessage(){document.getElementById("message").style.display="none";}
        function move(){var bar=document.getElementById("bar");var width=1;var id=setInterval(frame,50);
            function frame(){if(width>=100){clearInterval(id);}else{width++;bar.style.width=width+'%';}}
        }
        function about(){
            if(document.getElementById("about").style.display == "none") 
                document.getElementById("about").style.display = "block";
            else document.getElementById("about").style.display = "none";
        }
    </script>
</body>
</html>