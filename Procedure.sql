
CREATE PROCEDURE [dbo].[SP_InsertStudentsDormitory]
AS
BEGIN
    INSERT INTO [dbo].[StudentsDormitory] ([StudentID], [DormitoryID], [CheckStartDate])
    SELECT [sd].[StudentID], [sd].[DormitoryID], GETUTCDATE()
    FROM (
        SELECT 
            [s].[StudentID],
            [d].[DormitoryID],
            ROW_NUMBER() OVER(PARTITION BY [s].[StudentID] ORDER BY NEWID()) AS RowNum
        FROM [dbo].[Student] s
        LEFT JOIN [dbo].[Dormitory] d ON [s].[StudentID] % 3 = [d].[RoomNumber] % 3
        WHERE NOT EXISTS (
            SELECT 1
            FROM [dbo].[StudentsDormitory] sd
            WHERE [sd].[StudentID] = [s].[StudentID] AND [sd].[CheckEndDate] IS NULL
        )
    ) sd

    WHERE [sd].[RowNum] = 1;
END;
GO

CREATE OR ALTER PROCEDURE  [dbo].[SP_SortStudentRating]
AS
BEGIN
    SELECT
        [st].[StudentID],
        [st].[FirstName],
        [st].[LastName],
        [ss].[Score],
        [su].[SubjectName]
    FROM
        [dbo].[Student] st
    INNER JOIN
        [dbo].[StudentSubject] ss ON [st].[StudentID] = [ss].[StudentID]
    INNER JOIN
        [dbo].[Subject] su ON [su].[SubjectID] = [ss].[SubjectID]
    ORDER BY
        [ss].[Score] DESC;  
END;

GO
CREATE PROCEDURE SP_GetTopScores
    @TopCount INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH RankedScores AS (
        SELECT
            ss.[Score],
			ss.[StudentID],
            DENSE_RANK() OVER (ORDER BY ss.Score DESC) AS [Rank] --функція для ранжування однакових цінок,тобто якщо оцінки однакові ії присвоюється   однаковий "ранг" 
        FROM
            [dbo].[StudentSubject] ss
    )
    SELECT
        rs.[Score],
		s.[FirstName],
		s.[LastName]
    FROM
        [RankedScores] rs
	INNER JOIN 
		[Student] s ON rs.[StudentID] = s.[StudentID]
    WHERE
        rs.[Rank] <= @TopCount
END;
exec SP_GetTopScores @TopCount = 5;
drop  proc [SP_GetTopScores];