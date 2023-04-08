CREATE TABLE user_activity
(
	user_activity_id		int				CONSTRAINT user_activity_pk primary key generated always as identity,
	membership_id			varchar(32)		CONSTRAINT user_membership_fk references "user",
	event_type				varchar(64)		NOT NULL,
	event_data				json			NULL,
	created_at				timestamptz		NOT NULL
);