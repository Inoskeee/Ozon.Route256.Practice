create table if not exists orders
(
    order_id    bigserial                   not null primary key,
    region_id   integer                     not null,
    status      integer                     not null,
    customer_id bigint                      not null,
    comment     text,
    created_at  timestamp with time zone    not null
);

create table if not exists items
(
    id          bigserial   not null primary key,
    order_id    bigint      not null,
    barcode     text,
    quantity    integer     not null
);

do $$
begin
if not exists(select 1 from pg_type where typname = 'items_type') then
create type items_type as
    (
    id          bigint,
    order_id    bigint,
    barcode     text,
    quantity    integer
    );
end if;
end $$;

create table if not exists regions
(
    id   bigserial  not null primary key,
    name text       not null
);

insert into regions (name)
values
    ('Москва'),
    ('Санкт-Петербург'),
    ('Екатеренбург');

create table if not exists logs
(
    id          bigserial                   not null primary key,
    order_id    bigint                      not null,
    region_id   integer                     not null,
    status      integer                     not null,
    customer_id bigint                      not null,
    comment     text,
    created_at  timestamp with time zone    not null,
    updated_at  timestamp with time zone    not null
);
