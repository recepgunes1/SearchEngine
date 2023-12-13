#!/bin/bash

# Elasticsearch parameters
ELASTICSEARCH_HOST="elasticsearch"
ELASTICSEARCH_PORT="9200"
INDEX_NAME="link_and_tags"

# Wait for Elasticsearch to start up
echo "Waiting for Elasticsearch to start..."
while ! nc -z $ELASTICSEARCH_HOST $ELASTICSEARCH_PORT; do   
  sleep 1 # wait for 1 second before check again
done
echo "Elasticsearch started."

# Define the mappings
MAPPING_JSON='{
  "mappings": {
    "properties": {
      "Title": {
        "type": "completion"
      },
      "Tags": {
        "type": "completion"
      }
    }
  }
}'

# Apply the mappings
echo "Applying mappings to $INDEX_NAME index..."
curl -X PUT "http://$ELASTICSEARCH_HOST:$ELASTICSEARCH_PORT/$INDEX_NAME" -H 'Content-Type: application/json' -d "$MAPPING_JSON"

echo "Mappings applied successfully."
