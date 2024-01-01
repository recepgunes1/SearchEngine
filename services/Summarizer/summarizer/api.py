from flask import Blueprint, request, jsonify
from .model import DataObject
import base64

group = Blueprint('api', __name__)

@group.route('summarize', methods=['POST'])
def summarize():    
    if not request.is_json:
        return jsonify({"error": "Invalid content type"}), 415

    content = request.json

    if 'Data' not in content or not isinstance(content['Data'], str):
        return jsonify({"error": "Missing or invalid 'Data' field"}), 400

    raw_data = content['Data']
    decoded_data = base64.b64decode(raw_data).decode("utf-8")

    if not decoded_data.strip():
        return jsonify({"error": "'Data' field is empty"}), 400

    data_object = DataObject(decoded_data)
    summary = data_object.summarize()

    return jsonify({"summary": summary}), 200
