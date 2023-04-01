﻿CREATE TABLE video 
(
    video_id        varchar(32)     CONSTRAINT video_pk PRIMARY KEY,
    title           varchar(128)    NOT NULL,
    description     varchar(512)    NULL,
    channel_id      varchar(32)     NOT NULL CONSTRAINT video_channel_fk REFERENCES channel,
    duration        interval        NOT NULL,
    --category_id     int2            NULL,
    tags            json            NULL,
    --status_id       int2            NOT NULL,
    is_public       bool            NOT NULL DEFAULT true,
    views_count     int             NOT NULL DEFAULT 0,
    likes_count     int             NOT NULL DEFAULT 0,
    dislikes_count  int             NOT NULL DEFAULT 0,
    comments_count  int             NOT NULL DEFAULT 0,
    created_by      varchar(32)     NOT NULL CONSTRAINT video_membership_created_by_fk REFERENCES membership,
    created_at      timestamptz     NOT NULL,    
    updated_by      varchar(32)     NULL  CONSTRAINT video_membership_updated_by_fk REFERENCES membership,
    updated_at      timestamptz     NULL    
);
