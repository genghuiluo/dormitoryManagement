create table admin_acc(
username varchar(20),
password varchar(20),
building varchar(2),
power char(1) -- A/B/C
);

create table vistor(
idcard char(20),
vname varchar(10),
sname varchar(10),
sno char(8),
vtime date,
vbuilding varchar(2)
)

create table student_indor(
sno char(8),
sname varchar(10),
sex char(2),
school varchar(20),
major varchar(20),
stime datetime,
ltime datetime,
building varchar(2),
dor char(3)
)
