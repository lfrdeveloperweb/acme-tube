CREATE TABLE playlist_video
(
	playlist_id		varchar(32)		NOT NULL CONSTRAINT playlist_video_playlist_fk REFERENCES playlist,
	video_id        varchar(32)     NOT NULL CONSTRAINT playlist_video_video_fk REFERENCES video,
	created_at		timestamptz		NOT NULL,

	constraint playlist_video_pk primary key(playlist_id, video_id)
);

/*

	INSERT INTO playlist 
	(
		playlist_id, 
		title, 
		description, 
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