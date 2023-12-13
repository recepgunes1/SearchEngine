CREATE TABLE "Pages"
(
    "Id"                   text primary key not null,
    "Link"                 text,
    "Title"                text,
    "InnerText"            text,
    "CompressedSourceCode" bytea,
    "CreatedDateTime"      timestamptz
);
