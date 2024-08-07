
// JavaScript to add and remove the "running" class on hover
$(document).ready(function () {
	$('.icon').mouseenter(function () {
		// Select the video element inside the link and play it
		$(this).find('video')[0].play();
		$(this).addClass('running');
	});

	$('.icon').mouseleave(function () {
		// Select the video element inside the link and pause it
		var video = $(this).find('video')[0];
		video.pause();
		video.currentTime = 0; // Reset the video to the beginning
		$(this).removeClass('running');
	});

});