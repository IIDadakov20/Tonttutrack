document.addEventListener("DOMContentLoaded", function () {
    const toggleMenu = document.getElementById("toggle-menu");
    const slideOut = document.getElementById("tracker-menu");
    const recordButton = document.getElementById("record-btn");
    const togglePassword = document.getElementById("togglePassword");
    const passwordInput = document.querySelector("input[type='password']");

    let isRecording = false;

    // Отваряне и затваряне на менюто
    toggleMenu.addEventListener("click", function () {
        slideOut.classList.toggle("open");
        toggleMenu.classList.toggle("open");
        toggleMenu.classList.toggle("active"); // Добавяме/премахваме класа
    });
    
    // Превключване между "Start Recording" и "Save Recording"
    recordButton.addEventListener("click", function () {
        isRecording = !isRecording;
        recordButton.textContent = isRecording ? "Save Recording" : "Start Recording";
    });

    // Показване и скриване на паролата
    togglePassword.addEventListener("change", function () {
        passwordInput.type = this.checked ? "text" : "password";
    });
});
