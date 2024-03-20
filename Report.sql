use StudentSucces;

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'VW_StudentGroupView')
BEGIN
    CREATE  VIEW [VW_StudentGroupView] AS
    SELECT 
        [st].[StudentID],
        [st].[FirstName],
        [st].[LastName],
        [sg].[GroupID],
        [g].[GroupName]
    FROM 
        [dbo].[Student] st
    INNER JOIN 
        [dbo].[StudentGroup] sg ON [st].[StudentID] = [sg].[StudentID]
    INNER JOIN 
        [dbo].[Groups] g ON [sg].[GroupID] = [g].[GroupID];
END;

GO

CREATE OR ALTER PROCEDURE [SP_GetOverdueBooksReport]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.[StudentID],
        s.[FirstName],
        s.[LastName],
        COUNT(sb.[BookID]) AS NumberOfBooks,
        SUM(DATEDIFF(DAY, sb.[CheckEndDate], GETDATE())) AS DaysOverdue,
        SUM(b.[Price]) AS TotalDebt
    FROM
        [dbo].[Student] s
    INNER JOIN
        [dbo].[StudentBook] sb ON s.[StudentID] = sb.[StudentID]
    INNER JOIN
        [dbo].[Book] b ON sb.[BookID] = b.[BookID]
    WHERE
        sb.[CheckEndDate] < DATEADD(YEAR, -1, GETDATE()) OR sb.[CheckEndDate] IS NULL
    GROUP BY
        s.[StudentID],
        s.[FirstName],
        s.[LastName];
END;
EXEC  [SP_GetOverdueBooksReport];

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
EXEC [dbo].[SP_SortStudentRating];

GO

GO

CREATE  OR ALTER PROC [dbo].[ST_StudentRating] AS
BEGIN
SELECT
	st.[FirstName],
	st.[LastName],
	st.[Address],
	st.[Gender],
	st.[MaritalStatus],
	sb.[Score]
FROM 
	[dbo].[Student] st
LEFT JOIN 
	[dbo].[StudentSubject] sb ON st.StudentID = sb.StudentID
 ORDER BY
	sb.[Score] DESC
	END;

EXEC  ST_StudentRating ;
