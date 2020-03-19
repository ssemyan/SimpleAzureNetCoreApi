-- Create DB if not exists
 If(db_id(N'Items') IS NULL)
 BEGIN
	CREATE DATABASE [Items]
END;

use Items
go

-- Create Table if not exists
IF OBJECT_ID(N'dbo.Items', N'U') IS NULL 
BEGIN 
	create table Items
	(
		ItemId int identity primary key,
		ItemName nvarchar(255)
	)

	insert Items (ItemName) select 'Item One Azure'
	insert Items (ItemName) select 'Item Two Azure'
	insert Items (ItemName) select 'Item Three Azure'
END