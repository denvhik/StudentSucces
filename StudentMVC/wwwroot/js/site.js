//// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.
//$(document).ready(function () {
//    $('#MyTable').DataTable({
//        scrollCollapse: true,
//        pagging: true,
//        length: 5,
//        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
//        pageLength: 5,
//          order: []
//    });
//});
//function changeModalTitle(title) {
//    $('.modal-title').text(title);
//}
//function showModal(title) {
//    $('#personalModalLabel').text(title);
//    $('#modal-title').modal('show');
//}
////function validateForm(event) {
////    debugger;
////    event.preventDefault();

////    var email = $('#modalstudentform input[name="Gmail"]').val();
////    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

////    if (!emailPattern.test(email)) {
////        $('#email-error').show();
////        return false;
////    }
////    $('#email-error').hide();
////    saveStudent(event);
////}
//function saveStudent(event) {
//    debugger;
//    var token = $('input[name="__RequestVerificationToken"]').val();
//    event.preventDefault();
//    var formData = $('#modalstudentform').serialize();
//    var url = '/Student/Add'
//    var Id = $('#Id').val();
//    debugger;
//    if (Id !== undefined && Id !== "") {
//        url = '/Student/UpdateStudent';
//    }
//    $.ajax({
//        url: url,
//        type: 'POST',
//        data: formData,
//        headers: {
//            "__RequestVerificationToken": token
//        },
//        success: function (response) {
//            if (response.Message) {
//                closeForm();
//                debugger;
//                toastr.success(response.Message);
//                reloadPageOnCurrentPage();
//            } else {
//                toastr.error('An error occurred while adding the student.');
//            }
//        },
//        error: function (xhr) {
//            if (xhr.status === 400) {
//                var errorMessage = xhr.responseJSON && xhr.responseJSON.ErrorMessage ? xhr.responseJSON.ErrorMessage : 'An error occurred while processing your request.';
//                toastr.error(errorMessage);
//            } else {
//                toastr.error('An internal server error occurred.');
//            }
//        }

//    });
//    return false;
//}
//function deleteStudent(Id) {
//    var token = $('input[name="__RequestVerificationToken"]').val();
//    debugger;
//    var url = '/Student/Delete'
//    $.ajax({
//        url: url,
//        type: 'POST',
//        data: { Id: Id, __RequestVerificationToken:token },

//        success: function (response) {
//            if (response.success) {
//                toastr.success(response.message);
//                $('#MyTable').find('[value="' + Id + '"]').closest('tr').remove();
//            } else {
//                toastr.error('Failed to delete student. Please try again.');
//            }
//        },
//        error: function (error) {
//            toastr.error('Failed to delete student. Please try again.');
//        }
//    });
//}
//function showAddStudentForm() {
//    $('#personalModal').modal('show');
//}

//function closeForm() {
//    $('#personalModal').modal('hide');
//    $('#modalstudentform')[0].reset();

//}
//function GetById(Id) {
//    $.ajax({
//        url: '/Student/GetStudentById',
//        type: 'GET',
//        data: { Id: Id },
//        success: function (response) {
//            if (response.success) {

//                var student = response.student.Result;
//                $.each(student, function (key, value) {
//                    $('#modalstudentform input[name="' + key + '"]').val(value);
//                });
//                $('#Id').val(Id);
//                debugger;
//                $('.modal-title').text('Edit Student');

//                $('#personalModal').modal('show');
//                $('#personalModal').on('hidden.bs.modal', function (e) {
//                    closeForm();
//                });
//            } else {

//                console.error(response.message);
//            }
//        },
//        error: function (xhr, status, error) {
//            console.error(error);
//        }
//    });
//}
//function getCurrentPage() {
//    debbuger;
//    var info = $('#MyTable').DataTable().page.info();
//    return info.page;
//}
//function reloadPageOnCurrentPage() {
//    debbuger;
//    var currentPage = getCurrentPage();
//    location.reload();
//    $('#MyTable').DataTable().page(currentPage).draw('page');
//}

    function reloadGrid() {
        
        var grid = $("#StudentGrid").dxDataGrid("instance");
        grid.refresh();
    }


    function showToast(type,message) {
        $("#toast").dxToast("instance").option({
        type: type,
        message: message,
        visible: true
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
        studentId: studentId,
        bookId: bookId,
        dateTime: dateTime
    };
    $.ajax({
        url: '', 
        type: 'POST', 
        contentType: 'application/json', 
        data: JSON.stringify(data), 
        success: function (response) {
        
        },
        error: function (xhr, status, error) {
           
        }
    });
}