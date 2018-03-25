$(document).ready(function () {
    $("#ibtn-upload-attachment").click(function() {
<<<<<<< HEAD
        var chosen_files = $.FileDialog({
=======

        $.FileDialog({

>>>>>>> master
            accept: "*", // Accept any file type
            dragMessage: "Drop files here",
            dropheight: 400,
            errorMessage: "An error occured while loading file",
            multiple: true, //enable uploading multiple files
            readAs: "DataURL", // file reading mode: BinaryString, Text, DataURL or ArrayBuffer
            removeMessage: "Remove&nbsp;file",
            title: "Upload Attachments"
        });
<<<<<<< HEAD
        chosen_files.on('files.bs.filedialog', function(ev) {
            var files_list = ev.files;
            for(var i=0; i<files_list.length; i++){
                console.log(files_list[i].name);
            }
        });
=======

>>>>>>> master
    });

    $(".cbtn-add").click(function(){
        var empty_input = false;
        // fields validation
    	$(this).closest('.cdiv-step').find('input').each(function() {
    		if( $(this).val().trim() == "" && $(this).hasClass('cinp-dose') == false) {
    			$(this).addClass('input-error');
    			empty_input = true;
    		}
    		else {
    			$(this).removeClass('input-error');
    		}
        });
        if(empty_input == false){
            var copy = $(this).closest('.row').clone(true);
            copy.find("input").val("");
            $(this).addClass('hidden');
            $(this).prev().removeClass('hidden');
            $(this).closest('.container-fluid').append(copy);
        }
    });

    $('.btn-danger').click(function(){
        $(this).closest('.row').remove();
    });
});  
