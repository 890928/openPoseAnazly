var validate_url = location.href;
if (validate_url.indexOf('?') != -1) {
    var member_information = validate_url.split('?');
    var MembersId = '';
    var authcode = '';
    //在此直接將各自的參數資料切割放進ary中
    var member_authcode = member_information[1].split(',');
    var memberId = member_authcode[0].split('=');
    var auth = member_authcode[1].split('=');
    var memberId = memberId[1];
    var authcode = auth[1];

    var url = `Members/EmailValidate?MembersId=${memberId}&authcode=${authcode}`;
    fetch(url, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8'
            },
        })
        .then(response => {
            const result = response.json();
            result.then(body => {
                console.log("body =>", body);
                if (body["ErrorMessage"] == null) {
                    var dialog = document.querySelector('dialog');
                    dialog.innerHTML = `<div class="dialog_content"><p>${body['Data']}</p></div><button id="btn">確認</button>`
                    dialog.style = 'display:flex';
                    var btn = document.getElementById('btn');
                    btn.onclick = function() {
                        dialog.style = 'display:none';
                    };
                } else {
                    var dialog = document.querySelector('dialog');
                    dialog.innerHTML = `<div class="dialog_content"><p>${body["ErrorMessage"]}</p></div><button id="btn">確認</button>`
                    dialog.style = 'display:flex';
                    var btn = document.getElementById('btn');
                    btn.onclick = function() {
                        dialog.style = 'display:none';
                    };
                };
            });
        })
        .catch();
}
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

    //#region 進入歷程紀錄 
    var go_history = document.getElementById('go_history');
    go_history.onclick = function() { window.location.href = 'History.html' };
    //#endregion

    //#region 進入坐姿檢測
    var go_detection = document.getElementById('go_detection');
    var detection_page = document.getElementById('list_detection');
    detection_page.onclick = function() { window.location.href = 'Detection.html'; };
    go_detection.onclick = function() { window.location.href = 'Detection.html'; };
    //#endregion

    //#region 進入分析
    var go_analyze = document.getElementById('go_analyze');
    go_analyze.onclick = function() { window.location.href = 'Analyze.html' };
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
        window.location.href = 'index.html'
    }

} else {
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

function login_click() {
    loginpage.style = 'display:flex';
    wrapper.style = 'filter: blur(6px);pointer-events:none;user-select:none;';
    document.body.style.overflowY = 'hidden';

    var loginBtn = document.getElementById('login_btn');
    loginBtn.onclick = function() {
        login_btn();
    };

    //點擊'註冊帳號'顯示註冊畫面
    var register_member = document.getElementById('register_member');
    register_member.onclick = function() {
        register_click();
    };

    //點擊'忘記密碼'顯示忘記密碼畫面
    var forget_password = document.getElementById('forget_password');

    forget_password.onclick = function() {
        forget_page.style = 'display:flex';
        loginpage.style = 'display:none';
        wrapper.style = 'filter: blur(6px);pointer-events:none;user-select:none;';
        document.body.style.overflowY = 'hidden';

        modal_close(forget_page);

        var confirm = document.getElementById('confirm');
        confirm.onclick = function() { confirm_btn(); };
    };
    modal_close(loginpage);
};

function register_click() {
    registerpage.style = 'display:flex';
    wrapper.style = 'filter: blur(6px);pointer-events:none;user-select:none;';
    document.body.style.overflowY = 'hidden';

    var registerBtn = document.getElementById('register_btn');
    registerBtn.onclick = function() {
        register_btn();
    };

    //點擊'登入'顯示登入畫面
    var go_login = document.getElementById('goLogin');
    go_login.onclick = function() {
        loginpage.style = 'display:flex';
        registerpage.style = 'display:none';
    };
    modal_close(registerpage);
};

//#endregion

//關閉彈跳視窗
function modal_close(page) {
    var input = document.getElementsByTagName('input');
    document.addEventListener("click", function(e) {
        var ele = e.target;
        if (ele == login || ele == register) {};
        if (ele == document.body) {
            page.style = 'display:none';
            wrapper.style = 'filter:none;'
            document.body.style.overflowY = 'scroll';
        };

    });
    for (var i = 0; i < input.length; i++) {
        input[i].value = "";
    };
};

function register_btn() {
    const url = 'Members/Register';
    var account = document.getElementById('email').value;
    var password = document.getElementById('password').value;
    var passwordcheck = document.getElementById('checkpassword').value
    let errorStr = '';
    errorStr += (account === '') ? '請輸入帳號(電子信箱)<br>' : '';
    if (account !== '' && validate_email(account) == false) {
        errorStr += '帳號(電子信箱)格式錯誤<br>';
    }
    errorStr += (password === '') ? '請輸入密碼<br>' : '';
    errorStr += (passwordcheck === '') ? '請輸入確認密碼<br>' : '';
    if (password !== '' && passwordcheck !== '') {
        errorStr += (password !== passwordcheck) ? '密碼輸入不一致<br>' : '';
    }

    if (errorStr !== '') {
        var dialog = document.querySelector('dialog');
        dialog.innerHTML = `<div class="dialog_content"><p>${errorStr}</p></div><button id="btn">確認</button>`
        dialog.style = 'display:flex';
        var btn = document.getElementById('btn');
        btn.onclick = function() {
            dialog.style = 'display:none';
        };
        modal_close(dialog);
    } else {
        var payload = {
            MembersId: account,
            password: password,
            passwordcheck: passwordcheck,
        }
        console.log("payload =>", payload)

        fetch(url, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(payload)
            })
            .then(response => {
                const result = response.json();
                result.then(body => {
                    console.log("body =>", body);
                    if (body["ErrorMessage"] == null) {
                        var dialog = document.querySelector('dialog');
                        dialog.innerHTML = `<div class="dialog_content"><p>${body['Data']}</p></div><button id="btn">確認</button>`
                        dialog.style = 'display:flex';
                        var btn = document.getElementById('btn');
                        btn.onclick = function() {
                            dialog.style = 'display:none';
                        };
                    } else {
                        var dialog = document.querySelector('dialog');
                        dialog.innerHTML = `<div class="dialog_content"><p>${body["ErrorMessage"]}</p></div><button id="btn">確認</button>`
                        dialog.style = 'display:flex';
                        var btn = document.getElementById('btn');
                        btn.onclick = function() {
                            dialog.style = 'display:none';
                        };
                        registerpage.style = 'display:none';
                        wrapper.style = 'filter: none;pointer-events:click;';
                        document.body.style.overflowY = 'auto';
                    };

                });
            })
            .catch();

    }
};

//email驗證
function validate_email(value) {
    apos = value.indexOf("@")
    dotpos = value.lastIndexOf(".")
    if (apos < 1 || dotpos - apos < 2)
        return false
    else
        return true
}

function login_btn() {
    const url = 'Members/Login';
    var account = document.getElementById('login_email').value;
    var password = document.getElementById('login_password').value;
    let errorStr = '';
    errorStr += (account === '') ? '請輸入帳號(電子信箱)<br>' : '';
    if (account !== '' && validate_email(account) == false) {
        errorStr += '帳號(電子信箱)格式錯誤<br>';
    }
    errorStr += (password === '') ? '請輸入密碼<br>' : '';

    if (errorStr !== '') {
        var dialog = document.querySelector('dialog');
        dialog.innerHTML = `<div class="dialog_content"><p>${errorStr}</p></div><button id="btn">確認</button>`
        dialog.style = 'display:flex';
        var btn = document.getElementById('btn');
        btn.onclick = function() {
            dialog.style = 'display:none';
        };
        modal_close(dialog);
    } else {
        var payload = {
            MembersId: account,
            password: password,
        }
        fetch(url, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json; charset=utf-8'
                },

                body: JSON.stringify(payload)
            })
            .then(response => {
                const result = response.json()
                result.then(body => {
                        console.log(body);
                        if (body["ErrorMessage"] == null) {
                            localStorage.setItem("userToken", body["Token"]);
                            localStorage.setItem("userName", body["Name"]);
                            localStorage.setItem("userPhoto", body["ImageUrl"]);
                            localStorage.setItem("userId", body["MembersId"]);
                            loginpage.style = 'display:none';
                            wrapper.style = 'filter: none;pointer-events:click;';
                            document.body.style.overflowY = 'auto';
                            window.location.href = 'index.html'
                        } else {
                            var dialog = document.querySelector('dialog');
                            dialog.innerHTML = `<div class="dialog_content"><p>${body["ErrorMessage"]}</p></div><button id="btn">確認</button>`
                            dialog.style = 'display:flex';
                            var btn = document.getElementById('btn');
                            btn.onclick = function() {
                                dialog.style = 'display:none';
                                loginpage.style = 'display:none';
                                wrapper.style = 'filter: none;pointer-events:click;';
                                document.body.style.overflowY = 'auto';
                            };
                        };
                    })
                    .catch();
            });
    }
};

function confirm_btn() {
    const url = 'Members/ForgetPassword';
    var account = document.getElementById('forget_email').value;
    let errorStr = '';
    errorStr += (account === '') ? '請輸入帳號(電子信箱)\r\n' : '';
    if (account !== '' && validate_email(account) == false) {
        errorStr += '帳號(電子信箱)格式錯誤\r\n';
    }
    if (errorStr !== '') {
        window.alert(errorStr);
    } else {
        var payload = {
            MembersId: account,
        }
        console.log("payload =>", payload)

        fetch(url, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json; charset=utf-8'
                },

                body: JSON.stringify(payload)
            })
            .then(response => {
                const result = response.json()
                result.then(body => {
                    console.log(body);
                    if (body["ErrorMessage"] == null) {
                        var dialog = document.querySelector('dialog');
                        dialog.innerHTML = `<div class="dialog_content"><p>${body['Data']}</p></div><button id="btn">確認</button>`
                        dialog.style = 'display:flex';
                        var btn = document.getElementById('btn');
                        btn.onclick = function() {
                            dialog.style = 'display:none';
                        };
                    } else {
                        var dialog = document.querySelector('dialog');
                        dialog.innerHTML = `<div class="dialog_content"><p>${body["ErrorMessage"]}</p></div><button id="btn">確認</button>`
                        dialog.style = 'display:flex';
                        var btn = document.getElementById('btn');
                        btn.onclick = function() {
                            dialog.style = 'display:none';
                        };
                    };
                });
                forget_page.style = 'display:none';
                wrapper.style = 'filter: none;pointer-events:click;';
                document.body.style.overflowY = 'auto';
            })
            .catch();
    }
};