if object_id('dbo.Schedule', 'U') is null
	create table dbo.Schedule
	(
		Id int identity(1,1) not null,
		StartDateTimePeriod datetime not null,
		EndDateTimePeriod datetime not null,
		CreatedOn datetime not null default getdate(),
		Notes nvarchar(1000) null,
		constraint pkScheduleId primary key clustered (Id)
	)
go

if object_id('dbo.Shift', 'U') is null
	create table dbo.Shift
	(
		Id int identity(1,1) not null,
		EngineerId int not null,
		ShiftPeriod int not null,
		ScheduleId int not null,
		DateTimePeriod datetime not null,
		CreatedOn datetime not null default getdate(),
		constraint pkShiftId primary key clustered (Id),
		constraint fkShiftEngineerIdEngineerId foreign key (EngineerId) references Engineer(Id),
		constraint fkShiftScheduleIdScheduleId foreign key (ScheduleId) references Schedule(Id)
	)
go
