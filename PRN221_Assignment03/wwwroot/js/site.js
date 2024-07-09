// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => {
    LoadData();
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();
    connection.on("LoadPosts", function () {
        LoadData();
    });
    LoadData();
    function LoadData() {
        var tr = '';
        $.ajax({
            url: '/Posts/GetPosts',
            method: "GET",
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
            <td>${v.PostId}</td>
            <td>${v.Title}</td>
            <td>${v.Content}</td>
            <td>${v.CreatedDate}</td>
            <td>${v.UpdatedDate}</td>
            <td>${v.AuthorId}</td>
            <td>${v.CategoryId}</td>
            <td>
                <a href='../Posts/Edit/${v.PostId}'>Edit</a> |
                <a href='../Posts/Details/${v.PostId}'>Details</a> |
                <a href='../Posts/Delete/${v.PostId}'>Delete</a>
            </td>
            </tr>`
                })

                $("#tableBody").html(tr)
            }, error: (error) => {
                console.log(error)
            }

        });
    }


})