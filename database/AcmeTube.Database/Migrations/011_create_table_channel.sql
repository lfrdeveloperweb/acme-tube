CREATE TABLE channel 
(
	channel_id		varchar(32)			CONSTRAINT channel_pk PRIMARY KEY,
	title			varchar(128)		NOT NULL,
	description		text				NOT NULL,
	country_name	varchar(64)			NOT NULL,
	thumbnails_url	text				NULL,	
	tags			json				NULL,	
	created_by		varchar(32)			NOT NULL CONSTRAINT channel_user_fk REFERENCES "user",
	created_at		timestamptz			NOT NULL
);

/*

	INSERT INTO channel 
	(
		channel_id, 
		title, 
		description, 
		country_name,
		thumbnails_url, 
		tags, 
		created_by, 
		created_at
	)
	VALUES
	(
		'cfb1bd03f73639f6f324ccb5', 
		'Desenvolvedor.io', 
		'Canal oficial da plataforma desenvolvedor.io. Cursos online de programação e tecnologia.', 
		'Brasil',
		null, 
		'["dotnet","c#","sql server", "kafka", ""]', 
		'master', 
		current_timestamp
	);
*/