-- Insertar datos en la tabla "Teams"
INSERT INTO `Teams` (`Name`, `Coach`)
VALUES
    ('Equipo 1', 'Entrenador 1'),
    ('Equipo 2', 'Entrenador 2'),
    ('Equipo 3', 'Entrenador 3');

-- Insertar datos en la tabla "TeamMembers"
INSERT INTO `TeamMembers` (`TeamId`, `FirstName`, `LastName`, `BirthDate`, `Phone`)
VALUES
    (1, 'Juan', 'Pérez', '1990-05-15 00:00:00', '123-456-7890'),
    (1, 'María', 'González', '1992-08-20 00:00:00', '987-654-3210'),
    (2, 'Pedro', 'Sánchez', '1988-03-10 00:00:00', '555-123-4567'),
    (2, 'Ana', 'López', '1994-12-05 00:00:00', '777-888-9999'),
    (3, 'Luis', 'Martínez', '1991-07-25 00:00:00', '111-222-3333'),
    (3, 'Elena', 'Ramírez', '1993-02-18 00:00:00', '444-555-6666');

-- Puedes agregar más filas de datos de la misma manera
