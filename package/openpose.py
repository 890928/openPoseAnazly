import cv2
import mediapipe as mp
import math
import time
import json
from pathlib import Path
from distutils.util import strtobool
import os

mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_pose = mp.solutions.pose

def convert_bool(isData, default_val):
    if isData is None:
        return default_val
    try:
        return strtobool(isData)
    except:
        return default_val
    
def neckresult(x1,x2,rs,image):
    error = ''
    neck_check = True
    if rs.lt[x1][1]-rs.lt[x2][1] > 30 :
        error = '脖子前傾'
        neck_check = False
    elif rs.lt[x1][1]-rs.lt[x2][1] < -10 :
        error = '脖子後傾'
        neck_check = False
    if neck_check == False:
        x = int((rs.lt[x1][1] + rs.lt[x2][1])/2)
        y = int((rs.lt[x1][2] + rs.lt[x2][2])/2)
        cv2.circle(image,(x,y),35,(64,177,0),5)
    return error,neck_check,image

def angled(x1,x2,x3,post,rs,errorcircle):
    tf = True
    angle = int(math.degrees(math.atan2(rs.lt[x1][2]-rs.lt[x2][2],rs.lt[x1][1]-rs.lt[x2][1])-math.atan2(rs.lt[x3][2]-rs.lt[x2][2],rs.lt[x3][1]-rs.lt[x2][1])))
    if angle < 0:
        angle = angle + 360
    if angle > 180:
        angle = 360-angle
    angle -= 90
    if post == 'elbow ':
        if angle > 10 or angle < 0:
            tf = False
            errorcircle.append(x2)
        angle = str(angle)
    if post == 'waist ':
        if angle > 20 or angle < 10:
            tf = False
            errorcircle.append(x2)
        angle = str(angle)
    if post == 'knee ':
        if angle > 15 or angle < 0:
            tf = False
            errorcircle.append(x2)
        angle = str(angle)
    return tf,angle,errorcircle

def make_errorlist(rs,elbow,elbow_check,waist,waist_check,knee,knee_check,errorlist):
    if elbow_check == False:
        if int(elbow) > 10:
            errorlist.append('手肘高度過高')
        if int(elbow) < 0:
            errorlist.append('手肘高度過低')
    if waist_check == False:
        if int(waist) < 10:
            errorlist.append('身體前傾')
        if int(waist) > 20:
            errorlist.append('身體後傾')
    if knee_check == False:
        if abs(rs.lt[25][2] - rs.lt[26][2]) > 50 and abs(rs.lt[27][2] - rs.lt[28][2]) > 50:
            errorlist.append('翹腳')
        elif int(knee) > 15:
            errorlist.append('膝蓋角度過直')
        elif int(knee) < 0:
            errorlist.append('膝蓋角度過彎')
    return errorlist

def drawerror(image,errorcircle,rs):
    for num in errorcircle:
        if (num == 13 or num == 14):
            cv2.circle(image,(rs.lt[num][1],rs.lt[num][2]),25,(0,184,255),5)
        elif (num == 23 or num == 24):
            cv2.circle(image,(rs.lt[num][1],rs.lt[num][2]),35,(0,18,255),5)
        elif (num == 25 or num == 26):
            cv2.circle(image,(rs.lt[num][1],rs.lt[num][2]),30,(204,125,0),5)
    return image

def read(MembersId,imgpath,img,isData):
    img = imgpath + img
    errorlist = []
    ifFile = True
    direction = 0
    path = Path(img)
    if not path.is_file():
        errorlist.append('查無此檔案')
        data = {'error':errorlist}
        isData = False
        ifFile = False
    elif path.suffix != '.jpg' and path.suffix != '.png' and path.suffix != '.jpeg':
        errorlist.append('這不是圖片')
        data = {'error':errorlist}
        isData = False
        ifFile = True
    else:
        cap = cv2.VideoCapture(img)
        with mp_pose.Pose(
            min_detection_confidence=0.5,
            min_tracking_confidence=0.5) as pose:
          while cap.isOpened():
            eee = time.localtime() #時間
            te = time.strftime("%Y%m%d%H%M%S", eee)
            ret, frame = cap.read()
            image = frame
            if image is None:
                errorlist.append('找不到人像')
                data = {'error':errorlist}
                isData = False
                ifFile = True
                break
            elbow = 0
            elbow_check = False
            waist = 0
            waist_check = False
            knee = 0
            knee_check = False
            neck_check = False
            image.flags.writeable = False
            image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
            rs = pose.process(image)
            image.flags.writeable = True
            image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
            rs.lt = []
            mp_drawing.draw_landmarks(
                image,
                rs.pose_landmarks,
                mp_pose.POSE_CONNECTIONS,
                landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style()) #這四行在畫線
            if rs.pose_landmarks:
                for id,lm in enumerate(rs.pose_landmarks.landmark):
                    h,w,c = image.shape
                    cx,cy = int (lm.x*w),int(lm.y*h) #取座標
                    rs.lt.append([id,cx,cy])
                    #print([id,cx,cy])
                if (rs.lt[23][1] > rs.lt[25][1]) and (rs.lt[24][1] > rs.lt[26][1]) or (rs.lt[11][1] > rs.lt[13][1]) and (rs.lt[12][1] > rs.lt[14][1]):
                    direction = 1
                elif (rs.lt[23][1] < rs.lt[25][1]) and (rs.lt[24][1] < rs.lt[26][1]) or (rs.lt[11][1] < rs.lt[13][1]) and (rs.lt[12][1] < rs.lt[14][1]):
                    direction = 2
                alltf = True
                errorcircle = []
                if direction == 1:
                    tf,angle,errorcircle = angled(11,13,15,'elbow ',rs,errorcircle)
                    elbow = angle
                    elbow_check = tf
                    tf,angle,errorcircle = angled(11,23,25,'waist ',rs,errorcircle)
                    waist = angle
                    waist_check = tf
                    tf,angle,errorcircle = angled(23,25,27,'knee ',rs,errorcircle)
                    knee = angle
                    knee_check = tf
                    error,tf,image = neckresult(7, 11, rs ,image)
                    if(error != ''):
                        errorlist.append(error)
                    neck_check = tf
                if direction == 2:
                    tf,angle,errorcircle = angled(12,14,16,'elbow ',rs,errorcircle)
                    elbow = angle
                    elbow_check = tf
                    tf,angle,errorcircle = angled(12,24,26,'waist ',rs,errorcircle)
                    waist = angle
                    waist_check = tf
                    tf,angle,errorcircle = angled(24,26,28,'knee ',rs,errorcircle)
                    knee = angle
                    knee_check = tf
                    error,tf,image = neckresult(8, 12 ,rs ,image)
                    if(error != ''):
                        errorlist.append(error)
                    neck_check = tf
                image = drawerror(image,errorcircle,rs)
                errorlist = make_errorlist(rs,elbow,elbow_check,waist,waist_check,knee,knee_check,errorlist)
                if elbow_check == False or waist_check == False or knee_check == False or neck_check == False:
                    alltf = False
                newimagename = MembersId + te +'.jpg'
                if isData:
                    data = {'ResultId':str(te),'direction':direction,'neck_check': neck_check,'elbow': elbow,'elbow_check': elbow_check, 'waist': waist,'waist_check':waist_check, 'knee': knee,'knee_check':knee_check, 'ispeople':True,'result': str(alltf),'error':errorlist,'pyfilename':newimagename}
                    cv2.imwrite(imgpath + newimagename,image)#儲存路徑
                else:
                    data = {'ResultId':str(te),'direction':direction,'neck_check': neck_check,'elbow': elbow,'elbow_check': elbow_check, 'waist': waist,'waist_check':waist_check, 'knee': knee,'knee_check':knee_check, 'ispeople':True,'result': str(alltf),'error':errorlist}
                break
        cap.release()
        cv2.destroyAllWindows()
    if not isData or ifFile:
        os.remove(img) 
    with open('result.json', 'w', encoding='utf-8') as f:
        json.dump(data, f)