$(function()
{
	var rowSelector = ".filesTile li",
	layoutSelector = ".videoLayouts .videoLayout, .audioLayouts .audioLayout";
	
	$(rowSelector).draggable({
		revert: true,
		helper: 'clone',
		scroll: false,
		appendTo: '.squencePanel'
	});
	
	function setScrollbars() {
        baron({
            root: '.tab',
            scroller: '.filesTile',
            bar: '.scroller',
            direction: 'v',
            scrollingCls: '_scrolling',
            draggingCls: '_dragging'
        }).controls({
            track: '.tabScrollPanel'
        });

        var myScrollbar = baron({
            root: '.controlAudioPanel',
            scroller: '.audioLayouts',
            bar: '.scrollbar',
            direction: 'h',
            scrollingCls: '_scrolling',
            draggingCls: '_dragging'
        }).controls({
            track: '.sequenceScrollbar'
        });

        myScrollbar.onmouseup = function () {
            var offset = $(this).css('left'),
                width = $(this).width(),
                parentWidth = $(this).parent().width();

            $('.videoLayouts').scrollLeft(offset / width * parentWidth);
        };

        var click = false;

        myScrollbar.onclick = function () {
            click = true;
        };

        myScrollbar.onmousemove = function () {
            if (click) {
                var offset = $(this).css('left'),
                    width = $(this).width(),
                    parentWidth = $(this).parent().width();

                $('.videoLayouts').scrollLeft(offset / width * parentWidth);
            }
        };
	}

	$('.showHide, .blockUnblock, .muteSound').click(function()
	{
		$(this).toggleClass('active');
	});

	function generateScale(duration)
	{
        var timeScaleSize = 130,
            iters = Math.ceil(Math.ceil(duration) / 5) + 1,
            scale = $('.scale ul'),
            timeElement = document.createElement('li'),
            sequenceWidth = iters * timeScaleSize + "px";

        timeElement.innerHTML = "00:00:01";
        scale[0].appendChild(timeElement);

        for (var i = 1, seconds = 5, mins = 0, secs = 0; i < iters; i++ , seconds += 5) {
            timeElement = document.createElement('li');
            mins = Math.floor(seconds / 60);
            secs = seconds % 60;
            timeElement.innerHTML = "00:" + ((mins < 10) ? "0" + mins : mins) + ":"
                + ((secs < 10) ? "0" + secs : secs);
            scale[0].appendChild(timeElement);
        }

        scale.css('width', sequenceWidth);
        $('.videoLayout, .audioLayout').css('width', sequenceWidth);
        setScrollbars();
	}

	var vid = $('#videoPlayer video')[0];

	vid.addEventListener("loadedmetadata", function() {
		generateScale(this.duration);
    	});

    /*$('.save')[0].addEventListener('click', function()
    {
        var rows = {};

        $('.videoRow, .audioRow').each(function()
        {
            var index = $(this).attr('data-row-id');

            console.dir(rl.getRowInfo(index));
        });
    });*/

	$(layoutSelector).droppable({
		drop: function( event, ui ) 
		{
			var	clone = ui.draggable,
				isFind = false;

			for (var i = 0; i < clone[0].classList.length; i++) 
			{
				if (clone[0].classList[i] == "ui-draggable-dragging") 
				{
					isFind = true;
					return;
				}
			}

			var	row = document.createElement('div'),
				rowInfo = rl.getRowInfo(clone.attr("data-row-id")),
				img = document.createElement('img'),
				name = document.createElement('p'),
				effects = document.createElement('p'),
				numEffects = document.createElement('span');

			numEffects.innerHTML = '2';
			effects.innerHTML = "Effects ";
			effects.className = 'effectsNum';
			effects.appendChild(numEffects);
			name.className = "rowHeader";
			name.innerHTML = rowInfo['values']['fileName'];
			img.setAttribute('src', rowInfo['values']['imagePath']);
			img.setAttribute('alt', rowInfo['values']['fileName']);
			row.setAttribute('data-row-id', clone.attr("data-row-id"));
			row.className = (rowInfo['values']['type'] == "audio") ? 'audioRow' : 'videoRow';
			row.appendChild(name);
			row.appendChild(effects);
			row.appendChild(img);
			row.style.width = rowInfo['values']['duration'] / 5 * 200 + "px";

			if(!isFind)
			{
				$(this).append(row);
				$(row).draggable({
					axis: "x",
					containment: $(this),
					snap: true
				});
				$(row).resizable({
				    minHeight: 38,
				    maxHeight: 38,
				    containment: '.ui-droppable',
				    handles: 'e, w'
				});
				row.droppable = null;
			}
		}
	});
});
