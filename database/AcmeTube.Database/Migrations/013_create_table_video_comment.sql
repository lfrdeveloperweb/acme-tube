CREATE TABLE video_comment 
(
    video_comment_id    int             constraint video_comment_pk primary key generated always as identity,
    video_id            varchar(32)     NOT NULL CONSTRAINT video_comment_video_fk REFERENCES video,
    description         varchar(128)    NOT NULL,
    created_by          varchar(32)     NOT NULL CONSTRAINT video_comment_user_updated_by_fk REFERENCES "user",
    created_at          timestamptz     NOT NULL    
);