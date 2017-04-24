function Row ()
{
	this.fields = [];
	this.values = [];
}

Row.prototype.defineFields = function()
{
	this.fields = Object.keys(this.values);
}

Row.prototype.getRowProperties = function()
{
	this.defineFields();
	var fileProperties = $('.fileProperties .properties')[0],
		fileValues = $('.fileProperties .values')[0],
		spanElem = {},
		item = "";

	if (fileProperties.innerHTML != "") fileProperties.innerHTML = "";
	if (fileValues.innerHTML != "") fileValues.innerHTML = "";

	for (var i = 0; i < this.fields.length; i++)
	{
		spanElem = document.createElement("span");
		spanElem.innerHTML = this.fields[i];
		fileProperties.appendChild(spanElem);
	}

	for (var key in this.values)
	{
		spanElem = document.createElement("span");
		spanElem.innerHTML += this.values[key];
		fileValues.appendChild(spanElem);
	}
}

function RowList()
{
	this.list = [];
}

RowList.prototype.generateRowList = function()
{
	if (this.list.length == 0)
		console.log("error");

	var fileTile = document.getElementsByClassName('filesTile')[0],
		newRow = document.createElement("li"),
		image = document.createElement("img"),
		fileInfo = document.createElement("div"),
		fileType = "",
        item = "";

    var rowExists = false;

	for (var i = 0; i < this.list.length; i++)
    {
        
        item = this.list[i];

        if (fileTile.hasChildNodes()) {
            var fileTileChildNodes = fileTile.childNodes;
            Array.from(fileTileChildNodes).forEach(function (node, i, fileTileChildNodes) {

                if (node.hasChildNodes()) {
                    //
                    if (item.values["fileName"] == node.childNodes[1].textContent) {
                        console.log(item.values["fileName"] + ' == ' + node.childNodes[1].textContent);
                        rowExists = true;
                    }
                }
            });
        }

        if (!rowExists) {
            //
            newRow = document.createElement("li");
            image = document.createElement("img");
            fileInfo = document.createElement("div");

            //
            newRow.setAttribute("data-row-id", i);
            newRow.setAttribute("class", "ui-draggable ui-draggable-handle");
            image.setAttribute("src", (item.values["type"] != 'audio') ? item.values["path"] : "Images/3.png");
            image.setAttribute("alt", item.values["name"]);
            newRow.appendChild(image);
            fileInfo.className = item.values["type"] + "Tile";
            fileInfo.innerHTML = item.values["fileName"];
            newRow.appendChild(fileInfo);
            fileTile.appendChild(newRow);

            //
            $(newRow).click(function () {
                rl.showRowInfo($(this).attr("data-row-id"));
            });

            $(newRow).draggable({
                revert: true,
                helper: 'clone',
                scroll: false,
                appendTo: '.squencePanel'
            });
        } else {

            rowExists = false;
        }
	}
}

RowList.prototype.getRowIndex = function(elem) 
{
	if (elem != null)
	{
		return elem.prevAll().length;
	}
}

RowList.prototype.showRowInfo = function(index)
{
	this.list[index].getRowProperties();
}

RowList.prototype.getRowInfo = function(index)
{
	this.list[index].defineFields();
	return {'fields': this.list[index].fields, 
			'values': this.list[index].values};
}

	//
	rl = new RowList();

$(function() {
	$(".tabPanel").tabs();

	/*row1.values["fileName"] = "1.png";
	row1.values["path"] = "files/img/1.png";
	row1.values["imagePath"] = "files/img/1.png";
	row1.values["type"] = "image";

	rl.list.push(row1);

	row1 = new Row();

	row1.values["fileName"] = "audio1.mp3";
	row1.values["path"] = "files/audio/audio1.mp3";
	row1.values["imagePath"] = "";
	row1.values["type"] = "audio";
	row1.values['duration'] = 204;

	rl.list.push(row1);

	row1 = new Row();

	row1.values["fileName"] = "video.mp4";
	row1.values["path"] = "files/video/video.mp4";
	row1.values["type"] = "video";
	row1.values["imagePath"] = "files/video/thub.png";
	row1.values['duration'] = 385;*/

	var strlist = JSON.stringify(rl.list);

	$('.effectNames .colBody .active h2').click(function()
	{
		$(this).next().slideToggle();
		$('.effectNames .colBody > ul > li.active:before').animate({translate: 'rotate(-90deg)'}, 300);
	});
	
    $('.save').click({
        /*$.ajax({
            url: '/editor/give_rows/',
            type: 'POST',
            data: strlist,
            success: function (data) {
                alert(data);
            },
            contentType: 'application/json'
        });*/
    });

	/*rl.list.push(row1);

	rl.generateRowList();

	$('.filesTile li').click(function() 
	{
		rl.showRowInfo($(this).attr("data-row-id"));
	});*/

//	var csrftoken = document.cookie.substring(10, );
//
//	function csrfSafeMethod(method) {
//		return (/^(GET|HEAD|OPTIONS|TRACE)$/.test(method));
//	}

//	$.ajaxSetup({
//		beforeSend: function(xhr, settings) {
//			if (!csrfSafeMethod(settings.type) && !this.crossDomain) {
//				xhr.setRequestHeader('X-CSRFToken', csrftoken);
//			}
//		}
//	});
//
//	$('#files form').submit(function(e) {
//		var f = new FormData($(this)[0]);
//
//		$.ajax({
//			url: '/load_row_file/',
//			type: 'POST',
//			data: f,
//			async: false,
//			success: function (data) {
//				alert(data);
//			},
//			error: function(xhr, status, error) {
//				console.log(xhr);
//				console.log(status);
//				console.log(error);
//			},
//			cache: false,
//			contentType: false,
//			processData: false
//		});
//		return false;
//	});
});