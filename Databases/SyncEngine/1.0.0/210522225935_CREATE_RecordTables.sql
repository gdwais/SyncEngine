IF NOT EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'ProcessingStatus')
BEGIN
    CREATE TABLE ProcessingStatus
    (
        ProcessingStatusId INT NOT NULL CONSTRAINT PK_ProcessingStatus PRIMARY KEY IDENTITY(1,1),
        ProcessingStatus VARCHAR(20) NOT NULL
    )
END;

IF EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'ProcessingStatus')
BEGIN
    INSERT INTO ProcessingStatus
        (ProcessingStatus)
    VALUES
        ('Pending'),
        ('Transforming'),
        ('Transformed'),
        ('Validating'),
        ('InValid'),
        ('Validated'),
        ('Uploading'),
        ('Uploaded'),
        ('Processed')
;
END;

IF NOT EXISTS (SELECT 1
FROM INFORMATION_SCHEMA.TABLES AS it
WHERE it.TABLE_NAME = 'Record')
BEGIN
    CREATE TABLE [Record]
    (
        RecordId UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Record DEFAULT NEWID() PRIMARY KEY CLUSTERED,
        ClientId UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_RecordClient FOREIGN KEY REFERENCES Client (ClientId),
        RawData VARCHAR(MAX) NOT NULL,
        ProcessingStatusId INT NOT NULL CONSTRAINT FK_RecordProcessingStatus FOREIGN KEY REFERENCES ProcessingStatus (ProcessingStatusId),
        CreatedOn DATETIME2(2) NOT NULL CONSTRAINT DF_RecordCreatedOn DEFAULT GETUTCDATE()
    )
END;