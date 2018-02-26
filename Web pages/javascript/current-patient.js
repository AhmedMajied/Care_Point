$(document).ready(function () {
    $("#ibtn-upload-attachment").click(function() {
        $.FileDialog({
            accept: "*", // Accept any file type
            cancelButton: "Close",
            dragMessage: "Drop files here",
            dropheight: 400,
            errorMessage: "An error occured while loading file",
            multiple: true, //enable uploading multiple files
            readAs: "DataURL", // file reading mode: BinaryString, Text, DataURL or ArrayBuffer
            removeMessage: "Remove&nbsp;file",
            title: "Upload Attachments"
        });
    }); 
});  
