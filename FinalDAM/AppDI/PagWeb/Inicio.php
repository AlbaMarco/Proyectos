<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Inicio PokeDEXplorer</title>
    <link rel="stylesheet" href="./Estilos/estilo.css">
    <link rel="shortcut icon" href="./Estilos/Pokeball.png">
</head>
<body>
    <header>
        <h1 id="headerTitulo">¡Bienvenido a la página de PokeDEXploter!</h1>
        <h3 id="headerSubTitulo">La aplicación que te permite interactuar e investigar el mundo pokemon.</h3>
    </header>
    <nav>
        <ul>
            <li><a href="./Inicio.php">Inicio</a></li>
            <li><a href="./Descargas.php">Descargas</a></li>
            <li><a href="./Contacto.php">Contacto</a></li>
            <li><a href="./GuiaUser.php">Guia de Usuario</a></li>
            <li><a href="./Registro.php">Registro</a></li>
        </ul>
    </nav>
    <main>
        <div class="containerInicio">
            <h2 id="mainTitulo">Bienvenido al mundo de los Pokémon</h2>
            <p id="mainParr"><span class="spanContorno">&emsp;¿De donde viene la palabra "Pokémon"?</span><br><br>
                &emsp;&emsp;La palabra Pokémon deriva del acrónimo del concepto japonés Poketto Monsuta, que significa “monstruo de bolsillo”. <br> <br>
                <span class="spanContorno">&emsp;¿Están todas las generaciones?</span><br><br>
                &emsp;Actualmente hay nueve generaciones pero sólo se dispone de acceso hasta la Octava generación.<br><br>
                <span class="spanContorno">&emsp;¿En qué consiste esta aplicación?</span><br><br>
                &emsp;Esta aplicación dispone de distintas funcionalidades dependiendo del nivel de registro que se tiene. <br>
                No hace falta iniciar sesión para poder usar la aplicación ya que se dispone de un apartado que es gratuito, como una muestra ínfima.
                Hay distintos niveles, dependiendo del nivel que una persona sea, se podrá acceder a más contenidos, siendo el máximo al que más acceda. <br>
                Entre esas funcionales mencionadas están: Pokédex con datos de los Pokémon dividida por juego, información de movimeitnos, tipos, bayas,
                pokeballs, etc. <br>Se podrá crear un máximo de 5 equipos para poder tener enfrentamientos contra una máquina y se dispondrá de un ranking
                entre todos los jugadores. <br> <br>
                <span class="spanContorno">&emsp;¿Cómo acceder a la apliacción?</span> <br><br>
                &emsp;Para poder registrarse y acceder a la aplicación, se deberá de crear una cuenta desde el apartado de "Registro" para, posteriormente,
                descargarse la úlitma versión que esté disponible del programa PokeDEXplorer.
            </p>
            <div id="logoPokemon">
                <img id="logoPkm" src="./Imgs/International_Pokémon_logo.svg.png" alt="Imagen del logo de Pokémon">
            </div>
        </div>
    </main>
    <footer>
        <p id="ParrFooterInicio">© 2023 - <?php echo date("Y");?>. PokeDEXplorer</p>
        <h4>Alba Marco Checa</h4>
    </footer>
</body>
</html>
