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
	[DoctorTimeSlots]  VARCHAR(50) NOT NULL,
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
	[DosesRequired] int NOT NULL,
	[Priority] int NOT NULL,
	FOREIGN KEY ([VaccineTypeID]) REFERENCES [dbo].[VaccineTypes]([VaccineTypeID])
)
GO

CREATE TABLE  [dbo].[DailySchedule](
	[DailyScheduleID] int IDENTITY(1,1) PRIMARY KEY,
	[AppointmentDate] DATE NOT NULL
)


CREATE TABLE [dbo].[TimeSlots](
	[TimeSlotID] INT IDENTITY(1,1) PRIMARY KEY,
	[StartTime] Time NOT NULL,
    [SlotNumber] INT NOT NULL CHECK (SlotNumber BETWEEN 1 AND 20),
	[DailyScheduleID] int NOT NULL,
    [Available]  bit  NOT NULL,
	FOREIGN KEY ([DailyScheduleID]) REFERENCES [dbo].[DailySchedule]([DailyScheduleID])
)

/****** Object:  Table [dbo].[Appointment] ******/
CREATE TABLE [dbo].[Appointments](
    [AppointmentID] [int] IDENTITY(1,1) PRIMARY KEY,
    [ChildID] [int] NOT NULL,
	[DoctorID] [int] NOT NULL,
	[VaccineID] [int] NOT NULL,
	[TimeSlotID] int not null,
    [Status] [varchar](20) NOT NULL,
	FOREIGN KEY ([TimeSlotID]) REFERENCES [dbo].[TimeSlots]([TimeSlotID]),
    FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Children]([ChildID]),
	FOREIGN KEY ([DoctorID]) REFERENCES [dbo].[Accounts]([AccountID]),
	FOREIGN KEY ([VaccineID]) REFERENCES [dbo].[Vaccines]([VaccineID])
)	
GO


CREATE TABLE [dbo].[DoctorTimeSlots](
	DoctorTimeSlotID int IDENTITY(1,1) PRIMARY KEY,
	DoctorID int NOT NULL,
	SlotNumber int NOT NULL,
	Available bit NOT NULL,
	DailyScheduleID int NOT NULL,
	FOREIGN KEY ([DailyScheduleID]) REFERENCES [dbo].[DailySchedule]([DailyScheduleID]),
	FOREIGN KEY ([DoctorID]) REFERENCES [dbo].[Doctors]([DoctorID])
)

CREATE TABLE [dbo].[ChildTimeSlots] (
    ChildTimeSlotID int IDENTITY(1,1) PRIMARY KEY,
    ChildID INT NOT NULL,
    SlotNumber INT NOT NULL,
    DailyScheduleID INT NOT NULL,
    Available BIT NOT NULL,
	FOREIGN KEY ([DailyScheduleID]) REFERENCES [dbo].[DailySchedule]([DailyScheduleID]),
    FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Children]([ChildID])
);


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
INSERT [dbo].[Doctors] (AccountID, DoctorTimeSlots) VALUES  (8, '2,3,4,5,6,12,13,14,15,16'), (9, '1,2,3,4,5,11,12,13,14,15'), (10, '6,7,8,9,10,16,17,18,19,20'), (11, '5,6,7,8,9,10,11,12,13,14,15')

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
('Thao', 'Nguyen', 13.7, 0.95, 'FEMALE', '2022-02-20', 2),
('Hao', 'Nguyen', 20.0, 1.15, 'MALE', '2022-08-10', 5);
GO


/****** Insert:  Table [dbo].[VaccineTypes] ******/
INSERT INTO VaccineTypes ([Name], [Description]) 
VALUES 
('COVID-19', 'Vaccine phòng chống COVID-19'),
('Cúm mùa', 'Vaccine phòng chống cúm mùa'),
('Viêm gan B', 'Vaccine phòng chống viêm gan B'),
('bại liệt', 'Vaccine tiêm phòng bại liệt'),
('Bạch hầu - Ho gà - Uốn ván', 'Vaccine kết hợp phòng bệnh bạch hầu, ho gà và uốn ván');

/****** Insert:  Table [dbo].[Vaccines] ******/
INSERT INTO Vaccines ([Name], [VaccineTypeID], [Manufacturer], [Stock], [Price], [Description], [FromAge], [ToAge], [Period], [DosesRequired], [Priority]) 
VALUES 
('Pfizer-BioNTech', 1, 'Pfizer', 100, 250000, 'Vaccine mRNA phòng COVID-19', 12, 99, 6, 1, 1),
('Moderna', 1, 'Moderna', 150, 230000, 'Vaccine mRNA phòng COVID-19', 6, 99, 6, 2, 1),
('Vaxigrip Tetra', 2, 'Sanofi', 200, 180000, 'Vaccine phòng cúm mùa', 6, 99, 12, 1, 2),
('Engerix-B', 3, 'GSK', 80, 300000, 'Vaccine phòng viêm gan B', 0, 99, 6, 1, 2),
('ComBE Five', 5, 'Biological E. Limited', 350, 200000, 'Vaccine phòng bạch hầu, ho gà, uốn ván', 2, 12, 2, 3, 2),
('Pentaxim', 5, 'Sanofi Pasteur', 250, 500000, 'Vaccine 5 trong 1 phòng bạch hầu, ho gà, uốn ván, bại liệt và Hib', 2, 12, 2, 4, 1),
('MMR II', 3, 'Merck', 450, 400000, 'Vaccine phòng sởi, quai bị, rubella', 3, 99, 12, 2, 3),
('Varilrix', 3, 'GlaxoSmithKline', 200, 600000, 'Vaccine phòng thủy đậu', 12, 99, 12, 1, 3),
('OPV (Oral Polio Vaccine)', 4, 'WHO', 500, 0, 'Vaccine uống phòng bại liệt', 0, 5, 2, 3, 1),
('IPV (Inactivated Polio Vaccine)', 4, 'Sanofi Pasteur', 300, 350000, 'Vaccine tiêm phòng bại liệt', 2, 99, 2, 2, 1);


DBCC CHECKIDENT ('Appointments', RESEED, 0);
DBCC CHECKIDENT ('TimeSlots', RESEED, 0);

DBCC CHECKIDENT ('DailySchedule', RESEED, 0);
DBCC CHECKIDENT ('DoctorTimeSlots', RESEED, 0);
DBCC CHECKIDENT ('ChildTimeSlots', RESEED, 0);





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