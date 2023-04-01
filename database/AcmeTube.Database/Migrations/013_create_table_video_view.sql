CREATE TABLE video_view
(
    video_id            varchar(32)     NOT NULL CONSTRAINT video_rating_video_fk REFERENCES video,
    membership_id       varchar(32)     NOT NULL CONSTRAINT video_rating_membership_fk REFERENCES "user",
    created_at          timestamptz     NOT NULL,

    constraint video_view_pk primary key(video_id, membership_id)
);