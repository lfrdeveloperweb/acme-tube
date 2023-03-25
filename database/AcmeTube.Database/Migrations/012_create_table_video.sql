CREATE TABLE video 
(
    video_id        varchar(32)     CONSTRAINT video_pk PRIMARY KEY,
    title           varchar(128)    NOT NULL,
    description     varchar(512)    NULL,
    channel_id      varchar(32)     NULL CONSTRAINT video_channel_fk REFERENCES channel,
    category_id     int2            NULL,
    tags            json            NULL,
    status_id       int2            NOT NULL,
    created_by      varchar(32)     NOT NULL CONSTRAINT video_user_created_by_fk REFERENCES "user",
    created_at      timestamptz     NOT NULL,    
    updated_by      varchar(32)     NULL  CONSTRAINT video_user_updated_by_fk REFERENCES "user",
    updated_at      timestamptz     NULL,
    deleted_at      timestamptz     NULL    
);
