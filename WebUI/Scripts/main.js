$(document).ready(function () {
    $("._saveExecute").click(function () {
        var id = $(this).data("id");
        var parent = $(this).parent().parent();
        var selectListItem = parent.find("select");
        var selectedItemText = selectListItem.find(':selected').text();
        var resultId = selectListItem.val();
        var commentInput = parent.find("#item_CheckListComment");
        $.ajax({
            url: '/CheckLists/SaveExecute',
            type: 'Post',
            data: {
                id: id,
                resultId: resultId,
                comment: commentInput.val()
            },
            success: function (data) {
                var parentTD = selectListItem.parent();
                selectListItem.remove();
                parentTD[0].innerHTML = '<p style="color:'+ data.color + '">' + selectedItemText + '</p>';
                var commentParent = commentInput.parent();
                //selectListItem.remove();
                commentParent[0].innerHTML = '<p>' + commentInput.val() + '</p>';
            }
        });
    });
});