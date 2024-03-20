SELECT * FROM [dbo].[Student]
	WHERE ([BirthYear] <2004 AND [Gender] = 'Male')
OR  [Gender] = 'Female'
	ORDER BY  [BirthPlace] ASC;


SELECT 
    [StudentID],
    [FirstName],
    [LastName],
    [BirthYear],
    [BirthYear],
    [BirthPlace],
    [Gender],
    CASE 
        WHEN [Scholarship] > 1000 THEN 'Recipient'
        ELSE 'Not recipient'
    END AS ScholarshipStatus
FROM 
    [StudentSucces].[dbo].[Student]

SELECT 
	[dbo].[Student].[StudentID],
	[dbo].[Student].[FirstName],
	[dbo].[Student].[LastName],
	[dbo].[Student].[BirthPlace],
	[dbo].[Student].[BirthYear],
	[dbo].[Student].[Gender],
	[dbo].[Dormitory].[RoomNumber],
	[dbo].[Dormitory].[DormitoryName],
	[dbo].[Groups].[GroupName]
FROM
  [StudentsDormitory]
JOIN 
	[Student] ON [StudentsDormitory].[StudentID] = [Student].[StudentID]
JOIN 
	[Dormitory] ON [StudentsDormitory].[DormitoryID] = [Dormitory].[DormitoryID]
JOIN 
	[StudentGroup] ON [StudentGroup].[StudentID] = [Student].[StudentID]
JOIN
	[Groups] ON  [Groups].[GroupID] = [StudentGroup].[GroupID]
WHERE 
	([dbo].[Student].[BirthYear] < 2004 AND [dbo].[Student].[Gender] = 'Female')
OR
	[dbo].[Student].[BirthPlace] = 'Kyiv'


SELECT 
	[dbo].[Student].[StudentID],
	[dbo].[Student].[FirstName],
	[dbo].[Student].[LastName],
	[dbo].[Student].[BirthPlace],
	[dbo].[Student].[BirthYear],
	[dbo].[Student].[Gender],
	[dbo].[Dormitory].[RoomNumber],
	[dbo].[Dormitory].[DormitoryName],
	[dbo].[Groups].[GroupName]
FROM
  [StudentsDormitory]
JOIN 
	[Student] ON [StudentsDormitory].[StudentID] = [Student].[StudentID]
JOIN 
	[Dormitory] ON [StudentsDormitory].[DormitoryID] = [Dormitory].[DormitoryID]
JOIN 
	[StudentGroup] ON [StudentGroup].[StudentID] = [Student].[StudentID]
JOIN
	[Groups] ON  [Groups].[GroupID] = [StudentGroup].[GroupID]

SELECT * FROM 
	[dbo].[Student]
WHERE 
	[dbo].[Student].[FirstName] LIKE 'N%' AND [dbo].[Student].[StudentID] BETWEEN 6 AND 15;

SELECT  
	[dbo].[Student].[FirstName],
	[dbo].[Student].[LastName],
	[dbo].[Dormitory].[DormitoryName],
	[dbo].[StudentsDormitory].[DormitoryID]
FROM
	[StudentsDormitory]
JOIN
	[Student] ON [StudentsDormitory].[StudentID] = [Student].[StudentID]
JOIN 
	[Dormitory] ON [StudentsDormitory].[DormitoryID] = [Dormitory].[DormitoryID]

SELECT *
FROM [dbo].[Student]
WHERE [Student].[Scholarship] IN (2000);

SELECT 
	[StudentID],
	[FirstName],
	[LastName],
	[BirthPlace],
	[BirthYear],
	[Gender]
FROM 
	[dbo].[Student] s
WHERE EXISTS (
    SELECT 1
    FROM [dbo].[StudentHobby] sh
    WHERE sh.[StudentID] = s.[StudentID]
    AND sh.[HobbyID] = 1
);

SELECT *
FROM [dbo].[Student] s
WHERE  EXISTS (
    SELECT 1 
    FROM [dbo].[StudentSubject] ss
    WHERE ss.[StudentID] = s.[StudentID]
) AND 90<= ALL (
    SELECT [Score] 
    FROM [dbo].[StudentSubject] ss
    WHERE ss.[StudentID] = s.[StudentID] 
);

SELECT *
FROM [dbo].[Student] s
WHERE s.[StudentID] = ANY (
    SELECT sh.[StudentID]
    FROM [dbo].[StudentHobby] sh
);




SELECT
    [sg].[GroupID],
    AVG(ss.Score) AS AverageScore
FROM
    [dbo].[StudentGroup] sg
INNER JOIN [dbo].[StudentSubject] ss ON sg.StudentID = ss.StudentID
GROUP BY
    sg.GroupID;

SELECT s.*
FROM (
    SELECT s.*
    FROM
		[dbo].[Student] s
    INNER JOIN 
		[dbo].[StudentsDormitory] sd ON s.[StudentID] = sd.[StudentID]
    WHERE 
		sd.[DormitoryID] BETWEEN 2 AND 4
) AS s;

SELECT *
FROM [dbo].[Student]
WHERE [StudentID] IN (
    SELECT StudentID
    FROM [dbo].[Student]
    WHERE [MaritalStatus] = 'Married'
) AND [Gender] = 'Female';


SELECT *
FROM (
    SELECT [BirthPlace], COUNT(*) AS NumberOfStudents
    FROM [dbo].[Student]
    GROUP BY [BirthPlace]
) AS StudentData
PIVOT (
    SUM(NumberOfStudents) FOR [BirthPlace] IN ([Kyiv], [Lviv], [Kharkiv], [Odessa], [Dnipro], [Zaporizhzhia], [Ivano-Frankivsk], [Ternopil], [Chernivtsi])
) AS PivotTable;




UPDATE 
	[dbo].[Student]
SET 
	[MaritalStatus] = 'Married'
WHERE 
	[StudentID] IN (
SELECT TOP 10 [StudentID] FROM [dbo].[Student] ORDER BY ABS(CHECKSUM(NEWID()))
);

UPDATE ss
SET ss.[Score] = ABS(CHECKSUM(NEWID())) % 40 + 60
FROM [StudentSubject] ss
INNER JOIN (
    SELECT 
       [StudentID]
    FROM [Student] s 
) s ON ss.[StudentID] = s.[StudentID]
WHERE s.[StudentID] >= 10;


INSERT INTO [dbo].[Hobbie] (HobbyName)
VALUES ('Hokey'),
       ('Basketball'),
	   ('Baseball'),
	   ('HobbyHorsing'),
	   ('Tennis'),
       ('Skiing');


DECLARE @StudentID INT
DECLARE @MaxStudentID INT
DECLARE @RandomHobbyID INT
SELECT
	@MaxStudentID = MAX([StudentID]) FROM [dbo].[Student]
SET 
	@StudentID = 10
WHILE
	@StudentID <= @MaxStudentID
BEGIN
SET
	@RandomHobbyID = (SELECT TOP 1 [HobbyID] FROM [dbo].[Hobbie] ORDER BY NEWID())
INSERT INTO	
	[dbo].[StudentHobby] ([StudentID], [HobbyID])
VALUES
	(@StudentID, @RandomHobbyID)

SET
	@StudentID = @StudentID + 1
END


DELETE FROM [dbo].[StudentHobby]
WHERE [StudentID] >= 10;
