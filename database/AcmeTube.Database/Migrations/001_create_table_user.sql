﻿CREATE TABLE "user"
(
	membership_id			varchar(32)		CONSTRAINT user_pk PRIMARY KEY, 
										    CONSTRAINT user_membership_fk references "user",
	login					varchar(32)		NOT NULL CONSTRAINT un_user_login UNIQUE,
	document_number			varchar(11)		NOT NULL CONSTRAINT user_document_number_un UNIQUE,
	birth_date				date			NULL,
	email					varchar(128)	NOT NULL,
	email_confirmed			BOOLEAN         NOT NULL DEFAULT FALSE,	
	phone_number			varchar(20)		NULL,
	phone_number_confirmed  BOOLEAN         NOT NULL DEFAULT FALSE,
	password_hash			varchar(128)	NOT NULL,
	last_login_at			timestamptz    	NULL,
    login_count				SMALLINT		NOT NULL	DEFAULT 0,
	access_failed_count		int				NOT NULL	DEFAULT 0,
    locked_at				timestamptz		NULL,
	created_by				VARCHAR(32)		NULL		CONSTRAINT user_user_created_by_fk REFERENCES "user",
	created_at				timestamptz		NOT NULL,
	updated_by				varchar(32)		NULL		CONSTRAINT user_user_updated_by_fk REFERENCES "user",
	updated_at				timestamptz		NULL
);


/*

	INSERT INTO membership (membership_id, "name", role_id, created_at)
		VALUES
			('master', 'Master', 1, CURRENT_TIMESTAMP),
			('developer', 'developer', 1, CURRENT_TIMESTAMP)
	ON CONFLICT (membership_id) DO NOTHING;

	INSERT INTO "user" (membership_id, login, document_number, email, email_confirmed, "password_hash", created_at)
		VALUES 
			('master', 'master', '75725416070', 'master@acme-tube.com', true, 'AQAAAAEAACcQAAAAEPWEeTmGdPJPN3Y2IEEj3vT7p6E9IgtiCCMJfXTdfIdxpbKb9O/D8iGEPDpZM9IHFQ==', CURRENT_TIMESTAMP),
			('developer', 'developer', '72872357068', 'xxxx@gmail.com', true, 'AQAAAAEAACcQAAAAEPWEeTmGdPJPN3Y2IEEj3vT7p6E9IgtiCCMJfXTdfIdxpbKb9O/D8iGEPDpZM9IHFQ==', CURRENT_TIMESTAMP)
	ON CONFLICT (membership_id) DO NOTHING;

*/