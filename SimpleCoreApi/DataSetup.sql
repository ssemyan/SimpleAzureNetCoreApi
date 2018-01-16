-- Do this in master
CREATE LOGIN apiuser WITH password='[]';

CREATE USER apiuser FROM LOGIN apiuser;

EXEC sp_addrolemember db_datareader, 'apiuser';
EXEC sp_addrolemember db_datawriter, 'apiuser';


create table Items
(
	itemId int identity primary key,
	itemName nvarchar(255)
)

insert Items (itemName) select 'Item One Azure'
insert Items (itemName) select 'Item Two Azure'
insert Items (itemName) select 'Item Three Azure'