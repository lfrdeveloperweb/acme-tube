CREATE TABLE video_rating
(
    video_id            varchar(32)     NOT NULL CONSTRAINT video_rating_video_fk REFERENCES video,
    membership_id       varchar(32)     NOT NULL CONSTRAINT video_rating_membership_fk REFERENCES "user",
    is_like             bool            NOT NULL,
    created_at          timestamptz     NOT NULL,

    constraint video_rating_pk primary key(video_id, membership_id)
);