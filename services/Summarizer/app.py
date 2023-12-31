from flask import Flask, request, jsonify
import nltk
from nltk.tokenize import sent_tokenize, word_tokenize
from nltk.corpus import stopwords
import ssl
import logging
import gzip
import io


import base64

def decompress_gzip(compressed_data):
    # Ensure the input is in byte format
    if isinstance(compressed_data, str):
        # If the input is a string, it needs to be converted to bytes
        compressed_data = compressed_data.encode('utf-8')

    with gzip.GzipFile(fileobj=io.BytesIO(compressed_data), mode='rb') as gz:
        return gz.read().decode('utf-8')


# Set up basic configuration for logging
logging.basicConfig(level=logging.INFO)

try:
    _create_unverified_https_context = ssl._create_unverified_context
except AttributeError:
    pass
else:
    ssl._create_default_https_context = _create_unverified_https_context

nltk.download('punkt')
nltk.download('stopwords')

app = Flask(__name__)

class DataObject:
    def __init__(self, data):
        self.data = data

    def summarize(self):
        sentences = sent_tokenize(self.data)
        word_counts = {}
        stop_words = set(stopwords.words('english'))

        for sentence in sentences:
            words = word_tokenize(sentence)
            for word in words:
                if word.lower() not in stop_words and word.isalnum():
                    word_counts[word] = word_counts.get(word, 0) + 1

        sorted_words = sorted(word_counts.items(), key=lambda x: x[1], reverse=True)

        # Simple heuristic: take sentences that contain the most frequent words
        summary_sentences = [sentence for sentence in sentences if sorted_words[0][0] in sentence]

        logging.info("Summarization completed")
        return ' '.join(summary_sentences[:2])  # return first two sentences of the summary


@app.route('/summarize', methods=['POST'])
def summarize():
    logging.info("Received summarize request")
    
    if not request.is_json:
        logging.warning("Invalid content type")
        return jsonify({"error": "Invalid content type"}), 415

    content = request.json

    # Check if 'Data' field is present and valid
    if 'Data' not in content or not isinstance(content['Data'], str):
        logging.warning("Missing or invalid 'Data' field")
        return jsonify({"error": "Missing or invalid 'Data' field"}), 400

    data = content['Data']

    # Check if 'Data' field is empty
    if not data.strip():
        logging.warning("'Data' field is empty")
        return jsonify({"error": "'Data' field is empty"}), 400

    data_object = DataObject(base64.b64decode(data).decode("utf-8"))
    summary = data_object.summarize()

    logging.info("Sending response")
    return jsonify({"summary": summary}), 200


if __name__ == '__main__':
    app.run(debug=True, port=1453)
