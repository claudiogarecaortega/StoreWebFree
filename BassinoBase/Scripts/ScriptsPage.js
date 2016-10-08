$(document).ready(function () {
    // $(".select2").select2();
    $('.modalReusable').on('hidden.bs.modal', '.modal', function () {
        $(this).removeData('bs.modal');
        RefrescarGrilla();
        $("input[type=\"checkbox\"]").bootstrapSwitch('destroy');
    });
    
    $('.modalReusable').on('show.bs.modal', function () {
        //block();
    })
    $('.modalReusable').on('shown.bs.modal', function () {

    })
    $('.modalReusable').on('loaded.bs.modal', function (e) {
        // do cool stuff here all day… no need to change bootstrap
        //s unBlock();
    })
    CargarModalsReusablesLogin();
    checkLogin();
    CargarModalsReusables();
    LoadIndexPage();


    // $("[name='my-checkbox']").bootstrapSwitch();
    // LoadPartial("/Interfaz/Manage/_Index", "ContentBody"); // ConfigurarValidatorParaQueMuestreBienElEstiloDeLosErrores();
    // MostrarErroresDeValidacionEnLosFormularios();
});

$(document).ajaxComplete(function (event, request, settings) {
    // MostrarErroresDeValidacionEnLosFormularios();
    // CargarValidacionParaLosFormulariosQueSeCarganPorAjax();
});
function updateTitle(mensaje, moduloPadre) {
    var message = mensaje;
    $("#titleUpdate").text("");
   // $("#titleUpdate").append(message);
    var im = "<li><a href='#' onclick=\"LoadPartial('Home/Content', 'mainDiv' )\" id='moduloid'><i class='fa fa-home'></i> Inicio</a></li>" +
       "<li class='active' id='SubModulo'>" + moduloPadre + " / <sapn style='font-weight: bold'>" + mensaje + "</span></li>";

    $("#ModuloId").text("");
    $("#ModuloId").text(moduloPadre);
    $("#SubModulo").text("");
    $("#SubModulo").text(message);
    $("#infoId").text("");
    $("#infoId").append(im);
}
function readActions(controller, action) {
    var bandera = false;
    //run through each row
    $('.k-selectable tr').each(function (i, row) {

        // reference all the stuff you need first
        var $row = $(row),
            $family = $row.find('input[name*="actions"]');

        var id = $family[0].id;
        var url = controller + '/' + action + '/' + id + '';
        if ($family[0].checked) {
            bandera = true;
            //   $('#modal').modal('toggle', { backdrop: 'static', keyboard: false });
            $.get(url, function (data) {
                $('#modal').html(data);

                $('#modal').modal({ backdrop: 'static', keyboard: false });
            });
        }


    });
    if (!bandera) {
        var ac = "";
        if (action == "Edit")
            ac = "Editar";
        if (action == "Delete")
            ac = "Emilinar";
        if (action == "Details")
            ac = "ver los Detalles";

        alertWarnig("Debe seleccionar un Item para poder: " + ac);
    }
}
function readActionsNotifications() {
    var bandera = false;
    //run through each row
    $('.k-selectable tr').each(function (i, row) {

        // reference all the stuff you need first
        var $row = $(row),
            $family = $row.find('input[name*="actions"]');
        var url = "";
        var id = $family[0].id;
      

        
        if ($family[0].checked) {
            notificationReadToolbar(id);
            bandera = true;
            $.ajax({
                type: "POST",
                url: "/Notification/GetNotification",
                datatype: "JSON",
                data: { text: id },
                success: function (data) {
                    url = data.Url;
                  
                    //   $('#modal').modal('toggle', { backdrop: 'static', keyboard: false });
                    $.get(url, function (data) {
                        $('#modal').html(data);

                        $('#modal').modal({ backdrop: 'static', keyboard: false });
                    });
                }
            });
           
        }


    });
    if (!bandera) {
        
        

        alertWarnig("Debe seleccionar un Item para poder ver los Detalles" );
    }
}

function disableOnChecked(ids) {
    if ($("#" + ids)[0].checked) { $("#" + ids).val("0"); }
    else { $("#" + ids).val("1"); }
    //run through each row
    $('.k-selectable tr').each(function (i, row) {

        // reference all the stuff you need first
        var $row = $(row),
            $family = $row.find('input[name*="actions"]');
        var valor = $("#" + ids).val();
        var id = $family[0].id;
        //if ($family[0].checked) {
        if (id == ids) {
            if (!$("#" + ids)[0].checked)
                $("#" + ids).attr('checked', false);
            else
                $("#" + ids).attr('checked', true);
            // $("#" + id).removeAttr("disabled");

        } else {
            $("#" + id).attr('checked', false);
        }
        //else {

        //    if (valor == "1")
        //        $("#" + id).removeAttr("disabled");
        //    else
        //        $("#" + id).attr("disabled", true);

        //}



    });
}
function disableOnCheckedShip(ids) {

    if ($("#" + ids)[0].checked) { $("#" + ids).val("0"); }
    else { $("#" + ids).val("1"); }
    //run through each row
    $('.k-selectable tr').each(function (i, row) {

        // reference all the stuff you need first
        var $row = $(row),
            $family = $row.find('input[name*="actions"]');
        var valor = $("#" + ids).val();
        var id = $family[0].id;

        if (id == ids) {
            if (!$("#" + ids)[0].checked)
                $("#" + ids).attr('checked', false);
            else
                $("#" + ids).attr('checked', true);
            // $("#" + id).removeAttr("disabled");

        } else {
            $("#" + id).attr('checked', false);
        }
        //if ($family[0].checked) {
        //    $("#" + id).removeAttr("disabled");

        //} else {

        //    if (valor == "1")
        //        $("#" + id).removeAttr("disabled");
        //    else
        //        $("#" + id).attr("disabled", true);

        //}



    });
    disableButtons(ids);
}
function disableButtons(id) {
    $('#' + id).change(function () {
        if ($(this).is(":checked")) {
            $.ajax({
                type: "POST",
                data: { id: id },
                url: "/Shipment/GetStatus",

                success: function (data) {
                    if (data.send) {
                        $("#iniciar").hide();


                    } else {
                        $("#iniciar").css({ 'display': "inline" });
                        $("#endship").hide();
                        $("#rec").hide();
                        $("#send").hide();
                    }
                    if (data.end) {
                        $("#iniciar").hide();
                        $("#endship").hide();
                        $("#rec").hide();
                        $("#send").hide();
                    } else {
                        $("#endship").css({ 'display': "inline" });
                    }

                }
            });
        } else {
            $("#endship").css({ 'display': "inline" });
            $("#iniciar").css({ 'display': "inline" });
            $("#rec").css({ 'display': "inline" });
            $("#send").css({ 'display': "inline" });
        }
    });

}
function checkContract(id) {
    $.ajax({
        type: "POST",
        data: { id: id },
        url: "/Client/ReadContract",

        success: function (data) {
            if (data.result) {
                $("#syncContract").hide();
                $("#editContract").css({ 'display': "inline" });
            } else {
                $("#syncContract").css({ 'display': "inline" });
                $("#editContract").hide();
            }

        }
    });
}
function disableOnCheckedClient(ids) {
    checkContract(ids);
    if ($("#" + ids)[0].checked) { $("#" + ids).val("0"); }
    else { $("#" + ids).val("1"); }
    //run through each row
    $('.k-selectable tr').each(function (i, row) {

        // reference all the stuff you need first
        var $row = $(row),
            $family = $row.find('input[name*="actions"]');
        var valor = $("#" + ids).val();
        var id = $family[0].id;
        if (id == ids) {
            if (!$("#" + ids)[0].checked)
                $("#" + ids).attr('checked', false);
            else
                $("#" + ids).attr('checked', true);
            // $("#" + id).removeAttr("disabled");

        } else {
            $("#" + id).attr('checked', false);
        }
        //if ($family[0].checked) {
        //    $("#" + id).removeAttr("disabled");

        //} else {

        //    if (valor == "1")
        //        $("#" + id).removeAttr("disabled");
        //    else
        //        $("#" + id).attr("disabled", true);

        //}



    });
}
function validaCuit(sCUIT) {

    var aMult = '5432765432';
    var aMult = aMult.split('');

    if (sCUIT && sCUIT.length == 11) {
        aCUIT = sCUIT.split('');
        var iResult = 0;
        for (i = 0; i <= 9; i++) {
            iResult += aCUIT[i] * aMult[i];
        }
        iResult = (iResult % 11);
        iResult = 11 - iResult;

        if (iResult == 11) iResult = 0;
        if (iResult == 10) iResult = 9;

        if (iResult == aCUIT[10]) {
            return true;
        }
    }
    return false;
}
function LoadIndexPage() {
    LoadPartial("/Home/Content", 'mainDiv');
}

function Pdf(url) {
    $.ajax({
        type: "POST",
        url: url,
        processData: false,
        contentType: "application/xml; charset=utf-8",
        success: function (data) {
            var iframe = $('<iframe>');
            iframe.attr('src', '/pdf/yourpdf.pdf?options=first&second=here');
            $('#targetDiv').append(iframe);
        }
    });
}
function alertRead(id) {
    $.ajax({
        type: "POST",
        data: { id: id },
        url: "/Alert/ReadAlert",

        success: function (data) {

        }
    });
}
function alertNotification(id) {
    $.ajax({
        type: "POST",
        data: { id: id },
        url: "/Notification/ReadAlert",

        success: function (data) {

        }
    });
}
function alertReadToolbar(id) {
    $.ajax({
        type: "POST",
        data: { id: id },
        url: "/Alert/ReadAlert",

        success: function (data) {
            var valueLi = "";
            var numero = Number($("#valueAlert").text());
            numero -= 1;
            if (numero < 0) {
                $("#valueAlertNoti").text("0");
                $("#valueAlert").text("0");

            } else {
                $("#valueAlertNoti").text(numero);
                $("#valueAlert").text(numero);
            }

        }
    });
}
function notificationReadToolbar(id) {
    $.ajax({
        type: "POST",
        data: { id: id },
        url: "/Notification/ReadAlert",

        success: function (data) {
            var valueLi = "";
            var numero = Number($("#notificationNumber").text());
            numero -= 1;
            if (numero < 0) {
                $("#notificationNumber").text("0");
                $("#notificationLabel").text("Ud tiene 0 notificaciones");

            } else {
                $('#notili_' + id).remove();
                $("#notificationNumber").text(numero);
                $("#notificationLabel").text("Ud tiene " + numero + " notificaciones");
            }

        }
    });
}
function checkLogin() {
    $.ajax({
        type: "POST",
        url: "/User/CheckLogin",
        datatype: "JSON",
        success: function (data) {
            if (data == 'False') {
                $.get('/user/Login', function (data) {
                    $("#modalLogin").html(data);
                });
                $("#modalLogin").modal({
                    keyboard: false
                });

            } else {
                $.ajax({
                    type: "POST",
                    url: "/User/CheckFirstLogin",
                    datatype: "JSON",
                    success: function (data) {
                        if (data == 'True') {
                            alertDanger('debe actualizar su informacion');
                            LoadPartial('User/_UserProfile', 'mainDiv');

                        }
                    }
                });
            }
        }
    });
}

function CargarModalsReusables() {
    /// $('.modal-dialog').on('hidden.bs.modal', function () {
    $(".modalReusable").on("hidden.bs.modal", function () {
        $("input[type=\"checkbox\"]").bootstrapSwitch('destroy');
        $(this).removeData("bs.modal");
        updateMessagesModal();
    });
}
function CargarModalsReusablesLogin() {
    // $('.modal-dialog').on('hidden.bs.modal', function () {
    $("#modalLogin").on("hidden.bs.modal", function () {

        checkLogin();
    });
}
function modalCalendar(s) {
    var item = $(s).attr("data-cal-date");
    alert(item);
}
function modalSave(message) {
    $(".modalReusable").modal("hide");
    $("#ModalAlert").modal({
        keyboard: false
    });
    $("#MesasgeModal").text(message);
    $("#ModalAlert").on("shown.bs.modal", function () {
        $(this).delay(1000).queue(function () {
            $(this).modal("hide").dequeue();
        });
    });
    RefrescarGrilla();
}
function modalSaveShowData(message) {
    $(".modalReusable").modal("hide");
    $("#ModalAlert").modal({
        keyboard: false,
        backdrop: 'static'
    });
    $("#MesasgeModal").text(message);
    $("#myModalLabel").text("Bassino .Net");
    $("#ModalAlert").on("shown.bs.modal", function () {
        //$(this).delay(1000).queue(function () {
        //    $(this).modal("hide").dequeue();
        //});
    });
    RefrescarGrilla();
}
function modalSaveRefresAgenda(message) {
    $(".modalReusable").modal("hide");
    $("#ModalAlert").modal({
        keyboard: false
    });
    $("#MesasgeModal").text(message);
    $("#ModalAlert").on("shown.bs.modal", function () {
        $(this).delay(1000).queue(function () {
            $(this).modal("hide").dequeue();
        });
    });
    LoadPartial('/Interfaz/Bar/_Calendar', 'calendario');
}

function ConfigurarValidatorParaQueMuestreBienElEstiloDeLosErrores() {
    $.validator.setDefaults({
        highlight: function (element, errorClass, validClass) {
            if (element.type == "radio") {
                this.findByName(element.name).addClass(errorClass).removeClass(validClass);
            } else {
                $(element).addClass(errorClass).removeClass(validClass);
                $(element).closest(".form-group").addClass("has-error");
            }
        },
        unhighlight: function (element, errorClass, validClass) {
            if (element.type == "radio") {
                this.findByName(element.name).removeClass(errorClass).addClass(validClass);
            } else {
                $(element).removeClass(errorClass).addClass(validClass);
                $(element).closest(".form-group").removeClass("has-error");
            }
        }
    });
}

function MostrarErroresDeValidacionEnLosFormularios() {
    $("form").each(function () {
        $(this).find("div.form-group").each(function () {
            if ($(this).find("span.field-validation-error").length > 0) {
                $(this).addClass("has-error");
            }
        });
    });
}

function CargarValidacionParaLosFormulariosQueSeCarganPorAjax() {
    $.validator.unobtrusive.parse("form");
}

//$.ajaxSetup({
//    error: function(request) {
//        window.location = "Error/ErrorInterno";
//    }
//});

function gridOnChangeModal(arg) {
    var eventTarget = (event.target) ? $(event.target) : $(event.srcElement);
    var EsAccion = eventTarget.closest("td").hasClass("acciones");

    if (EsAccion)
        return;

    var item = this.select();
    var id = this.dataItem(item).id;

    var controller = this.select().closest("div[data-role=\"grid\"]").attr("controller");

    $("#modal").removeData();

    $("#modal").modal({
        remote: "../" + controller + "/Details/" + encodeURIComponent(id)
    });

    $("#modal").modal("show");
}

function gridOnChange(arg) {
    var eventTarget = (event.target) ? $(event.target) : $(event.srcElement);
    var EsAccion = eventTarget.closest("td").hasClass("acciones");

    if (EsAccion)
        return;

    var item = this.select();
    var id = this.dataItem(item).id;

    var controller = this.select().closest("div[data-role=\"grid\"]").attr("controller");

    window.location = "../" + controller + "/Details/" + encodeURIComponent(id);
}

function gridOnChangeNone(arg) {
    return false;
}

function updateMessagesModal() {
    var valueLi = "";
    var numero = Number($("#valueNotification").text());
    if (numero > 0) {
        numero -= 1;
        $("#valueNotification").text(numero);
        $("#liheader").text("Ud tiene : " + numero + " mensajes");
        $.ajax({
            type: "POST",
            url: "/Messaging/GetNewMessages",
            datatype: "JSON",
            success: function (data) {
                $("#messageList").empty();
                $.each(data, function (i, item) {
                    if (item.IsUrgent) {
                        valueLi = "<li><a href='/Messaging/Details/" + item.id + "'  data-target='#modal' data-toggle='modal' data-placement='top'><div class='pull-left'><img src='dist/img/user2-160x160.jpg' class='img-circle' alt='User Image'></div><h4>" + item.from + "<small><i class='fa fa-clock-o'></i>" + item.time + "</small></h4><p>" + item.subject + "</p></a></li>";
                        $('#messageList').append(valueLi);
                    } else {
                        valueLi = "<li style='background-color:red'><a href='/Messaging/Details/" + item.id + "'  data-target='#modal' data-toggle='modal' data-placement='top'><div class='pull-left'><img src='dist/img/user2-160x160.jpg' class='img-circle' alt='User Image'></div><h4>" + item.from + "<small><i class='fa fa-clock-o'></i>" + item.time + "</small></h4><p>" + item.subject + "</p></a></li>";
                        $('#messageList').append(valueLi);
                    }
                });

            }
        });
    }
}

//******************************************************
function ContentChange(viewId, menu) {
    $("#_profile, #_index").each(function () {
        //debugger 
        if ($.trim($(this)[0].id) == viewId) {
            $(this).show();
            $(menu).addClass("active-menu");
            return false;
        } else {
            $(menu).removeClass("active-menu");

            $(this).hide();
        }
    });
}


//******************************************************

function AbrirPopup(sUrl, iWidth, iHeight, iLeft, iTop, Scroll, wName) {
    var oWin;
    var sLeft = "";
    var sTop = "";
    var sWidth = "";
    var sHeight = "";

    //Defino el alto y ancho de la ventana si no fue establecido
    iWidth = (iWidth || -1);
    iHeight = (iHeight || -1);
    if (iWidth <= 0) {
        iWidth = 1024;
    }
    sWidth = "width=" + iWidth;

    if (iHeight <= 0) {
        iHeight = 768;
    }
    sHeight = ",height=" + iHeight;

    //Determino la ubicacion que me pidio o bien centro el popup
    if (!iLeft || iLeft <= 0) {
        iLeft = parseInt(screen.width) / 2;
        iLeft = iLeft - iWidth / 2;
    }
    if (!iTop || iTop <= 0) {
        iTop = parseInt(screen.height) / 2;
        iTop = iTop - iHeight / 2;
    }

    sLeft = ",left=" + iLeft;
    sTop = ",top=" + iTop;

    oWin = window.open(sUrl, wName, sWidth + sHeight + sLeft + sTop + ",toolbar=no,menubar=no,location=no,resizable=yes,maximized=yes,scrollbars=1");
    return oWin;
}

//function to update panel quick into user content

function getContent() {

    if ($("#_Imbox").is(":visible"))
        $("#_Imbox").hide();
    else
        $("#_Imbox").show();


}

function Reply(id) {
    var control = $("#" + id);
    if (control.is(":visible")) {
        control.hide();
        $("#btnSubmit").val("Reply");
    } else {
        control.show();
        $("#btnSubmit").val("Cancel");
    }

}

function showContent(id) {
    CargarModalsReusables();
    var control = $("#" + id);
    if (control.is(":visible")) {
        control.hide();
        $("#_index").show();

    } else {
        control.show();
        $("#_index").hide();
    }
}

function additionalParameters() {
    return {
        filtro: $("#filterText").val()
        // EmpresaID: $("#EmpresaID").val()
    };
}
//url absoluta
function SaveLogin() {
    $.ajax({
        type: "POST",
        url: "/Interfaz/Account/SaveLogin",
        datatype: "html",
        success: function (data) {

        }
    });
}

function resizeGrid() {
    var gridElement = $("#Grid"),
        dataArea = gridElement.find(".k-grid-content").first(),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 0;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(gridHeight - otherElementsHeight);
}

$(window).resize(function () {
    resizeGrid();
});

function RefrescarGrilla() {
    $(".k-grid").each(function () {
        $(this).data("kendoGrid").dataSource.page(1);
        $(this).data("kendoGrid").dataSource.read();
    });
}