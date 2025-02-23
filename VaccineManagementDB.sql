USE [master]

/***** Object: Database [ChildrenVaccineScheduleTrackingSystem] *****/
--DROP DATABASE [ChildrenVaccineScheduleTrackingSystem]
GO
CREATE DATABASE [ChildrenVaccineScheduleTrackingSystem]
GO

USE [ChildrenVaccineScheduleTrackingSystem]
GO

/****** Object:  Table [dbo].[Role] ******/
/*****
CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [varchar](20) NOT NULL
)
GO
******/

/****** Object:  Table [dbo].[Account] ******/
CREATE TABLE [dbo].[Accounts](
    [AccountID] [int] IDENTITY(1,1) PRIMARY KEY,
    [Firstname] [nvarchar](100) NOT NULL,
	[Lastname] [nvarchar](100) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
    [Password] [varchar](200) NOT NULL,
    [PhoneNumber] [varchar](20) NOT NULL,
    [Email] [varchar](100) NOT NULL,
	[Avatar] [varchar](max) NULL,
    [Status] [varchar](20) NOT NULL
)
GO

/****** Object:  Table [dbo].[Parent] ******/
CREATE TABLE [dbo].[Parents](
	[ParentID] [int] IDENTITY(1,1) PRIMARY KEY,
	[AccountID] [int] NOT NULL,
	FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Accounts]([AccountID])
)
GO

/****** Object:  Table [dbo].[Doctor] ******/
CREATE TABLE [dbo].[Doctors](
	[DoctorID] [int] IDENTITY(1,1) PRIMARY KEY,
	[AccountID] [int] NOT NULL,
	FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Accounts]([AccountID])
)
GO

/****** Object:  Table [dbo].[Staff] ******/
CREATE TABLE [dbo].[Staffs](
	[StaffID] [int] IDENTITY(1,1) PRIMARY KEY,
	[AccountID] [int] NOT NULL,
	FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Accounts]([AccountID])
)
GO

/****** Object:  Table [dbo].[Child] ******/
CREATE TABLE [dbo].[Children] (
    [ChildID] [int] IDENTITY(1,1) PRIMARY KEY,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Weight] [decimal](8,2) NOT NULL,
	[Height] [decimal](8,2) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[ParentID] [int] NOT NULL,
    FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Parents]([ParentID])
)
GO

/****** Object:  Table [dbo].[VaccineTypes] ******/
CREATE TABLE [dbo].[VaccineTypes](
	[VaccineTypeID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [ntext] NOT NULL
)
GO

/****** Object:  Table [dbo].[Vaccines] ******/
CREATE TABLE [dbo].[Vaccines](
    [VaccineID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [varchar](100) NOT NULL,
	[VaccineTypeID] [int] NOT NULL,
	[Manufacturer] [nvarchar](200) NOT NULL,
    [Stock] [int] NOT NULL,
    [Price] [decimal](20,2) NOT NULL,
    [Description] NTEXT NOT NULL,
	[FromAge] [int] NOT NULL,
	[ToAge] [int] NOT NULL,
	[Period] [int] NOT NULL,
	FOREIGN KEY ([VaccineTypeID]) REFERENCES [dbo].[VaccineTypes]([VaccineTypeID])
)
GO

/****** Insert:  Table [dbo].[Account] ******/
INSERT [dbo].[Accounts] ([Firstname], [Lastname], [Username], [Password], [PhoneNumber], [Email], [Avatar], [Status]) VALUES
('Hoang', 'Nguyen', 'hoangnguyen123', '123hoang', '0947568394', 'hoangnguyen1334@gmail.com', NULL, 'ACTIVE'),
('Lan', 'Nguyen', 'lannguyen123', '123lan', '0932568394', 'lannguyen13234@gmail.com', NULL, 'ACTIVE'),
('Quang', 'Nguyen', 'quangnguyen123', '123quang', '0932522394', 'quangnguyen13234@gmail.com', NULL, 'ACTIVE'),
('Binh', 'Le', 'binhlew123', '123binh', '0345522394', 'binhle21@gmail.com', NULL, 'ACTIVE'),
('Minh', 'Pham', 'minhpham123', '123minh', '0912345678', 'minhpham123@gmail.com', NULL, 'ACTIVE'),
('Hanh', 'Tran', 'hanhtran123', '123hanh', '0923456789', 'hanhtran123@gmail.com', NULL, 'ACTIVE'),
('Duc', 'Vo', 'ducvo123', '123duc', '0934567890', 'ducvo123@gmail.com', NULL, 'ACTIVE'),
('Tuan', 'Dang', 'tuandang123', '123tuan', '0945678901', 'tuandang123@gmail.com', NULL, 'ACTIVE'),
('Ly', 'Ho', 'lyho123', '123ly', '0956789012', 'lyho123@gmail.com', NULL, 'ACTIVE'),
('Anh', 'Nguyen', 'anguyen123', '123anh', '0912345679', 'anguyen123@gmail.com', NULL, 'ACTIVE'),
('Mai', 'Tran', 'maitran123', '123mai', '0923456790', 'maitran123@gmail.com', NULL, 'ACTIVE'),
('Tu', 'Le', 'tule123', '123tu', '0934567891', 'tule123@gmail.com', NULL, 'ACTIVE'),
('Quyen', 'Nguyen', 'quyennguyen123', '123quyen', '0945678902', 'quyennguyen123@gmail.com', NULL, 'ACTIVE'),
('Admin', 'System', 'admin123', 'adminpassword', '0987654321', 'admin123@vaccinesystem.com', NULL, 'ACTIVE');
GO
DBCC CHECKIDENT ('[Accounts]', RESEED, 0);

/****** Insert:  Table [dbo].[Parent] ******/
INSERT [dbo].[Parents] (AccountID) VALUES (1), (2), (3), (4), (5) ,(6), (7)

/****** Insert:  Table [dbo].[Doctor] ******/
INSERT [dbo].[Doctors] (AccountID) VALUES  (8), (9), (10), (11)

/****** Insert:  Table [dbo].[Staff] ******/
INSERT [dbo].[Staffs] (AccountID) VALUES  (12), (13), (14)

/****** Insert:  Table [dbo].[Child] ******/
INSERT [dbo].[Children] ([FirstName], [LastName], [Weight], [Height], [Gender], [DateOfBirth], [ParentID]) VALUES
('Thuy', 'Nguyen', 11.3, 0.8, 'FEMALE', '2022-01-01', 1),
('Thu', 'Nguyen', 22.3, 1.23, 'FEMALE', '2022-12-25', 1),
('Minh', 'Nguyen', 20.3, 1.1, 'MALE', '2022-1-1', 2),
('Hoa', 'Tran', 15.2, 0.95, 'FEMALE', '2022-03-15', 3),
('Kien', 'Pham', 18.5, 1.05, 'MALE', '2021-07-19', 3),
('Lan', 'Le', 17.0, 1.1, 'FEMALE', '2022-04-30', 4),
('Duy', 'Nguyen', 20.0, 1.15, 'MALE', '2022-08-10', 1),
('Thao', 'Nguyen', 13.7, 0.95, 'FEMALE', '2022-02-20', 2);
GO
DBCC CHECKIDENT ('[Appointments]', RESEED, 0);
/****** Insert:  Table [dbo].[VaccineTypes] ******/
INSERT [dbo].[VaccineTypes] ([Name], [Description]) VALUES 
('COVID-19', 'COVID-19 (from English : corona virus disease 2019 meaning corona virus disease 2019 ) [ 10 ] is an acute infectious respiratory disease caused by the coronavirus SARS-CoV-2 and its variants . This is a virus discovered in an outbreak investigation originating from a large seafood and animal market in Wuhan , Hubei Province , China . The virus causes acute respiratory infections in humans and has been shown to spread from person to person. In addition to this newly discovered coronavirus, there are 6 other coronaviruses known today that can infect people from person to person . The disease was first discovered during the COVID-19 pandemic of 2019–2020.'), 
('Rota virus', 'Rotaviruses are the most common cause of diarrhoeal disease among infants and young children.[1] Nearly every child in the world is infected with a rotavirus at least once by the age of five.[2] Immunity develops with each infection, so subsequent infections are less severe. Adults are rarely affected.[3] Rotavirus is a genus of double-stranded RNA viruses in the family Reoviridae. There are nine species of the genus, referred to as A, B, C, D, F, G, H, I and J. Rotavirus A is the most common species, and these rotaviruses cause more than 90% of rotavirus infections in humans.'), 
('Influenza seasonal', 'Flu season is an annually recurring time period characterized by the prevalence of an outbreak of influenza (flu). The season occurs during the cold half of the year in each hemisphere. It takes approximately two days to show symptoms. Influenza activity can sometimes be predicted and even tracked geographically. While the beginning of major flu activity in each season varies by location, in any specific location these minor epidemics usually take about three weeks to reach its pinnacle, and another three weeks to significantly diminish.')
GO

/****** Insert:  Table [dbo].[Vaccines] ******/
INSERT [dbo].[Vaccines] ([Name], [VaccineTypeID], [Manufacturer], [Stock], [Price], [Description], [FromAge], [ToAge], [Period]) VALUES
('AZD1222', 1, 'AstraZeneca', 100, 100000, 'In April 2021, the Medicines and Healthcare products Regulatory Agency (MHRA) confirmed a possible link between the AstraZeneca Covid-19 vaccine and these rare blood clots, but emphasised that the benefits of the vaccine continued to outweigh the risks for the vast majority of people.', 60, 200, 10),
('VAXIGRIP TETRA', 2, 'Sanofi Pasteur (France)', 100, 356000, 'Vaxigrip Tetra is a vaccine. This vaccine helps to protect you or your child against influenza (flu). Vaxigrip Tetra is used to prevent flu in persons of 6 months of age and older. If you are pregnant, one dose of vaccine given to you during pregnancy may protect your baby from birth to less than 6 months of age.', 6, 200, 4)
GO



CREATE TABLE [dbo].[DailyScheldue] (
	SlotID int IDENTITY(1,1) PRIMARY KEY,
	StartTime datetime NOT NULL,
	EndTime datetime NOT NULL,
	Slot int NOT NULL
)


CREATE TABLE [dbo].[WeeklyScheldue](
	WeeklyScheldueID int IDENTITY(1,1) PRIMARY KEY,
	[Weekday] NVARCHAR NOT NULL, 
)

CREATE TABLE [dbo].[DoctorScheldue](
	DocotrScheldueID int IDENTITY(1,1) PRIMARY KEY,
	DoctorID int NOT NULL,
	SlotID int,
	WeeklyScheldueID int NOT NULL,
	FOREIGN KEY ([WeeklyScheldueID]) REFERENCES [dbo].[WeeklyScheldue]([WeeklyScheldueID]),
	FOREIGN KEY ([SlotID]) REFERENCES [dbo].[DailyScheldue]([SlotID]),
	FOREIGN KEY ([DoctorID]) REFERENCES [dbo].[Doctors]([DoctorID])
)

CREATE TABLE [dbo].[AppointmentScheldue](
	[AppointmentScheldue] [int] IDENTITY(1,1) PRIMARY KEY,
	DateTime Datetime not null,
)

CREATE TABLE  [dbo].[DailySchedule](
	[DailyScheduleID] int IDENTITY(1,1) PRIMARY KEY,
	[AppointmentDate] DATE NOT NULL
)
INSERT INTO [dbo].[DailySchedule]([AppointmentDate]) VALUES 
('2025-2-17')

CREATE TABLE [dbo].[TimeSlots](
	[TimeSlotID] INT IDENTITY(1,1) PRIMARY KEY,
	[StartTime] Time NOT NULL,
    [SlotNumber] INT NOT NULL CHECK (SlotNumber BETWEEN 1 AND 20),
	[DailyScheduleID] int NOT NULL,
    [Available]  bit  NOT NULL,
	FOREIGN KEY ([DailyScheduleID]) REFERENCES [dbo].[DailySchedule]([DailyScheduleID])
)
INSERT INTO [dbo].[TimeSlots] ([StartTime], [SlotNumber], [Available], [DailyScheduleID])  
VALUES  
('07:00:00', 1, 1, 1),
('07:45:00', 2, 1, 1),
('08:30:00', 3, 1, 1),
('09:15:00', 4, 1, 1),
('10:00:00', 5, 1, 1),
('10:45:00', 6, 1, 1),
('11:30:00', 7, 1, 1),
('12:15:00', 8, 1, 1),
('13:00:00', 9, 1, 1),
('13:45:00', 10, 1, 1),
('14:30:00', 11, 1, 1),
('15:15:00', 12, 1, 1),
('16:00:00', 13, 1, 1),
('16:45:00', 14, 1, 1),
('17:30:00', 15, 1, 1),
('18:15:00', 16, 1, 1),
('19:00:00', 17, 1, 1),
('19:45:00', 18, 1, 1),
('20:30:00', 19, 1, 1),
('21:15:00', 20, 1, 1);



/****** Object:  Table [dbo].[Appointment] ******/
CREATE TABLE [dbo].[Appointments](
    [AppointmentID] [int] IDENTITY(1,1) PRIMARY KEY,
    [ChildID] [int] NOT NULL,
	[DoctorID] [int] NOT NULL,
	[VaccineTypeID] [int] NOT NULL,
	[TimeSlotID] int not null,
--    [Time] [datetime] NOT NULL,
    [Status] [varchar](20) NOT NULL,
	FOREIGN KEY ([TimeSlotID]) REFERENCES [dbo].[TimeSlots]([TimeSlotID]),
    FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Children]([ChildID]),
	FOREIGN KEY ([DoctorID]) REFERENCES [dbo].[Accounts]([AccountID]),
	FOREIGN KEY ([VaccineTypeID]) REFERENCES [dbo].[VaccineTypes]([VaccineTypeID])
)	
GO

EXEC sp_rename 'dbo.Appointments.TimeSlotsID', 'TimeSlotID', 'COLUMN';

DBCC CHECKIDENT ('TimeSlots', RESEED, 0);


-- data bảng Appointment
INSERT INTO [dbo].[Appointments] ([ChildID], [DoctorID], [VaccineTypeID], [TimeSlotID], [Status])
VALUES 
(1, 2, 3, 1, 'Confirmed'),   -- 07:00
(2, 3, 1, 5, 'Pending'),     -- 10:00
(3, 1, 2, 10, 'Confirmed'),  -- 13:30
(4, 2, 1, 3, 'Confirmed'),   -- 08:30
(5, 3, 4, 7, 'Pending'),     -- 11:15
(6, 1, 2, 12, 'Canceled'),  -- 14:00
(7, 2, 5, 6, 'Confirmed'),   -- 09:30
(8, 3, 3, 18, 'Pending');    -- 16:30



-- data bảng TimeSlots
DECLARE @StartTime TIME = '07:00:00'
DECLARE @SlotNumber INT = 1

WHILE @SlotNumber <= 20
BEGIN
    INSERT INTO [dbo].[TimeSlots] ([StartTime], [AppointmentDate], [SlotNumber], [Available])
    VALUES (@StartTime, '2025-02-15', @SlotNumber, 'Available')

    -- Cộng thêm 45 phút cho slot tiếp theo
    SET @StartTime = DATEADD(MINUTE, 45, @StartTime)
    SET @SlotNumber = @SlotNumber + 1
END

CREATE TABLE [dbo].[Images] (
	[ImageID] [int] IDENTITY(1,1) PRIMARY KEY,
	[FileExtension][varchar](6) NOT NULL,
	[FileSize][int] NOT NULL,
	[FilePath][varchar] NOT NULL
)

/**
CREATE TRIGGER trg_UpdateTimeSlotStatus
ON Appointment
AFTER INSERT
AS
BEGIN
    UPDATE TimeSlot
    SET Available = 0
    WHERE TimeSlotID IN (SELECT TimeSlotsID FROM inserted);
END;

CREATE TRIGGER trg_ResetTimeSlotStatus
ON Appointment
AFTER DELETE
AS
BEGIN
    UPDATE TimeSlot
    SET Available = 1
    WHERE TimeSlotID IN (SELECT TimeSlotsID FROM deleted);
END;
**/

/****
/****** Object:  Table [dbo].[Package] ******/
CREATE TABLE Package(
	[PackageID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NOT NULL
)
GO

/****** Object:  Table [dbo].[VaccineContainer] ******/
CREATE TABLE VaccineContainer(
	[VaccineContainerID] [int] IDENTITY(1,1) PRIMARY KEY,
	[PackageID] [int] NOT NULL,
	[VaccineID] [int] NOT NULL,
	FOREIGN KEY ([VaccineID]) REFERENCES [dbo].[Vaccine]([VaccineID]),
	FOREIGN KEY ([PackageID]) REFERENCES [dbo].[Package]([PackageID])
)
GO

/****** Object:  Table [dbo].[MedicalBook] ******/
CREATE TABLE [dbo].[MedicalBook](
	[BookID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ChildID] [int] NOT NULL,
	[PackageID] [int] NOT NULL,
	FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Child]([ChildID]),
	FOREIGN KEY ([PackageID]) REFERENCES [dbo].[Package]([PackageID])
)
GO

/****** Object:  Table [dbo].[Record] ******/
CREATE TABLE [dbo].[Record] (
    [RecordID] [int] IDENTITY(1,1) PRIMARY KEY,
	[DoctorID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
    [RecordTime] [datetime] NOT NULL,
    [SideEffect] [ntext] NULL,
    FOREIGN KEY ([BookID]) REFERENCES [dbo].[MedicalBook]([BookID]),
	FOREIGN KEY ([DoctorID]) REFERENCES [dbo].[Account]([AccountID])
)
GO

/****** Object:  Table [dbo].[Blog] ******/
CREATE TABLE [dbo].[Blog](
	[BlogID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ManagerID] [int] NOT NULL,
    [Author] [nvarchar](100) NOT NULL,
    [Topic] [nvarchar](255) NOT NULL,
	FOREIGN KEY ([ManagerID]) REFERENCES [dbo].[Account]([AccountID])
)
GO

/****** Object:  Table [dbo].[Notification] ******/
CREATE TABLE [dbo].[Notification](
    [NotificationID] [int] IDENTITY(1,1) PRIMARY KEY,
    [ParentsID] [int] NOT NULL,
    [Message] [nvarchar](255) NOT NULL,
    [Time] [datetime] NOT NULL,
    FOREIGN KEY ([ParentsID]) REFERENCES [dbo].[Account]([AccountID])
)
GO

/****** Object:  Table [dbo].[Feedback] ******/
CREATE TABLE Feedback(
    [FeedbackID] [int] IDENTITY(1,1) PRIMARY KEY,
	[AppointmentID] [int] NOT NULL,
    [ParentsID] [int] NOT NULL,
    [FeedbackMessage] [nvarchar](255) NOT NULL,
    FOREIGN KEY ([AppointmentID]) REFERENCES [dbo].[Appointment]([AppointmentID]),
    FOREIGN KEY ([ParentsID]) REFERENCES [dbo].[Account]([AccountID])
)
GO

/****** Object:  Table [dbo].[PackagePayment] ******/
CREATE TABLE [dbo].[PackagePayment](
    [PaymentID ] [int] IDENTITY(1,1) PRIMARY KEY,
	[ParentsID] [int] NOT NULL,
	[PackageID] [int] NOT NULL,
    [Total] [decimal](20,2) NOT NULL,
    [Time] [datetime] NOT NULL,
    [Status] [varchar](20) NOT NULL,
	FOREIGN KEY ([ParentsID]) REFERENCES [dbo].[Account]([AccountID]),
	FOREIGN KEY ([PackageID]) REFERENCES [dbo].[Package]([PackageID])
)
GO

/****** Object:  Table [dbo].[AppointmentPayment] ******/
CREATE TABLE [dbo].[AppointmentPayment](
    [PaymentID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ParentsID] [int] NOT NULL,
	[AppointmentPaymentID] [int] NOT NULL,
    [Total] [decimal](20,2) NOT NULL,
    [Time] [datetime] NOT NULL,
    [Status] [varchar](20) NOT NULL,
	FOREIGN KEY ([ParentsID]) REFERENCES [dbo].[Account]([AccountID]),
	FOREIGN KEY ([AppointmentPaymentID]) REFERENCES [dbo].[Appointment]([AppointmentID])
)
GO
******/