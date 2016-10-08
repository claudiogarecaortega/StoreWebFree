function mostrarNotificacionValid(mensaje) {


    $("#bodyModal").empty();
    $("#modalValidations").modal();
    var alert = "<div class=\"alert alert-success fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Ok!</strong> " + mensaje + ".</div>";
    $("#mensajeNotificacion").html(alert);
    $("#areaNotificacion").show(0).delay(30000).hide(0);
    $(".alert").alert();

}

function mostrarErrorValid(mensaje) {
    $("#modalValidations").modal();
    var alert = "<div class=\"alert alert-danger fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Error!</strong> " + mensaje + ".</div>";
    $("#areaErrorLogin").html(alert);
    $("#mensajeErrorLogin").show(0).delay(30000).hide(0);
    $(".alert").alert();
}


function mostrarErrorValidation(mensaje) {

    $("#modalValidationsErrors").modal();
    var alert = "<div class=\"alert alert-danger fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Error!</strong> " + mensaje + ".</div>";
    $("#areaErrorLoginVal").html(alert);
    $("#mensajeErrorLoginVal").show(0).delay(30000).hide(0);
    $(".alert").alert();
}
function alertUrgentMessage(mensaje) {
   
    var alert = "<div class='alert alert-danger alert-dismissable'><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><h4><i class='icon fa fa-ban'></i> Alert!</h4>" + mensaje + "</div>";
    $("#alertDiv").html(alert);
    $("#alertDiv").show(0);
    $(".alert").alert();
}
function alertUrgentMessageModal(mensaje) {

    var alert = "<div class='alert alert-danger alert-dismissable'><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><h4><i class='icon fa fa-ban'></i> Alert!</h4>" + mensaje + "</div>";
    $("#alertDivModalImagenes").html(alert);
    $("#alertDivModalImagenes").show(0);
    $(".alert").alert();
}
function alertSuccessModal(mensaje) {
   var alert = "<div class='alert alert-success alert-dismissable'><h4><i class='icon fa fa-check'></i> Alert!</h4>" + mensaje + "</div>";
   $("#alertDivModalImagenes").html(alert);
   $("#alertDivModalImagenes").show(0);
    $(".alert").alert();
}
function alertInfoModal(mensaje) {
  var alert = "<div class='alert alert-info alert-dismissable'><h4><i class='icon fa fa-info'></i> Alert!</h4>" + mensaje + "</div>";
  $("#alertDivModalImagenes").html(alert);
  $("#alertDivModalImagenes").show(0);
    $(".alert").alert();
}
function alertDanger(mensaje) {
    var delay = 3000;
    if (($("#modal").data('bs.modal') || {}).isShown) {
        $(".modalReusable").modal("hide");
        RefrescarGrilla();
        delay = 2000;
    }
    var alert = "<div class='alert alert-danger alert-dismissable'><h4><i class='icon fa fa-ban'></i> Alert!</h4>"+mensaje+"</div>";
    $("#alertDiv").html(alert);
    $("#alertDiv").show(0).delay(delay).hide(0);
    $(".alert").alert();
}
function alertWarnigButton(mensaje) {
    var delay = 3000;
    if (($("#modal").data('bs.modal') || {}).isShown) {
        $(".modalReusable").modal("hide");
        RefrescarGrilla();
        delay = 1000;
    }
    var alert = "<div class='alert alert-warning alert-dismissable'><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><h4><i class='icon fa fa-warning'></i> Alert!</h4>" + mensaje + "</div>";
    $("#alertDiv").html(alert);
    $("#alertDiv").show(0);
    $(".alert").alert();
}
function alertWarnig(mensaje) {
    var delay = 3000;
    if (($("#modal").data('bs.modal') || {}).isShown) {
        $(".modalReusable").modal("hide");
        RefrescarGrilla();
        delay = 1000;
    }
    var alert = "<div class='alert alert-warning alert-dismissable'><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><h4><i class='icon fa fa-warning'></i> Alert!</h4>" + mensaje + "</div>";
    $("#alertDiv").html(alert);
    $("#alertDiv").show(0).delay(delay).hide(0);
    $(".alert").alert();
}

function alertInfo(mensaje) {
    var delay = 3000;
    if (($("#modal").data('bs.modal') || {}).isShown) {
        $(".modalReusable").modal("hide");
        RefrescarGrilla();
        delay = 1000;
    }
    var alert = "<div class='alert alert-info alert-dismissable'><h4><i class='icon fa fa-info'></i> Alert!</h4>" + mensaje + "</div>";
    $("#alertDiv").html(alert);
    $("#alertDiv").show(0).delay(delay).hide(0);
    $(".alert").alert();
}
function alertSuccessReload(mensaje) {
    var delay = 3000;
    if (($("#modal").data('bs.modal') || {}).isShown) {
        $(".modalReusable").modal("hide");
        RefrescarGrilla();
        delay = 1000;
    }
    var alert = "<div class='alert alert-success alert-dismissable'><h4><i class='icon fa fa-check'></i> Alert!</h4>" + mensaje + "</div>";
    $("#alertDiv").html(alert);
    $("#alertDiv").show(0).delay(delay).hide(0);
    $(".alert").alert();
}
function alertSuccess(mensaje) {
    var delay = 3000;
    if (($("#modal").data('bs.modal') || {}).isShown) {
        $(".modalReusable").modal("hide");
        RefrescarGrilla();
        delay = 1000;
    }
    var alert = "<div class='alert alert-success alert-dismissable'><h4><i class='icon fa fa-check'></i> Alert!</h4>" + mensaje + "</div>";
    $("#alertDiv").html(alert);
    $("#alertDiv").show(0).delay(delay).hide(0);
    $(".alert").alert();
}
function block() {
    $.blockUI({
        css: {
            color: "#000",
            border: "none",
            backgroundColor: "none",
            cursor: "wait"
        },
        theme: false,
        baseZ: 2000,
        message: "<img style=\"width: 90px;\" src=\"../Content/Images/loading.gif\" />"
    });
}

function unBlock() {
    $.unblockUI({

    });
}

function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}