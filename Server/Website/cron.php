<?php
    include('conf.php');
    
    $con->set_charset("utf8");
    // Get all table names from the database
    $tables = array();
    $sql = "SHOW TABLES";
    $result = mysqli_query($con, $sql);
    while ($row = mysqli_fetch_row($result)) {
        $tables[] = $row[0];
    }
    $sqlscript = "";
    foreach ($tables as $table) {
        // Prepare sqlscript for creating table structure
        $query = "SHOW CREATE TABLE $table";
        $result = mysqli_query($con, $query);
        $row = mysqli_fetch_row($result);
        $sqlscript .= "\n\n" . $row[1] . ";\n\n";
        $query = "SELECT * FROM $table";
        $result = mysqli_query($con, $query);
        $columncount = mysqli_num_fields($result);
        // Prepare sqlscript for dumping data for each table
        for ($i = 0; $i < $columncount; $i ++) {
            while ($row = mysqli_fetch_row($result)) {
                $sqlscript .= "INSERT INTO $table VALUES(";
                for ($j = 0; $j < $columncount; $j ++) {
                    $row[$j] = $row[$j];
                 if (isset($row[$j])) {
                        $sqlscript .= '"' . $row[$j] . '"';
                    } else {
                        $sqlscript .= '""';
                    }
                    if ($j < ($columncount - 1)) {
                        $sqlscript .= ',';
                    }
                }
                $sqlscript .= ");\n";
            }
        }
        $sqlscript .= "\n"; 
    }

    if(!empty($sqlscript)) {
        $filename = 'backups/database-' . date("Y-m") . '.sql';
        $file = fopen($filename, "w");
        fwrite($file, $sqlscript);
        fclose($file);
    }
    
    // mysqli_query($con,"update student set status=default,cf=default,s1=default,s2=default,s3=default,s4=default,s5=default,s6=default,s7=default,s8=default,s9=default,s10=default");
    mysqli_close($con);
?>