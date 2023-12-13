FROM postgres:16.0

RUN apt-get update && apt-get install -y python3 postgresql-plpython3-16 dnsutils

COPY ./init_database.sql /docker-entrypoint-initdb.d/init_database.sql

RUN  apt-get clean && \
     rm -rf /var/cache/apt/* /var/lib/apt/lists/*

CMD ["postgres"]
