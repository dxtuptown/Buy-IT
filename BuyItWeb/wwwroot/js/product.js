var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    console.log("called");
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall"
        },
        "columns": [
            { "data": "productname", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "listprice", "width": "15%" },
            { "data": "category.categoryname", "width": "15%" },
            {
                "data": "ProductID",
                "render": function (data) {
                    return `
                        <div class="col-6 text-end">
                            <a href="/admin/product/edit?id=${data}" class="btn btn-primary"><i class="bi bi-pencil-square"></i> Edit</a> |
                            <a href="/admin/product/details?id=${data}" class="btn btn-primary"><i class="bi bi-info-circle"></i> Details</a> |
                            <a href="/admin/product/delete?id=${data}" class="btn btn-primary"><i class="bi bi-trash"></i> Delete</a>
                        </div>`;
                },
                "width": "25%"
            }
        ]
    });
}

//var dataTable;
//function loadDatatable() {
//    console.log("called");
//    dataTable = $('#tblData').DataTable({
//        "ajax": {
//            "url": "/admin/product/getall"
//        },
//        "columns": [
//            { "data": "title", "width": "15%" },
//            { "data": "isbn", "width": "15%" },
//            { "data": "author ", "width": "15%" },
//            { "data": "category.name", "width": "15%" },
//            {
//                "data": "id"
//                , "render": function (data) {
//                    return `    
//                    <div>
//                        <a href="/Admin/Product/Upsert?id=${data}"
//                        class="btn btn-primary">
//                        <i class="bi bi-pencil"></i>
//                        </a>
//                        <a onclick="Delete('/Admin/Product/Delete/${data}')"
//                        class="btn btn-primary">
//                        <i class="bi bi-trash3-fill"></i>
//                        </a>
//                    </div>
//                    `
//                }
//                , "width": "15%"
//            }
//        ]
//    });
//}
//function Delete(url) {
//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: "DELETE",
//                success: function (data) {
//                    //loadDatatable;
//                    if (data.success) {
//                        dataTable.ajax.reload();
//                        Swal.fire(
//                            'Deleted',
//                            'This product has been deleted',
//                            'success'
//                        )
//                    } else {
//                        //Handle error case
//                    }
//                }
//            })
//        }
//    });
//}