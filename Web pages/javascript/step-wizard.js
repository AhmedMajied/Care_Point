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
    	
    	// fields validation
    	parent_fieldset.find('input[type="text"], textarea').each(function() {
    		if( $(this).val().trim() == "" ) {
    			$(this).addClass('input-error');
    			next_step = false;
    		}
    		else {
    			$(this).removeClass('input-error');
    		}
    	});
    	
    	if( next_step ) {
    		parent_fieldset.fadeOut(400, function() {
				//update current step
				current_step++;
				if($('.f1').children().length == current_step){
					$("#ibtn-nxt").css('display', 'none');
					$("#ibtn-submit").css('display', 'inline-block');
				}
				if($("#ibtn-prev").is(':disabled')){
					$("#ibtn-prev").prop('disabled', false);
				}
    			// change icons
    			current_active_step.removeClass('active').addClass('activated').next().addClass('active');
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
			if(current_step == 1){
				$("#ibtn-prev").prop('disabled', true);
			}
			if($("#ibtn-submit").css('display') == 'inline-block'){
				$("#ibtn-submit").css('display', 'none');
				$("#ibtn-nxt").css('display', 'inline-block');
			}
    		// change icons
    		current_active_step.removeClass('active').prev().removeClass('activated').addClass('active');
    		// progress bar
    		bar_progress(progress_line, 'left');
    		// show previous step
    		$(this).prev().fadeIn();
    	});
	});
	
	$('.f1-step-icon').on('click', function(e){
		e.preventDefault();
		if($(this).parent().hasClass('activated')){
			var parent_fieldset = $('.f1 .cdiv-step:nth-child(' + current_step + ')');
			var next_step = true;
			// navigation steps / progress steps
			var current_active_step = $('.modal-header').find('.f1-step.active');
			var progress_line = $('.modal-header').find('.f1-progress-line');
			
			// fields validation
			parent_fieldset.find('input[type="text"], textarea').each(function() {
				if( $(this).val().trim() == "" ) {
					$(this).addClass('input-error');
					next_step = false;
				}
				else {
					$(this).removeClass('input-error');
				}
			});
			
			if( next_step ) {
				parent_fieldset.fadeOut(400, function() {
					//update current step
					current_step++;
					if($('.f1').children().length == current_step){
						$("#ibtn-nxt").css('display', 'none');
						$("#ibtn-submit").css('display', 'inline-block');
					}
					if($("#ibtn-prev").is(':disabled')){
						$("#ibtn-prev").prop('disabled', false);
					}
					// change icons
					current_active_step.removeClass('active').addClass('activated').next().addClass('active');
					// progress bar
					bar_progress(progress_line, 'right');
					// show next step
					$(this).next().fadeIn();
				});
			}
		}
	});
});
