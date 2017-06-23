$(function () {
	$(".dropdownList").click(function () {
		$(this).children(".dropdown").fadeToggle();
	});

	$( "#slider-range" ).slider({
		range: "max",
		min: 1,
		max: 100,
		value: 1,
		slide: function( event, ui ) {
			$( "#slider-range + p > input" ).val( ui.value + "%");
		}
    });

    $("#slider-range-blue, #slider-range-red, #slider-range-green").slider({
        range: "max",
        min: 1,
        max: 255,
        value: 1,
        slide: function (event, ui) {
            $(this).siblings("p").text(ui.value);
        }
    });

    $( "#slider-range + p > input" ).val( $( "#slider-range" ).slider( "value" ) + "%" );

    $( '.submitField input[type="submit"]' ).click(function (event) {
    	event.preventDefault();
    	$( ".mask" ).fadeOut();
    	$( ".createSequence" ).fadeOut();
    });

    $(".mainHeader > ul > li > a").click(function (event) {
    	event.preventDefault();
    	$(this).children("ul").slideToggle();
    });

    $(".openProjects").click(function () {
        $(".openSequence").fadeIn();
        $(".mask").fadeIn();
        console.log("hui");
    });

    $(".mask").click(function () {
        $(".openSequence").fadeOut();
        $(".mask").fadeOut();
    });
});