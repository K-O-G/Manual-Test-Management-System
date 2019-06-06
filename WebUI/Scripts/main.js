$(document).ready(function () {
    var body = $(document.body);
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

    
    $("#Create").click(function (e) {
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
    $("#SaveEd").click(function (e) {
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
            url: '/Cases/Edit',
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
        var rowIndex = $(this).parent().parent().index(); 
        row[0].removeAttribute("id");
        row.find("._stepNumber").text(rowIndex);
        row.insertBefore("._footerTr");
    });

    body.on("click",
        "._removeStep",
        function () {
            $(this).parent().parent().remove();
            resetSequence();
        });

    function resetSequence() {
        $("._stepRow").each(function () {
            var rowIndex = $(this).parent().children().index($(this));
            $(this).find("._stepNumber").text(rowIndex);
        });
    }

    $("._saveTestEntitiesExecute").click(function () {
        var caseId = $(this).data("id");
        var parent = $(this).parent().parent();
        var selectListItems = parent.find("select");
        var stepResultsDictionary = new Object();
        selectListItems.each(function () {
            stepResultsDictionary[parseInt($(this).parent().parent().data("stepid"))] = parseInt($(this).val());
        });

        var commentInput = parent.find("#item_CaseComment");
        $.ajax({
            url: '/TestCaseEntities/SaveExecute',
            type: 'Post',
            data: {
                caseId: caseId,
                stepResultsJson: JSON.stringify(stepResultsDictionary),
                comment: commentInput.val()
            },
            success: function (data) {
                var results = JSON.parse(data.stepResultsResult);
                for (var key in results) {
                    var selector = "[data-stepid='" + key + "']";
                    var select = $(selector).find("select");
                    var resultValue = select.find(':selected').text();
                    var parentTD = select.parent();
                    select.remove();
                    parentTD[0].innerHTML = '<p style="color:' + results[key] + '">' + resultValue + '</p>';
                };
                var commentParent = commentInput.parent();
                commentParent[0].innerHTML = '<p>' + commentInput.val() + '</p>';
            }
        });
    });

});