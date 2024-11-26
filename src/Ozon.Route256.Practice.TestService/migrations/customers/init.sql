CREATE TABLE IF NOT EXISTS customers
(
    customer_id  bigserial                NOT NULL PRIMARY KEY,
    region_id    bigint                   NOT NULL,
    full_name   text                      NOT NULL,
    created_at  timestamp with time zone  NOT NULL DEFAULT (now() at time zone 'utc'),
    UNIQUE(full_name),
    CHECK (length(full_name) <= 255)
);

CREATE TABLE IF NOT EXISTS regions
(
    id   bigint  NOT NULL PRIMARY KEY,
    name text    NOT NULL
);

INSERT INTO regions (id, name)
VALUES
    (1, 'Москва'),
    (2, 'Санкт-Петербург'),
    (3, 'Екатеренбург');
