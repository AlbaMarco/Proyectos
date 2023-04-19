-- https://inazuma.fandom.com/es/wiki/Categor%C3%ADa:Equipos_(IE_Original_T1)

CREATE TABLE personajes (
  id INT PRIMARY KEY AUTO_INCREMENT,
  nombre_castellano VARCHAR(100),
  nombre_japones VARCHAR(100),
  pais VARCHAR(100),
  descripcion TEXT,
  imagen BLOB,
  instituto VARCHAR(100),
  supertecnicas TEXT,
  posicion VARCHAR(50),
  elemento VARCHAR(50)
);

-- Insertar información de personajes de la primera temporada (datos ficticios) con imagen codificada en base64
INSERT INTO personajes (nombre_castellano, nombre_japones, pais, descripcion, imagen, instituto, supertecnicas, posicion, elemento) VALUES
('Juan Pérez', 'Taro Yamada', 'Japón', 'Jugador talentoso y apasionado por el fútbol. Es el delantero estrella del Instituto Raimon.', 
LOAD_FILE('C:/imagenes/mark_evans.jpg'), 'Instituto Raimon', 'Tiro Poderoso, Regate Veloz', 'Delantero', 'Fuego');