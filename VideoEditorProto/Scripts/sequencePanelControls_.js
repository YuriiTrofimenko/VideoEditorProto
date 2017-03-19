$(function()
{
	var rowSelector = ".filesTile li",
	layoutSelector = ".videoLayouts .videoLayout, .audioLayouts .audioLayout";

	$(rowSelector).draggable({
		revert: true,
		helper: 'clone',
		scroll: false
	});

	$('.showHide, .blockUnblock, .muteSound').click(function()
	{
		$(this).toggleClass('active');
	});

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
				rowInfo = rl.getRowInfo(ui.draggable.attr("data-row-id")),
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
			img.setAttribute('src', rowInfo['values']['path']);
			img.setAttribute('alt', rowInfo['values']['fileName']);
			row.className = 'videoRow';
			row.appendChild(name);
			row.appendChild(effects);
			row.appendChild(img);

			if(!isFind)
			{
				console.log(row);

				$(this).append(row);
				$(row).draggable({
					axis: "x,y",
					containment: $(this),
					snap: true
				});
				row.droppable = null;
			}
		}
	});
});