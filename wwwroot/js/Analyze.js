//#region 點選logo主頁
var logo = document.getElementById('logo');
logo.onclick = function () { window.location.replace('Index.html'); };
//#endregion
if (window.screen.width <= 650) {
    var can = document.getElementById("chart");
    can.innerHTML = `<canvas id="chartRadar" width="400" height="380"></canvas>`;
} else {
    var can = document.getElementById("chart");
    can.innerHTML = `<canvas id="chartRadar" width="200" height="100"></canvas>`;
}
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

   //#region 顯示登入、註冊視窗
   var wrapper = document.getElementById('wrapper');
   var login = document.getElementById('login');
   login.onclick = function() { login_click(); };

   var menu = document.getElementById('menu');
   var list = document.getElementById('list');
   var close = document.getElementById('close');
   //登入判斷
   if (localStorage.getItem("userToken") != null) {
    const user_name = localStorage.getItem("userName");
    const user_img = localStorage.getItem("userPhoto");
    const newBlock = document.getElementById('right_side');
    //顯示使用者資訊
    newBlock.innerHTML = `<div class="user_information" id="user_information">
         <div class="photo"><img src="${user_img}" ></div>
         <div class="user_name">${user_name}</div>
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

    //進入歷程紀錄
    var history_click = document.getElementById('history');
    history_click.onclick = function() { window.location.href = 'History.html' };

    //登出
    var logout = document.getElementById('logout');
    logout.onclick = function() {
        localStorage.removeItem('userName');
        localStorage.removeItem('userToken');
        localStorage.removeItem('userPhoto');
        localStorage.removeItem('userId');
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


//#region 進入系統回饋
var feedback_page = document.getElementById('list_feedback');
feedback_page.onclick = function () { window.location.href = 'Feedback.html' };
//#endregion

//#region 進入會員資料
var feedback_page = document.getElementById('list_members');
feedback_page.onclick = function () { window.location.href = 'History.html' };
//#endregion

//登出(RWD)
var logout_list = document.getElementById('list_logout');
logout_list.onclick = function () {
    localStorage.removeItem('userName');
    localStorage.removeItem('userToken');
    localStorage.removeItem('userPhoto');
    localStorage.removeItem('userId');
    window.location.href = 'index.html';
}

} else {
    alert("登入失效，請重新登入");
    location = './Index.html';
    var register = document.getElementById('register');
    var register_list = document.getElementById('list_register');
    register_list.onclick = function () { register_click(); };
    register.onclick = function () { register_click(); };

    var loginpage = document.getElementById('popup_login');
    var registerpage = document.getElementById('popup_register');
    var forget_page = document.getElementById('popup_forget');

    menu.onclick = function menu_onclick() {
        list.style = 'display:inline-block'
        menu.style = 'display:none'
        close.style = 'display:flex'
        close.onclick = function () {
            menu.style = 'display:flex';
            close.style = 'display:none';
            list.style = 'display:none';
        }
    }

    //檢測畫面(RWD)
    var detection_page = document.getElementById('list_detection');
    detection_page.onclick = function () { window.location.href = 'Detection.html'; };

    //回饋畫面(RWD)
    var feedback_page = document.getElementById('list_feedback');
    feedback_page.onclick = function () { window.location.href = 'Feedback.html' };

    //登入畫面(RWD)
    var login_list = document.getElementById('list_login');
    login_list.onclick = function () { login_click(); };
}
//取得使用者Name
const UserName = localStorage.getItem("userName");
let UserPhoto = localStorage.getItem("userPhoto");
const UserId = localStorage.getItem("userId");
const UserToken = localStorage.getItem("userToken");

//取得網址DetectionId
var url = location.href;
if (url.indexOf('?') != -1) {
    var DetectionId = ''
    //在此直接將各自的參數資料切割放進ary中
    var ary1 = url.split('?');
    var ary2 = ary1[1].split('=');
    DetectionId = ary2[1];
}
//取得MemberId
const UserMemberId = localStorage.getItem("UserId");
//跳轉歷程記錄頁面
var GoHistory = document.getElementById('GoHistory');
GoHistory.onclick = function () {
    location = './History.html';
};
//雷達圖
function makechart(jsonData) {
    var neck = 0;
    var elbow = 0;
    var waist = 0;
    var knee = 0;
    var result = 0;
    var total = 0;
    //建立暫存陣列
    const data = [];
    jsonData.forEach((item, index) => {
        index + 1;
        data.push(item);
    })
    data.forEach((item) => {
        total++;
        if (item.neck_check == true) {
            neck++;
        }
        if (item.elbow_check == true) {
            elbow++;
        }
        if (item.waist_check == true) {
            waist++;
        }
        if (item.knee_check == true) {
            knee++;
        }
        if (item.result == true) {
            result++;
        }
    });
    if (total == 0) {
        total = 5;
    }
    //定義變數
    var chartRadarDOM;
    var chartRadarData;
    var chartRadarOptions;

    //載入雷達圖
    Chart.defaults.global.legend.display = false;
    Chart.defaults.global.defaultFontColor = 'rgba(0,0,74, 1)';
    chartRadarDOM = document.getElementById("chartRadar");
    chartRadarData;
    chartRadarOptions = {
        scale: {
            ticks: {
                fontSize: 16,
                beginAtZero: true,
                maxTicksLimit: 7,
                min: 0,
                max: total
            },
            pointLabels: {
                fontSize: 20,
                color: '#0044BB'
            },
            gridLines: {
                color: '#009FCC'
            }
        }
    };

    console.log("---------Rader Data--------");
    var graphData = new Array();
    graphData.push(result);
    graphData.push(elbow);
    graphData.push(knee);
    graphData.push(neck);
    graphData.push(waist);


    console.log("--------Rader Create-------------");
    console.log(graphData);

    //CreateData
    chartRadarData = {
        labels: ['總體', '手肘', '膝蓋', '脖子', '腰部'],
        datasets: [{
            label: "正確次數",
            backgroundColor: "rgba(17, 34, 51,0.8)",
            borderColor: "rgba(63,63,74,.8)",
            pointBackgroundColor: "rgba(63,63,74,1)",
            pointBorderColor: "rgba(0,0,0,0)",
            pointHoverBackgroundColor: "#fff",
            pointHoverBorderColor: "rgba(0,0,0,0.3)",
            pointBorderWidth: 5,
            data: graphData
        }]
    };

    //Draw
    var chartRadar = new Chart(chartRadarDOM, {
        type: 'radar',
        data: chartRadarData,
        options: chartRadarOptions
    });

}

//點擊查看圖片
var picture_button = document.getElementById("gopicture");
picture_button.onclick = function () {
    var analyze_chart_disease = document.getElementById("analyze_chart_disease");
    analyze_chart_disease.style.display = "none";
    var analyze_UserPicture = document.getElementById("analyze_UserPicture");
    analyze_UserPicture.style.display = "flex";
};
//點擊返回分析
var picture_button = document.getElementById("goAnalyze");
picture_button.onclick = function () {
    var analyze_chart_disease = document.getElementById("analyze_chart_disease");
    analyze_chart_disease.style.display = "flex"
    var analyze_UserPicture = document.getElementById("analyze_UserPicture");
    analyze_UserPicture.style.display = "none";
};
//接照片API
const jsonUrl = 'Detection/GetDetectionDetail';

let jsonData = {}; //建立物件存取JSON資料

fetch(jsonUrl, {
    method: 'POST',
    body: JSON.stringify({ "MembersId": UserId, "DetectionId": DetectionId }),
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json; charset=utf-8',
        'Authorization': 'Bearer ' + UserToken
    }
})
    .then((response) => {
        return response.json();
    }).then((data) => {
        if (data.Status == 200) {
            jsonData = data.Data;
            console.log(jsonData);
            makechart(jsonData);
            GetSelectTime(jsonData);
            GetImage(jsonData);
        } else {
            Logout();
        }

    }).catch(function(error){
        var PasswordMessageText = document.getElementById("Message_Title");
        PasswordMessageText.innerHTML ="登入逾時，請重新登入";
        OutMessage();
        localStorage.removeItem('userName');
        localStorage.removeItem('userToken');
        localStorage.removeItem('userPhoto');
        localStorage.removeItem('userMeberId');
        var Message_true = document.getElementById("Message_true");
        Message_true.onclick = function() {
            var popup_window = document.getElementById("Message-container");
            popup_window.style.display = "none";
            window.location.href = './index.html';
        };
    });
//登入逾期視窗
function OutMessage() {
    var popup_window = document.getElementById("Message-container");

    popup_window.style.display = "block";

    var Message_true = document.getElementById("Message_true");
    Message_true.onclick = function() {
        var popup_window = document.getElementById("Message-container");
        popup_window.style.display = "none";
    };
}
//前往評分說明
var goillustrate = document.getElementById("goillustrate");
goillustrate.onclick = function () {
    window.open('https://webapp.cgmh.org.tw/article/document/art_atch/00346-20200617-081351.pdf');
};
//取得下拉式選單資料
function GetSelectTime(jsonData) {

    //建立暫存陣列
    const data = [];
    jsonData.forEach((item, index) => {
        index + 1;
        data.push(item);
    })
    let str = `                    
		<option style="color: #b6b6b6" disabled selected>請選擇檢測時間</option>
    `;
    data.forEach((item) => {
        var Date = item.time.replace('T', ' '); //去掉ISO時間格式英文
        str += `
	  	<option value=${item.time}>${Date}</option>
      `;
    });
    var selectdatetime = document.getElementById("selectdatetime")
    selectdatetime.innerHTML = str;
}


//取得照片路徑
function GetImage(jsonData) {

    //建立暫存陣列
    const data = [];
    jsonData.forEach((item, index) => {
        var num = index + 1;
        if (num == 1) {
            data.push(item);
        }
    })

    let str = ``;
    let error = ``;
    var ErrorMessage = document.getElementById("ErrorMessage")
    ErrorMessage.style.backgroundColor = "white";
    ErrorMessage.innerHTML = "";
    data.forEach((item) => {
        str += `
            <img src="${item.imagefile}">
        `;
        if (item.result) {
            ErrorMessage.style.backgroundColor = "green";
            ErrorMessage.innerHTML = "姿勢正確";
        }
        else {
            ErrorMessage.style.backgroundColor = "red";
            ErrorMessage.innerHTML = item.error;
        }

    });

    var PictureImg = document.getElementById("Picture")
    PictureImg.innerHTML = str;
}

//下拉式選單被選擇時
function selectchange() {
    var selected = document.getElementById("selectdatetime").value;
    let selectjsonData = {};
    selectjsonData = jsonData;
    jsonData = jsonData.filter(function (item) {
        return item.time == selected;
    }); //篩選結果
    GetImage(jsonData);
    jsonData = selectjsonData;
}
//Token逾期登出
function TokenLogout() {
    alert("登入逾期，請重新登入");
    localStorage.removeItem('userName');
    localStorage.removeItem('userToken');
    localStorage.removeItem('userPhoto');
    localStorage.removeItem('userMeberId');
    window.location.href = './index.html';
}
//部位篩選
function selectbody() {
    let selectjsonData = {};
    selectjsonData = jsonData;
    var w = document.getElementById("w").innerHTML;
    var e = document.getElementById("e").innerHTML;
    var n = document.getElementById("n").innerHTML;
    var k = document.getElementById("k").innerHTML;
    if (w != "") {
        jsonData = jsonData.filter(function (item) {
            return item.waist_check == false;
        }); //篩選結果
    }
    if (e != "") {
        jsonData = jsonData.filter(function (item) {
            return item.elbow_check == false;
        }); //篩選結果
    }
    if (n != "") {
        jsonData = jsonData.filter(function (item) {
            return item.neck_check == false;
        }); //篩選結果
    }
    if (k != "") {
        jsonData = jsonData.filter(function (item) {
            return item.knee_check == false;
        }); //篩選結果
    }
    GetSelectTime(jsonData);
    GetImage(jsonData);
    jsonData = selectjsonData;
}
//隱藏div
function checkboxwaist() {
    var w = document.getElementById("w");
    if (w.innerHTML == "") {
        w.innerHTML = "1"
    } else {
        w.innerHTML = ""
    }
    selectbody();
}

function checkboxelbow() {
    var e = document.getElementById("e");
    if (e.innerHTML == "") {
        e.innerHTML = "1"
    } else {
        e.innerHTML = ""
    }
    selectbody();
}

function checkboxneck() {
    var n = document.getElementById("n");
    if (n.innerHTML == "") {
        n.innerHTML = "1"
    } else {
        n.innerHTML = ""
    }
    selectbody();
}

function checkboxknee() {
    var k = document.getElementById("k");
    if (k.innerHTML == "") {
        k.innerHTML = "1"
    } else {
        k.innerHTML = ""
    }
    selectbody();
}