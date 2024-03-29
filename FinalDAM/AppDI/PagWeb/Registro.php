<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Registro de cuenta PokeDEXplorer</title>
    <link rel="stylesheet" href="./Estilos/estilo.css">
    <link rel="shortcut icon" href="./Estilos/Pokeball.png">
</head>
<body>
    <nav class="navNoHead">
        <ul>
            <li><a href="./Inicio.php">Inicio</a></li>
            <li><a href="./Descargas.php">Descargas</a></li>
            <li><a href="./Contacto.php">Contacto</a></li>
            <li><a href="./GuiaUser.php">Guia de Usuario</a></li>
            <li><a href="./Registro.php">Registro</a></li>
        </ul>
    </nav>
    <main>
        <div class="container">
            <h1 id="h1Registro">Registro de un nuevo entrenador PokeDEXplorer</h1>
            <form class="formRegistro" action="./php/db.php" method="POST">
                <label class="registro" for="nombre">Nombre:</label>
                <input class="registro" type="text" name="nombre" required maxlength="50">
                <label class="registro" for="password">Contraseña:</label>
                <input class="registro" type="password" name="password" required maxlength="20">
                <input class="registro" type="submit" name="click" value="Registra tu aventura">
            </form>
        </div>
    </main>
    <footer id="fotNoScroll">
        <p id="ParrFooterInicio">© 2023 - <?php echo date("Y");?>. PokeDEXplorer</p>
        <h4>Alba Marco Checa</h4>
    </footer>
</body>
</html>