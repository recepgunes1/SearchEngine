FROM postgres:16.0

RUN apt-get update && apt-get install -y postgresql-plpython3-16

COPY ./UrlExtractor /docker-entrypoint-initdb.d
