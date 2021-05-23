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
WHERE it.TABLE_NAME = 'BatchStage')
BEGIN
    CREATE TABLE BatchStage
    (
        BatchStageId INT NOT NULL CONSTRAINT PK_BatchStage PRIMARY KEY IDENTITY(1,1),
        BatchStage VARCHAR(20) NOT NULL
    )
END;

IF EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'BatchStage')
BEGIN
    INSERT INTO BatchStage
        (BatchStage)
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
WHERE it.TABLE_NAME = 'Batch')
BEGIN
    CREATE TABLE [Batch]
    (
        BatchId UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Batch DEFAULT NEWID() PRIMARY KEY CLUSTERED,
        ClientId UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_BatchClient FOREIGN KEY REFERENCES Client (ClientId),
        [FileName] VARCHAR(50) NULL,
        SafeFileName VARCHAR(50) NULL,
        BatchStageId INT NOT NULL CONSTRAINT FK_BatchBatchStage FOREIGN KEY REFERENCeS BatchStage (BatchStageId),
        CreatedOn DATETIME2(2) NOT NULL CONSTRAINT DF_BatchCreatedOn DEFAULT GETUTCDATE()
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

