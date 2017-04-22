$(function () {
	$("a.signin").click(function (event) {
		event.preventDefault();
		$(".signin.modal").fadeIn();
		$(".mask").fadeIn();
	});

	$("a.signup").click(function (event) {
		event.preventDefault();
		$(".signup.modal").fadeIn();
		$(".mask").fadeIn();
	});

	$(".mask").click(function () {
		$(".modal").fadeOut();
		$(".mask").fadeOut();
	});
});