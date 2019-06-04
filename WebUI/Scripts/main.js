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
                parentTD[0].innerHTML = '<p style="color:' + data.color + '">' + selectedItemText + '</p>';
                var commentParent = commentInput.parent();
                //selectListItem.remove();
                commentParent[0].innerHTML = '<p>' + commentInput.val() + '</p>';
            }
        });
    });

    var stepCounter = 1;

    $("#Create").click(function (e) {
        stepCounter = 1;
        e.preventDefault();
        var caseStepViewModel = {
            TestCaseId: $("#TestCaseId").val(),
            CaseId: $("#CaseId").val(),
            CaseSummary: $("#CaseSummary").val(),
            CasePreconditions: $("#CasePreconditions").val(),
            CaseSteps: []
        };
        $("._stepRow").each(function () {
            caseStepViewModel.CaseSteps.push({
                CaseStepNumber: $(this).find("._stepNumber").text(),
                CaseStepDescription: $(this).find("._stepDescription").val(),
                CaseStepExpectedResult: $(this).find("._stepExpectedResult").val()
            });
        });

        $.ajax({
            url: '/Cases/Create',
            type: 'POST',
            data: {
                caseModelJson: JSON.stringify(caseStepViewModel)
            },
            success: function (data) {
                window.location.href = data.url;
            }
        });
    });

    $("#add_step").click(function () {
        var row = $("#rowTemplate").clone().addClass("_stepRow").show();
        row[0].removeAttribute("id");
        row.find("._stepNumber").text(stepCounter);
        stepCounter++;
        row.insertBefore("._footerTr");
    });
    $("._removeStep").click(function () {
        stepCounter--;
        $(this).parent().parent().remove();
    });
});