CREATE TABLE subscription 
(
    channel_id      varchar(32)     NOT NULL CONSTRAINT subscription_channel_fk REFERENCES channel,
    membership_id   varchar(32)     NOT NULL CONSTRAINT subscription_user_fk REFERENCES "user",   
    created_at      timestamptz     NOT NULL,

    CONSTRAINT subscription_pk primary key(channel_id, membership_id)
);
