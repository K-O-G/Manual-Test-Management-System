$(document).ready(function () {
    $("._saveExecute").click(function () {
        var id = $(this).data("id");
        var selectListItem = $(this).parent().parent().find("select");
        var selectedItemText = selectListItem.find(':selected').text();
//        var selectedItemColor = selectListItem.find(':selected').color();
        var resultId = selectListItem.val();
        $.ajax({
            url: '/CheckLists/SaveExecute',
            type: 'Post',
            data: {
                id: id,
                resultId: resultId
            },
            success: function () {
                var parentTD = selectListItem.parent();
                selectListItem.remove();
                parentTD[0].innerHTML = '<p>' + selectedItemText + '</p>';
//                parent[0].innerHTML = '<span style="color:' + selectedItemColor + '">' + selectedItemText + '</span>';
            }
        });
    });
});