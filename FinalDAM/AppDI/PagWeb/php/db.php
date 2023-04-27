<?php
    $servername = "db4free.net:3306";
    $username = "albaroot";
    $password = "albaroot";
    $dbname = "appfinal";
    $regCorrecto = "../PagsRegistro/RegCorrecto.html";
    $regIncorrecto = "../PagsRegistro/RegIncorrecto.html";

    $conn = new mysqli($servername, $username, $password, $dbname);
    // Verificación de la conexión, si falla se verá.
    if ($conn->connect_error) {
        die("Conexión fallida: " . $conn->connect_error);
    }

    if(isset($_POST['click'])){
        
        // $_POST lo que hace es coger el nombre del html.
        echo "<script>alert('he entrado')</script>";
        $nombre = $_POST["nombre"];
        $contrasena = $_POST["password"];

        // Generar un hash con password_hash() y salt aleatorio. Viene incluido por la función nativa.
        $hash = password_hash($contrasena, PASSWORD_DEFAULT);

        // Inserción de datos en la tabla "usuarios"
        $sql = "INSERT INTO `USERS` (`USER`, `PASS`, `ULT_VISITA`, `LEVELU`, `LEVELA`) VALUES ('$nombre', '$hash', '0', 1, 0)";

        if ($conn->query($sql) === TRUE) {
            header("Location: $regCorrecto");
        } else {
            header("Location: $regIncorrecto");
        }
    }
    $conn->close();
?>