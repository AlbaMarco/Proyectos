<?php
    $servername = "db4free.net:3306";
    $username = "albaroot";
    $password = "albaroot";
    $dbname = "appfinal";
    $regCorrecto = "../PagsRegistro/RegCorrecto.html?=1234";
    $regIncorrecto = "../PagsRegistro/RegIncorrecto.html";

    $conn = new mysqli($servername, $username, $password, $dbname);
    // Verificación de la conexión, si falla se verá.
    if ($conn->connect_error) {
        die("Conexión fallida: " . $conn->connect_error);
    }

    if(isset($_POST['click'])){
        // $_POST lo que hace es coger el nombre del html. 
        // Eliminar caracteres raro y miro la longitud, al igual que en el html.
        $nombre = substr($_POST["nombre"], 0, 50);
        $contrasena = substr($_POST["password"], 0, 20);
        
        // Fecha actual en formato yyyy-mm-dd
        $fecha_actual = date("Y-m-d");

        $nombre = mysqli_real_escape_string($conn, $_POST["nombre"]);
        $contrasena = mysqli_real_escape_string($conn, $_POST["password"]);
        
        // Eliminar cualquier carácter que no sea letra o número del nombre de usuario
        $nombre = preg_replace("/[^a-zA-Z0-9]+/", "", $nombre);

        // Generar un hash con password_hash() y salt aleatorio. Viene incluido por la función nativa.
        $hash = password_hash($contrasena, PASSWORD_DEFAULT);

        // Comprobar si el usuario ya existe en la base de datos
        $sql = "SELECT COUNT(*) AS cuenta FROM `USERS` WHERE `USER` = '$nombre'";
        $result = $conn->query($sql);
        $fila = $result->fetch_assoc();
        $numReg = $fila["cuenta"]; // Almacena el valor del campo 'cuenta'

        if ($numReg > 0) {
            die("El nombre de usuario ya existe. Por favor, elige otro.");
        }

        // Inserción de datos en la tabla "usuarios"
        $stmt = $conn->prepare("INSERT INTO `USERS` (`USER`, `PASS`, `ULT_VISITA`, `LEVELU`, `LEVELA`) VALUES (?, ?, ?, 1, 0)");
        $stmt->bind_param("sss", $nombre, $hash, $fecha_actual); // Inserto tres strings.

        if ($stmt->execute()) {
            $last_id = $conn->insert_id;
            header("Location: ../PagsRegistro/RegCorrecto.php?ID=$last_id");
        } else {
            header("Location: $regIncorrecto");
        }
    }
    $conn->close();
?>