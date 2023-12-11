-- =========================================
-- Create FileTable template
-- =========================================
USE AnnuaireEntreprise;

CREATE TABLE Admin (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    PSEUDO VARCHAR(255),
    PWD VARCHAR(255)
);


INSERT INTO Admin (PSEUDO, PWD)
VALUES
    ('OGROUP', 'annuaire2023');

delete from admin where id=1;