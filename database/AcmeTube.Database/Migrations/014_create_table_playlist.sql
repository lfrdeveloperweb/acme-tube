CREATE TABLE playlist 
(
	playlist_id		varchar(32)			CONSTRAINT playlist_pk PRIMARY KEY,
	title			varchar(128)		NOT NULL,
	description		text				NOT NULL,
	tags			json				NULL,
	created_by		varchar(32)			NOT NULL CONSTRAINT playlist_user_fk REFERENCES "user",
	created_at		timestamptz			NOT NULL
);

/*

	INSERT INTO playlist 
	(
		playlist_id, 
		title, 
		description, 
		thumbnails_url, 
		tags, 
		created_by, 
		created_at
	)
	VALUES
	(
		'73639ff24cc6f3cfb1bd03b5', 
		'Desenvolvedor.io', 
		'Canal oficial da plataforma desenvolvedor.io. Cursos online de programação e tecnologia.', 
		null, 
		'["nba"]', 
		'', 
		current_timestamp
	);
*/