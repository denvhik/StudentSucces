use StudentSucces;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.Dormitory' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[Dormitory]
(
    [DormitoryID] INT IDENTITY(1,1),
    [DormitoryName] VARCHAR(100) NOT NULL,
    [RoomNumber] INT NOT NULL,
    [Capacity] INT DEFAULT 3,
    [CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
    [ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
    [ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

    CONSTRAINT [PK_studentSucces_DormitoryID] PRIMARY KEY ([DormitoryID]),

    [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[DormitoryHistory]));
END


IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.Student' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[Student]
(
	 [StudentID] INT IDENTITY(1,1) --**добавити автоінкремент--
	,[FirstName] VARCHAR(50) NOT NULL
	,[LastName] VARCHAR(50) NOT NULL
    ,[MiddleName] VARCHAR(50) NULL
    ,[TicketNumber] VARCHAR(50) NOT NULL
    ,[BirthYear] INT NOT NULL
    ,[BirthPlace] VARCHAR(100) NOT NULL
    ,[Address] VARCHAR(100) NOT NULL
    ,[Gender] VARCHAR(20) NOT NULL
    ,[MaritalStatus] VARCHAR(50) NULL
    ,[Scholarship] DECIMAL(18,2) NULL
	,[CreatedDateTime] DATETIME DEFAULT GETUTCDATE()
	,[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0
	,[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE()
	,[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0

	,CONSTRAINT [PK_StudentSucces_StudentsID] PRIMARY KEY ([StudentID])

	,[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START
    ,[SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END
    ,PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[StudentsHistory]));
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.StudentsDormitory' AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[StudentsDormitory]
    (
        [StudentDormitoryID] INT IDENTITY (1,1),
        [StudentID] INT NOT NULL,
        [DormitoryID] INT NOT NULL,
        [CheckStartDate] DATETIME DEFAULT GETUTCDATE(),
        [CheckEndDate] DATETIME NULL,
		CONSTRAINT [PC_StudentSucces_StudentDormitoryID] PRIMARY KEY ([StudentDormitoryID]),
        CONSTRAINT [FK_StudentDormitory_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student]([StudentID]),
        CONSTRAINT [FK_StudentDormitory_DormitoryID] FOREIGN KEY ([DormitoryID]) REFERENCES [dbo].[Dormitory]([DormitoryID]),

		[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
		[SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
		PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime]),
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[StudentsDormitoryHistory]));
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.Groups' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[Groups] (
    [GroupID] INT IDENTITY(1,1),
    [GroupName] VARCHAR(50)NOT NULL,
   -- [MonitorID] INT NOT NULL,--
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

	CONSTRAINT [PK_studentSucces_GroupID] PRIMARY KEY (GroupID),

    [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[GroupsHistory]));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.Subject' AND type = 'U')
BEGIN
CREATE  TABLE [dbo].[Subject] (
    [SubjectID] INT IDENTITY(1,1),
    [SubjectName] VARCHAR(100) NOT NULL,
    [TeacherID] INT NOT NULL,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

	CONSTRAINT [PK_studentSucces_SubjectID] PRIMARY KEY (SubjectID),

    [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SubjectsHistory]));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.Teacher' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[Teacher] (
    [TeacherID] INT IDENTITY(1,1),
    [TeacherName] VARCHAR(100) NOT NULL,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

	CONSTRAINT [PK_studentSucces_TeachersID] PRIMARY KEY (TeacherID),

    [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[TeachersHistory]));
	END

		IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.TeacherSubject' AND type = 'U')
BEGIN
	CREATE TABLE [dbo].[TeacherSubject] (
    [TeacherID] INT NOT NULL,
    [SubjectID] INT NOT NULL,

    CONSTRAINT [FK_TeacherSubject_TeacherID] FOREIGN KEY ([TeacherID]) REFERENCES [dbo].[Teacher]([TeacherID]),
    CONSTRAINT [FK_TeacherSubject_SubjectID] FOREIGN KEY ([SubjectID]) REFERENCES [dbo].[Subject]([SubjectID]),
    CONSTRAINT [PK_TeacherSubject] PRIMARY KEY ([TeacherID], [SubjectID]),

    [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[TeachersSubjectHistory]));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.Hobbie' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[Hobbie] (
    [HobbyID] INT IDENTITY(1,1),
    [HobbyName] VARCHAR(100) NOT NULL,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

	CONSTRAINT [PK_studentSucces_HobbyID] PRIMARY KEY (HobbyID),

	[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[HobbiesHistory]));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.Book' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[Book] (
    [BookID] INT IDENTITY(1,1),
    [Author] VARCHAR(100) NOT NULL,
    [Title] VARCHAR(100),
    [Genre] VARCHAR(50)NOT NULL,
    [Price] DECIMAL(18,2) NULL,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	
	CONSTRAINT [PK_studentSucces_BookID] PRIMARY KEY (BookID),

	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.BooksHistory));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.StudentGroup' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[StudentGroup] (
    [StudentID] INT NOT NULL,
    [GroupID] INT NOT NULL,
	[Boss] bit,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

    CONSTRAINT [PK_StudentSucces_StudentGroupid] PRIMARY KEY (StudentID, GroupID),
    CONSTRAINT [FK_StudentSucces_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student],
	CONSTRAINT [FK_StudentSucces_GroupId] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Groups],

	[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[StudentGroupHistory]));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.StudentSubject' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[StudentSubject] (
    [StudentID] INT NOT NULL,
    [SubjectID] INT NOT NULL,
    [Score] INT NOT NULL,
	[StartDate] DATE ,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

    CONSTRAINT [PK_StudentSucces_StudentSubjectID] PRIMARY KEY (StudentID, SubjectID),
	CONSTRAINT [FK_StudentSucces_StudenSubjecttID] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student],
	CONSTRAINT [FK_StudentSucces_SubjectID] FOREIGN KEY ([SubjectID]) REFERENCES [dbo].[Subject],

	[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[StudentSubjectHistory]));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.StudentHobby' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[StudentHobby] (
    [StudentID] INT NOT NULL,
    [HobbyID] INT NOT NULL,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

	CONSTRAINT [PK_StudentSucces_StudentHobbyID] PRIMARY KEY (StudentID, HobbyID),
    CONSTRAINT [FK_StudentSucces_StudentHobbyID] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student],
    CONSTRAINT [FK_StudentSucces_HobbyID] FOREIGN KEY ([HobbyID]) REFERENCES [dbo].[Hobbie],

	[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[StudentHobbyHistory]));
	END

	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.StudentBook' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[StudentBook] (
    [StudentID] INT NOT NULL,
    [BookID] INT NOT NULL,
	[PriceCheck] DECIMAL (3,0) NOT NULL,
	[CheckStartDate] DATETIME DEFAULT GETUTCDATE(),
    [CheckEndDate] DATETIME NULL,
	[CreatedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[CreatedBy] UNIQUEIDENTIFIER DEFAULT 0x0,
	[ModifiedDateTime] DATETIME DEFAULT GETUTCDATE(),
	[ModifiedBy] UNIQUEIDENTIFIER DEFAULT 0x0,

	CONSTRAINT [CK_StudentSucces_PriceCheck] CHECK (PriceCheck <=100),
	CONSTRAINT [PK_StudentSucces_StudentBookID] PRIMARY KEY (StudentID, BookID),
    CONSTRAINT [FK_StudentSucces_StudentBookID] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student],
    CONSTRAINT [FK_StudentSucces_BookID] FOREIGN KEY ([BookID]) REFERENCES [dbo].[Book],

	[SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[StudentBookHistory]));
	END


	IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.GroupEnrollment' AND type = 'U')
BEGIN
	CREATE TABLE [dbo].[GroupEnrollment] (
    [GroupEnrollmentID] INT IDENTITY(1,1),
    [GroupID] INT NOT NULL,
    [StudentID] INT NOT NULL,
    [EnrollmentStartDate] DATETIME NOT NULL,
    [EnrollmentEndDate] DATETIME NULL,

	CONSTRAINT [PK_StudentSucces_GroupEnrolmentID] PRIMARY KEY ([GroupEnrollmentID]),
    CONSTRAINT [FK_GroupEnrollment_GroupID] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Groups]([GroupID]),
    CONSTRAINT [FK_GroupEnrollment_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student]([StudentID]),

    [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[GroupEnrollmentHistory]));
	END

IF NOT EXISTS (
    SELECT * 
    FROM sys.indexes 
    WHERE name = 'IX_EnrollmentStartDate_EnrollmentEndDate' 
    AND object_id = OBJECT_ID('dbo.GroupEnrollment')
)
BEGIN
    CREATE INDEX IX_EnrollmentStartDate_EnrollmentEndDate 
    ON [dbo].[GroupEnrollment] ([EnrollmentStartDate], [EnrollmentEndDate]);
    PRINT 'Індекс IX_EnrollmentStartDate_EnrollmentEndDate був створений у таблиці dbo.GroupEnrollment.'
END

IF NOT EXISTS (
    SELECT * 
    FROM sys.indexes 
    WHERE name = 'IX_StudentBook_StudentID_BookID' 
    AND object_id = OBJECT_ID('dbo.StudentBook')
)
BEGIN
    CREATE INDEX IX_StudentBook_StudentID_BookID 
    ON dbo.StudentBook (StudentID, BookID);
    PRINT 'Індекс IX_StudentBook_StudentID_BookID був створений у таблиці dbo.StudentBook.'
END


		IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dbo.TeacherSubject' AND type = 'U')
BEGIN
	CREATE TABLE [dbo].[TeacherSubject] (
    [TeacherID] INT NOT NULL,
    [SubjectID] INT NOT NULL,

    CONSTRAINT [FK_TeacherSubject_TeacherID] FOREIGN KEY ([TeacherID]) REFERENCES [dbo].[Teacher]([TeacherID]),
    CONSTRAINT [FK_TeacherSubject_SubjectID] FOREIGN KEY ([SubjectID]) REFERENCES [dbo].[Subject]([SubjectID]),
    CONSTRAINT [PK_TeacherSubject] PRIMARY KEY ([TeacherID], [SubjectID]),

    [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START,
    [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME([SysStartTime], [SysEndTime])
)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[TeacherSubjectHistory]));
	END

	ALTER TABLE [dbo].[StudentsDormitory]