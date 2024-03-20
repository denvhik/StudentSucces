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

CREATE PROCEDURE [CalculateScholarshipForStudent]
    @StudentID INT,
    @Month INT,
    @Year INT
AS
IF NOT EXISTS (
    SELECT 1
    FROM StudentScholarshipAudit
    WHERE StudentID = @StudentID
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
CREATE PROCEDURE [CalculateScholarshipForAllStudents]
    @Month INT,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StudentID INT;
    DECLARE @StudentCount INT;
    SELECT @StudentCount = COUNT(*) FROM Student;
    DECLARE @Counter INT;

    SET @Counter = 1;
    WHILE @Counter <= @StudentCount
    BEGIN
        SELECT @StudentID = [StudentID]
        FROM (
            SELECT ROW_NUMBER() OVER (ORDER BY StudentID) AS RowNum, [StudentID]
            FROM Student
        ) AS NumberedStudents
        WHERE RowNum = @Counter;
        EXEC CalculateScholarshipForStudent @StudentID, @Month, @Year;
        SET @Counter = @Counter + 1;
    END;
END;

 EXEC CalculateScholarshipForAllStudents @Month=2,@Year = 2024;
 EXEC CalculateScholarshipForStudent @StudentID = 3, @Month = 3, @Year = 2024;
 DROP PROC [CalculateScholarshipForStudent];