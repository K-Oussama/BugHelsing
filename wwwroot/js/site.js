// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function OpenAddModal(name, id) {
    var data = { Number: id };
    let pth = '/' + name + '/Create';
    $.ajax(
        {
            type: 'GET',
            url: pth,
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#modal-edit-content').html(result);
                $('#modal-edit').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}
function OpenAddModalProjects(name) {
    let pth = '/'+name+'/Create';
    $.ajax(
        {
            type: 'GET',
            url: pth,
            contentType: 'application/json; charset=utf=8',
            success: function (result) {
                $('#modal-add-content').html(result);
                $('#modal-add').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}
function OpenEditModalProjects(name,id) {
    var data = { Number: id };
    let pth = '/' + name + '/Edit';
    $.ajax(
        {
            type: 'GET',
            url: pth,
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#modal-edit-content').html(result);
                $('#modal-edit').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}
function OpenDeleteModal(name,id) {
    var data = { Number: id };
    let pth = '/' + name + '/Delete';
    $.ajax(
        {
            type: 'GET',
            url: pth,
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#modal-delete-content').html(result);
                $('#modal-delete').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}
function OpenDetailsModal(name,id) {
    var data = { Number: id };
    let pth = '/' + name + '/Details';
    $.ajax(
        {
            type: 'GET',
            url: pth,
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#modal-details-content').html(result);
                $('#modal-details').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

