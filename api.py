from flask import Flask, request
from package.openpose import read,convert_bool
import json

app = Flask(__name__)
app.config['SERVER_NAME'] = 'openposesystem.sytes.net:5000'

@app.route("/pose",methods=["GET"])
def pose():
    MembersId = request.args.get('MembersId')
    imgpath = request.args.get('imgpath')
    img = request.args.get('img')
    isData = convert_bool(request.args.get('isData'), False)
    read(MembersId,imgpath,img,isData)
    with open('result.json', 'r', encoding='utf-8') as f:
        output = json.load(f)
    return output

if __name__ == '__main__':
    app.run()
    