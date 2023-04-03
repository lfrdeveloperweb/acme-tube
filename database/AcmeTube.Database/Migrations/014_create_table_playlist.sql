CREATE TABLE playlist
(
	playlist_id		varchar(32)		CONSTRAINT playlist_pk PRIMARY KEY,
	channel_id      varchar(32)		NULL CONSTRAINT playlist_channel_fk REFERENCES channel,
	membership_id   varchar(32)     NULL CONSTRAINT playlist_user_fk REFERENCES "user",
	title			varchar(128)	NOT NULL,
	description		text			NOT NULL,
	scope			varchar(16)		NOT NULL,
	created_by		varchar(32)		NOT NULL CONSTRAINT playlist_membership_created_by_fk REFERENCES "user",
	created_at		timestamptz		NOT NULL,
	updated_by		varchar(32)		NULL		CONSTRAINT membership_membership_updated_by_fk REFERENCES membership,
	updated_at		timestamptz		NULL
);

/*

	INSERT INTO playlist 
	(
		playlist_id, 
		channel_id,
		title, 
		description, 
		is_public,
		scope,
		created_by, 
		created_at
	)
	VALUES
	(
		'cc6f3cfb1bd09ff243b57363', 
		'cfb1bd03f73639f6f324ccb5',
		'The best videos', 
		'The best videos in the world', 
		true,
		'channel',
		'developer', 
		current_timestamp
	);
*/