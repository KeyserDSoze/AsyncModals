var ajaxHelper =
{
    zindexcreated: 1041,
    install: function (sender) {
        ajaxHelper.sender = sender ? sender : console.log;
        ajaxHelper.normalModal =
            '<div class="modal fade" id="{modalid}" tabindex="-1" style="z-index:{zindex}" data-update="{targetupdate}" role="dialog" aria-hidden="true">' +
            '<div class="modal-dialog" role="document">' +
            '<div class="modal-content">' +
            '<div class="modal-header">' +
            '<h5 class="modal-title">{title}</h5>' +
            '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
            '<span aria-hidden="true">&times;</span>' +
            '</button>' +
            '</div>' +
            '<div class="modal-body">' +
            '</div>' +
            '<img class="ajax-spinner" src="../../images/30.gif" />' +
            '</div>' +
            '</div>' +
            '</div>';
        $("#confirmModal").on("hide.bs.modal", function () {
            $("#confirmModal .ajax-spinner").hide();
        });
        $("body").append('<div id="loader"><img class= "spinner" src = "../../images/802.gif" /></div >');
    },
    normalModal: "",
    onclick: function (event, handler) {
        var url = $(handler).data('url');
        var model = $(handler).data('model');
        var target = $(handler).data('target') + ajaxHelper.guid();
        var ajaxMethod = $(handler).data('ajax-method');
        var title = $(handler).data('title');
        if (!title)
            title = "&nbsp;";
        var targetUpdate = $(handler).data('update');
        var modalHtml = ajaxHelper.normalModal.replace("{modalid}", target).replace("{zindex}", ajaxHelper.zindexcreated++).replace("{targetupdate}", targetUpdate).replace("{title}", title);
        $("body").append(modalHtml);
        $("#" + target).on("hide.bs.modal", function () {
            $("#" + target).remove();
        });
        $('#loader').show();
        $.ajax({
            type: ajaxMethod,
            url: url,
            data: model,
            success: function (data) {
                $("#loader").hide();
                $("#" + target + " .modal-body").html(data);
                $("#" + target).modal('show');
            },
            error: function (data)
            {
                $("#loader").hide();
                failurePopUp(data.status);
            }
        });
    },
    onSubmit: function (formObject, event) {
        event.preventDefault();
        var formData = $(formObject).serialize();
        var url = $(formObject).attr('action');
        if (!url)
            url = window.location.href;
        var target = "";
        var targetUpdate = "";
        var start = formObject.parentElement;
        while (start && start !== null) {
            if ($(start).hasClass("modal")) {
                target = $(start).attr("id");
                targetUpdate = $(start).data("update");
                $("#" + target + " .ajax-spinner").show();
                break;
            }
            start = start.parentElement;
        }
        $.ajax({
            type: 'POST',
            url: url,
            data: formData,
            success: function (data) {
                if (targetUpdate && targetUpdate.length > 0)
                    $("#" + targetUpdate).html(data);
                console.log(target);
                $("#" + target).modal('hide');
                successPopUp("OK");
            },
            error: function (data) {
                ajaxHelper.onFailure(data);
                $("#" + target + " .ajax-spinner").hide();
            }
        });
    },
    onConfirm: function (url, formData, updateId) {
        $('#confirmModal').modal('show');
        $("#confirmModal #confirmDelete").click(
            function () {
                $("#confirmModal .ajax-spinner").show();
                $.ajax({
                    type: formData ? 'POST' : 'GET',
                    url: url,
                    data: formData,
                    success: function (data) {
                        if (updateId && updateId.length > 0)
                            $("#" + updateId).html(data);
                        successPopUp("OK");
                        ajaxHelper.closeConfirm();
                    },
                    error: function (data) {
                        ajaxHelper.onFailure(data);
                        $("#confirmModal .ajax-spinner").hide();
                    }
                });
            });
    },
    closeConfirm: function () {
        $('#confirmModal').modal('hide');
    },
    onFailure: function (data) {
        if (data && data.responseText && data.responseText.indexOf("document.location") === 0)
            eval(data.responseText);
        else {
            if (data && data.responseText)
                failurePopUp(data.responseText);
            else
                failurePopUp("NOT OK");
        }
    },
    sender: console.log,
    guid: function () {
        return 'xxxxxxxxxxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
};

ajaxHelper.install();

function successPopUp(value) {
    console.log(value);
}

function failurePopUp(value) {
    console.log("failed: " + value);
}