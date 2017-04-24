//Когда страница редактирования загрузилась
jQuery(document).ready(function ($) {
    "use strict";
    //localforage.clear();
    //JS Project Model
    var jsProjectModel = jsProjectModel || {};

    //проверяем, есть ли в локальном хранилище информация о пользователе
        //если есть -
            //проверяем, есть ли на сервере проекты с ИД пользователя, показываем список проектов, создать проект
                //при загрузке проекта -
                    //1. аякс со списком of items like this: "тип объекта - id - версия",
                    //2. на сервере смотрим все записи в БД по проекту, возвращаем джейсон
                        //с объектами, которых нет на клиенте, или у которых версии устарели
                    //3. на клиенте визуализируем текущее состояние проекта
        //если нет - предлагаем:
            //1. sign in / зарегистрироваться (поп - ап окно: имя, е-мэйл, пароль) -
                //аякс(создаем запись в БД, возвращаем объект Пользователь)
            //2. создать проект (поп - ап окно: пустой список проектов, создать проект(передаем юзера)) -
                //аякс(создаем запись в БД, возвращаем объект Проект)
    localforage.getItem("user", function (err, blob) {

        //
        console.log("user: " + blob);

        if (blob == null) {

            showRequestAccountPopUp();
        } else {

            showProjectsPopUp();
        }
    });

    function showRequestAccountPopUp() {

        var result = '';
        while (result == '') {

            result = prompt('Input 1 for SignIn or 2 for SignUp: ', '');
        }
        if (result == 1) {

            showSignInPopUp();
        } else if (result == 2) {

            showSignUpPopUp();
        }
    }

    function showSignUpPopUp() {
        //alert('SignUp');
        var userName = '';
        var userEMail = '';
        var userPassword = '';
        while (userName == '') {

            userName = prompt('Input your name:', '');
        }
        while (userEMail == '') {

            userEMail = prompt('Input e-mail:', '');
        }
        while (userPassword == '') {

            userPassword = prompt('Input password:', '');
        }

        //console.log("name: " + userName + " " + userEMail + " " + userPassword);
        if (userName != '' && userEMail != '' && userPassword != '') {

            var newUserFormData = new FormData();

            newUserFormData.append("name", userName);
            newUserFormData.append("email", userEMail);
            newUserFormData.append("password", userPassword);

            $.ajax({
                type: "POST",
                //url: '/Default/Upload',
                url: '/Editor/CreateUser',
                dataType: 'json',
                contentType: false,
                processData: false,
                data: newUserFormData,
                success: function (result) {
                    //a
                    var resultJS = JSON.parse(JSON.stringify(result));
                    console.log("New user was created: "
                        + resultJS[0].id
                        + " " + resultJS[0].name
                        + " " + resultJS[0].email
                        + " " + resultJS[0].password
                    );
                    localforage.setItem("user", resultJS[0], function (err, blob) {
                        //nothing
                    }).then(function () {

                        localforage.getItem("user", function (err, blob) {

                            //var localBlobUrl = window.URL.createObjectURL(blob);

                            jsProjectModel.user = blob;
                            console.log("jsProjectModel.user: " + jsProjectModel.user);
                        });
                    });
                },
                error: function (xhr, status, p3) {
                    //
                    console.log("Error: " + xhr.responseText);
                }
            });
        }
    }

    function showSignInPopUp()
    {
        var userName = '';
        var userPassword = '';
        while (userName == '') {

            userName = prompt('Input your name:', '');
        }
        while (userPassword == '') {

            userPassword = prompt('Input password:', '');
        }

        //console.log("name: " + userName + " " + userPassword);
        if (userName != '' && userPassword != '') {

            var newUserFormData = new FormData();

            newUserFormData.append("name", userName);
            newUserFormData.append("password", userPassword);

            $.ajax({
                type: "POST",
                url: '/Editor/GetUser',
                dataType: 'json',
                contentType: false,
                processData: false,
                data: newUserFormData,
                success: function (result) {
                    //
                    var resultJS = JSON.parse(JSON.stringify(result));
                    console.log("New user was gotten: " + resultJS[0]);
                    if (resultJS == '') {

                        alert('User not exists');
                    } else {

                        console.dir(resultJS[0]);
                        console.log("New user was gotten: "
                            + resultJS[0].id
                            + " " + resultJS[0].name
                            + " " + resultJS[0].email
                            + " " + resultJS[0].password
                        );
                    }
                    

                    localforage.setItem("user", resultJS[0], function (err, blob) {
                        //nothing
                    }).then(function () {

                        localforage.getItem("user", function (err, blob) {

                            //var localBlobUrl = window.URL.createObjectURL(blob);

                            jsProjectModel.user = blob;
                            console.log("jsProjectModel.user: " + jsProjectModel.user);
                        });
                    });
                },
                error: function (xhr, status, p3) {
                    //
                    console.log("Error: " + xhr.responseText);
                }
            });
        }
    }

    function showProjectsPopUp() {
        //alert('Projects');
        var userId;
        localforage.getItem("user", function (err, blob) {

            //console.log("user: " + blob.id);
            userId = blob.id;
            $.ajax({
                type: "POST",
                url: '/Editor/getProjectNamesByUserId',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: "{'_userId':'" + userId + "'}",
                success: function (result) {
                    //
                    var resultJS = JSON.parse(JSON.stringify(result));
                    console.log("The projects was gotten: " + resultJS);
                    if (resultJS == '') {

                        alert('Projects not exists');
                        if (confirm('Do you want to create the new project?')) {

                            var projectName = prompt('Input project name: ', '');
                            if (projectName != '') {

                                //send ajax post request for create new project
                                //and get his data from response

                                var newProjectFormData = new FormData();

                                newProjectFormData.append("userid", userId);
                                newProjectFormData.append("name", projectName);

                                $.ajax({
                                    type: "POST",
                                    url: '/Editor/CreateProject',
                                    dataType: 'json',
                                    contentType: false,
                                    processData: false,
                                    data: newProjectFormData,
                                    success: function (result) {
                                        //
                                        var resultJS = JSON.parse(JSON.stringify(result));
                                        console.log("New project was created: "
                                            + resultJS.id
                                            + " " + resultJS.name
                                            + " " + resultJS.userid
                                        );
                                        localforage.setItem("project", resultJS, function (err, blob) {
                                            //nothing
                                        }).then(function () {

                                            localforage.getItem("project", function (err, blob) {

                                                //var localBlobUrl = window.URL.createObjectURL(blob);

                                                jsProjectModel.user = blob;
                                                console.log("jsProjectModel.user: " + jsProjectModel.user);
                                            });
                                        });
                                    },
                                    error: function (xhr, status, p3) {
                                        //
                                        console.log("Error: " + xhr.responseText);
                                    }
                                });
                            }
                        }

                    } else {

                        console.log("The projects was gotten: " + resultJS);
                        //console.dir(resultJS);
                        if (resultJS.length > 0) {

                            resultJS.forEach(function (item, i, resultJS) {

                                console.log("The project #"
                                    + i
                                    + ": " + item.Id
                                    + "; " + item.Name
                                );
                            });
                        }                        
                    }

                    /*localforage.setItem("user", resultJS, function (err, blob) {
                        //nothing
                    }).then(function () {

                        localforage.getItem("user", function (err, blob) {

                            var localBlobUrl = window.URL.createObjectURL(blob);

                            jsProjectModel.user = localBlobUrl;
                            console.log("jsProjectModel.user: " + jsProjectModel.user);
                        });
                    });*/
                },
                error: function (xhr, status, p3) {
                    //
                    console.log("Error: " + xhr.responseText);
                }
            });
        });
    }
});

/*function showMessage() {
    alert('Hi!');
}

mediaSender.showMessage();*/

var returnedUrl = "";

window.URL = window.URL || window.webkitURL;
//выбран(ы) медиафайл(ы) в стандартном диалоге,
//открывающемся по клику на кнопке "Импорт"
$('#uploadFiles').on('change', function (e) {
    //получаем массив файлов
    var files = this.files;
    //если массив не пуст
    if (files.length > 0) {
        //если в ДОМе существует объект FormData
        if (window.FormData !== undefined) {
            //получаем со страницы элемент-контейнер для набора превью
            var fileTile = document.getElementsByClassName('filesTile')[0];
            //флаг "превью файла с данным именем не существует"
            var rowExists = false;
            //переменная для имени текущего файла
            var tmpName = "";
            //для хранения типа текущего файла
            var currentFileType;
            //пустой массив для будущих вспомогательных элементов video
            var videos = [];
            //для хранения данных для создаваемого превью
            var row2;
            //пока в массиве есть файлы
            for (var x = 0; x < files.length; x++) {
                //rl = new RowList();
                tmpName = files[x].name;
                //если в элементе-контейнере уже есть дочерние элементы (превью), ...
                if (fileTile.hasChildNodes()) {
                    //... перебираем их
                    var fileTileChildNodes = fileTile.childNodes;
                    Array.from(fileTileChildNodes).forEach(function (node, i, fileTileChildNodes) {
                        //проверяем, есть ли дочерние элементы у текущего превью
                        if (node.hasChildNodes()) {
                            //если текстовый узел равен имени текущего файла, ...
                            if (tmpName == node.childNodes[1].textContent) {
                                //устанавливаем флаг, что файл с таким именем уже есть в ресурсах
                                rowExists = true;
                            }
                        }
                    });
                }
                
                if (!rowExists) {

                    const currentName = tmpName;
                    const currentExt = currentName.substr(currentName.length - 3, 3).toLowerCase();
                    
                    switch (currentExt) {
                        case 'mp4':
                        case 'avi':
                            currentFileType = 'video';
                            break;
                        case 'jpg':
                        case 'jpeg':
                        case 'png':
                            currentFileType = 'image';
                            break;
                        case 'mp3':
                            currentFileType = 'audio';
                            break;
                        default:
                            currentFileType = 'unknown';
                    }
                    
                    if (currentFileType == 'video') {

                        const currentVideo = document.createElement('video');
                        currentVideo.setAttribute("id", "video" + x);

                        currentVideo.addEventListener('loadeddata', function (e) {

                            row2 = new Row();

                            const canvas = document.createElement('canvas');
                            canvas.setAttribute("id", "canvas" + x);
                            // необходимо установить те же размеры, что и у загруженного видео, иначе превью будет обрезано
                            canvas.width = this.videoWidth;
                            canvas.height = this.videoHeight;

                            const context = canvas.getContext('2d');
                            //context.drawImage(currentVideo, 0, 0, this.videoWidth, this.videoHeight);
                            context.drawImage(currentVideo, 0, 0, 124, 82);

                            const dataURL = canvas.toDataURL(); // вот и ссылка с превью
                            row2.values["path"] = dataURL;

                            //console.log(this.duration); // а здесь длительность видео в секундах
                            console.log(currentName);
                            row2.values["fileName"] = currentName;
                            //row2.values["path"] = "Images/2.png";
                            row2.values["type"] = "video";
                            row2.values["width"] = canvas.width;
                            //console.log(row2);
                            rl.list.push(row2);
                            rl.generateRowList();

                            /*$('.filesTile li').click(function () {
                                rl.showRowInfo($(this).attr("data-row-id"));
                            });*/
                        });

                        currentVideo.onerror = function () {
                            URL.revokeObjectURL(this.src);
                            //this.parentNode.removeChild(this);
                            this.remove();
                            //div.innerHTML = "not image";
                            alert("Тип файла" + currentName + " не соответствует расширению");
                        };

                        currentVideo.src = window.URL.createObjectURL(files[x]);



                        var data = new FormData();
                        data.append("file" + x, files[x]);
                        $.ajax({
                            type: "POST",
                            //url: '/Editor/Upload',
                            url: '/Editor/CreateRow',
                            contentType: false,
                            processData: false,
                            data: data,
                            success: function (result) {
                                //alert(result);
                                applyChanges(result);
                                //console.log("call applyChanges()");
                            },
                            error: function (xhr, status, p3) {
                                alert(xhr.responseText);
                            }
                        });
                    } else if (currentFileType == 'image') {
                        
                        const currentImage = document.createElement('img');
                        currentImage.setAttribute("id", "image" + x);

                        currentImage.onload = function (e) {

                            row2 = new Row();

                            const canvas = document.createElement('canvas');
                            canvas.setAttribute("id", "canvas" + x);
                            // необходимо установить те же размеры, что и у загруженного image, иначе превью будет обрезано
                            canvas.width = this.naturalWidth;
                            canvas.height = this.naturalWidth;

                            const context = canvas.getContext('2d');
                            context.drawImage(this, 0, 0, 124, 82);

                            const dataURL = canvas.toDataURL(); // вот и ссылка с превью
                            row2.values["path"] = dataURL;

                            row2.values["fileName"] = currentName;
                            //row2.values["path"] = "Images/2.png";
                            row2.values["type"] = "image";
                            row2.values["width"] = canvas.width;
                            //console.log(row2);
                            rl.list.push(row2);
                            rl.generateRowList();

                            /*$('.filesTile li').click(function () {
                                rl.showRowInfo($(this).attr("data-row-id"));
                            });*/
                        };

                        currentImage.onerror = function () {
                            URL.revokeObjectURL(this.src);
                            //this.parentNode.removeChild(this);
                            this.remove();
                            //div.innerHTML = "not image";
                            alert("Тип файла" + currentName + " не соответствует расширению");
                        };

                        currentImage.src = window.URL.createObjectURL(files[x]);



                        /*var data = new FormData();
                        data.append("file" + x, files[x]);
                        $.ajax({
                            type: "POST",
                            url: '/Default/Upload',
                            contentType: false,
                            processData: false,
                            data: data,
                            success: function (result) {
                                //alert(result);
                                applyChanges(result);
                                //console.log("call applyChanges()");
                            },
                            error: function (xhr, status, p3) {
                                alert(xhr.responseText);
                            }
                        });*/
                    } else if (currentFileType == 'audio') {
                        
                        const currentAudio = document.createElement('audio');
                        currentAudio.setAttribute("id", "audio" + x);

                        currentAudio.onloadeddata = function (e) {

                            row2 = new Row();

                            //const canvas = document.createElement('canvas');
                            //canvas.setAttribute("id", "canvas" + x);
                            // необходимо установить те же размеры, что и у загруженного image, иначе превью будет обрезано
                            //canvas.width = this.naturalWidth;
                            //canvas.height = this.naturalWidth;

                            //const context = canvas.getContext('2d');
                            //context.drawImage(this, 0, 0);

                            //const dataURL = canvas.toDataURL(); // вот и ссылка с превью
                            row2.values["path"] = this.src;

                            row2.values["fileName"] = currentName;
                            //row2.values["path"] = "Images/2.png";
                            row2.values["type"] = "audio";
                            row2.values["width"] = "";
                            console.log(row2);
                            rl.list.push(row2);
                            rl.generateRowList();

                            /*$('.filesTile li').click(function () {
                                rl.showRowInfo($(this).attr("data-row-id"));
                            });*/
                        };

                        currentAudio.onerror = function () {
                            URL.revokeObjectURL(this.src);
                            //this.parentNode.removeChild(this);
                            this.remove();
                            //div.innerHTML = "not image";
                            alert("Тип файла" + currentName + " не соответствует расширению");
                        };

                        currentAudio.src = window.URL.createObjectURL(files[x]);
                        //currentAudio.load();


                        /*var data = new FormData();
                        data.append("file" + x, files[x]);
                        $.ajax({
                            type: "POST",
                            url: '/Default/Upload',
                            contentType: false,
                            processData: false,
                            data: data,
                            success: function (result) {
                                //alert(result);
                                applyChanges(result);
                                //console.log("call applyChanges()");
                            },
                            error: function (xhr, status, p3) {
                                alert(xhr.responseText);
                            }
                        });*/
                    } else {

                        alert("Неизвестный тип файла " + currentName);
                    }                    
                } else {

                    rowExists = false;
                    alert("Файл с именем " + tmpName + " уже присутствует в ресурсах");
                }
            }
            
        } else {
            alert("Браузер не поддерживает загрузку файлов HTML5!");
        }
        //console.dir(rl);
        
    }
});

function applyChanges(_returnedUrl) {

    //console.log(_returnedUrl);
    $.ajax({
        type: "GET",
        url: _returnedUrl,
        contentType: false,
        processData: false,
        'xhrFields': {
            'responseType' : 'blob'
        },
        'dataType': 'binary',
        //data: data,
        success: function (result) {
            //alert(result);
            //applyChanges(result);
            setData(result);
        },
        error: function (xhr, status, p3) {
            alert(xhr.responseText);
        }
    });
}

function setData(_data) {

    localforage.setItem("tmpvideo", _data, function (err, blob) {


    }).then(function() {

        //console.log("_data: " + _data);
        localforage.getItem("tmpvideo", function (err, blob) {
            // Создание data URI для помещения _data в video
            //console.log("Data: " + blob);
            
            var localBlobUrl = window.URL.createObjectURL(blob);

            $('#videoPlayer video').attr('src', localBlobUrl);
            $("#videoPlayer video")[0].load();
        });
    });
}

//context menu
$('#menu-edit-item').click(function () {
    console.log("edit");
});
$('#menu-delete-item').click(function () {
    console.log("delete");
});

function getData() { }