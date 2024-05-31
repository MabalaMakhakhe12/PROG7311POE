$(document).ready(function () {
    // Initialize Select2
    $('.select2').select2();

    // Initialize Bootstrap Datepicker
    $('.datepicker').datepicker({
        format: 'mm/dd/yyyy',
        todayHighlight: true
    });

    // Initialize DataTables
    $('#example').DataTable();

    // Initialize Toastr
    toastr.options = {
        "closeButton": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
    };

    // Initialize SweetAlert2
    $('.swal').on('click', function () {
        Swal.fire('Hello world!');
    });
});
