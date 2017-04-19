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
    $( "#slider-range + p > input" ).val( $( "#slider-range" ).slider( "value" ) + "%" );

    $( '.submitField input[type="submit"]' ).click(function (event) {
    	event.preventDefault();
    	$( ".mask" ).fadeOut();
    	$( ".createSequence" ).fadeOut();
    });
});