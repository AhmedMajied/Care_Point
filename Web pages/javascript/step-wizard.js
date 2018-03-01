current_step = 1;
function bar_progress(progress_line_object, direction) {
	var number_of_steps = progress_line_object.data('number-of-steps');
	var now_value = progress_line_object.data('now-value');
	var new_value = 0;
	if(direction == 'right') {
		new_value = now_value + ( 100 / number_of_steps );
	}
	else if(direction == 'left') {
		new_value = now_value - ( 100 / number_of_steps );
	}
	progress_line_object.attr('style', 'width: ' + new_value + '%;').data('now-value', new_value);
}

function update_step_nav_buttons(){
	if(current_step == 1){
		$("#ibtn-prev").prop('disabled', true);
		$("#ibtn-nxt").css('display', 'inline-block');
		$("#ibtn-submit").css('display', 'none');
	}
	else if(current_step == $('.f1').children().length){
		$("#ibtn-prev").prop('disabled', false);
		$("#ibtn-nxt").css('display', 'none');
		$("#ibtn-submit").css('display', 'inline-block');
	}
	else{
		$("#ibtn-prev").prop('disabled', false);
		$("#ibtn-nxt").css('display', 'inline-block');
		$("#ibtn-submit").css('display', 'none');
	}
}

jQuery(document).ready(function() {
    //Form
    $('.f1 .cdiv-step:first').fadeIn('slow');
    
    $('.f1 input[type="text"], .f1 input[type="password"], .f1 textarea').on('focus', function() {
    	$(this).removeClass('input-error');
    });
    
    // next step
    $('#ibtn-nxt').on('click', function() {
		var parent_fieldset = $('.f1 .cdiv-step:nth-child(' + current_step + ')');
    	var next_step = true;
    	// navigation steps / progress steps
    	var current_active_step = $('.modal-header').find('.f1-step.active');
    	var progress_line = $('.modal-header').find('.f1-progress-line');
    	
    	if( next_step ) {
    		parent_fieldset.fadeOut(400, function() {
				//update current step
				current_step++;
				update_step_nav_buttons();
    			// change icons
    			current_active_step.removeClass('active').addClass('activated').next().removeClass('activated').addClass('active');
    			// progress bar
    			bar_progress(progress_line, 'right');
    			// show next step
	    		$(this).next().fadeIn();
	    	});
    	}
    });
    
    // previous step
    $('#ibtn-prev').on('click', function() {
    	// navigation steps / progress steps
    	var current_active_step = $('.modal-header').find('.f1-step.active');
    	var progress_line = $('.modal-header').find('.f1-progress-line');
    	$('.f1 .cdiv-step:nth-child(' + current_step + ')').fadeOut(400, function() {
			//update current step
			current_step--;
			update_step_nav_buttons();
    		// change icons
    		current_active_step.removeClass('active').addClass('activated').prev().removeClass('activated').addClass('active');
    		// progress bar
    		bar_progress(progress_line, 'left');
    		// show previous step
    		$(this).prev().fadeIn();
    	});
	});
	
	$('.f1-step-icon').on('click', function(e){
		e.preventDefault();
		var selected_step = $(this).parent();
		if(selected_step.hasClass('activated')){
			var parent_fieldset = $('.f1 .cdiv-step:nth-child(' + current_step + ')');
			// navigation steps / progress steps
			var current_active_step = $('.modal-header').find('.f1-step.active');
			var progress_line = $('.modal-header').find('.f1-progress-line');
			var temp = current_step;
			parent_fieldset.fadeOut(400, function() {
				//update current step
				current_step = selected_step.prevUntil('.f1-steps').length;
				update_step_nav_buttons();
				// change icons
				current_active_step.removeClass('active').addClass('activated').next();
				selected_step.removeClass('activated').addClass('active');
				// progress bar
				if(temp < current_step){
					while(temp < current_step){
						bar_progress(progress_line, 'right');
						temp ++;
					}
				}
				else{
					while(temp > current_step){
						bar_progress(progress_line, 'left');
						temp --;
					}
				}
				// show next step
				$('.f1 .cdiv-step:nth-child(' + current_step + ')').fadeIn();
			});
		}
	});
});
