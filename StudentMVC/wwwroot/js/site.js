    function reloadGrid() {
        
        var grid = $("#StudentGrid").dxDataGrid("instance");
        grid.refresh();
    }


    function showToast(type,message) {
        $("#toast").dxToast("instance").option({
        type: type,
        message: message,
            visible: true,
            displayTime: 2000
        
    });
}


    function readresponse(response) {

        if (response.data.success) {
            var message = response.data.message
            showToast("success", message)
        } else {
            showToast("error", response.error.message)
        }
    }


    function openStudentLibraryPopup() {
        $("#studentLibrary").dxPopup("instance").show();
    }

function ClosePopUp() {
    $("#studentLibrary").dxPopup("instance").hide();
}

function sendData()
{
    var studentId = document.getElementById("studentId").value;
    var bookId = document.getElementById("bookId").value;
    var dateTime = document.getElementById("dateTime").value;
    var data = {
        StudentId: studentId,
        BookId: bookId,
        DateTime: dateTime
    };
    $.ajax({
        url: '/Student/BookMenagment', 
        type: 'POST', 
        data: data,
        success: function (response) {
            var message = response.message;
            if (response.succes) {
                showToast("success", message);
            } else {
                showToast("error", message);
            }
        },
        error: function (xhr) {
            var errorMessage = xhr.responseJSON.message;
            showToast("error", errorMessage);
        }
    });
}