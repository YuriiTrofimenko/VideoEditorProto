var controls = {
    video: $("#videoPlayer video"),
    playPause: $(".playPause"),
    currentTime: $("time.currentTime"),
    duration: $(".player > header > time"),
    stop: $(".stop"),
    nextFrame: $(".nextFrame"),
    nextFrames: $(".nextFrames"),
    prevFrame: $(".prevFrame"),
    prevFrames: $(".prevFrames"),
    progressBar: $(".innerProgressBar"),
    screenShot: $(".screenShot"),
    runner: $(".runner"),
    mute: $('.mute'),
    volumeBar: $('.volumeBar'),
    volumeRunner: $('.volumeRunner'),
    volumeLine: $('.volumeLine'),
    volumeWrapp: $('.volumeWrapp')
	};

document.addEventListener('DOMContentLoaded', function()
{
    var video = controls.video[0],
        koef = 0,
        paused = false,
        fps = 25,
        frameTime = 1 / fps,
        videoParts = 20;

    function toggleVideo(elem)
    {
		(video.paused) ? video.play() : video.pause();
        elem.toggleClass("paused");
    }

    video.addEventListener('click', toggleVideo);

    controls.playPause.click(function(e){
    	e.preventDefault();
    	toggleVideo($(this));
    });

    function timeFormat (time)
    {
        var m = Math.floor(time / 60),
            s = Math.floor(time % 60),
            ms = Math.round(100*(time - parseInt(time)), 2);
        return  (m < 10? "0" + m : m) + ":" + 
                (s < 10? "0" + s : s) + ":" + 
                (ms < 10? "0" + ms : ms);
    }

    video.addEventListener("loadedmetadata", function() {
        controls.duration.text(timeFormat(video.duration));
        koef = controls.progressBar.width();
    });

    video.addEventListener("timeupdate", function() {
        var percent = video.currentTime / video.duration;

        if (koef == 0)
            koef = controls.progressBar.width();
        
        console.log(video.duration);
        controls.runner.css({left: (percent * koef) + "px"});
        controls.currentTime.text(timeFormat(video.currentTime));
    });

    controls.runner.draggable({
        axis: "x",
        containment: ".progressBar"
    });

    controls.runner[0].onmousedown = function() {
        if (!video.paused)
        {
            video.pause();
            paused = true;
        }
    };

    controls.runner[0].onmouseup = function() {
        var leftOffset = parseInt($(this).css('left'));
        video.currentTime = (video.duration * leftOffset / controls.progressBar.width()).toFixed(16);

        if (paused)
        {
            video.play();
            paused = false;
        }

        $(this).css('left', leftOffset);
        console.log($(this).css('left'));
        console.dir(video.currentTime);
    };

    controls.nextFrame.click(function(e) {
        e.preventDefault();
        video.currentTime += frameTime;
    });

    controls.nextFrames.click(function(e) {
        e.preventDefault();
        video.currentTime += video.duration / videoParts;
    });

    controls.prevFrame.click(function(e) {
        e.preventDefault();
        video.currentTime -= frameTime;
    });

    controls.prevFrames.click(function(e) {
        e.preventDefault();
        video.currentTime -= video.duration / videoParts;
    });

    controls.stop.click(function(e) {
        e.preventDefault();
        video.pause();
        video.currentTime = 0;
    });

    controls.mute.click(function(e) {
        e.preventDefault();

        if ($(this).hasClass("mutable"))
            video.muted = false;
        else
            video.muted = true;
            //video.volume = 0.5;

        $(this).toggleClass("mutable");
    });

    controls.screenShot.click(function(e) {
        e.preventDefault();
    });

    controls.volumeRunner.draggable({
        axis: "y",
        containment: ".volumeBar"
    });

    function checkVolumeRunner () {
        if (0 > parseInt(controls.volumeRunner.css('top')))
            controls.volumeRunner.css('top', 0 + "px");
    }

    controls.mute.mouseenter(function() {
        controls.volumeWrapp.css({'display': 'block'});
        checkVolumeRunner();
    });

    controls.volumeRunner.on('drag', function() {
        checkVolumeRunner();
        var vbHeight = controls.volumeBar.height() - 7,
            percent = 0;

        percent = parseInt(controls.volumeRunner.css('top')) / (vbHeight);
        video.volume = 1 - percent;
        controls.volumeLine.css("height", (1 - percent + 0.1) * vbHeight);
    });

    controls.volumeWrapp.mouseleave(function() {
        controls.volumeWrapp.css({'display': 'none'});
        checkVolumeRunner();
    })
});