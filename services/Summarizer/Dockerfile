FROM python:3.8.18-slim

WORKDIR /app

COPY services/Summarizer/ /app/

RUN pip3 install -r /app/requirements.txt && rm -rf /root/.cache/pip

EXPOSE 1453

CMD [ "python3", "wsgi.py"]
