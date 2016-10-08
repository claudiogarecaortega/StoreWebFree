function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        var percentComplete = Math.round(e.loaded * 100 / e.total);
        $("#FileProgress").css("width", percentComplete + '%').attr('aria-valuenow', percentComplete);
        $('#FileProgress span').text(percentComplete + "%");
    }
    else {
        $('#FileProgress span').text('unable to compute');
    }
}

function completeHandler() {
    $('#createView').empty();
    $('.CreateLink').show();
    $.unblockUI();
}


function successHandler(data) {
    if (data.statusCode == 200) {
        alertSuccessModal("Las imagenes se han guardado correctamente");
    }
    else {
        alertInfoModal("Ups :( algo a ha salido mal, por favor intentalo nuevamente");
        //alert(data.status);
    }
}

function errorHandler(xhr, ajaxOptions, thrownError) {
    alertUrgentMessageModal("Ha ocuurrido un error al guardar las imagenes, por favor intente nuevamente. \n error: " + thrownError)
    //alert("There was an error attempting to upload the file. (" + thrownError + ")");
}

function OnDeleteAttachmentSuccess(data) {

    if (data.ID && data.ID != "") {
        $('#Attachment_' + data.ID).fadeOut('slow');
    }
    else {
        alter("Unable to Delete");
        console.log(data.message);
    }
}

function Cancel_btn_handler() {
    $('#createView').empty();
    $('.CreateLink').show();
    $.unblockUI();
}