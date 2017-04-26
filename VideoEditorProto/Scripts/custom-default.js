//Когда страница Default загрузилась
jQuery(document).ready(function ($) {
    "use strict";

    //
    //localforage.clear();
    
    //проверяем, есть ли на сервере проекты с ИД пользователя, показываем список проектов, создать проект
    //при загрузке проекта -
    //1. аякс со списком of items like this: "тип объекта - id - версия",
    //2. на сервере смотрим все записи в БД по проекту, возвращаем джейсон
    //с объектами, которых нет на клиенте, или у которых версии устарели
    //3. на клиенте визуализируем текущее состояние проекта
    
    //1. sign in / зарегистрироваться (поп - ап окно: имя, е-мэйл, пароль) -
    //аякс(создаем запись в БД, возвращаем объект Пользователь)
    //2. создать проект (поп - ап окно: пустой список проектов, создать проект(передаем юзера)) -
    //аякс(создаем запись в БД, возвращаем объект Проект)

    //
    //проверяем, есть ли в локальном хранилище информация о пользователе
    localforage.getItem("user", function (err, blob) {

        //
        console.log("user: " + blob);

        //если нет - предлагаем:
        if (blob == null) {

            showRequestAccountHrefs();
        } else {

            //если есть -
            $("#edit-a").css("display", "block");
            $("#signin-a").css("display", "none");
            $("#signup-a").css("display", "none");
        }
    });

    function showRequestAccountHrefs() {

        //var result = '';
        $("#edit-a").css("display", "none");
        $("#signin-a").css("display", "block");
        $("#signup-a").css("display", "block");
    }

    $("#signin-a").click(function myfunction(event) {

        event.preventDefault();

        if ($(".signup").css("display") == "block") {

            $(".signup").css("display", "none");
        }

        $(".signin").css("display", "block");
    });

    $("#signup-a").click(function myfunction(event) {

        event.preventDefault();

        if ($(".signin").css("display") == "block") {

            $(".signin").css("display", "none");
        }

        $(".signup").css("display", "block");
    });

    $('form.signup.modal input[name=submit]').click(function showSignUpPopUp(event) {

        event.preventDefault();
        
        //
        var userName = '';
        //var userSurName = '';
        var userEMail = '';
        var userPassword = '';
        var userPassword1 = '';

        userName = $('form.signup.modal input[name=name]').val();
        //userSurName = $('form.signup.modal input[name=surname]').val();
        userEMail = $('form.signup.modal input[name=mail]').val();
        userPassword = $('form.signup.modal input[name=password]').val();
        userPassword1 = $('form.signup.modal input[name=password1]').val();

        //console.log("name: " + userName + " " + userEMail + " " + userPassword);

        if (userName != ''
            && userEMail != ''
            && userPassword != ''
            && (userPassword == userPassword1)) {

            console.log("name: " + userName + " " + userEMail + " " + userPassword);

            var newUserFormData = new FormData();

            newUserFormData.append("name", userName);
            newUserFormData.append("email", userEMail);
            newUserFormData.append("password", userPassword);

            $.ajax({
                type: "POST",
                url: '/Default/CreateUser',
                dataType: 'json',
                contentType: false,
                processData: false,
                data: newUserFormData,
                success: function (result) {
                    //
                    var resultJS = JSON.parse(JSON.stringify(result));
                    //console.dir(resultJS);
                    resultJS = (resultJS[0] !== undefined) ? resultJS[0] : resultJS;
                    console.log("New user was created: "
                        + resultJS.id
                        + " " + resultJS.name
                        + " " + resultJS.email
                        + " " + resultJS.password
                    );
                    localforage.setItem("user", resultJS, function (err, blob) {
                        //nothing
                    }).then(function () {

                        localforage.getItem("user", function (err, blob) {

                            //
                            console.log("New user is in the local model: " + blob);

                            $(".signup").css("display", "none");

                            $("#edit-a").css("display", "block");
                            $("#signin-a").css("display", "none");
                            $("#signup-a").css("display", "none");
                        });
                    });
                },
                error: function (xhr, status, p3) {
                    //
                    console.log("Error: " + xhr.responseText);
                }
            });
        }
    });

    $('form.signin.modal input[name=submit]').click(function showSignInPopUp(event) {

        event.preventDefault();

        var userEMail = '';
        var userPassword = '';

        userEMail = $('form.signin.modal input[name=mail]').val();
        userPassword = $('form.signin.modal input[name=password]').val();

        //console.log("name: " + userName + " " + userPassword);
        if (userEMail != '' && userPassword != '') {

            var newUserFormData = new FormData();

            newUserFormData.append("email", userEMail);
            newUserFormData.append("password", userPassword);

            $.ajax({
                type: "POST",
                url: '/Default/GetUser',
                dataType: 'json',
                contentType: false,
                processData: false,
                data: newUserFormData,
                success: function (result) {
                    //
                    var resultJS = JSON.parse(JSON.stringify(result));

                    console.log("New user was gotten: " + resultJS);

                    if (resultJS == '') {

                        alert('User not exists');
                    } else {

                        console.dir(resultJS);

                        resultJS = (resultJS[0] !== undefined) ? resultJS[0] : resultJS;

                        console.log("New user was gotten: "
                            + resultJS.id
                            + " " + resultJS.name
                            + " " + resultJS.email
                            + " " + resultJS.password
                        );

                        localforage.setItem("user", resultJS, function (err, blob) {
                            //nothing
                        }).then(function () {

                            localforage.getItem("user", function (err, blob) {

                                //var localBlobUrl = window.URL.createObjectURL(blob);

                                //jsProjectModel.user = blob;
                                console.log("Existing user is in the local model: " + blob);

                                $(".signin").css("display", "none");

                                $("#edit-a").css("display", "block");
                                $("#signin-a").css("display", "none");
                                $("#signup-a").css("display", "none");
                            });
                        });
                    }
                },
                error: function (xhr, status, p3) {
                    //
                    console.log("Error: " + xhr.responseText);
                }
            });
        }
    });
});