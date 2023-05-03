<?php
    $servername = "db4free.net:3306";
    $username = "albaroot";
    $password = "albaroot";
    $dbname = "appfinal";

    $conn = new mysqli($servername, $username, $password, $dbname);
    // Verificación de la conexión, si falla se verá.
    if ($conn->connect_error) {
        die("Conexión fallida: " . $conn->connect_error);
    }

    $iden = $_GET['ID'];
    $query = "SELECT USER FROM USERS WHERE ID = '$iden'";
    $data = mysqli_query($conn, $query) or die (mysqli_error($conn));
    $row = mysqli_fetch_array($data);
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro completado PokeDEXplorer</title>
    <link rel="stylesheet" href="../Estilos/estilo.css" type="text/css">
</head>
<body>
    <nav>
        <ul>
            <li><a href="../Inicio.php">Inicio</a></li>
            <li><a href="../Descargas.html">Descargas</a></li>
            <li><a href="../Contacto.html">Contacto</a></li>
            <li><a href="../Registro.html">Registro</a></li>
        </ul>
    </nav>
    <main>
        <p class="regCorrecto">¡Bienvenido <span id="refNoDeco"><?php echo $row['USER'];?></span>!</p>
        <p class="regCorrecto">¡Felicidades! Su cuenta de Explorador Pokemon ha sido completado.<p class="regCorrecto">Si desea 
            empezar a utilizar la aplicación, deberá de ir a <a id="refNoDeco" href="../Descargas.html">DESCARGAS</a> y
            descargarse el ejecutable más reciente. <br><br>
        </p>
        <p class="regCorrecto">Muchas gracias por darle una oportunidad a la aplicación.</p>
    </main>
    <footer>
        <p id="ParrFooterInicio">© 2023 - <?php echo date("Y");?>. PokeDEXplorer</p>
        <h4>Alba Marco Checa</h4>
    </footer>
</body>
</html>