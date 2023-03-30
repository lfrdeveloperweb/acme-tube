CREATE TABLE channel 
(
    channel_id          varchar(32)     CONSTRAINT channel_pk PRIMARY KEY,
    name                varchar(128)    NOT NULL,
    description         text            NOT NULL,
    country_name        varchar(64)     NOT NULL,
    thumbnails_url      text            NULL,    
    tags                json            NULL,
    links               json            NULL,
    videos_count        int             NOT NULL DEFAULT 0,
    views_count         int             NOT NULL DEFAULT 0,
    subscribers_count   int             NOT NULL DEFAULT 0,
    created_by          varchar(32)     NOT NULL CONSTRAINT channel_user_fk REFERENCES "user",
    created_at          timestamptz     NOT NULL,
    updated_by          varchar(32)     NULL  CONSTRAINT video_user_updated_by_fk REFERENCES "user",
    updated_at          timestamptz     NULL
);

/*

    INSERT INTO channel 
    (
        channel_id, 
        name, 
        description, 
        country_name,
        thumbnails_url, 
        tags,
        created_by, 
        created_at
    )
    VALUES
    (
        '03f73639fd24ccb56f3cfb1b', 
        'ACME Tube', 
        'Canal oficial da plataforma ACME Tube. Videos, series, documentários e muito mais para seu entretenimento', 
        'Brasil',
        null, 
        '{"facebook":"https://www.facebook.com/acme-tube", "instagram":"https://www.instagram.com/acme-tube", "twitter":"https://twitter.com/acme-tube"}'
        '["videos","series","documentarios"]', 
        'master', 
        current_timestamp
    );
*/