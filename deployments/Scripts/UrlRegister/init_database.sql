CREATE TABLE "RegisteredUrls"
(
    "Id"              text primary key not null,
    "Link"            text,
    "CreatedDateTime" timestamptz
);
INSERT INTO "RegisteredUrls" ("Id", "Link", "CreatedDateTime") VALUES ('1', 'https://examoke.com', '2021-01-01 00:00:00');