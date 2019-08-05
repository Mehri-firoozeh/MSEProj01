-- Schema V1.4 20January2016
Create table Phase
(
	PhaseID int,
	Name varchar(24),
	Description varchar(240),
	Primary key (PhaseID)
);
Insert into Phase values (0, 'Start Up', 'Initialize a new Project');
Insert into Phase values (1, 'Solution Outline', 'Broad stroke outline of Project structure');
Insert into Phase values (2, 'Macro Design', 'Specify Architecture');
Insert into Phase values (3, 'Micro Design', 'Specify Detailed Development plan');
Insert into Phase values (4, 'Build & Test', 'Write and Test code');
Insert into Phase values (5, 'Deploy', 'Deploy Project to Production Environment');
Insert into Phase values (6, 'Transition & Close', 'Finalize Development and free resources');

create table Vertical
(
    VerticalID int,
    Name varchar(50),
    Description varchar(240),
    primary key (VerticalID)
);

insert into Vertical values (0, 'Warehouse Solutions', 'Project dealing with managing a warehouse');
insert into Vertical values (1, 'Merchandising Solutions', 'Project dealing with managing a warehouse');
insert into Vertical values (2, 'Membership Solutions', 'Projects for marketing and managing Memberships');
insert into Vertical values (3, 'Distribution Solutions', 'Projects aimed toward inventory transfer');
insert into Vertical values (4, 'International Solutions', 'Projects helping manage international business');
insert into Vertical values (5, 'Ancillary Solutions', 'Miscelaneous Projects');
insert into Vertical values (6, 'eBusiness Solutions', 'eCommerce and web services');
insert into Vertical values (7, 'Corporate Solutions', 'Internal business management Projects');

create table UserRole
(
    RoleID int,
    Name varchar(50),
    Description varchar(240),
    primary key (RoleID)
);

create table AllowedUser
(
    UserID uniqueidentifier,
    Email varchar(240) not null,
    RoleID int not null default 0,
    primary key (UserID),
    foreign key (RoleID) references UserRole (RoleID)
);

create table Project
(
    ProjectID varchar(240),
    Name varchar(240),
    Description varchar(400),
    VerticalID int,
    primary key (ProjectID),
    foreign key (VerticalID) references Vertical (VerticalID)
);

create table ProjectPhase
(
    PhaseID int,
    ProjectID varchar(240),
    UpdateKey varchar(100),
    UpdateCount int,
    LatestUpdate smalldatetime,
    primary key (PhaseID, ProjectID, UpdateKey),
    foreign key (PhaseID) references Phase (PhaseID),
    foreign key (ProjectID) references Project (ProjectID)    
);

create table StatusUpdate
(
    ProjectID uniqueidentifier,
    ProjectUpdateID uniqueidentifier,
    PhaseID int,
    VerticalID int,
    RecordDate smalldatetime,
    UpdateKey nvarchar(100),
    UpdateValue nvarchar(max)
    primary key (ProjectID, ProjectUpdateID, UpdateKey),
    foreign key (ProjectID) references Project (ProjectID),
    foreign key (PhaseID) references Phase (PhaseID),
    foreign key (VerticalID) references Vertical (VerticalID),
    foreign key (ProjectUpdateID) references ProjectUpdate (ProjectUpdateID)
);

create table ProjectUpdate
(
    ProjectUpdateID uniqueidentifier,
    ProjectID uniqueidentifier,
    Subject varchar(400),
    Body varchar(max),
    primary key (ProjectUpdateID),
    foreign key (projectID) references Project (ProjectID)
);






-- User the following lines to remove everything if reset is needed

  --drop table StatusUpdate;
  --drop table ProjectPhase;
  --drop table Project;
  --drop table Vertical;
  --drop table Phase;
  --drop table AllowedUser;
  --drop table UserRole;
  
  -- Use these lines to delete all data
  delete from StatusUpdate;
  delete from ProjectPhase;
  delete from Project;