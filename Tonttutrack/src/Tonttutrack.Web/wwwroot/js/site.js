////////////////////////////////////////////////////////////
// Dropdown функционалност за акаунт
$(function () {
    const dropdownButton = document.getElementById('custom-dropdown-button');
    const dropdownMenu = document.getElementById('custom-dropdown-menu');

    dropdownButton.addEventListener('click', function (e) {
        e.stopPropagation();
        dropdownMenu.classList.toggle('show');
    });

    document.addEventListener('click', function (e) {
        if (!dropdownMenu.contains(e.target) && !dropdownButton.contains(e.target)) {
            dropdownMenu.classList.remove('show');
        }
    });
});

////////////////////////////////////////////////////////////

$('#userUpdateForm').on('submit', function (e) {
    e.preventDefault();

    var formData = {
        Username: $('#newUserUsername').val(),
        Email: $('#newUserEmail').val()
    };

    $('#userUpdateForm .text-danger').html('');

    $(this).find('.submitButton').prop('disabled', true); // Disable submit button

    $.ajax({
        url: '/user/updateUser',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            $('#account-name').text(formData.Username);
        },
        error: function (xhr) {
            var errors = xhr.responseJSON;

            // Loop through the errors and show them under the corresponding input field
            for (var field in errors) {
                var fieldErrors = errors[field];  // Array of error messages for the field
                if (fieldErrors && fieldErrors.length > 0) {
                    // Display the error message for the respective field
                    var errorMessage = fieldErrors.join('<br />');  // Join multiple errors into one string
                    if (field == "") {
                        $('#userUpdateForm').find('.formErrorMessage').html(errorMessage);
                        document.getElementsByClassName("formErrorMessage")[0].style.display = "block";
                    } else {
                        // Find the span next to the input field and insert the error message
                        $('input[name="Item1.' + field + '"]').next('.text-danger').html(errorMessage);
                    }
                }
            }
        },
        complete: function () {
            $('.submitButton').prop('disabled', false); // Re-enable submit button
        }
    });
});

$('#userPasswordUpdateForm').on('submit', function (e) {
    e.preventDefault();

    var formData = {
        CurrentPassword: $('#currentPassword').val(),
        NewPassword: $('#newPassword').val(),
        ConfirmPassword: $('#confirmPassword').val()
    };

    $('#userPasswordUpdateForm .text-danger').html('');

    $(this).find('.submitButton').prop('disabled', true); // Disable submit button

    $.ajax({
        url: '/user/updateUserPassword',
        type: 'PATCH',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
        },
        error: function (xhr) {
            var errors = xhr.responseJSON;

            // Loop through the errors and show them under the corresponding input field
            for (var field in errors) {
                var fieldErrors = errors[field];  // Array of error messages for the field
                if (fieldErrors && fieldErrors.length > 0) {
                    // Display the error message for the respective field
                    var errorMessage = fieldErrors.join('<br />');  // Join multiple errors into one string
                    if (field == "") {
                        $('#userPasswordUpdateForm').find('.formErrorMessage').html(errorMessage);
                        document.getElementsByClassName("formErrorMessage")[1].style.display = "block";
                    } else {
                        // Find the span next to the input field and insert the error message
                        $('input[name="Item2.' + field + '"]').next('.text-danger').html(errorMessage);
                    }
                }
            }
        },
        complete: function () {
            $('.submitButton').prop('disabled', false); // Re-enable submit button
        }
    });
});

$('#userDeleteForm').on('submit', function (e) {
    e.preventDefault();

    var password = $('#deletePassword').val()

    $('#userDeleteForm .text-danger').html('');

    $(this).find('.submitButton').prop('disabled', true); // Disable submit button

    $.ajax({
        url: '/user/deleteUser',
        type: 'DELETE',
        contentType: 'application/json',
        data: JSON.stringify(password),
        success: function (response) {
            location.reload();
        },
        error: function (xhr) {
            var errors = xhr.responseJSON;

            // Loop through the errors and show them under the corresponding input field
            for (var field in errors) {
                var fieldErrors = errors[field];  // Array of error messages for the field
                if (fieldErrors && fieldErrors.length > 0) {
                    // Display the error message for the respective field
                    var errorMessage = fieldErrors.join('<br />');  // Join multiple errors into one string
                    $('#userDeleteForm').find('.formErrorMessage').html(errorMessage);
                    document.getElementsByClassName("formErrorMessage")[2].style.display = "block";
                }
            }
        },
        complete: function () {
            $('.submitButton').prop('disabled', false); // Re-enable submit button
        }
    });
});