import ssl
import nltk
from flask import Flask

def create_app():
    try:
        _create_unverified_https_context = ssl._create_unverified_context
    except AttributeError:
        pass
    else:
        ssl._create_default_https_context = _create_unverified_https_context

    nltk.download('punkt')

    nltk.download('stopwords')

    app = Flask(__name__)
    
    from summarizer.api import group

    app.register_blueprint(group, url_prefix='/api')

    return app
