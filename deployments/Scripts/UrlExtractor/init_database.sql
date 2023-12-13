CREATE TABLE "ExtractedUrls"
(
    "Id"              text primary key not null,
    "Link"            text,
    "CreatedDateTime" timestamptz
);

CREATE EXTENSION IF NOT EXISTS dblink;

CREATE EXTENSION IF NOT EXISTS plpython3u;

CREATE OR REPLACE FUNCTION nslookup_grep_cut(url text)
    RETURNS text AS
$$
import subprocess

try:
    # Construct the shell command with the provided URL parameter
    command = "nslookup " + url + " | grep 'Address:' | cut -d ':' -f 2 | tail -1"
    
    # Execute the command and process the output
    result = subprocess.check_output(command, shell=True)
    return result.strip().decode('utf-8')
except Exception as e:
    return str(e)
$$ LANGUAGE plpython3u;

CREATE VIEW view_tag_extractor AS
SELECT *
FROM dblink('hostaddr=' || nslookup_grep_cut('tagextractor.database') ||
            ' port=5432 dbname=TagExtractor user=postgres password=Password123',
            'SELECT "Link", "Tags", "CreatedDateTime" FROM public."Tags"')
         AS t(Link text, Tags text, CreatedDateTime timestamptz);

CREATE VIEW view_page_downloader AS
SELECT *
FROM dblink('hostaddr=' || nslookup_grep_cut('pagedownloader.database') ||
            ' port=5432 dbname=PageDownloader user=postgres password=Password123',
            'SELECT "Link", "Title", "InnerText" FROM public."Pages"')
         AS t(Link text, Title text, InnerText text);

CREATE VIEW view_last_one_minute AS
SELECT EU."Id", EU."Link", VTE."tags", VPD."title", VPD."innertext"
FROM public.view_tag_extractor VTE
         INNER JOIN public."ExtractedUrls" EU on VTE.link = EU."Link"
         INNER JOIN public.view_page_downloader VPD on EU."Link" = VPD.link
WHERE VTE.createddatetime >= NOW() - INTERVAL '1 minute';
