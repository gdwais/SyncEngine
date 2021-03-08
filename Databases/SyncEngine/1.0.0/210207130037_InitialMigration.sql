-- drop table [Transaction];
-- drop table TransactionStage;
-- drop table Client;

select NEWID();

IF NOT EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'Client')
BEGIN
    CREATE TABLE Client
    (
        ClientId UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Client PRIMARY KEY,
        Name VARCHAR(50) NOT NULL
    )
END;

IF NOT EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'TransactionStage')
BEGIN
    CREATE TABLE TransactionStage
    (
        TransactionStageId INT NOT NULL CONSTRAINT PK_TransactionStage PRIMARY KEY IDENTITY(1,1),
        TransactionStage VARCHAR(20) NOT NULL
    )
END;

IF EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'TransactionStage')
BEGIN
    INSERT INTO TransactionStage
        (TransactionStage)
    VALUES
        ('Created'),
        ('Processing'),
        ('Valid'),
        ('Invalid'),
        ('Complete')
;
END;

IF NOT EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'Transaction')
BEGIN
    CREATE TABLE [Transaction]
    (
        TransactionId UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Transaction PRIMARY KEY CLUSTERED,
        ClientId UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_TransactionClient FOREIGN KEY REFERENCES Client (ClientId),
        [FileName] VARCHAR(50) NULL,
        SafeFileName VARCHAR(50) NULL,
        TransactionStageId INT NOT NULL CONSTRAINT FK_TransactionTransactionStage FOREIGN KEY REFERENCeS TransactionStage (TransactionStageId),
        CreatedOn DATETIME2(2) NOT NULL CONSTRAINT DF_TransactionCreatedOn DEFAULT GETDATE()
    )
END;


IF EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'Client')
BEGIN
    INSERT INTO Client
        (ClientId, Name)
    VALUES
        ('4bf690da-8e1b-41e5-91ce-5af1bdec2f8d', 'Noble Truss'),
        ('3fdd2489-f0ce-4dca-9767-125c7cf2c7c3', 'Test Client');
END;

