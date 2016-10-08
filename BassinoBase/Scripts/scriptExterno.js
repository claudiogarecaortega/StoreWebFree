function fn_cantidad() {
    cantidad = $("#grilla tbody").find("tr").length;
    $("#span_cantidad").html(cantidad);
};

function LoadCotnent() {
    $(".js-reload").on("click", function(evt) {

        evt.preventDefault();
        evt.stopPropagation();
        var $_Stores = $("#ContentBody");
        var url = $(this).data("url");

        $.get(url, function (data) {
            block();
            $("#ContentBody").html(data);
        }).done(function() {
            unBlock();
        });
    });
}

function LoadIndex() {
    $(".js-reload-Index").on("click", function(evt) {

        evt.preventDefault();
        evt.stopPropagation();
        var $_Stores = $("#ContentIndex");
        var url = $(this).data("url");

        $.get(url, function(data) {
            $("#ContentIndex").html(data);
        });
    });
}
function loadOccurrence() {

    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Interfaz/Occurrence/GetOccurrences?idDependiente=" + $("#IdDependiente").val() + "&isBar=" + $("#IsBar").val(), //Relative or absolute path to response.php file
        success: function (data) {

            var r = JSON.stringify(data);
            return r;
        }
    });
}

function setActive(idIl, idUl) {
    var i = "#" + idIl + " li";
    $(i).each(function () {
      //  debugger
        // this is inner scope, in reference to the .phrase element
        var current = $(this);
       
        if (current[0].id == idUl)
                current.addClass("active");
            else
                current.removeClass("active");
            // add current text to our current phrase
           // phrase += current.text();
      
        // now that our current phrase is completely build we add it to our outer array
       // phrases.push(phrase);
    });
}

function LoadPartial(ur, container) {

    var url = ur;
    block();
    $.get(url, function (data) {
       
        $("#" + container + "").html(data);
    }).done(function () {
        unBlock();
    });
}
function LoadPartialMenu(ur, container, idIl, idUl) {
    LoadPartial(ur, container);
    setActive(idIl, idUl);

}

function LoadPartialId(ur, container, id) {

    var url = ur + "/" + id;
    block();
    $.get(url, function(data) {
        $("#" + container + "").html(data);
    }).done(function () {
        unBlock();
    });
}


function LoadIndexfun(ur) {


    var $_Stores = $("#ContentIndex");
    var url = ur;

    $.get(url, function(data) {
        $("#ContentIndex").html(data);
    });
}

function fn_dar_eliminar() {
    $("a.elimina").click(function() {
        id = $(this).parents("tr").find("td").eq(0).html();
        respuesta = confirm("Desea eliminar el Codigo?");
        if (respuesta) {
            $(this).parents("tr").fadeOut("normal", function() {
                $(this).remove();
                alert("Codigo  eliminado");
            });
        }
    });
};

function Editar() {
    var uno = "";
    $("#GridPrese tr").click(function() {
        var uno = $(this).children();
        //$(this).css('color', 'white');
        //$("#acciones").fadeIn('slow');
        //var a = $(".td1:first").val();

    });
    alert(uno);
}

function agregar() {
    var ComboPresentacion = document.getElementById("DescripcionPresentacion");
    var idprese = ComboPresentacion.value;
    var textpresentacion = ComboPresentacion.options[ComboPresentacion.selectedIndex].text;
    var CodigodeBarras = document.getElementById("CodigoDeBarras").value;
    var ViasAdministracion = document.getElementById("viaMedicacion");
    var viasseleccionadas = "";
    $("#viaMedicacion option").each(function() {
        if ($(this).attr("selected")) {
            viasseleccionadas = viasseleccionadas + " - " + $(this).text();
        }

    });
    var i = $("#GridPrese > tbody > tr").length;
    cadena = "<tr onclick='Editar()'>";

    cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].MonodrogaID' name='ListaDeCodigos[" + i + "].MonodrogaID' value='" + 0 + "' /></td>";
    cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].CodigoAtcID' name='ListaDeCodigos[" + i + "].CodigoAtcID' value='" + 0 + "' /></td>";
    cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].CodigoAtc' name='ListaDeCodigos[" + i + "].CodigoAtc' value='" + 0 + "' /></td>";
    cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].CodigoMonodroga' name='ListaDeCodigos[" + i + "].CodigoMonodroga' value='" + 0 + "' /></td>";
    cadena = cadena + "<td >   " + textpresentacion + "</td>";

    cadena = cadena + "<td > " + viasseleccionadas + " </td>";
    cadena = cadena + "<td > " + CodigodeBarras + " </td>";

    //cadena = cadena + "<td><a class='elimina'><img src='/mhoweb/Images/delete.png' onclick='fn_dar_eliminar()' /></a></td>";
    cadena = cadena + "</tr>";
    $("#GridPrese tbody").append(cadena);

}

function showModal(modalContainerId, modalBodyId, id) {

    var url = $(modalContainerId).data("url");

    $.get(url, { id: id }, function(data) {
        $(modalBodyId).html(data);
        $(modalContainerId).modal("show");
    });
}

function lanzarAvanzado() {
    /*	var ur = document.URL;
	            var valor= document.getElementById()
            	$.ajax({
            		url: ur + + "&nivel=" + idSelected2
					 ,
            		type: "POST",
            		cache: false,
            		dataType: 'json',
            		success: function (Data) {
            			var options = "";
            			$.each(Data, function (index, values) {
            				options += '<option value="' + values.Value + '">' + values.Text + '</option>';
            			});
            			$("#" + nombre).html(options);
            		},
            		error: function () {


            		}
            	});
				*/
}

function CambioCascada(selectedElement, idSelected2, nombre, urls) {
    $.ajax({
        url: urls + "GetSubValues?id=" + selectedElement.val() + "&nivel=" + idSelected2,
        type: "POST",
        cache: false,
        dataType: "json",
        success: function(Data) {
            var options = "";
            $.each(Data, function(index, values) {
                options += "<option value=\"" + values.Value + "\">" + values.Text + "</option>";
            });
            $("#" + nombre).html(options);
        },
        error: function() {


        }
    });
}


$(document).on("ready", function() {
    var ur = document.URL;
    var n = ur.search("Edit");
    if (n == -1)
        ur = "./";
    else
        ur = "../";

    $(document).delegate("#Nivelpadre", "change", function() {
        CambioCascada($(this), 2, "Nivelpadre2", ur);
    });
    $(document).delegate("#Nivelpadre2", "change", function() {
        CambioCascada($(this), 3, "Nivelpadre3", ur);
    });
    $(document).delegate("#Nivelpadre3", "change", function() {
        CambioCascada($(this), 4, "Nivelpadre4", ur);
    });
    //Evento que actualiza los valores de los combos al momento de seleccionar algun valor.
    $(document).delegate("#MP_Nivelpadre", "change", function() {
        CambioCascada($(this), 2, "MP_Nivelpadre2", ur);
    });
    $(document).delegate("#MP_Nivelpadre2", "change", function() {
        CambioCascada($(this), 3, "MP_Nivelpadre3", ur);
    });
    $(document).delegate("#MP_Nivelpadre3", "change", function() {
        CambioCascada($(this), 4, "MP_CodigoAtcNivel", ur);
    });
    $(document).delegate("#MP_CodigoAtcNivel", "change", function() {
        CambioCascada($(this), 5, "MP_Nivelpadre5", ur);
    });
    $(document).delegate("#MP_CodigoAtcNivel", "change", function() {
        idSelected = $(this).val();
        $.ajax({
            url: "" + ur + "GetCodigoAtc?id=" + idSelected,
            type: "POST",
            cache: false,
            dataType: "json",
            success: function(Data) {
                $("#MP_Codigoatc").val(Data).attr("readonly", true);

            },
            error: function() {
                alert("error...");
            }
        });
    });
    $(document).delegate("#add", "click", function() {

        var mensaje = document.getElementById("Codigoo").value;
        var mensaje2 = document.getElementById("CodigooMono").value;


        var nivel5 = document.getElementById("Nivelpadre4");
        var descripcion = nivel5.options[nivel5.selectedIndex].text;
        var id = nivel5.options[nivel5.selectedIndex].value;
        porcion = mensaje.substring(5);
        var codigo_completo = mensaje + "" + mensaje2;
        var repetido = "";
        cantidad = $("#grilla tbody").find("tr").length;
        for (var i = 0; i < cantidad; i++) {

            codigo = document.getElementById("ListaDeCodigos[" + i + "].CodigoAtc").value;
            codigo2 = document.getElementById("ListaDeCodigos[" + i + "].CodigoMonodroga").value;
            codigo = codigo + "" + codigo2;
            if (codigo == codigo_completo) {
                repetido = codigo;
            }
        }

        if (repetido != "") {
            alert("El codigo " + repetido + "  ya fue agregado, por favor verifique la información");
        } else {
            var i = $("#grilla > tbody > tr").length;
            cadena = "<tr>";

            cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].MonodrogaID' name='ListaDeCodigos[" + i + "].MonodrogaID' value='" + id + "' /></td>";
            cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].CodigoAtcID' name='ListaDeCodigos[" + i + "].CodigoAtcID' value='" + id + "' /></td>";
            cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].CodigoAtc' name='ListaDeCodigos[" + i + "].CodigoAtc' value='" + mensaje + "' /></td>";
            cadena = cadena + "<td style='display: none;'>   <input style='display: none;' readOnly='true' type='text' class='required' id='ListaDeCodigos[" + i + "].CodigoMonodroga' name='ListaDeCodigos[" + i + "].CodigoMonodroga' value='" + mensaje2 + "' /></td>";
            cadena = cadena + "<td >   " + mensaje2 + "</td>";
            cadena = cadena + "<td > " + codigo_completo + " </td>";


            cadena = cadena + "<td><a class='elimina'><img src='/mhoweb/Images/delete.png' onclick='fn_dar_eliminar()' /></a></td>";
            cadena = cadena + "</tr>";
            $("#grilla tbody").append(cadena);
        }


    });
});

$(document).ready(function() {
    LoadCotnent();
    LoadIndex();
});

function reLoad() {

    //LoadCotnent();
    //LoadIndex();

}

function justNumbers(e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
}