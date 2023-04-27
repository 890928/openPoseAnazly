/*if(localStorage.getItem("userToken")==null)
{
    alert("登入失效，請重新登入");
    location = './Index.html';
}*/
//驗證登入是否逾期

Feedback_Insert = document.getElementById('Feedback_Insert');
textarea = document.getElementById('FI_textarea');
button = document.getElementById('FI_Button');
Fscore = 0;
score1 = document.getElementById('FI_Star1');
score2 = document.getElementById('FI_Star2');
score3 = document.getElementById('FI_Star3');
score4 = document.getElementById('FI_Star4');
score5 = document.getElementById('FI_Star5');
const Account = localStorage.getItem("userId");
const Token = localStorage.getItem('userToken');
const UserName = localStorage.getItem("userName");
const UserPhoto = localStorage.getItem("userPhoto");



//#region 點選logo主頁
var logo = document.getElementById('logo');
logo.onclick = function() { window.location.replace('index.html'); };
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
    let strings = Token.split(".");
    var userinfo = JSON.parse(decodeURIComponent(escape(window.atob(strings[1].replace(/-/g, "+").replace(/_/g, "/")))));
    if (userinfo.role.length == 2) {
        admin = true;
    } else {
        admin = false;
    }
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

    var history_click = document.getElementById('history');
    history_click.onclick = function() {
        window.location.href = 'History.html'
    }

    //登出
    var logout = document.getElementById('logout');
    logout.onclick = function() {
        localStorage.removeItem('userName');
        localStorage.removeItem('userToken');
        localStorage.removeItem('userPhoto');
        localStorage.removeItem('userMeberId');
        window.location.href = 'index.html';
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

    //#region 進入系統回饋(RWD)
    var feedback_page = document.getElementById('list_feedback');
    feedback_page.onclick = function() { window.location.href = 'Feedback.html' };
    //#endregion

    //#region 進入會員資料(RWD)
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

const Show = () => {
    const uri = 'Feedback/GetAllSystemFeedback';
    return fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8;',
                'Authorization': 'Bearer ' + Token
            }
        })
        .then((response) => {
            return response.json();
        })
        .then((Data) => {
            if (Data.Data == undefined) {
                console.log(Data.ErrorMessage);
                ShowData = Data.ErrorMessage;
            } else {
                ShowData = Data.Data;
                ShowData.sort((a, b) => {
                    if (a.create_time < b.create_time) {
                        return 1;
                    } else {
                        return -1;
                    }
                });
                Feedback_show(ShowData, 0);
            }
        })
        .catch(function(error){
            popup_error('登入逾期，請重新登入');
        });
};
Show();

function Feedback_show(jsonData, startPrint) {
    let num = jsonData.length;
    endPrint = (startPrint + 5 <= num ? startPrint + 5 : num);
    for (i = startPrint; i < endPrint; i++) {
        let jDscore = '';
        switch (jsonData[i].score) {
            case 5:
                jDscore = './Image/Feedback_5Star.png';
                break;
            case 4:
                jDscore = './Image/Feedback_4Star.png';
                break;
            case 3:
                jDscore = './Image/Feedback_3Star.png';
                break;
            case 2:
                jDscore = './Image/Feedback_2Star.png';
                break;
            case 1:
                jDscore = './Image/Feedback_1Star.png';
                break;
        }
        let image = jsonData[i].Image
        let imagelink = './Image/' + image.split('e\\')[1];
        let time = jsonData[i].create_time
        let timelink = time.split('T')[0] + ' ' + time.split('T')[1]
        let FV_Box = document.createElement('div');
        FV_Box.setAttribute('class', 'FV_Box');
        FV_Box.setAttribute('id', `FV_Box${i}`);
        FV_Box.innerHTML = `
        <div class="Box_main" id="Box_main${i}">
            <div class='Box_AccountHidden' id='Box_AccountHidden${i}'>${jsonData[i].MembersId}</div>
                <div class="Box_Title">
                    <div class="BoxT_left">
                        <div class="BoxTl_Photo">
                            <img src='${imagelink}'>
                        </div>
                    <div class="BoxTl_Name">${jsonData[i].name}</div>
                </div>
                <div class=BoxT_right>
                    <div class="BoxTrA">
                        <div class='BoxTrA_Score'>
                            <img src='${jDscore}'>
                        </div>
                        <div class='BoxTrA_CreateTime'>
                            ${timelink}
                        </div>
                    </div>
                    <div class="BoxTr_Button" id="BoxTr_Button${i}">
                    </div>
                </div>
            </div>
            <div class="Box_Content">
                ${jsonData[i].content}
            </div>
        </div>
        <div class="Box_Reply" id="Box_Reply${i}">
            <hr class="BoxR_Line"></hr>
            <div class="BoxRB">
                <div class="BoxRB_Man" id='BoxRB_Man${i}'>管理員：</div>
                <div class="BoxRB_Text" id='BoxRB_Text${i}'>${jsonData[i].reply}</div>
                <textarea class="BoxRI_Textarea" id='BoxRI_Textarea${i}'>${jsonData[i].reply}</textarea>
                <div class="BoxRI_Button" id='BoxRI_Button${i}'>回覆</div>
            </div>
        </div>
        `
        if (jsonData[i].MembersId == Account) {
            Fscore = jsonData[i].score
            ScoreChange(Fscore);
            textarea.innerHTML = jsonData[i].content;
            button.innerHTML = '修改'
            let Insert_box = document.getElementById('Insert_box');
            Insert_box.style.display = 'none';
            Feedback_Insert.append(FV_Box);
            let BoxTr_Button = document.getElementById(`BoxTr_Button${i}`);
            BoxTr_Button.innerHTML = `<img src='./Image/Feedback_edit.png' id='Edit'>`;
            BoxTr_Button.style.display = "flex";
            let Edit_img = document.getElementById('Edit');
            Edit_img.addEventListener('click', function() {
                let Insert_FV_Box = Feedback_Insert.getElementsByClassName('FV_Box')[0];
                Insert_FV_Box.style.display = 'none';
                Insert_box.style.display = 'block';
            }, false);
        } else {
            let Feedback_View = document.getElementById('Feedback_View');
            Feedback_View.append(FV_Box);
        }
        if (jsonData[i].reply != null && jsonData[i].reply_time > jsonData[i].create_time) {
            let Box_Reply = document.getElementById(`Box_Reply${i}`);
            Box_Reply.style.display = 'flex';
        } else {
            let Box_Reply = document.getElementById(`Box_Reply${i}`);
            Box_Reply.style.display = 'none';
        }
        if (admin == true) {
            Feedback_Insert.style.display = 'none';
            let BoxTr_Button = document.getElementById(`BoxTr_Button${i}`);
            BoxTr_Button.innerHTML = `<img src='./Image/Feedback_reply.png' id='ReplyImg_${i}'>`;
            BoxTr_Button.style.display = "flex";
        }
    }
    if (num - endPrint > 0) {
        let showmore = document.createElement('div');
        showmore.setAttribute('id', 'showmore');
        showmore.innerHTML = '查看更多評價與回饋';
        Feedback_View.append(showmore);
        showmore.addEventListener('click', function() {
            Feedback_View.removeChild(showmore);
            Feedback_show(ShowData, endPrint);
        }, false);
    }
    document.addEventListener('click', function(e) {
        eid = e.target.id;
        console.log(eid);
        if (eid.split('_')[0] == 'ReplyImg') {
            en = eid.split('_')[1];
            let Box_Reply = document.getElementById(`Box_Reply${en}`);
            Box_Reply.style.display = 'flex';
            let BoxRB_Man = document.getElementById(`BoxRB_Man${en}`);
            BoxRB_Man.innerHTML = '新回覆';
            let BoxRB_Text = document.getElementById(`BoxRB_Text${en}`);
            BoxRB_Text.style.display = 'none';
            let BoxRI_Textarea = document.getElementById(`BoxRI_Textarea${en}`);
            BoxRI_Textarea.style.display = 'block';
            BoxRI_Textarea.innerHTML = '';
            let BoxRI_Button = document.getElementById(`BoxRI_Button${en}`);
            BoxRI_Button.style.display = 'flex';
            BoxRI_Button.addEventListener('click', function() {
                let Box_AccountHidden = document.getElementById(`Box_AccountHidden${en}`)
                console.log(Box_AccountHidden.innerHTML)
                Reply(Box_AccountHidden.innerHTML, BoxRI_Textarea.value);
            }, false);
        }
    }, false);

    score1.addEventListener('click', function() {
        Fscore = 1;
        ScoreChange(Fscore);
    }, false);
    score2.addEventListener('click', function() {
        Fscore = 2;
        ScoreChange(Fscore);
    }, false);
    score3.addEventListener('click', function() {
        Fscore = 3;
        ScoreChange(Fscore);
    }, false);
    score4.addEventListener('click', function() {
        Fscore = 4;
        ScoreChange(Fscore);
    }, false);
    score5.addEventListener('click', function() {
        Fscore = 5;
        ScoreChange(Fscore);
    }, false);
    if (button.innerHTML != '修改') {
        textarea.addEventListener('focus', function() {
            textarea.innerHTML = "";
        }, false);
        textarea.addEventListener('blur', function() {
            if (textarea.innerHTML == "") {
                textarea.innerHTML = "留下您的使用回饋或問題(非必填)";
            }
        }, false);
    }
    button.addEventListener('click', function() {
        var FI_EM = document.getElementById("FI_ErrorMessage");
        if (Fscore == 0) {
            FI_EM.innerHTML = '再送出之前請先做評分';
            FI_EM.setAttribute("class", "FI_ErrorMessage");
        } else {
            FI_EM.innerHTML = '';
            FI_EM.setAttribute("class", "");
            var textareaValue = textarea.value;
            if (textareaValue == "留下您的使用回饋或問題(非必填)") {
                textareaValue = "";
            }
            console.log(textareaValue);
            Insert(textareaValue, Fscore);
        }
    }, false);
}



function Insert(textareaValue, Insertscore) {
    const url_Create = 'Feedback/CreateSystemFeedback';
    fetch(url_Create, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + Token
            },
            method: 'POST',
            body: JSON.stringify({
                score: Insertscore,
                content: textareaValue
            })
        })
        .then(response => {
            return response.json()
        })
        .catch(function(error){
            popup_error('登入逾期，請重新登入');
        })
    popup('感謝你的回饋');
};

function Reply(Id, Replytext) {
    const url_Reply = 'Feedback/ReplySystemFeedback';
    fetch(url_Reply, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + Token
            },
            method: 'POST',
            body: JSON.stringify({
                MembersId: Id,
                reply: Replytext
            })
        })
        .then(response => {
            return response.json()
        })
    popup('回覆成功');
};


function ScoreChange(Cscore) {
    if (Cscore >= 1) { score1.src = "./Image/Feedback_BlueStar.png"; }
    if (Cscore >= 2) { score2.src = "./Image/Feedback_BlueStar.png"; }
    if (Cscore >= 3) { score3.src = "./Image/Feedback_BlueStar.png"; }
    if (Cscore >= 4) { score4.src = "./Image/Feedback_BlueStar.png"; }
    if (Cscore >= 5) { score5.src = "./Image/Feedback_BlueStar.png"; }
    if (Cscore < 2) { score2.src = "./Image/Feedback_Star.png"; }
    if (Cscore < 3) { score3.src = "./Image/Feedback_Star.png"; }
    if (Cscore < 4) { score4.src = "./Image/Feedback_Star.png"; }
    if (Cscore < 5) { score5.src = "./Image/Feedback_Star.png"; }
}

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
        location.reload();
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