USE [master]

/***** Object: Database [ChildrenVaccineScheduleTrackingSystem] *****/
--DROP DATABASE [ChildrenVaccineScheduleTrackingSystem]
GO
CREATE DATABASE [ChildrenVaccineScheduleTrackingSystem]
GO

USE [ChildrenVaccineScheduleTrackingSystem]
GO

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
	[DoctorTimeSlots]  VARCHAR(max) NULL,
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
	[Available] [bit] NOT NULL,
	[ParentID] [int] NOT NULL,
    FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Parents]([ParentID])
)
GO

/**
DBCC CHECKIDENT ('[Accounts]', RESEED, 0);
DBCC CHECKIDENT ('[Parents]', RESEED, 0);
DBCC CHECKIDENT ('[Doctors]', RESEED, 0);
DBCC CHECKIDENT ('[Staffs]', RESEED, 0);
DBCC CHECKIDENT ('[Children]', RESEED, 0);
**/

/****** Insert:  Table [dbo].[Account] ******/
INSERT [dbo].[Accounts] ([Firstname], [Lastname], [Username], [Password], [PhoneNumber], [Email], [Avatar], [Status]) VALUES
('Hoang', 'Nguyen', 'hoangnguyen123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0947568394', 'hoangnguyen1334@gmail.com', NULL, 'ACTIVE'),
('Lan', 'Nguyen', 'lannguyen123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0932568394', 'lannguyen13234@gmail.com', NULL, 'ACTIVE'),
('Quang', 'Nguyen', 'quangnguyen123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0932522394', 'quangnguyen13234@gmail.com', NULL, 'ACTIVE'),
('Binh', 'Le', 'binhlew123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0345522394', 'binhle21@gmail.com', NULL, 'ACTIVE'),
('Minh', 'Pham', 'minhpham123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0912345678', 'minhpham123@gmail.com', NULL, 'ACTIVE'),
('Hanh', 'Tran', 'hanhtran123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0923456789', 'hanhtran123@gmail.com', NULL, 'ACTIVE'),
('Duc', 'Vo', 'ducvo123', '123duc', '0934567890', 'ducvo123@gmail.com', NULL, 'ACTIVE'),
('Tuan', 'Dang', 'tuandang123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0945678901', 'tuandang123@gmail.com', NULL, 'ACTIVE'),
('Ly', 'Ho', 'lyho123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0956789012', 'lyho123@gmail.com', NULL, 'ACTIVE'),
('Anh', 'Nguyen', 'anguyen123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0912345679', 'anguyen123@gmail.com', NULL, 'ACTIVE'),
('Mai', 'Tran', 'maitran123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0923456790', 'maitran123@gmail.com', NULL, 'ACTIVE'),
('Tu', 'Le', 'tule123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0934567891', 'tule123@gmail.com', NULL, 'ACTIVE'),
('Quyen', 'Nguyen', 'quyennguyen123', 'AQAAAAIAAYagAAAAEBDq2mCUviZ2lpTxoZhSjzshArzkYyj8aOQp8pT/X1c6z3OdAyapkMXbWbu9draK6g==', '0945678902', 'quyennguyen123@gmail.com', NULL, 'ACTIVE');
GO
/** Password: Test@123 **/

/****** Insert:  Table [dbo].[Parent] ******/
INSERT [dbo].[Parents] (AccountID) VALUES (1), (2), (3), (4), (5) ,(6), (7)

/****** Insert:  Table [dbo].[Doctor] ******/
/*****
INSERT [dbo].[Doctors] (AccountID, DoctorTimeSlots) VALUES  (8, '2,3,4,5,6,12,13,14,15,16'), (9, '1,2,3,4,5,11,12,13,14,15'), (10, '6,7,8,9,10,16,17,18,19,20'), (11, '5,6,7,8,9,10,11,12,13,14,15')
****/

INSERT [dbo].[Doctors] (AccountID, DoctorTimeSlots) VALUES  
(8, 'Monday:1,2,3,4,5,10,11,12,13,14,15|Tuesday:6,7,8,9,10,16,17,18,19,20|Wednesday:1,2,3,4,5,10,11,12,13,14,15|Thursday:6,7,8,9,10,16,17,18,19,20|Friday:1,2,3,4,5,10,11,12,13,14,15|Saturday:6,7,8,9,10,16,17,18,19,20'),

(9, 'Monday:6,7,8,9,10,16,17,18,19,20|Tuesday:1,2,3,4,5,10,11,12,13,14,15|Wednesday:6,7,8,9,10,16,17,18,19,20|Thursday:1,2,3,4,5,10,11,12,13,14,15|Friday:6,7,8,9,10,16,17,18,19,20|Saturday:1,2,3,4,5,10,11,12,13,14,15'),

(10, 'Monday:1,2,3,4,5,6,7,8,9,10|Tuesday:11,12,13,14,15,16,17,18,19,20|Wednesday:1,2,3,4,5,6,7,8,9,10|Thursday:11,12,13,14,15,16,17,18,19,20|Friday:1,2,3,4,5,6,7,8,9,10|Saturday:11,12,13,14,15,16,17,18,19,20'),

(11, 'Monday:11,12,13,14,15,16,17,18,19,20|Tuesday:1,2,3,4,5,6,7,8,9,10|Wednesday:11,12,13,14,15,16,17,18,19,20|Thursday:1,2,3,4,5,6,7,8,9,10|Friday:11,12,13,14,15,16,17,18,19,20|Saturday:1,2,3,4,5,6,7,8,9,10');
/*
DBCC CHECKIDENT ('DailySchedule', RESEED, 0);
DBCC CHECKIDENT ('TimeSlots', RESEED, 0);

DBCC CHECKIDENT ('Appointments', RESEED, 0);
DBCC CHECKIDENT ('DoctorTimeSlots', RESEED, 0);
DBCC CHECKIDENT ('ChildTimeSlots', RESEED, 0);
*/
/****** Insert:  Table [dbo].[Staff] ******/
INSERT [dbo].[Staffs] (AccountID) VALUES  (12), (13)

/****** Insert:  Table [dbo].[Child] ******/
INSERT [dbo].[Children] ([FirstName], [LastName], [Weight], [Height], [Gender], [DateOfBirth], [ParentID], [Available]) VALUES
('Thuy', 'Nguyen', 11.3, 0.8, 'FEMALE', '2022-01-01', 1, 1),
('Thu', 'Nguyen', 22.3, 1.23, 'FEMALE', '2022-12-25', 1, 1),
('Minh', 'Nguyen', 20.3, 1.1, 'MALE', '2022-1-1', 2, 1),
('Hoa', 'Tran', 15.2, 0.95, 'FEMALE', '2022-03-15', 3, 1),
('Kien', 'Pham', 18.5, 1.05, 'MALE', '2021-07-19', 3, 1),
('Lan', 'Le', 17.0, 1.1, 'FEMALE', '2022-04-30', 4, 1),
('Duy', 'Nguyen', 20.0, 1.15, 'MALE', '2022-08-10', 1, 1),
('Thao', 'Nguyen', 13.7, 0.95, 'FEMALE', '2022-02-20', 2, 1),
('Hao', 'Nguyen',	20.0, 1.15, 'MALE', '2022-08-10', 5, 1);
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

/**
DBCC CHECKIDENT ('[VaccineTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Vaccines]', RESEED, 0);
**/

/****** Insert:  Table [dbo].[VaccineTypes] ******/
INSERT INTO VaccineTypes ([Name], [Description]) 
VALUES 
(N'COVID-19', N'Vaccine phòng chống COVID-19'),
(N'Cúm mùa', N'Vaccine phòng chống cúm mùa'),
(N'Viêm gan B', N'Vaccine phòng chống viêm gan B'),
(N'bại liệt', N'Vaccine tiêm phòng bại liệt'),
(N'Bạch hầu - Ho gà - Uốn ván', N'Vaccine kết hợp phòng bệnh bạch hầu, ho gà và uốn ván');

/****** Insert:  Table [dbo].[Vaccines] ******/
INSERT INTO Vaccines ([Name], [VaccineTypeID], [Manufacturer], [Stock], [Price], [Description], [FromAge], [ToAge], [Period], [DosesRequired], [Priority]) 
VALUES 
('Pfizer-BioNTech', 1, 'Pfizer', 100, 250000, N'Vaccine mRNA phòng COVID-19', 12, 99, 6, 1, 1),
('Moderna', 1, 'Moderna', 150, 230000, N'Vaccine mRNA phòng COVID-19', 6, 99, 6, 2, 1),
('Vaxigrip Tetra', 2, 'Sanofi', 200, 180000, N'Vaccine phòng cúm mùa', 6, 99, 12, 1, 2),
('Engerix-B', 3, 'GSK', 80, 300000, N'Vaccine phòng viêm gan B', 0, 99, 6, 1, 2),
('ComBE Five', 5, 'Biological E. Limited', 350, 200000, N'Vaccine phòng bạch hầu, ho gà, uốn ván', 2, 12, 2, 3, 2),
('Pentaxim', 5, 'Sanofi Pasteur', 250, 500000, N'Vaccine 5 trong 1 phòng bạch hầu, ho gà, uốn ván, bại liệt và Hib', 2, 12, 2, 4, 1),
('MMR II', 3, 'Merck', 450, 400000, N'Vaccine phòng sởi, quai bị, rubella', 3, 99, 12, 2, 3),
('Varilrix', 3, 'GlaxoSmithKline', 200, 600000, N'Vaccine phòng thủy đậu', 12, 99, 12, 1, 3),
('OPV (Oral Polio Vaccine)', 4, 'WHO', 500, 0, N'Vaccine uống phòng bại liệt', 0, 5, 2, 3, 1),
('IPV (Inactivated Polio Vaccine)', 4, 'Sanofi Pasteur', 300, 350000, N'Vaccine tiêm phòng bại liệt', 2, 99, 2, 2, 1);

/** Ko biết test như nào :( v **/
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
	[AccountID] [int] NOT NULL,
	[VaccineID] [int] NOT NULL,
	[TimeSlotID] int not null,
    [Status] [varchar](20) NOT NULL,
	FOREIGN KEY ([TimeSlotID]) REFERENCES [dbo].[TimeSlots]([TimeSlotID]),
    FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Children]([ChildID]),
	FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Accounts]([AccountID]),
	FOREIGN KEY ([VaccineID]) REFERENCES [dbo].[Vaccines]([VaccineID])
)	
GO

CREATE TABLE [dbo].[CancelAppointment](
	[CancelAppointmentID] [int] IDENTITY(1,1) PRIMARY KEY,
	[AppointmentID] [int] NOT NULL,
	[CancelDate] [DateTime] NOT NULL,
	[Reason] NText NOT NULL,
	FOREIGN KEY ([AppointmentID]) REFERENCES [dbo].[Appointments]([AppointmentID]),
)
GO

 

CREATE TABLE [dbo].[ChildTimeSlots] (
    ChildTimeSlotID int IDENTITY(1,1) PRIMARY KEY,
    ChildID INT NOT NULL,
    SlotNumber INT NOT NULL,
    DailyScheduleID INT NOT NULL,
    Available BIT NOT NULL,
	FOREIGN KEY ([DailyScheduleID]) REFERENCES [dbo].[DailySchedule]([DailyScheduleID]),
    FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Children]([ChildID])
);

/**
DBCC CHECKIDENT ('Appointments', RESEED, 0);
DBCC CHECKIDENT ('TimeSlots', RESEED, 0);

DBCC CHECKIDENT ('DailySchedule', RESEED, 0);
DBCC CHECKIDENT ('DoctorTimeSlots', RESEED, 0);
DBCC CHECKIDENT ('ChildTimeSlots', RESEED, 0);
/** Ko biết test như nào :( ^ **/
**/

/****** Object:  Table [dbo].[Record] ******/
CREATE TABLE [dbo].[VaccineRecords] (
    [VaccineRecordID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ChildID] [int] NOT NULL,
	[VaccineTypeID] [int] NOT NULL,
	[VaccineID] [int] NULL,
	[AppointmentID] [int] NULL, 
    [Date] [date] NOT NULL,
    [Note] [ntext] NULL,
	FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Children]([ChildID]),
	FOREIGN KEY ([VaccineTypeID]) REFERENCES [dbo].[VaccineTypes]([VaccineTypeID]),
	FOREIGN KEY ([VaccineID]) REFERENCES [dbo].[Vaccines]([VaccineID]),
	FOREIGN KEY ([AppointmentID]) REFERENCES [dbo].[Appointments]([AppointmentID])
)
GO

/****** Object:  Table [dbo].[VaccineCombos] ******/
CREATE TABLE [dbo].[VaccineCombos](
	[VaccineComboID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NOT NULL
)
GO

/****** Object:  Table [dbo].[VaccineContainer] ******/
CREATE TABLE [dbo].[VaccineContainers](
	[VaccineContainerID] [int] IDENTITY(1,1) PRIMARY KEY,
	[VaccineComboID] [int] NOT NULL,
	[VaccineID] [int] NOT NULL,
	FOREIGN KEY ([VaccineID]) REFERENCES [dbo].[Vaccines]([VaccineID]),
	FOREIGN KEY ([VaccineComboID]) REFERENCES [dbo].[VaccineCombos]([VaccineComboID])
)
GO

/**
DBCC CHECKIDENT ('VaccineCombos', RESEED, 0);
DBCC CHECKIDENT ('VaccineContainers', RESEED, 0);
DBCC CHECKIDENT ('VaccineRecords', RESEED, 0);
**/

INSERT INTO [dbo].[VaccineRecords] ([ChildID], [VaccineTypeID], [VaccineID], [AppointmentID], [Date], [Note]) VALUES
(1, 1, 1, NULL, '2024-10-22', N'Không có dị ứng.'),
(1, 2, 3, NULL, '2024-01-12', N'Không có dị ứng.'),
(2, 1, 1, NULL, '2024-11-10', N'Không có dị ứng.'),
(2, 3, 8, NULL, '2024-03-22', N'Không có dị ứng.'),
(4, 4, 10, NULL, '2024-05-12', N'Không có dị ứng.');
GO

INSERT INTO [dbo].[VaccineCombos] ([Name], [Description]) VALUES 
(N'Trẻ sơ sinh', N'Gồm các vaccine thiết yếu cho hệ miễn dịch của trẻ.'),
(N'Tiền học đường',N'Gồm các vaccine cho bé trong độ tuổi chuẩn bị đến trường.');
GO

INSERT INTO [dbo].[VaccineContainers] ([VaccineComboID], [VaccineID]) VALUES
(1, 3),
(1, 4),
(1, 5),
(2, 1),
(2, 3),
(2, 9),
(2, 7);
GO
