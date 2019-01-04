DROP TABLE fast_table;
DROP TABLE slow_table;

CREATE TABLE fast_table (
	name character varying 	PRIMARY KEY,
	value int
);

CREATE TABLE slow_table (
	name char(8) PRIMARY KEY,
	value int 
);

INSERT INTO fast_table VALUES('rob', 1);
INSERT INTO slow_table VALUES('rob', 1);


INSERT INTO fast_table VALUES(generate_series(1,100000),random());
INSERT INTO slow_table VALUES(generate_series(1,100000),random());
