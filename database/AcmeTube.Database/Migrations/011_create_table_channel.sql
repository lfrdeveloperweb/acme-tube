CREATE TABLE channel 
(
	channel_id				varchar(32)			CONSTRAINT channel_pk PRIMARY KEY,
	title							varchar(128)		NOT NULL,
	description				text						NOT NULL,
	thumbnails_url		text						NULL,
	keywords					json						NULL,
	created_by				varchar(32)			NOT NULL CONSTRAINT channel_user_fk REFERENCES "user",
	created_at				timestamptz			NOT NULL,
);