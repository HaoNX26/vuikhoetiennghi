// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

$(document).ready(function () {
    //$('[data-mask]').inputmask()
    ////
    //$('.date-picker').each(function () {
    //    if (/^\d{4}[\/,-]\d{1,2}[\/,-]\d{1,2}/.test($(this).val())) {               
    //        var dateParts = $(this).val().split(/[\/,-]/)
    //        if (dateParts.length === 3) {                
    //            $(this).val(dateParts[2] + '/' + dateParts[1] + '/' + dateParts[0])
    //        }
    //    }
    //})
    //$('.date-picker').datetimepicker({
    //    locale: 'en', keepOpen: false, showClose: true,

    //    format: 'DD/MM/YYYY'
    //});
    ////
    //$('.time-picker').datetimepicker({
    //    locale: 'en', keepOpen: false, showClose: true,

    //    format: 'HH:mm'
    //});
    ////
    //$('.datetime-picker').each(function () {
    //    if (/^\d{4}[\/,-]\d{1,2}[\/,-]\d{1,2}/.test($(this).val())) {
    //        var datetimeParts = $(this).val().split(' ')            
    //        var dateParts = datetimeParts[0].split(/[\/,-]/)
    //        if (dateParts.length === 3) {
    //            $(this).val(dateParts[2] + '/' + dateParts[1] + '/' + dateParts[0] + (datetimeParts.length > 1 ? ' ' + datetimeParts[1] : ''))
    //        }
    //    }
    //})
    //$('.datetime-picker').datetimepicker({
    //    locale: 'en', keepOpen: false, showClose: true,

    //    format: 'DD/MM/YYYY HH:mm'
    //});
    //$('.select2').select2({
    //    width: "100%"
    //});
    //
    if (typeof InitForm === "function") {
        InitForm()
    }
})

function _getFormData(formId, stringVal) {
    var formData = new FormData()
    $("#" + formId).find("input:not(:submit,:reset,:button),select,textarea").each(function () {
        var ctrName = $(this).attr("name")
        if (ctrName) {
            var ctrNameDB = ctrName
            if (ctrName.indexOf("|") >= 0) {
                ctrNameDB = ctrName.substr(0, ctrName.indexOf("|"))
            }
            if (!formData.get(ctrNameDB)) {
                if ($(this).is(":file")) {
                    var files = $(this)[0].files
                    for (var i = 0; i < files.length; i++) {
                        formData.append(ctrNameDB + (i > 0 ? i : ""), files[i])
                    }
                }
                else {
                    var ctrValue
                    if ($(this).is(":checkbox")) {
                        if (stringVal) {
                            ctrValue = ''
                            $("#" + formId).find(":checkbox[name='" + ctrName + "']").each(function () {
                                if ($(this).is(":checked")) {
                                    ctrValue = ctrValue + ',' + $(this).val()
                                }
                                if (ctrValue) {
                                    ctrValue = ctrValue.substr(1, ctrValue.length - 1)
                                }
                            })
                        }
                        else {
                            ctrValue = $(this).prop("checked");
                        }
                    }
                    else if ($(this).is(":radio")) {
                        if (stringVal) {
                            $("#" + formId).find(":radio[name='" + ctrName + "']").each(function () {
                                if ($(this).is(":checked")) {
                                    ctrValue = $(this).val()
                                    return false;
                                }
                            })
                        } else {
                            ctrValue = $(this).prop("checked");
                        }
                    }
                    else {
                        ctrValue = ($(this).val() ? $(this).val() + '' : '')
                    }
                    //
                    formData.append(ctrNameDB, ctrValue)
                }
            }
        }
    })
    //
    return formData
}

function _getFormObject(formId, stringVal) {
    var obj = {}
    $("#" + formId).find("input:not(:submit,:reset,:button,:file),select,textarea").each(function () {
        //var ctrId = $(this).attr("id")
        var ctrName = $(this).attr("name")
        if (ctrName) {
            var ctrNameDB = ctrName
            if (ctrName.indexOf("|") >= 0) {
                ctrNameDB = ctrName.substr(0, ctrName.indexOf("|"))
            }
            if (!obj[ctrNameDB]) {
                var ctrValue
                if ($(this).is(":checkbox")) {
                    if (stringVal) {
                        ctrValue = ''
                        $("#" + formId).find(":checkbox[name='" + ctrName + "']").each(function () {
                            if ($(this).is(":checked")) {
                                ctrValue = ctrValue + ',' + $(this).val()
                            }
                            if (ctrValue) {
                                ctrValue = ctrValue.substr(1, ctrValue.length - 1)
                            }
                        })
                    }
                    else {
                        ctrValue = $(this).prop("checked");
                    }
                }
                else if ($(this).is(":radio")) {
                    if (stringVal) {
                        $("#" + formId).find(":radio[name='" + ctrName + "']").each(function () {
                            if ($(this).is(":checked")) {
                                ctrValue = $(this).val()
                                return false;
                            }
                        })
                    } else {

                        ctrValue = $(this).prop("checked");
                    }
                }
                else {
                    ctrValue = ($(this).val() ? $(this).val() + '' : '')
                }
                //
                obj[ctrNameDB] = ctrValue
            }
        }
    })
    //
    return obj
}

function _ajaxSync(url, data, doneFunc, alwaysFunction, failFunction) {
    $.ajax({
        url: url,
        async: false,
        method: "POST",
        dataType: "json",        
        data: data
    }).done(function (response) {
        if (doneFunc) {
            doneFunc(response)
        }
    }).always(function () {
        if (alwaysFunction) {
            alwaysFunction()
        }        
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert(textStatus)
        if (textStatus === "error") {
            location.reload()
        }
        else {
            if (failFunction) {
                failFunction(jqXHR, textStatus, errorThrown)
            }
        }
    })
}

function _ajax(url, data, doneFunc, alwaysFunction, failFunction) {
    $.ajax({
        url: url,
        async: true,
        method: "POST",
        dataType: "json",
        beforeSend: showLoading(),
        data: data
    }).done(function (response) {
        if (doneFunc) {
            doneFunc(response)
        }
    }).always(function () {
        if (alwaysFunction) {
            alwaysFunction()
        }
        else {
            hideLoading()
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert(textStatus)
        if (textStatus === "error") {
            location.reload()
        }
        else {
            if (failFunction) {
                failFunction(jqXHR, textStatus, errorThrown)
            }
        }
    })
}

function _ajaxWithArrayBody(url, data, doneFunc, alwaysFunction, failFunction) {
    $.ajax({
        url: url,
        async: true,
        method: "POST",
        contentType: 'application/json',
        dataType: "json",
        beforeSend: showLoading(),
        data: JSON.stringify(data)
    }).done(function (response) {
        if (doneFunc) {
            doneFunc(response)
        }
    }).always(function () {
        if (alwaysFunction) {
            alwaysFunction()
        }
        else {
            hideLoading()
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert(textStatus)
        if (textStatus === "error") {
            location.reload()
        }
        else {
            if (failFunction) {
                failFunction(jqXHR, textStatus, errorThrown)
            }
        }
    })
}

function _ajaxFormData(url, data, doneFunc, alwaysFunction, failFunction) {
    $.ajax({
        url: url,
        async: true,
        method: "POST",
        dataType: "json",
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: showLoading(),
        data: data
    }).done(function (response) {
        if (doneFunc) {
            doneFunc(response)
        }
    }).always(function () {
        if (alwaysFunction) {
            alwaysFunction()
        }
        else {
            hideLoading()
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert(textStatus)
        if (textStatus === "error") {
            location.reload()
        }
        else {
            if (failFunction) {
                failFunction(jqXHR, textStatus, errorThrown)
            }
        }
    })
}
//change password
function ShowChangePassword() {
    var url = "/System/ShowChangePassword";
    //
    showLoading()
    $("#edit-body-change-password").load(url, function () {
        $("#modal-change-password").modal({backdrop: 'static', keyboard: false})
        hideLoading()
    })
}
function ChangePassword() {
    var url = "/System/ChangePassword";
    var obj = _getFormObject("edit-body-change-password")
    _ajax(url, obj, function (response) {
        if (response.success === 'Y') {
            alert(response.message)
            $("#modal-change-password").modal("hide")
        }
        else {
            alert(response.message)
        }
    })
}
//** Loading panel **//
function showLoading(ctrlID) {
    var loadingPanel = '<div class="loading-overlay fixed-wrapper opacity5" style="background-color: #fff;' + (ctrlID ? ' position: absolute;' : 'position: fixed;') + '"></div>'
        + '<div class="loading-panel fixed-wrapper loading-icon" style="' + (ctrlID ? 'position: absolute;' : 'position: fixed;') + '"></div>'
    var $container = ctrlID ? $("#" + ctrlID) : $("body")
    $container.append(loadingPanel)
    $container.find(".loading-overlay").stop().animate({ opacity: 0.5 }, 600)
    $container.find(".loading-panel").stop().animate({ opacity: 1 }, 600)
}

function hideLoading(ctrlID) {
    var $container = ctrlID ? $("#" + ctrlID) : $("body")
    $container.find(".loading-overlay, .loading-panel").stop().animate({ opacity: 0 }, 600, function () {
        $(this).remove()
    })
}