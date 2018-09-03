if object_id('dbo.Engineer', 'U') is null
	create table dbo.Engineer
	(
		Id int identity(1,1) not null,
		Password nvarchar(50) not null default 'pass@word123', --TODO: Password nvarchar(50) not null default 'pass@word123' although it's a  sample app, the password shouldn't store as raw string     
		FirstName nvarchar(200) not null,
		LastName nvarchar(200) not null,
		Email nvarchar(100) null,
		UserRole nvarchar(25) not null default 'User', --Admin,Manager,Lead,User
		IsActive bit not null default 1,
		constraint pkEngineerId primary key clustered (Id)
	)
go