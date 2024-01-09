from nltk.tokenize import sent_tokenize, word_tokenize
from nltk.corpus import stopwords

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

        return ' '.join(summary_sentences[2:5]) 
