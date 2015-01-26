$("#AddCommentForm").submit(function (event) {
    event.preventDefault();
    var $form = $(this),
        actionUrl = $form.attr("action");

    var setErrors = function(errors) {
        var summary = $form.find("#AddCommentSummary");
        summary.empty();
        var errorList = summary.append("<ul></ul>");
        for (var i = 0; i < errors.length; i++) {
            errorList.append("<li class=\"label alert\">" + errors[i] + "</li>");
        }
    };

    var captcha = $form.find("textarea[name='g-recaptcha-response']").val();

    if (captcha.length === 0) {
        var errors = new Array(1);
        errors[0] = "Please fill out captcha to submit.";
        setErrors(errors);
        return;
    }

    var postId = $form.find("input[name='PostId']").val();
    var replyCommentId = $form.find("input[name='ReplyCommentId']").val();
    var commenterId = $form.find("input[name='CommenterId']").val();
    var name = $form.find("input[name='Name']").val();
    var email = $form.find("input[name='Email']").val();
    var url = $form.find("input[name='Url']").val();
    var body = $form.find("textarea[name='Body']").val();
    var antiForgery = $form.find("input[name='__RequestVerificationToken']").val();
    
    var button = $form.find("input[type='submit']");
    button.attr("disabled", "true");

    var posting = $.post(actionUrl,
        {
            PostId: postId,
            ReplyCommentId: replyCommentId,
            CommenterId: commenterId,
            Name: name,
            Email: email,
            Url: url,
            Body: body,
            __RequestVerificationToken: antiForgery,
            Captcha: captcha
        });

    
    posting.error(function (data) {
        var responseErrors = data.responseJSON.Errors;
        setErrors(responseErrors);
        button.attr("disabled", "false");
    });

    posting.done(function (data) {
        $form.empty().append("<div class=\"label info\">" + data.Message + "</div>");
    });
});