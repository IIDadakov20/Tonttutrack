document.getElementById('togglePassword').addEventListener('change', function() {
    var passwordInput = document.getElementById('passwordInput');
    var confirmPasswordInput = document.getElementById('confirmPasswordInput');

    if (window.location.pathname === "/Authentication/Register") {
        if (this.checked) {
            passwordInput.type = 'text';
            confirmPasswordInput.type = 'text';
        } else {
            passwordInput.type = 'password';
            confirmPasswordInput.type = 'password';
        }
    }
    else {
        if (this.checked) {
            passwordInput.type = 'text';
        } else {
            passwordInput.type = 'password';
        }
    }
});
document.getElementById('togglePassword').onmousedown = function (event) {
    event.preventDefault();
}