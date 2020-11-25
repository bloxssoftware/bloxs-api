import re
import json
import hmac
from datetime import datetime
from requests import urllib3, Request, Session
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)


apiKey = "apiKey";
apiSecret = "apiSecret";
customerName = "customerName";
base_url = "https://" + customerName + ".bloxs.io/"

class BloxsAPI:
    def __init__(self, apiKey, apiSecret):
        self.apiKey = apiKey
        self.apiSecret = apiSecret

    def send(self, prepared_request, session=None):
        if not session:
            session = Session()
        timestamp = str(int(datetime.now().timestamp()))
        body_length = len(prepared_request.body) if prepared_request.body else 0
        token_parts = f'{apiKey}:{timestamp}:{body_length}'
        signature = hmac.digest(apiSecret.encode(), token_parts.encode(), 'sha256').hex()
        headers = {'X-Timestamp': timestamp,
                   'Authorization': f'bloxs {apiKey}:{signature}'}

        prep_request.headers.update(headers)
        return session.send(prepared_request)

def print_response(response):
    answer = ''
    answer += 'HTTP/1.1 {0} {1}\r\n'.format(response.status_code, response.reason)
    answer += '\r\n'.join(['{0}: {1}'.format(key, value) for key, value in response.headers.items()])
    answer += '\r\n'
    answer += response.content.decode(errors='ignore')
    answer += '\r\n'
    print(answer)

api = BloxsAPI(apiKey, apiSecret)


# TEST API
url = '{0}/Test'.format(base_url.rstrip('/'))
prep_request = Request('GET', url).prepare()
response = api.send(prep_request, None)
print_response(response)
