/*global jQuery:false */
jQuery(document).ready(function($) {
"use strict";

	function validateFields(){
		
		var f = $('form.validateform').find('.field'),
		//var f = this,
		ferror = false, 
		emailExp = /^[^\s()<>@,;:\/]+@\w[\w\.-]+\.[a-z]{2,}$/i;

		f.children('input').each(function(){ // run all inputs

		    var i = $(this); // current input
		    var rule = i.attr('data-rule');

		    if( rule != undefined ){

				var ierror = false; // error flag for current input
				var pos = rule.indexOf(':', 0);
				if(pos >= 0){
					
					var exp = rule.substr( pos + 1, rule.length );
					rule = rule.substr(0, pos);
				}else{
					
					rule = rule.substr( pos+1, rule.length );
				}
				
				switch(rule){
					case 'required':
					if(i.val()==''){ ferror=ierror=true; }
					break;

					case 'maxlen':
					if(i.val().length > parseInt(exp)){ ferror=ierror=true; }
					break;
					
					case 'minlen':
					if(i.val().length < parseInt(exp)){ ferror=ierror=true; }
					break;

					case 'email':
					if(!emailExp.test(i.val())){ ferror=ierror=true; }
					break;

					case 'checked':
					if(!i.attr('checked')){ ferror=ierror=true; }
					break;
					
					case 'regexp':
					exp = new RegExp(exp);
					if(!exp.test(i.val())){ ferror=ierror=true; }
					break;
			    }
			    i.next('.validation').html( ( ierror ? (i.attr('data-msg') != undefined ? i.attr('data-msg') : 'wrong Input') : '' ) ).show('blind');
		    }
		});
		f.children('textarea').each(function(){ // run all inputs

		    var i = $(this); // current input
		    var rule = i.attr('data-rule');
		    if( rule != undefined ){
			var ierror=false; // error flag for current input
			var pos = rule.indexOf( ':', 0 );
			if( pos >= 0 ){
				
			    var exp = rule.substr(pos+1, rule.length);
			    rule = rule.substr(0, pos);
			}else{
				
			    rule = rule.substr(pos+1, rule.length);
			}
			
			switch(rule){
			    case 'required':
				if( i.val()=='' ){ ferror=ierror=true; }
				break;

			    case 'maxlen':
				if( i.val().length > parseInt(exp) ){ ferror=ierror=true; }
				break;
				
				case 'minlen':
				if(i.val().length < parseInt(exp)){ ferror=ierror=true; }
				break;
			  }
			  i.next('.validation')
				.html((ierror ? (i.attr('data-msg') != undefined ? i.attr('data-msg') : 'wrong Input') : ''))
				.show('blind');
		    }
		});
		if(ferror) {
			
			return false;
		} else {
			
			//var str = $(this).serialize();
			//all fields are valid
			return true;
		}
	}
	
	function startLoadingAnimation() // - функция запуска анимации
	{
	  // найдем элемент с изображением загрузки и уберем невидимость:
	  var imgObj = $("#loadImg");
	  $("#pop-up-wrap").css("z-index", "2001");
	  imgObj.show();
	 
	  // вычислим в какие координаты нужно поместить изображение загрузки,
	  // чтобы оно оказалось в серидине страницы:
	  //var centerY = $(window).scrollTop() + ($(window).height() + imgObj.height())/2;
	  //var centerX = $(window).scrollLeft() + ($(window).width() + imgObj.width())/2;
	 
	  // поменяем координаты изображения на нужные:
	  //imgObj.offset({top:centerY, left:centerX});
	}
	 
	function stopLoadingAnimation() // - функция останавливающая анимацию
	{
	  $("#loadImg").hide();
	  $("#pop-up-wrap").css("z-index", "-2001");
	}
	
	$('.form-control').change(validateFields);
	$('.form-control').keyup(validateFields);

	//Contact
	$('form.validateform').submit(function(event){
		//console.log("pre-validation");
		event.preventDefault();
		//console.log("preventDefault");
		if(!validateFields()){
			
			return false;
		}
		//console.log("post-validation");
		startLoadingAnimation();  // - запустим анимацию загрузки
		//console.log("ajax");
		  $.ajax({
			type: 'POST',
            url: '/Default/SendMessage',
			//contentType: 'text/plain',
			data: {
				'from_name': $('input#name').val(), // get name from form
				'from_email': $('input#email').val(), // get email from form
				'subject': $('input#subject').val(), // get subject from form
				'message': $('textarea[name="message"]').val() // get message from form
			},
			cache: false
		  }).done(function(responseText, textStatus, jqXHR) { // if got code 200
			  //console.log(responseText);
			  if(responseText == 'ok'){
				  
				  //console.log($("#sendmessage"));
				  $("#sendmessage").addClass("show").delay( 500 ).fadeIn( 500 );
				  $("#sendmessage").removeClass("show").delay( 5000 ).fadeOut( 500 );
			  } else {
				  
				  var tmpErrorText = $("#errormessage").text();
				  $("#errormessage").text("Сообщение не отправлено. Пожалуйста, сначала заполните все поля");
				  $("#errormessage").addClass("show").delay( 500 ).fadeIn( 500 );
				  $("#errormessage").removeClass("show").delay( 5000 ).fadeOut( 500 );
				  $("#errormessage").text(tmpErrorText);
			  }
			  stopLoadingAnimation();
		  }).fail(function(jqXHR, textStatus, errorThrown) {
			  
			//console.log($("#errormessage"));
			$("#errormessage").addClass("show").delay( 500 ).fadeIn( 500 );
			$("#errormessage").removeClass("show").delay( 5000 ).fadeOut( 500 );
			//console.log(jqXHR);
			//console.log('textStatus: ' + textStatus);
			stopLoadingAnimation();
		  });
		
		return false;
	}); 

});