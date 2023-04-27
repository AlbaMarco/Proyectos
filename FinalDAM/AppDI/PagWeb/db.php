<?php
$servername = "db4free.net:3306";
$username = "albaroot";
$password = "albaroot";
$dbname = "appfinal";
$nuevaURL = "./Inicio.html";

$conn = new mysqli($servername, $username, $password, $dbname);
echo "<script>alert('hola')</script>";
// Verificación de la conexión, si falla se verá.
if ($conn->connect_error) {
    die("Conexión fallida: " . $conn->connect_error);
}

if(isset($_POST['submit'])){
    //header("Location: $nuevaURL");
    echo "<script>alert('hola2')</script>";
    // $_POST lo que hace es coger el nombre del html.
    $nombre = $_POST["nombre"];
    $contrasena = $_POST["contrasena"];

    // Inserción de datos en la tabla "usuarios"
    $sql = "INSERT INTO `USERS` (`USER`, `PASS`, `ULT_VISITA`, `LEVELU`, `LEVELA`) VALUES ('$nombre', '$contrasena', '0', 1, 0)";

    if ($conn->query($sql) === TRUE) {
        echo "Registro exitoso";
    } else {
        echo "Error: " . $sql . "<br>" . $conn->error;
    }
}
$conn->close();
?>