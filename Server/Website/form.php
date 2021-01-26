<?php
    error_reporting(0);
    
    $message = array();
    
    session_start();
    if(empty($_SESSION)) {header("Location: index.php?login=expried");exit;}
    if(!$_SESSION['regno']) {header("Location: index.php?login=expried");exit;}
    
    $regno = $_SESSION['regno'];
    $date = date("Y/m/d");
    
    include('conf.php');
    $query=mysqli_query($con,"select * from student where regno='$regno'");
    $num_rows=mysqli_num_rows($query);
    if ($num_rows>0) {
        $row = mysqli_fetch_array($query);
	    if ($row['status'] == '0') {
	        header("Location: home.php?form=disabled");
            exit;
	    }
    } else {
        header("Location: index.php?invalid=login");
        exit;
    }
    
    if($_GET) {
        if($_GET['form']) {
            if($_GET['form'] == "college") {
                $_SESSION['flag'] = "college";
            } elseif($_GET['form'] == "faculty") {
                $_SESSION['flag'] = "faculty";
            } else {
                header("Location: home.php");
                exit;
            }
        } else {
            header("Location: home.php");
            exit;
        }
        
    }
    
    $query = mysqli_query($con,"select * from student where regno='{$_SESSION['regno']}'");
    $student = mysqli_fetch_array($query);
    $_SESSION['cf'] = $student['cf'];
    $_SESSION['s1'] = $student['s1'];
    $_SESSION['s2'] = $student['s2'];
    $_SESSION['s3'] = $student['s3'];
    $_SESSION['s4'] = $student['s4'];
    $_SESSION['s5'] = $student['s5'];
    $_SESSION['s6'] = $student['s6'];
    $_SESSION['s7'] = $student['s7'];
    $_SESSION['s8'] = $student['s8'];
    $_SESSION['s9'] = $student['s9'];
    $_SESSION['s10'] = $student['s10'];
    
    if($_GET['form'] == "college") {
        if($_SESSION['cf'] != 0) {
            header("Location: home.php?form=donecollege");
            exit;
        }
    }
    if($_GET['form'] == "faculty") {
        if($_SESSION['count'] <= ($_SESSION['s1'] + $_SESSION['s2'] + $_SESSION['s3'] + $_SESSION['s4'] + $_SESSION['s5'] + $_SESSION['s6'] + $_SESSION['s7'] + $_SESSION['s8'] + $_SESSION['s9'] + $_SESSION['s10'])) {
            header("Location: home.php?form=donefaculty");
            exit;
        }
    }
    
    if(isset($_POST['r1'])) {
        $r1 = $_POST['r1'];
        if($_POST['r21'] == "YES") $r2 = $_POST['r22']; else $r2 = "NO";
        $r31 = $_POST['r31'];
        if($_POST['r32'] == null) $r32 = "NULL"; else $r32 = "'".$_POST['r32']."'";
        if($_POST['r33'] == null) $r33 = "NULL"; else $r33 = "'".$_POST['r33']."'";
        $r34 = $_POST['r34'];
        if($_POST['r35'] == null) $r35 = "NULL"; else $r35 = "'".$_POST['r35']."'";
        $r4 = $_POST['r4'];
        $r5 = $_POST['r5'];
        if($_POST['r61'] == "YES") $r6 = $_POST['r62']; else $r6 = "NO";
        if($_SESSION['regno']) {
            $query=mysqli_query($con,"select * from student where regno='{$_SESSION['regno']}' && cf='0'");
            $num_rows=mysqli_num_rows($query);
            if ($num_rows>0) {
                mysqli_query($con,"insert into college values('$date', {$_SESSION['regno']}, '{$_SESSION['sem']}', '{$_SESSION['branch']}', '$r1', '$r2', '$r31', $r32, $r33, $r34, $r35, '$r4', '$r5', '$r6')");
                mysqli_query($con,"update student set cf='1' where regno='{$_SESSION['regno']}'");
                $message = array("#33CC33","Your feedback received successfully. <br> Thank you!");
                $_SESSION['flag'] = "exit";
            } else {
                $message = array("#FF2222","Error! while upadating values, try again.");
                $_SESSION['flag'] = "exit";
            }
        } else {
            mysqli_close($con);
            session_destroy();
            header("Location: index.php");
            exit;
        }
    }
        
    if(isset($_POST['sub'])) {
        $_SESSION['flag'] = "submitted";
        $_SESSION['status'] = $_POST["sub"];
        
        switch($_SESSION['status']) {
            case 1: $_SESSION['subject'] = $_SESSION['sub1']; break;
            case 2: $_SESSION['subject'] = $_SESSION['sub2']; break;
            case 3: $_SESSION['subject'] = $_SESSION['sub3']; break;
            case 4: $_SESSION['subject'] = $_SESSION['sub4']; break;
            case 5: $_SESSION['subject'] = $_SESSION['sub5']; break;
            case 6: $_SESSION['subject'] = $_SESSION['sub6']; break;
            case 7: $_SESSION['subject'] = $_SESSION['sub7']; break;
            case 8: $_SESSION['subject'] = $_SESSION['sub8']; break;
            case 9: $_SESSION['subject'] = $_SESSION['sub9']; break;
            case 10: $_SESSION['subject'] = $_SESSION['sub10']; break;
        }
        
        $query = mysqli_query($con,"select * from subject where classcode='{$_SESSION['classcode']}' and semester='{$_SESSION['sem']}' and subject='{$_SESSION['subject']}'");
        $row = mysqli_fetch_array($query);
        
        $_SESSION['staffid'] = $row['staffid'];
        $_SESSION['code'] = $row['code'];
        $_SESSION['staffname'] = $row['staffname'];
        $_SESSION['staffdept'] = $row['staffdept'];
    }
        
    if(isset($_POST['s1'])) {
        $s1 = $_POST['s1'];
        $s2 = $_POST['s2'];
        $s3 = $_POST['s3'];
        $s4 = $_POST['s4'];
        $s5 = $_POST['s5'];
        $s6 = $_POST['s6'];
        $s7 = $_POST['s7'];
        $s8 = $_POST['s8'];
        $s9 = $_POST['s9'];
        $s10 = $_POST['s10'];
        $total = $s1+ $s2 + $s3 + $s4 + $s5 + $s6 + $s7 + $s8 + $s9 + $s10;
            
        if($_SESSION['regno']) {
            $query=mysqli_query($con,"select * from student where regno='{$_SESSION['regno']}' && s{$_SESSION['status']}='0'");
            $num_rows=mysqli_num_rows($query);
            if ($num_rows>0) {
                mysqli_query($con,"insert into feedback values('$date','{$_SESSION['regno']}','{$_SESSION['sem']}','{$_SESSION['branch']}','{$_SESSION['staffid']}','{$_SESSION['staffname']}','{$_SESSION['staffdept']}','{$_SESSION['subject']}','{$_SESSION['code']}','$s1','$s2','$s3','$s4','$s5','$s6','$s7','$s8','$s9','$s10','$total')");
                mysqli_query($con,"update student set s{$_SESSION['status']}='1' where regno='{$_SESSION['regno']}'");
	            mysqli_close($con);
	            if(($_SESSION['count'] -1) <= ($_SESSION['s1'] + $_SESSION['s2'] + $_SESSION['s3'] + $_SESSION['s4'] + $_SESSION['s5'] + $_SESSION['s6'] + $_SESSION['s7'] + $_SESSION['s8'] + $_SESSION['s9'] + $_SESSION['s10'])) {
                    goto finish;
                }
            } else {
                $message = array("#FF2222","Error! while upadating values, try again.");
                $_SESSION['flag'] = "exit";
            }
        } else {
            mysqli_close($con);
            session_destroy();
            header("Location: index.php");
            exit;
        }
        header("Location: form.php?form=faculty");
        exit;
        finish:
        $message = array("#33CC33","Your feedback received successfully. <br> Thank you!");
        $_SESSION['flag'] = "exit";
    }
?>


<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="theme-color" content="#3CB372">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Feedback System of MPNMJ Engineering college">
    <meta name="author" content="Ananth B">
    <meta name="robots" content="noindex, nofollow">
    <title>Feedback Form</title>
    <link rel="icon shortcut" href="logo.png">
    <style>
        @import url('https://fonts.googleapis.com/css?family=Roboto|Sen|Ubuntu&display=swap');
        body{width:92%;margin:0;padding:4%;min-width:256px;background:#EEE;font-family:"Roboto", sans-serif}
        h2{margin:10px 40px;text-align:center;font-family:"Sen", sans-serif}
        .form{position:relative;top:5px;margin:15px -0.5% 0 0;padding:0} .date{float:right}
        p{margin:0;padding:4px 8px;font-size:16px} a{text-decoration:none} span{margin:0 8px}
        .info,.sub{border:1px solid #555;margin:10px 0}
        .baner{color:white;background:#555;margin:0}
        input,select{font-family: inherit;font-size:14px}
        input{color:white;background:#666;padding:3px 16px;margin:auto 2px;border:2px solid #666}
        input:hover,input:active{background:#555;border:2px solid #555}
        select{padding:2px 32px;border:2px solid #DDD;background:#DDD}
        td{padding:2px 8px} td:first-child{padding:2px 14px}
        .extra{padding:8px} .text{margin:15px 0 5px 0;padding:0} .mt{margin-top:8px}
        .main{width:100%;margin:0;display:table;border:1px solid #555}
        .column{width:50%;display:table-cell} .column tr{vertical-align:top} 
        .column td{padding-bottom:12px;border-bottom:border:1px solid #555}
        @media screen and (max-width: 640px) {
            p{padding: 4px} span{margin:0 4px} td:first-child{padding:2px 6px}
            .main{width:99.6%} .column{width:100%;display:block;border:none!important}
            .cmt{width:94%;margin:6px 3%} .container{padding:0 8px}
        }
    </style>
</head>
<body>
    <h2>Feedback Form</h2>
    <?php if($_SESSION['flag'] != "exit") {
            if($_SESSION['flag'] == "college") echo "<p class='form'>College & Department form<span class='date'>Date: ".date("d/m/Y")."</span></p>";
            else echo "<p class='form'>Faculty evaluation form<span class='date'>Date: ".date("d/m/Y")."</span></p>";
        }
    ?>
    
    <div class="info">
        <p class="baner">Student details</p>
        <p>
            <span>Register number:&nbsp;<?php echo $_SESSION['regno']?></span>
            <span>Year:&nbsp;<?php echo $_SESSION['year']?></span>
            <span>Semester:&nbsp;<?php echo $_SESSION['sem']?></span>
            <span>Branch:&nbsp;<?php echo $_SESSION['branch']?></span>
        </p>
    </div>

    <?php if($_SESSION['flag'] == "college"): ?>
    <form method='post' action='form.php?form=college'>
        <div class="info" style="overflow-x:auto">
            <p style="min-width:840px" class="baner">About college</p>
            <table style="min-width:840px">
                <tr>
                    <td>1) How do you find the functioning of the colege ?</td>
                    <td><label for="l11">5</label><input type="radio" id="l11" name="r1" value=5 required></td>
                    <td><label for="l12">4</label><input type="radio" id="l12" name="r1" value=4></td>
                    <td><label for="l13">3</label><input type="radio" id="l13" name="r1" value=3></td>
                    <td><label for="l14">2</label><input type="radio" id="l14" name="r1" value=2></td>
                    <td><label for="l15">1</label><input type="radio" id="l15" name="r1" value=1></td>
                </tr>
                <tr>
                    <td>2) Do you convey your grievances to the Principal ?</td>
                    <td colspan="2"><label for="l21">Yes</label><input type="radio" id="l21" onclick="check()" name="r21" value="YES" required></td>
                    <td colspan="2"><label for="l22">No</label><input type="radio" id="l22" onclick="check()" name="r21" value="NO"></td>
                </tr>
                <tr id="r2" style="display:none">
                    <td>&emsp; How well your grievances are redressed ?</td>
                    <td colspan="4"><label for="l23">Redressed Well</label><input type="radio" id="l23" name="r22" value="YES/RW" required></td>
                    <td colspan="4"><label for="l24">Some what Redressed</label><input type="radio" id="l24" name="r22" value="YES/SR"></td>
                </tr>
            </table>
            
             <table style="min-width:420px" id="question">
                <tr>
                    <td>*) Are you Hosteler ?</td>
                    <td colspan="2"><label for="l01">Yes</label><input type="radio" id="l01" onclick="hosteler()" name="r0" value="YES" required></td>
                    <td colspan="2"><label for="l02">No</label><input type="radio" id="l02" onclick="dayscholar()" name="r0" value="NO"></td>
                </tr>
            </table>
                
            <table style="min-width:840px;display:none" id="common">
                <tr>
                    <td>3) How are the "Infrastructural Facilities" in the college ?</td>
                </tr>
            </table>
            <table style="min-width:480px;display:none" id="hosteler">
                <tr>
                    <td>&emsp; A) Library Facilities</td>
                    <td><label for="l311">5</label><input type="radio" id="l311" name="r31" value=5></td>
                    <td><label for="l312">4</label><input type="radio" id="l312" name="r31" value=4></td>
                    <td><label for="l313">3</label><input type="radio" id="l313" name="r31" value=3></td>
                    <td><label for="l314">2</label><input type="radio" id="l314" name="r31" value=2></td>
                    <td><label for="l315">1</label><input type="radio" id="l315" name="r31" value=1></td>
                </tr>
                <tr>
                    <td>&emsp; B) Hostel Facilities</td>
                    <td><label for="l321">5</label><input type="radio" id="l321" name="r32" value=5></td>
                    <td><label for="l322">4</label><input type="radio" id="l322" name="r32" value=4></td>
                    <td><label for="l323">3</label><input type="radio" id="l323" name="r32" value=3></td>
                    <td><label for="l324">2</label><input type="radio" id="l324" name="r32" value=2></td>
                    <td><label for="l325">1</label><input type="radio" id="l325" name="r32" value=1></td>
                </tr>
                <tr>
                    <td>&emsp; C) Hostel Food</td>
                    <td><label for="l331">5</label><input type="radio" id="l331" name="r33" value=5></td>
                    <td><label for="l332">4</label><input type="radio" id="l332" name="r33" value=4></td>
                    <td><label for="l333">3</label><input type="radio" id="l333" name="r33" value=3></td>
                    <td><label for="l334">2</label><input type="radio" id="l334" name="r33" value=2></td>
                    <td><label for="l335">1</label><input type="radio" id="l335" name="r33" value=1></td>
                </tr>
                <tr>
                    <td>&emsp; D) Canteen Facilities</td>
                    <td><label for="l341">5</label><input type="radio" id="l341" name="r34" value=5></td>
                    <td><label for="l342">4</label><input type="radio" id="l342" name="r34" value=4></td>
                    <td><label for="l343">3</label><input type="radio" id="l343" name="r34" value=3></td>
                    <td><label for="l344">2</label><input type="radio" id="l344" name="r34" value=2></td>
                    <td><label for="l345">1</label><input type="radio" id="l345" name="r34" value=1></td>
                </tr>
            </table>
            
            <table style="min-width:480px;display:none" id="dayscholar">
                <tr>
                    <td>&emsp; A) Library Facilities</td>
                    <td><label for="l311">5</label><input type="radio" id="l311" name="r31" value=5></td>
                    <td><label for="l312">4</label><input type="radio" id="l312" name="r31" value=4></td>
                    <td><label for="l313">3</label><input type="radio" id="l313" name="r31" value=3></td>
                    <td><label for="l314">2</label><input type="radio" id="l314" name="r31" value=2></td>
                    <td><label for="l315">1</label><input type="radio" id="l315" name="r31" value=1></td>
                </tr>
                <tr>
                    <td>&emsp; B) Canteen Facilities</td>
                    <td><label for="l341">5</label><input type="radio" id="l341" name="r34" value=5></td>
                    <td><label for="l342">4</label><input type="radio" id="l342" name="r34" value=4></td>
                    <td><label for="l343">3</label><input type="radio" id="l343" name="r34" value=3></td>
                    <td><label for="l344">2</label><input type="radio" id="l344" name="r34" value=2></td>
                    <td><label for="l345">1</label><input type="radio" id="l345" name="r34" value=1></td>
                </tr>
                <tr>
                    <td>&emsp; C) Transport Facilities </td>
                    <td><label for="l351">5</label><input type="radio" id="l351" name="r35" value=5></td>
                    <td><label for="l352">4</label><input type="radio" id="l352" name="r35" value=4></td>
                    <td><label for="l353">3</label><input type="radio" id="l353" name="r35" value=3></td>
                    <td><label for="l354">2</label><input type="radio" id="l354" name="r35" value=2></td>
                    <td><label for="l355">1</label><input type="radio" id="l355" name="r35" value=1></td>
                </tr>
            </table>
            
            
        </div>
        <div class="info" style="overflow-x:auto">
            <p style="min-width:940px" class="baner">About department</p>
            <table style="min-width:940px">
                <tr>
                    <td>1) How do you rate the functioning of the Department ?</td>
                    <td><label for="l41">5</label><input type="radio" id="l41" name="r4" value=5 required></td>
                    <td><label for="l42">4</label><input type="radio" id="l42" name="r4" value=4></td>
                    <td><label for="l43">3</label><input type="radio" id="l43" name="r4" value=3></td>
                    <td><label for="l44">2</label><input type="radio" id="l44" name="r4" value=2></td>
                    <td><label for="l45">1</label><input type="radio" id="l45" name="r4" value=1></td>
                </tr>
                <tr>
                    <td>2) How do you rate the Laboratory facilities in the Department ? </td>
                    <td><label for="l51">5</label><input type="radio" id="l51" name="r5" value=5 required></td>
                    <td><label for="l52">4</label><input type="radio" id="l52" name="r5" value=4></td>
                    <td><label for="l53">3</label><input type="radio" id="l53" name="r5" value=3></td>
                    <td><label for="l54">2</label><input type="radio" id="l54" name="r5" value=2></td>
                    <td><label for="l55">1</label><input type="radio" id="l55" name="r5" value=1></td>
                </tr>
                <tr>
                    <td>3) Do you corvey your grievances to be Department Head ? </td>
                    <td colspan="2"><label for="l61">Yes</label><input type="radio" id="l61" onclick="check()" name="r61" value="YES" required></td>
                    <td colspan="2"><label for="l62">No</label><input type="radio" id="l62" onclick="check()" name="r61" value="NO"></td>
                </tr>
                <tr id="r6" style="display:none">
                    <td>&emsp; How well your grievances are redressed ? </td>
                    <td colspan="4"><label for="l63">Redressed Well</label><input type="radio" id="l63" name="r62" value="YES/RW" required></td>
                    <td colspan="4"><label for="l64">Some what Redressed</label><input type="radio" id="l64" name="r62" value="YES/SR"></td>
                </tr>
            </table>
        </div>
        <center><p>5-Excellent &emsp; 4-Very Good &emsp; 3-Good &emsp; 2-Satisfactory &emsp; 1-Poor</p></center>
        <center><br>
            <a href='home.php'><input type='button' value='Exit'></a>
            <input type='submit' value='Submit'>
        </center><br>
    </form>
    <?php endif; ?>
    
    <?php if($_SESSION['flag'] == "faculty"): ?>
    <div class='sub'>
        <center>
        <form method='post' action='form.php?form=faculty'>
            <p class='baner'>Select subject</p>
                <p class='extra'>
                    <select name='sub' id='subject'>
                    <?php
                    if($_SESSION['s1'] == 0) {
                        if($_SESSION['sub1'] != "") echo "<option value=1>",$_SESSION['sub1'],"</option>";
                    }
                    if($_SESSION['s2'] == 0) {
                        if($_SESSION['sub2'] != "") echo "<option value=2>",$_SESSION['sub2'],"</option>";
                    }
                    if($_SESSION['s3'] == 0) {
                        if($_SESSION['sub3'] != "") echo "<option value=3>",$_SESSION['sub3'],"</option>";
                    }
                    if($_SESSION['s4'] == 0) {
                        if($_SESSION['sub4'] != "") echo "<option value=4>",$_SESSION['sub4'],"</option>";
                    }
                    if($_SESSION['s5'] == 0) {
                        if($_SESSION['sub5'] != "") echo "<option value=5>",$_SESSION['sub5'],"</option>";
                    }
                    if($_SESSION['s6'] == 0) {
                        if($_SESSION['sub6'] != "") echo "<option value=6>",$_SESSION['sub6'],"</option>";
                    }
                    if($_SESSION['s7'] == 0) {
                        if($_SESSION['sub7'] != "") echo "<option value=7>",$_SESSION['sub7'],"</option>";
                    }
                    if($_SESSION['s8'] == 0) {
                        if($_SESSION['sub8'] != "") echo "<option value=8>",$_SESSION['sub8'],"</option>";
                    }
                    if($_SESSION['s9'] == 0) {
                        if($_SESSION['sub9'] != "") echo "<option value=9>",$_SESSION['sub9'],"</option>";
                    }
                    if($_SESSION['s10'] == 0) {
                        if($_SESSION['sub10'] != "") echo "<option value=10>",$_SESSION['sub10'],"</option>";
                    }
                    ?>
                    </select>
                <input type='submit' value='Submit'>
            </p>
        </form>
        </center>
    </div><br>
    <center><a href='home.php'><input type='button' value='Exit'></a></center>
    <?php endif; ?>
    
    <?php if($_SESSION['flag'] == "submitted"): ?>
    <div class='info'>
        <p class='baner'>Subject details</p>
        <p>
            <span>Subject&nbsp;name:&nbsp;<?php echo $_SESSION['subject']?></span>
            <span>Subject&nbsp;code:&nbsp;<?php echo $_SESSION['code']?></span>
        </p>
    </div>
    <div class='info'>
        <p class='baner'>Staff details</p>
        <p>
            <span>Staff&nbsp;name:&nbsp;<?php echo $_SESSION['staffname']?></span>
            <span>Department:&nbsp;<?php echo $_SESSION['staffdept']?></span>
        </p>
    </div>
    <form method='post' action='form.php?form=faculty'>
        <div class='main'>
            <div class='column' style='border-right:1px solid #555'>
                <table>
                    <tr><td class='text mt'>1) Punctuality</td><td>
                    <select name='s1' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>2) Lesson planning</td><td>
                    <select name='s2' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>3) Clarity of Explanation</td><td>
                    <select name='s3' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>4) Subject Knowledge</td><td>
                    <select name='s4' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>5) Syllabus Coverage</td><td>
                    <select name='s5' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                </table>
            </div>
            <div class='column'>
                <table>
                    <tr><td class='text mt'>6) Answer to the questions / Clarifying doubts</td><td>
                    <select name='s6' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>7) Motivating Students</td><td>
                    <select name='s7' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>8) Integrity</td><td>
                    <select name='s8' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>9) Use of teaching aids & Course content</td><td>
                    <select name='s9' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                    <tr><td class='text'>10) Amicability with students</td><td>
                    <select name='s10' required>
                        <option value=10>10</option>
                        <option value=9>9</option>
                        <option value=8>8</option>
                        <option value=7>7</option>
                        <option value=6>6</option>
                        <option value=5>5</option>
                        <option value=4>4</option>
                        <option value=3>3</option>
                        <option value=2>2</option>
                        <option value=1>1</option>
                    </select></td></tr>
                </table>
            </div>
        </div>
        <br>
        <div>
            <center>
                <a href='form.php?form=faculty'><input type='button' value='Back'></a>
                <input type='submit' value='Submit'>
            </center>
        </div>
        <br>
    </form>
    <?php elseif($_SESSION['flag'] == "exit"): ?>
    <center><br>
        <p style="color:<?php echo $message[0]?>"><?php echo $message[1]?></p><br>
        <form action='index.php'><input type='submit' value='Exit'></form><br>
    </center>
    <?php endif; ?>
    
    <script>
        function hosteler() {
            document.getElementById("question").style.display = "none";
            document.getElementById("common").style.display = "block";
            document.getElementById("hosteler").style.display = "block";
            document.getElementById("l311").required = true;
            document.getElementById("l321").required = true;
            document.getElementById("l331").required = true;
            document.getElementById("l341").required = true;
            
        }
        function dayscholar() {
            document.getElementById("question").style.display = "none";
            document.getElementById("common").style.display = "block";
            document.getElementById("dayscholar").style.display = "block";
            document.getElementById("l311").required = true;
            document.getElementById("l341").required = true;
            document.getElementById("l351").required = true;
        }
        function check() {
            if(document.getElementById("l21").checked == true) {
                document.getElementById("r2").style.display = "table-row";
            } else {
                document.getElementById("r2").style.display = "none";
                document.getElementById("l24").checked = true;
            }
            if(document.getElementById("l61").checked == true) {
                document.getElementById("r6").style.display = "table-row";
            } else {
                document.getElementById("r6").style.display = "none";
                document.getElementById("l64").checked = true;
            }
            if(window.history.replaceState) {
                window.history.replaceState(null,null,window.location.href);
            }
        }
    </script>
</body>
</html>