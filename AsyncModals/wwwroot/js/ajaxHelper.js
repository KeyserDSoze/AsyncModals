var ajaxHelper =
{
    install: function (sender) {
        ajaxHelper.sender = sender ? sender : console.log;
    },
    targetUpdate: "",
    target: "",
    onclick: function (event, handler) {
        var url = $(handler).data('url');
        var model = $(handler).data('model');
        ajaxHelper.target = $(handler).data('target');
        var ajaxMethod = $(handler).data('ajax-method');
        var title = $(handler).data('title');
        if (title)
            $(ajaxHelper.target + ' .modal-title').html(title);
        ajaxHelper.targetUpdate = $(handler).data('update');
        $(ajaxHelper.target).on("hide.bs.modal", function () {
            ajaxHelper.contextUpdate = "";
            ajaxHelper.target = "";
        });
        $.ajax({
            type: ajaxMethod,
            url: url,
            data: model,
            success: function (data) {
                $(ajaxHelper.target + ' .modal-body').html(data);
                $(ajaxHelper.target + ' > .modal').modal('show');
            }
        });
    },
    onSubmit: function (formObject, event) {
        event.preventDefault();
        var formData = $(formObject).serialize();
        var url = $(formObject).attr('action');
        if (!url)
            url = window.location.href;
        $.ajax({
            type: 'POST',
            url: url,
            data: formData,
            success: ajaxHelper.onSucces,
            error: ajaxHelper.onFailure
        });
    },
    onSucces: function (data) {
        console.log(data);
        if (ajaxHelper.targetUpdate.length > 0)
            $(ajaxHelper.targetUpdate).html(data);
        $(ajaxHelper.target).modal('hide');
        ajaxHelper.contextUpdate = "";
        ajaxHelper.target = "";
        ajaxHelper.sender("OK");
    },
    onFailure: function (data) {
        if (data && data.responseText && data.responseText.indexOf("document.location") === 0)
            eval(data.responseText);
        if (ajaxHelper.targetUpdate.length > 0)
            ajaxHelper.sender(data);
        else
            ajaxHelper.sender("NOT OK");
    },
    sender: console.log
};