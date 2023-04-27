let jsonData = {}; //建立物件存取JSON資料
const Account = localStorage.getItem("userId");
const Token = localStorage.getItem('userToken')
const UserName = localStorage.getItem("userName");
const UserPhoto = localStorage.getItem("userPhoto");

//#region 點選logo主頁
var logo = document.getElementById('logo');
logo.onclick = function() { window.location.replace('Index.html'); };
//#endregion

//#region 進入檢測畫面
var detection = document.getElementById('detection');
detection.onclick = function() { window.location.href = 'Detection.html'; };
//#endregion

//#region 進入系統回饋
var feedback = document.getElementById('feedback');
feedback.onclick = function() { window.location.href = 'Feedback.html' };
//#endregion

//登入判斷
if (Token != null) {
    const newBlock = document.getElementById('right_side');
    //顯示使用者資訊
    newBlock.innerHTML = `<div class="user_information" id="user_information">
        <div class="photo"><img src="${UserPhoto}" ></div>
        <div class="user_name">${UserName}</div>
    </div>
    <div class="dropdown" id="dropdown">
        <div class="other" id="other"><img src="../Image/down-arrow .png" style="width: 100%;"></div>
        <div class="dropdown-content">
        <span id="history">歷程記錄</span>
        <span id="logout">登出</span>
    </div>
    </div>`;

    var user_information = document.getElementById('user_information');
    user_information.onclick = function() { window.location.href = 'History.html' };

    // 進入歷程紀錄 
    var history_click = document.getElementById('history');
    history_click.onclick = function() { window.location.href = 'History.html' };

    //登出
    var logout = document.getElementById('logout');
    logout.onclick = function() {
        localStorage.removeItem('userName');
        localStorage.removeItem('userToken');
        localStorage.removeItem('userPhoto');
        localStorage.removeItem('userMeberId');
        window.location.href = 'index.html';
        window.setTimeout(window.alert('此帳號已登出'), 1000);
    }

    //RWD下的header
    list.innerHTML = `<li id="list_detection">坐姿檢測</li>
                    <li id="list_feedback">系統回饋</li>
                    <li id="list_members">會員資料</li>
                    <li id="list_logout">登出</li>`
    menu.onclick = function menu_onclick() {
        list.style = 'display:inline-block'
        menu.style = 'display:none'
        close.style = 'display:flex'
        close.onclick = function() {
            menu.style = 'display:flex';
            close.style = 'display:none';
            list.style = 'display:none';
        }
    }

    //#region 進入坐姿檢測

    var detection_page = document.getElementById('list_detection');
    detection_page.onclick = function() { window.location.href = 'Detection.html'; };
    //#endregion


    //#region 進入系統回饋
    var feedback_page = document.getElementById('list_feedback');
    feedback_page.onclick = function() { window.location.href = 'Feedback.html' };
    //#endregion

    //#region 進入會員資料
    var feedback_page = document.getElementById('list_members');
    feedback_page.onclick = function() { window.location.href = 'History.html' };
    //#endregion

    //登出(RWD)
    var logout_list = document.getElementById('list_logout');
    logout_list.onclick = function() {
        localStorage.removeItem('userName');
        localStorage.removeItem('userToken');
        localStorage.removeItem('userPhoto');
        localStorage.removeItem('userMeberId');
        window.setTimeout(window.alert('此帳號已登出'), 1000);
        window.location.href = 'index.html';
    }

} else {
    popup("尚未登入，請回首頁登入");
    location = './index.html';
    //檢測畫面(RWD)
    var detection_page = document.getElementById('list_detection');
    detection_page.onclick = function() { window.location.href = 'Detection.html'; };

    //回饋畫面(RWD)
    var feedback_page = document.getElementById('list_feedback');
    feedback_page.onclick = function() { window.location.href = 'Feedback.html' };

    //登入畫面(RWD)
    var login_list = document.getElementById('list_login');
    login_list.onclick = function() { login_click(); };
}

const SaveImage = (b64) => {
    const uri = 'Result/SaveImage';
    return fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8;',
                'Authorization': 'Bearer ' + Token
            },
            body: JSON.stringify({
                Image64: b64,
                Name: UserName
            })
        })
        .then((response) => {
            if (response.status != 200) {
                popup('存取照片錯誤');
                return;
            }
            return response.json();
        })
        .then((Data) => {
            filep = Data.Data;
            filenameD = filep.split('Image\\')[1];
            filepathD = filep.split('Image\\')[0] + 'Image\\';
            Correction();
            //GetScreenshotfile();
        })
        .catch(function(error){
            console.log(error);
            if(error == `TypeError: Cannot read properties of undefined (reading 'Data')`){
                popup_error('登入逾期，請重新登入');
            }
        })
};
/*
const GetScreenshotfile = () => {
    const uri = 'Result/GetScreenshotfile';
    return fetch(uri, {
        method:'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8;',
            'Authorization':'Bearer ' + Token
        },
        body:JSON.stringify({
            MembersId:Account
        })
    })
    .then((response) => {
        if(response.status != 200){
            popup('存取IP錯誤');
            return;
        }
        console.log("response:", response) 
        return response.json();
    })
    .then((Data) =>{
        console.log(Data);
        shotname = Data;
        Correction(shotname);
    })

};
*/
const Correction = () => {
    const uri = 'Result/Correction';
    return fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8;',
                'Authorization': 'Bearer ' + Token
            },
            body: JSON.stringify({
                MembersId: Account,
                IsData: ds_tf,
                FileName: filenameD,
                Filepath: filepathD
            })
        })
        .then((response) => {
            if (response.status != 200) {
                popup('檢測圖片錯誤');
                return;
            }
            console.log("response:", response)
            return response.json();
        })
        .then((Data) => {
            console.log(Data.Data);
            if (ds_tf == true) {
                tf = Data.Data.split(',')[0];
            } else {
                tf = Data.Data;
            }
            if (tf == true) {
                pose_tf = true;
            } else {
                pose_tf = false;
            }
            return Data;
        })
        .catch(function(error){
            console.log(error);
            if(error == `TypeError: Cannot read properties of undefined (reading 'Data')`){
                popup_error('登入逾期，請重新登入');
            }
        })
};

const DStart = () => {
    const uri = 'Detection/Start';
    return fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8;',
                'Authorization': 'Bearer ' + Token
            },
            body: JSON.stringify({
                MembersId: Account
            })
        })
        .then((response) => {
            return response.json();
        })
        .then((Data) => {
            if (Data.Data == undefined) {
                popup(Data.ErrorMessage);
                DStartData = Data.ErrorMessage;
                return;
            } else {
                console.log(Data.Data);
                DStartData = Data.Data;
            }
        })
        .catch(function(error){
            console.log(error);
            if(error == `TypeError: Cannot read properties of undefined (reading 'Data')`){
                popup_error('登入逾期，請重新登入');
            }
        });
};

const DEnd = () => {
    const uri = 'Detection/End';
    return fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8;',
                'Authorization': 'Bearer ' + Token
            },
            body: JSON.stringify({
                MembersId: Account
            })
        })
        .then((response) => {
            return response.json();
        })
        .then((Data) => {
            if (Data.Data == undefined) {
                popup(Data.ErrorMessage);
                DEndData = Data.ErrorMessage;
                return;
            } else {
                console.log(Data.Data);
                DEndData = Data.Data;
                Did = DEndData.split(',')[1];
            }
        })
        .catch(function(error){
            console.log(error);
            if(error == `TypeError: Cannot read properties of undefined (reading 'Data')`){
                popup_error('登入逾期，請重新登入');
            }
        })
};

window.onload = function() {
    let D_ExplainPopButton = document.getElementById('D_ExplainPopButton');
    D_ExplainPopButton.addEventListener('click', function() {
        let D_ExplainPopupBack = document.getElementById('D_ExplainPopupBack');
        D_ExplainPopupBack.style.display = 'flex';
        let Explain_ExitButton = document.getElementById('Explain_ExitButton');
        Explain_ExitButton.addEventListener('click', function() {
            D_ExplainPopupBack.style.display = 'none';
        }, false);
    }, false);
    let time_minutes = 0;
    let time_seconds = 0;
    secondsRemaining = 0;
    timer = document.querySelector('#timer');
    timer.textContent = `${paddedFormat(time_minutes)}:${paddedFormat(time_seconds)}`;;
    ds = document.getElementById("Detection_StartEndButton");
    dt = document.getElementById('Detection_text');
    ds_tf = false;
    cam_op_tf = false;
    pose_tf = false;
    init();
    ds.addEventListener('click', function() {
        if (ds_tf == false) {
            DStart();
            startCount(1, timer);
            ds.setAttribute('class', 'Detection_EndButton');
            ds.innerHTML = "停止錄製";
            dt.innerHTML = '檢測中，請保持坐姿端正';
            dt.style = 'color:black';
            ds_tf = true;
        } else if (ds_tf == true) {
            DEnd();
            clearInterval(count);
            ds.setAttribute('class', 'Detection_StartButton');
            ds.innerHTML = "開始錄製";
            timer.textContent = `${paddedFormat(0)}:${paddedFormat(0)}`;
            ds_tf = false;
            popup('檢測已終止');
        }
    }, false);
};

setInterval(function() {
    if (ds_tf == false && cam_op_tf == true) {
        st();
        if (pose_tf == false) {
            dt.innerHTML = "<img class='img' src='../Image/Detection_exclamation.png'>當前坐姿不良";
            dt.style = 'color:red';
        } else if (pose_tf == true) {
            dt.innerHTML = "<img class='img' src='../image/Detection_collect.png'>當前坐姿正確";
            dt.style = 'color: rgba( 49, 131, 207, 1);';
        }
    }
}, 3000);

const constraints = {
    audio: false,
    video: {
        facingMode: "environment"
    }
};

const dp = document.getElementById("Detection_Picture");
const init = () => {
    video = createVideo("vid", dp.clientWidth, dp.clientHeight);
    canvas = createCanvas("canvas", dp.clientWidth, dp.clientHeight);
    getCameraStream(video);
    getFrameFromVideo(video, canvas);
    //dp.appendChild(video);
    dp.appendChild(canvas);
    //console.log("init");
};

const getFrameFromVideo = (video, canvas) => {
    canvas.style.width = "48vw";
    canvas.style.height = "36vw";
    const ctx = canvas.getContext("2d"); //用canvas做2d繪製
    ctx.clearRect(0, 0, canvas.width, canvas.height); //清除之前用的區域
    ctx.save(); //儲存當前的繪製
    ctx.translate(video.width, 0); //將圖像喬到旁邊的位置
    ctx.scale(-1, 1); //再反轉回來正確的位置，這樣就轉一圈了
    ctx.drawImage(video, 0, 0, video.width, video.height); //上影片
    ctx.restore(); //讀取先前存的
    requestAnimationFrame(() => getFrameFromVideo(video, canvas)); //不斷執行
};

const getCameraStream = video => {
    navigator.mediaDevices
        .getUserMedia(constraints)
        .then(function success(stream) {
            video.srcObject = stream;
            cam_op_tf = true;
        })
        .catch(handleMediaStreamError)
};

function handleMediaStreamError(error) {
    console.log('navigator.getUserMedia error: ', error);
}

const createVideo = (id, width, height) => {
    const video = document.createElement("video");
    video.id = id;
    video.width = width;
    video.height = height;
    video.autoplay = true;
    //video.controls = true;
    return video;
};

const createCanvas = (id, width, height) => {
    const canvas = document.createElement("canvas");
    canvas.id = id;
    canvas.width = width;
    canvas.height = height;
    return canvas;
};

function handleMediaStreamError(error) {
    console.log('navigator.getUserMedia error: ', error);
}

function paddedFormat(num) {
    return num < 10 ? "0" + num : num;
}

function startCount(duration, timer) {
    let secondsRemaining = duration;
    let min = 0;
    let sec = 0;
    count = setInterval(function() {
        min = parseInt(secondsRemaining / 60);
        sec = parseInt(secondsRemaining % 60);
        secondsRemaining = secondsRemaining + 1;
        timer.textContent = `${paddedFormat(min)}:${paddedFormat(sec)}`;
        if (secondsRemaining % 5 == 1) {
            st();
        }
        if (secondsRemaining > 2400) {
            clearInterval(count);
            ds.innerHTML = "檢測結束";
            dt.innerHTML = '檢測結束';
            ds_tf = false;
            popup('已完成坐姿檢測了');
            DEnd();
        };
    }, 1000);
};

function st() {
    var canvas = document.getElementById('canvas');
    canvas.setAttribute("crossOrigin", 'Anonymous');
    var contentType = 'image/jpeg';
    var base64data = canvas.toDataURL(contentType);
    //console.log(base64data);
    let base64 = base64data.split(',')[1];
    //console.log(base64);
    SaveImage(base64);
};

function popup(Content) {
    var dialog = document.querySelector('dialog');
    dialog.innerHTML = `<div class="dialog_content"><p>${Content}</p></div><button id="btn">確認</button>`
    dialog.style = 'display:flex';
    let dwrapper = document.getElementById('dialog_wrapper');
    dwrapper.style.display = 'block';
    var btn = document.getElementById('btn');
    btn.onclick = function() {
        dwrapper.style.display = 'none';
        dialog.style = 'display:none';
        location = '../Analyze.html?DetectionId=' + Did;
    };
}

function popup_error(Content) {
    var dialog = document.querySelector('dialog');
    dialog.innerHTML = `<div class="dialog_content"><p>${Content}</p></div><button id="btn">確認</button>`
    dialog.style = 'display:flex';
    let dwrapper = document.getElementById('dialog_wrapper');
    dwrapper.style.display = 'block';
    var btn = document.getElementById('btn');
    btn.onclick = function() {
        dwrapper.style.display = 'none';
        dialog.style = 'display:none';
        localStorage.removeItem('userName');
        localStorage.removeItem('userToken');
        localStorage.removeItem('userPhoto');
        localStorage.removeItem('userMeberId');
        window.location.href = '../index.html';
    };
}