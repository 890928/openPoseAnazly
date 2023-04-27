   //取得使用者Name
   const UserName = localStorage.getItem("userName");
   let UserPhoto = localStorage.getItem("userPhoto");
   const UserId = localStorage.getItem("userId");
   const UserToken = localStorage.getItem("userToken");
   //偵測大小
   if (window.screen.width <= 550) {
        var can = document.getElementById("chart_button");
        can.innerHTML = `歷程分析`;
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
               localStorage.removeItem('userId');
               window.location.href = 'index.html';
           }
           //取的會員資料
       var MemberImage = document.getElementById('MemberImage');
       var Member_Name = document.getElementById('Member_Name');
       var Member_Account = document.getElementById('Member_Account');
       console.log(UserPhoto);
       MemberImage.innerHTML = `
            <div id="MemberImgbox">
            <div id="MemberImgbox_img">
                <img src='${UserPhoto}' id="UserPicture">
            </div>
            </div>
            <div id="Imgicon">
                <img src="./Image/history_camera.png">
            </div>
        `;
       Member_Name.innerHTML = '會員名稱:' + UserName;
       Member_Account.innerHTML = '會員帳號:' + UserId;

   } else {
       alert("登入失效，請重新登入");
       location = './Index.html';
       var register = document.getElementById('register');
       var register_list = document.getElementById('list_register');
       register_list.onclick = function() { register_click(); };
       register.onclick = function() { register_click(); };

       var loginpage = document.getElementById('popup_login');
       var registerpage = document.getElementById('popup_register');
       var forget_page = document.getElementById('popup_forget');

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
   //取得當前時間
   var day1 = new Date();
   day1.setTime(day1.getTime() - 72 * 60 * 60 * 1000);
   var s1 = day1.getFullYear() + "-" + (day1.getMonth() + 1) + "-" + day1.getDate();
   beforeDay = s1.split("-")
   if (beforeDay[1].length == 1) {
       beforeDay[1] = "0" + beforeDay[1];
   }
   if (beforeDay[2].length == 1) {
       beforeDay[2] = "0" + beforeDay[2];
   }
   s1 = beforeDay[0] + "-" + beforeDay[1] + "-" + beforeDay[2];

   var day2 = new Date();
   day2.setTime(day2.getTime() + 24 * 60 * 60 * 1000);
   var s2 = day2.getFullYear() + "-" + (day2.getMonth() + 1) + "-" + day2.getDate();
   beforeDay1 = s2.split("-")
   if (beforeDay1[1].length == 1) {
       beforeDay1[1] = "0" + beforeDay1[1];
   }
   if (beforeDay1[2].length == 1) {
       beforeDay1[2] = "0" + beforeDay1[2];
   }
   s2 = beforeDay1[0] + "-" + beforeDay1[1] + "-" + beforeDay1[2];

   document.getElementById("date_start").value = s1;
   document.getElementById("date_end").value = s2;


   //驗證使用者是否登入

   //更換大頭貼
   var UserPicture = document.getElementById('MemberImage');
   var MemberImgbox = document.getElementById('MemberImgbox');
   var Userfile = document.getElementById('Userfile');
   UserPicture.onclick = function() {
       Userfile.click();
   }
   const myFile = document.querySelector('#Userfile')

   myFile.addEventListener('change', function(e) {
       const file = e.target.files[0]
       const Url = URL.createObjectURL(file);
       getUrlBase64(Url).then(base64 => {
           ChageFileApi(base64);
       }).catch((err) => {});
   })

   //轉Base64
   function getUrlBase64(url) {
       return new Promise((resolve, reject) => {
           let canvas = document.createElement("canvas");
           const ctx = canvas.getContext("2d");
           let img = new Image();
           img.crossOrigin = "Anonymous"; //开启img的“跨域”模式
           img.src = url;
           img.onload = function() {
               canvas.height = img.height;
               canvas.width = img.width;
               ctx.drawImage(img, 0, 0, img.width, img.height); //参数可自定义
               const dataURL = canvas.toDataURL("image/jpeg", 0.5); //获取Base64编码
               resolve(dataURL);
               canvas = null; //清除canvas元素
               img = null; //清除img元素
           };
           img.onerror = function() {
               reject(new Error("Could not load image at " + url));
           };
       });
   }
   //傳大頭貼API
   function ChageFileApi(FileBase64) {
       const ChangFileUrl = 'Members/Changephoto';
       let changfile = {};

       fetch(ChangFileUrl, {
               method: 'POST',
               body: JSON.stringify({
                   "Headshot_Base64": FileBase64
               }),
               headers: {
                   'Accept': 'application/json',
                   'Content-Type': 'application/json; charset=utf-8',
                   'Authorization': 'Bearer ' + UserToken
               }
           })
           .then((response) => {
               return response.json();
           }).then((data) => {
               changfile = data;
               if (data.Status == 200) {
                   localStorage.removeItem("userPhoto");
                   localStorage.setItem("userPhoto", changfile.ImageUrl);
                   var MessageText = document.getElementById("Message_Title");
                   MessageText.innerHTML = changfile.Data;
                   PaswordMessage();
                   Message_true.onclick = function() {
                       var popup_window = document.getElementById("Message-container");
                       popup_window.style.display = "none";
                       window.location.href = window.location.href;
                   };
               } else {
                   TokenLogout();
               }

           }).catch(function(error){
            var PasswordMessageText = document.getElementById("Message_Title");
            PasswordMessageText.innerHTML ="登入逾時，請重新登入";
            PaswordMessage();
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

   }




   //完成後的API資料
   const jsonUrl = 'Detection/GetAllData';

   let jsonData = {}; //建立物件存取JSON資料

   fetch(jsonUrl, {
           method: 'POST',
           body: JSON.stringify({ "MemberId": UserId }),
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
               if (jsonData != null) {
                startSelect()
               }
           } else {
               TokenLogout();
           }

       })
       .catch(function(error){
        var PasswordMessageText = document.getElementById("Message_Title");
        PasswordMessageText.innerHTML ="登入逾時，請重新登入";
        PaswordMessage();
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


   function GetPagelength(data) {

       var pageNum = 10; //每頁資料數量
       var datalength = data.length; //資料筆數
       var MaxPage = Math.ceil(datalength / pageNum); //最大頁數
       console.log("全部資料:" + datalength + "筆 每頁資料:" + pageNum + "筆 總頁數:" + MaxPage);
       var NowPage = 1;
       if (NowPage > MaxPage) {
           NowPage = MaxPage;
       }
       var Delete = document.getElementById("page");
       Delete.innerHTML = "";
       //將本來隱藏最大頁數清除
       var Delete = document.getElementById("MaxPageHidden");
       Delete.innerHTML = "";
       //紀錄最大頁數將它設為隱藏 
       var HiddenPagetext = document.createTextNode(MaxPage);
       document.getElementById("MaxPageHidden").appendChild(HiddenPagetext);
       GetPage(NowPage, data);
       let Alldata = [];
        //圖表資料
        let ChartData=[]
        let ChartTime=[]
       jsonData.forEach((item) => {
            Alldata.push(item)
       })
       Alldata.forEach((item) => {
            var startDate_sp = item.start_time.split('T'); //去掉ISO時間格式英文
            splitstartDate = startDate_sp[1].split(":");
            startDate = startDate_sp[0] + " " + splitstartDate[0] + ":" + splitstartDate[1];
            ChartTime.push(startDate);
            ChartData.push(parseInt(item.np));
       })
      makechart(ChartData,ChartTime)
   }
   //印出頁數
   function GetPage(NowPage, jsondata) {

       //將本來的表單清除
       var Delete = document.getElementById("table_content");
       Delete.innerHTML = "";
       //將本來頁數清除
       var Delete = document.getElementById("page");
       Delete.innerHTML = "";
       //將本來隱藏當前頁數清除
       var Delete = document.getElementById("NowPageHidden");
       Delete.innerHTML = "";
       //紀錄當前頁數將它設為隱藏 
       var HiddenPagetext = document.createTextNode(NowPage);
       document.getElementById("NowPageHidden").appendChild(HiddenPagetext);
       var MaxPage = document.getElementById("MaxPageHidden").innerHTML;
       //目前頁數要顯示資料的索引
       var pageNum = 10; //每頁資料數量
       var minPageData = (NowPage * pageNum) - pageNum + 1; //當前頁數最小資料
       var MaxPageData = (NowPage * pageNum); //當前頁數最大資料
       //確認開始時間排序
       var StartTimeHidden = document.getElementById("StartTimeHidden");
       let StartSort = ``;
       if (StartTimeHidden.innerHTML == "0") {
           StartSort = `<div id="StartDateSort_Down" onclick="StartDateSort()"><img src="./Image/TableSortDown.png"></div>`
       } else if (StartTimeHidden.innerHTML == "1") {
           StartSort = `<div id="StartDateSort_Up" onclick="StartDateSort()"><img src="./Image/TableSortUp.png"></div>`
       }
       //確認結束時間排序
       var EndTimeHidden = document.getElementById("EndTimeHidden");
       let EndSort = ``;
       if (EndTimeHidden.innerHTML == "0") {
           EndSort = `<div id="EndDateSort_Down" onclick="EndDateSort()"><img src="./Image/TableSortDown.png"></div>`
       } else if (EndTimeHidden.innerHTML == "1") {
           EndSort = `<div id="EndDateSort_Up" onclick="EndDateSort()"><img src="./Image/TableSortUp.png"></div>`
       }
       //確認良率排序
       var BadSortHidden = document.getElementById("BadSortHidden");
       let Badsort = ``;
       if (BadSortHidden.innerHTML == "0") {
           Badsort = `<div id="BadSort_Down" onclick="BadSort()"><img src="./Image/TableSortDown.png"></div>`
       } else if (BadSortHidden.innerHTML == "1") {
           Badsort = `<div id="BadSort_Up" onclick="BadSort()"><img src="./Image/TableSortUp.png"></div>`
       }
       //建立暫存陣列
       const data = [];
       jsonData.forEach((item, index) => {

               // 獲取陣列索引，但因為索引是從 0 開始所以要 +1。
               const num = index + 1;

               // 當 num 比 minData 大且又小於 maxData 就push進去新陣列。
               if (num >= minPageData && num <= MaxPageData) {
                   data.push(item);
               }
           })
           //取得當前解析度
       if (window.screen.width < 500) {
           var StartDateTitle = "開始";
           var EndDataTitle = "結束";
       } else {
           var StartDateTitle = "開始時間";
           var EndDataTitle = "結束時間";
       }
       //頁數內容
       let str = `                    
        <thead>
            <th>${StartDateTitle}${StartSort}</th>
            <th>${EndDataTitle}${EndSort}</th>
            <th>良率${Badsort}</th>
            <th>坐姿結果</th>
            <th>分析結果</th>
        </thead>
    `;
       data.forEach((item) => {

           var startDate_sp = item.start_time.split('T'); //去掉ISO時間格式英文
           var endDate_sp = item.end_time.split('T'); //去掉ISO時間格式英文
           var np = item.np + "%";
           splitstartDate = startDate_sp[1].split(":");
           startDate = startDate_sp[0] + " " + splitstartDate[0] + ":" + splitstartDate[1];
           splitendtDate = endDate_sp[1].split(":");
           endDate = endDate_sp[0] + " " + splitendtDate[0] + ":" + splitstartDate[1];
           str += `
            <tr>
                <td class="table_time">${startDate}</td>
                <td class="table_time">${endDate}</td>
                <td>${np}</td>
                <td>${item.np_result}</td>
                <td><button onclick="location.href='./Analyze.html?DetectionId=${item.DetectionId}'">分析結果</button></td>
            </tr>
      `;
       });

       var table_content = document.getElementById("table_content")
       table_content.innerHTML = str;
       //顯示頁數在頁面上
       for (var i = NowPage - 3; i <= NowPage + 3; i++) {
           if (i > 0 && i <= MaxPage) {
               if (i == NowPage) {
                   var new_Page = document.createElement("div");
                   new_Page.className = "NowPage";
                   var Pagetext = document.createTextNode(i);
                   new_Page.appendChild(Pagetext);
                   document.getElementById("page").appendChild(new_Page);
                   var new_Page = document.createElement("div");
               } else {
                   var new_Page = document.createElement("div");
                   new_Page.onclick = function() {
                       this.NowPage = this.innerHTML;
                       GetPage(parseInt(this.innerHTML), jsondata);
                   };
                   new_Page.className = "OtherPage";
                   var Pagetext = document.createTextNode(i);
                   new_Page.appendChild(Pagetext);
                   document.getElementById("page").appendChild(new_Page);
               }
           }
       }
       //設定上一頁
       if (NowPage > 1) {
           var NextPageL = document.getElementById("pageImgL_img");
           NextPageL.style.display = "flex";
           NextPageL.onclick = function() {
               GetPage(parseInt(NowPage) - 1, jsondata);
           };
       } else {
           var NextPageL = document.getElementById("pageImgL_img");
           NextPageL.style.display = "none";
       }
       //設定下一頁
       if (NowPage < MaxPage) {
           var NextPageR = document.getElementById("pageImgR_img");
           NextPageR.style.display = "flex";
           NextPageR.onclick = function() {
               GetPage(parseInt(NowPage) + 1, jsondata);
           };
       } else {
           var NextPageR = document.getElementById("pageImgR_img");
           NextPageR.style.display = "none";
       }
   }


   //時間資料排序
   var StartTimeHidden = document.getElementById("StartTimeHidden");
   StartTimeHidden.innerHTML = "0";

   function StartDateSort() {
       var StartTimeHidden = document.getElementById("StartTimeHidden");
       if (StartTimeHidden.innerHTML == "0") {
           jsonData.sort(function(a, b) {
               return b.start_time > a.start_time ? 1 : -1
           })
           StartTimeHidden.innerHTML = "1";
       } else {
           jsonData.sort(function(a, b) {
               return b.start_time < a.start_time ? 1 : -1
           })
           StartTimeHidden.innerHTML = "0";
       }
       GetPagelength(jsonData);
   }
   var EndTimeHidden = document.getElementById("EndTimeHidden");
   EndTimeHidden.innerHTML = "0";

   function EndDateSort() {
       var EndTimeHidden = document.getElementById("EndTimeHidden");
       if (EndTimeHidden.innerHTML == "0") {
           jsonData.sort(function(a, b) {
               return b.end_time > a.end_time ? 1 : -1
           })
           EndTimeHidden.innerHTML = "1";
       } else {
           jsonData.sort(function(a, b) {
               return b.end_time < a.end_time ? 1 : -1
           })
           EndTimeHidden.innerHTML = "0";
       }
       GetPagelength(jsonData);
   }

   //不良率資料排序
   var SortHidden = document.getElementById("BadSortHidden");
   SortHidden.innerHTML = "0";

   function BadSort() {
       var SortHidden = document.getElementById("BadSortHidden");
       var Down = document.getElementById("BadSort_Down");
       var Up = document.getElementById("BadSort_Up");

       if (SortHidden.innerHTML == "0") {
           jsonData.sort((a, b) => {
               return b.np - a.np;
           });
           SortHidden.innerHTML = "1";
       } else if (SortHidden.innerHTML == "1") {
           jsonData.sort((a, b) => {
               return a.np - b.np;
           });
           SortHidden.innerHTML = "0";
       }
       GetPagelength(jsonData);

   }
   //接收篩選資料
   var select = document.getElementById("search_select")
   select.addEventListener('click', function PoseResultselect() {
           var date_start = document.querySelectorAll('input[type="date"]')[0].value;
           var date_end = document.querySelectorAll('input[type="date"]')[1].value;
           var time_start = document.querySelectorAll('input[type="time"]')[0].value;
           var time_end = document.querySelectorAll('input[type="time"]')[1].value;
           var datetime_start = date_start + "T" + time_start;
           var datetime_end = date_end + "T" + time_end;
           if (date_start != "" && date_end == "") {
               alert("請輸入結束時間");
           } else if (date_end != "" && date_start == "") {
               alert("請輸入開始時間");
           } else if (date_start > date_end) {
               alert("開始日期需要小於結束日期");
           } else if (date_start > date_end && time_start > time_end) {
               alert("開始時間需要小於結束時間")
           } else if (date_start != "" && date_end != "" && time_start != "" && time_end != "") {
               let selectjsonData = {};
               selectjsonData = jsonData;
               jsonData = Object.values(jsonData).filter(function(item) {
                   return item.start_time >= datetime_start && item.end_time <= datetime_end;
               });
               GetPagelength(jsonData);
               jsonData = selectjsonData;
           } else if (date_start != "" && date_end != "") {
               let selectjsonData = {};
               selectjsonData = jsonData;
               jsonData = jsonData.filter(function(item) {
                   return item.start_time >= datetime_start && item.end_time <= datetime_end;
               });
               GetPagelength(jsonData);
               jsonData = selectjsonData;
           } else {
               alert("請輸入篩選條件");
           }
       }, true)
    
    //放入檢測ID到網址
   function GetDetectionId(Id) {
       window.open('./Analyze.html?DetectionId=' + Id);
   }

   //修改密碼彈跳視窗
   function customizeWindowEvent() {
       var popup_window = document.getElementById("window-container");

       var oldpassword = document.getElementById("old").value = "";
       var newpassword = document.getElementById("new").value = "";
       var checknewpassword = document.getElementById("checknew").value = "";

       popup_window.style.display = "block";

       window.onclick = function close(e) {
           if (e.target == popup_window) {
               popup_window.style.display = "none";
           }
       }
   }

   function PaswordMessage() {
       var popup_window = document.getElementById("Message-container");

       popup_window.style.display = "block";

       var Message_true = document.getElementById("Message_true");
       Message_true.onclick = function() {
           var popup_window = document.getElementById("Message-container");
           popup_window.style.display = "none";
       };
   }
   //登出
   function Logout() {
       localStorage.removeItem('userName');
       localStorage.removeItem('userToken');
       localStorage.removeItem('userPhoto');
       localStorage.removeItem('userId');
       window.location.href = './index.html';
   }
   //按下修改密碼鍵
   function changepassword() {
       var oldpassword = document.getElementById("old").value;
       var newpassword = document.getElementById("new").value;
       var checknewpassword = document.getElementById("checknew").value;
       var str = "";
       if (oldpassword == "") {
           str = `請輸入舊密碼<br>`;
       }
       if (newpassword == "") {
           str += `請輸入新密碼<br>`;
       }
       if (checknewpassword == "") {
           str += `請輸入確認新密碼`;
       }
       if (str == "") {
           //完成後的API資料
           const passwordUrl = 'Members/ChangePassword';

           var PasswordData = {}; //建立物件存取JSON資料


           fetch(passwordUrl, {
                   method: 'POST',
                   body: JSON.stringify({
                       "Password": oldpassword,
                       "Newpassword": newpassword,
                       "NewpasswordCheck": checknewpassword
                   }),
                   headers: {
                       'Accept': 'application/json',
                       'Content-Type': 'application/json; charset=utf-8',
                       'Authorization': 'Bearer ' + UserToken
                   }
               })
               .then((response) => {
                   return response.json();
               }).then((data) => {
                   PasswordData = data;
                   // if(PasswordData!="200")
                   // {
                   //     TokenLogout();
                   // }
                   if (data.Status == 200) {
                       var PasswordMessageText = document.getElementById("Message_Title");
                       PasswordMessageText.innerHTML = PasswordData.Data + "，請重新登入";
                       PaswordMessage();
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
                   } else if (data.Status == 404) {
                       var PasswordMessageText = document.getElementById("Message_Title");
                       PasswordMessageText.innerHTML = PasswordData.ErrorMessage;
                       PaswordMessage();
                   } else if (data.status == 400) {
                       var PasswordMessageText = document.getElementById("Message_Title");
                       PasswordMessageText.innerHTML = "確認新密碼錯誤";
                       PaswordMessage();
                   } else {
                       TokenLogout();
                   }

               }).catch(function(error){
                var PasswordMessageText = document.getElementById("Message_Title");
                PasswordMessageText.innerHTML ="登入逾時，請重新登入";
                PaswordMessage();
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
           var popup_window = document.getElementById("window-container");
           popup_window.style.display = "none";
       } else {
           var PasswordMessageText = document.getElementById("Message_Title");
           PasswordMessageText.innerHTML = str;
           PaswordMessage();
       }
   }
   //刪除帳號
   function DeleteMember() {
       var Delete_window = document.getElementById("Delete-container");

       Delete_window.style.display = "block";

       window.onclick = function close(e) {
           if (e.target == Delete_window) {
               Delete_window.style.display = "none";
           }
       }
   }
   var Delete_false = document.getElementById("Delete_false");
   var Delete_true = document.getElementById("Delete_true");
   Delete_false.onclick = function() {
       var Delete_window = document.getElementById("Delete-container");
       Delete_window.style.display = "none";
   };
   Delete_true.onclick = function() {
       const DeleteUrl = 'Members/DeleteMembers';

       fetch(DeleteUrl, {
               method: 'POST',
               body: {},
               headers: {
                   'Accept': 'application/json',
                   'Content-Type': 'application/json; charset=utf-8',
                   'Authorization': 'Bearer ' + UserToken
               }
           })
           .then((response) => {
               return response.json();
           }).then((data) => {
               var Delete_window = document.getElementById("Delete-container");
               Delete_window.style.display = "none";
               localStorage.removeItem('userName');
               localStorage.removeItem('userToken');
               localStorage.removeItem('userPhoto');
               localStorage.removeItem('userId');
               var DeleteMessage_window = document.getElementById("Message-container");
               DeleteMessage_window.style.display = "block";
               var DeleteMessageText = document.getElementById("Message_Title");
               DeleteMessageText.innerHTML = "您已成功註銷帳號";
               PaswordMessage();
               Message_true.onclick = function() {
                   var popup_window = document.getElementById("Message-container");
                   popup_window.style.display = "none";
                   window.location.href = './index.html';
               };
           }).catch(function(error){
            var PasswordMessageText = document.getElementById("Message_Title");
            PasswordMessageText.innerHTML ="登入逾時，請重新登入";
            PaswordMessage();
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
   };

   //Token逾期登出
   function TokenLogout() {
       localStorage.removeItem('userName');
       localStorage.removeItem('userToken');
       localStorage.removeItem('userPhoto');
       localStorage.removeItem('userId');
       window.location.href = './index.html';
   }

//畫折線圖
function OpenChart() {
    var chart_window = document.getElementById("chart-container");

    chart_window.style.display = "block";

    window.onclick = function close(e) {
        if (e.target == chart_window) {
            chart_window.style.display = "none";
        }
    }
}

if(window.screen.width <= 450){
    var size=12
}
else if(window.screen.width <= 600){
    var size=14
}
else if (window.screen.width <= 650) {
    var size=16
} 
else if(window.screen.width <= 860){
    var size=18
}
else{
    var size=20
}

if(window.screen.width <= 500){
    var xsize=8
}
else if(window.screen.width <= 860){
    var xsize=10
}
else{
    var xsize=12
}
if(window.screen.width <= 600){
    stepSizeIn=25
}
else{
    stepSizeIn=20
}

function makechart(Data,Time){
    var chartOptions = {
        title: {
            display: true,
            text: '歷程分析圖表',
            fontSize: size
        },
        scales: {
            xAxes: [{
                // x 軸標題
                scaleLabel:{
                  display: true,
                },
                ticks: {
                    fontSize:xsize
                  },
              }],
            yAxes: [{
              ticks: {
                beginAtZero: true,
                min: 0,
                max: 100,
                stepSize: stepSizeIn,
                fontSize:xsize
              },
            // y 軸標題
             scaleLabel:{
                display: true,
                fontSize: size,
                labelString:"良率",
              },
            }]
          }
    };
    var ctx = document.getElementById('myChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: Time,
                    datasets: [{
                        label: '良率',
                        data: Data,
                        fill: false,
                        borderColor: 'rgb(75, 192, 192)',
                    }]
                    
                },
                options:chartOptions
            });
}

function startSelect() {
    var date_start = document.querySelectorAll('input[type="date"]')[0].value;
    var date_end = document.querySelectorAll('input[type="date"]')[1].value;
    var time_start = document.querySelectorAll('input[type="time"]')[0].value;
    var time_end = document.querySelectorAll('input[type="time"]')[1].value;
    var datetime_start = date_start + "T" + time_start;
    var datetime_end = date_end + "T" + time_end;
    let selectjsonData = {};
    selectjsonData = jsonData;
    jsonData = Object.values(jsonData).filter(function(item) {
        return item.start_time >= datetime_start && item.end_time <= datetime_end;
    });
    console.log(jsonData)
    GetPagelength(jsonData);
    jsonData = selectjsonData;
}
startSelect()
