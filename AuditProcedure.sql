IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentScholarshipAudit' AND type = 'U')
BEGIN
    CREATE TABLE [StudentScholarshipAudit](
        [StudentID] INT,
		[StudentAuditID] INT IDENTITY(1,1),
        [Month] INT,
        [Year] INT,
        [Amount] DECIMAL(18, 2),
        [AuditDate] DATETIME DEFAULT GETUTCDATE(),
        CONSTRAINT PK_StudentScholarshipAudit PRIMARY KEY  ([StudentAuditID]),
        CONSTRAINT FK_StudentID FOREIGN KEY (StudentID) REFERENCES Student([StudentID]),
        [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
        [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
        PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime)
    )
    WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ScholarshipAuditHistory));
END;
GO

CREATE OR ALTER PROCEDURE [CalculateScholarshipForStudent]
    @StudentID INT,
    @Month INT,
    @Year INT
AS
IF NOT EXISTS (
    SELECT 1
    FROM [dbo].[StudentScholarshipAudit]
    WHERE [dbo].[StudentScholarshipAudit].[StudentID] = @StudentID
      AND Month = @Month
      AND Year = @Year
)
BEGIN
    SET NOCOUNT ON;

    DECLARE @Rating DECIMAL(10, 2)
    DECLARE @ScholarshipAmount DECIMAL(10, 2)

    SELECT @Rating = [StudentSubject].[Score]
    FROM [StudentSucces].[dbo].[StudentSubject]
    WHERE [StudentID] = @StudentID

    IF @Rating > 96
        SET @ScholarshipAmount = 5000.00
    ELSE IF @Rating >= 83 AND @Rating <= 96
        SET @ScholarshipAmount = 2000.00
    ELSE
        SET @ScholarshipAmount = 0.00 
    
    PRINT @ScholarshipAmount;

    INSERT INTO StudentScholarshipAudit ([StudentID], [Month], [Year],[Amount])
    VALUES (@StudentID, @Month, @Year, @ScholarshipAmount)
END
ELSE
BEGIN
    PRINT 'Scholarship already calculated for this student for the current month and year.'
END
GO
CREATE  OR ALTER PROCEDURE [CalculateScholarshipForAllStudents]
    @Month INT,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StudentID INT;
    DECLARE [StudentCursor ]CURSOR FOR 
        SELECT [dbo].[Student].[StudentID] FROM [dbo].[Student]
        ORDER BY [Student].[StudentID];

    OPEN [StudentCursor];

    FETCH NEXT FROM [StudentCursor] INTO @StudentID;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC [CalculateScholarshipForStudent] @StudentID, @Month, @Year;
        FETCH NEXT FROM [StudentCursor] INTO @StudentID;
    END;

    CLOSE StudentCursor;
    DEALLOCATE StudentCursor;
END;

 EXEC [CalculateScholarshipForAllStudents] @Month=3,@Year = 2024;
 EXEC [CalculateScholarshipForStudent] @StudentID = 3, @Month = 3, @Year = 2024;
 DROP PROC [CalculateScholarshipForAllStudents];