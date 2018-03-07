var drugs;

$(document).ready(function () {
    
    $("#ibtn-upload-attachment").click(function() {
        $.FileDialog({
            accept: "*", // Accept any file type
            dragMessage: "Drop files here",
            dropheight: 400,
            errorMessage: "An error occured while loading file",
            multiple: true, //enable uploading multiple files
            readAs: "DataURL", // file reading mode: BinaryString, Text, DataURL or ArrayBuffer
            removeMessage: "Remove&nbsp;file",
            title: "Upload Attachments"
        });
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
    
    $("#ibtn-add-prescription").click(function () {
        
        if (drugs == null) {
            $.ajaxSetup({ async: false });
            $.post("/Medicine/GetAllMedicines", {}, function (data) {
                var datalist = $("#drugs");
                drugs = data;

                for (var i = 0; i < drugs.length; i++) {
                    var option = document.createElement("option");
                    option.value = drugs[i].Name;
                    datalist.append(option);
                }

            });
        }
        
    })  
});  



