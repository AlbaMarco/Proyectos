<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Descargas PokeDEXplorer</title>
    <link rel="stylesheet" href="./Estilos/estilo.css" type="text/css">
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
        <div id="logoPokemonIzq">
            <img id="logoDescargas" src="./Imgs/PikachuDownloadIzq.png" alt="Imagen de un Pokémon pulsando botoncitos">
        </div>
        </div>
        <div class="containerDown">
            <p class="pVersiones">Segunda versión PokeDEXplorer | Lanzada [X - xxx - 2023] | V.2</p>
            <div>
                <ul class="ulDescargas">
                    <li>Nuevo sistema de encriptación</li>
                    <li>Nuevos niveles de acceso para el rol ADMINISTRADOR</li>
                    <li>Creación de equipos Pokemon</li>
                    <li>Enfrentamientos Pokemon</li>
                    <li>Disponibles Rankings del equipo por usuario</li>
                    <li>Opciones nuevas habilitadas</li>
                    <li>Eliminada la opción de registro, será necesario hacerlo desde la página web.</li>
                    <li>Aumentado el contenido de Acceso Gratuito</li>
                    <li>Base de datos en la nube.</li>
                    <li>Aumentado el tamaño base de la aplicación y hecho escalable según tamaño</li>
                </ul>
                <a id="refNoDecoDes">&emsp;NO DISPONIBLE POR EL MOMENTO</a>
            </div>
        </div>
        <div class="containerDown">
            <p class="pVersiones">Primera versión PokeDEXplorer | Lanzada [3 - mar - 2023] | V.1</p>
            <div>
                <ul class="ulDescargas">
                    <li>Creación de 4 funcionalidades de información</li>
                    <li>Base de datos embebida</li>
                    <li>Distintos roles de usuarios con distinto acceso</li>
                    <li>Acceso gratuito implementado</li>
                    <li>Evitado SQLInjection</li>
                    <li>Añadidos ToolTips para los usuarios</li>
                    <li>Añadidos atajos de teclado</li>
                </ul>
                <a download href="./Descargas/Instalador PokeDEXplorer Version 1.exe" id="refNoDecoDes">&emsp;DESCARGAR</a>
            </div>
        </div>
        <div id="logoPokemonDrch">
            <img id="logoDescargas" src="./Imgs/PikachuDownloadDch.png" alt="Imagen de un Pokémon pulsando botoncitos">
        </div>
    </main>
    <footer>
        <p id="ParrFooterInicio">© 2023 - <?php echo date("Y");?>. PokeDEXplorer</p>
        <h4>Alba Marco Checa</h4>
    </footer>
</body>
</html>