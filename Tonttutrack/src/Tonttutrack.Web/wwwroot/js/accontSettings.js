document.addEventListener("DOMContentLoaded", function() {
    const settingsButton = document.getElementById("settingsButton");
    const closeSettingsPopup = document.getElementById("closeSettingsPopup");
    const settingsContent = document.getElementById("settingsPopup");

    const updateAccountButton = document.getElementById("updateAccountButton");
    const updateAccountFields = document.getElementById("updateAccountFields");

    const updatePasswordButton = document.getElementById("updatePasswordFieldsButton");
    const updatePasswordFields = document.getElementById("updatePasswordFields");

    const deleteAccountButton = document.getElementById("deleteAccountButton");
    const deleteAccountFields = document.getElementById("deleteAccountFields");
    const confirmationPopup = document.getElementById("confirmationPopup");

    let scrollY = 0;

    function disableScroll() {
        scrollY = window.scrollY;
        document.body.style.position = "fixed";
        document.body.style.top = `-${scrollY}px`;
        document.body.style.width = "100%";
        document.documentElement.classList.add("blur-scrollbar"); // Добавя ефекта върху скрол лентата
    }

    function enableScroll() {
        document.body.style.position = "";
        document.body.style.top = "";
        window.scrollTo(0, scrollY);
        document.documentElement.classList.remove("blur-scrollbar"); // Премахва ефекта върху скрол лентата 
    }

    const openContent = (content) => {
        if (content) {
            content.classList.remove("hidden");
            content.classList.add("show");
        }
    };

    const closeContent = (content) => {
        if (content) {
            content.classList.remove("show");
            content.classList.add("hidden");
        }
    };

    const disableOtherButtons = (buttonToExclude) => {
        const buttons = [updateAccountButton, updatePasswordButton, deleteAccountButton];
        buttons.forEach(button => {
            if (button !== buttonToExclude) {
                button.classList.add("disabled");
            }
        });
    };

    const enableButton = (button) => {
        button.classList.remove("disabled");
    };

    if (settingsButton && settingsContent) {
        settingsButton.addEventListener("click", () => {
            openContent(settingsContent);
            disableScroll(); // Забранява скролването
        });
    }

    if (closeSettingsPopup && settingsContent) {
        closeSettingsPopup.addEventListener("click", () => {
            closeContent(settingsContent);
            enableScroll(); // Възстановява скролването
        });
    }

    if (updateAccountButton && updateAccountFields) {
        updateAccountButton.addEventListener("click", () => {
            // Скрии останалите полета
            if (updatePasswordFields) closeContent(updatePasswordFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            openContent(updateAccountFields);

            // Сивей бутоните за другите секции
            disableOtherButtons(updateAccountButton);
            enableButton(updateAccountButton); // Запази активния бутон
        });
    }

    if (updatePasswordButton && updatePasswordFields) {
        updatePasswordButton.addEventListener("click", () => {
            // Скрии останалите полета
            if (updateAccountFields) closeContent(updateAccountFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            openContent(updatePasswordFields);

            // Сивей бутоните за другите секции
            disableOtherButtons(updatePasswordButton);
            enableButton(updatePasswordButton); // Запази активния бутон
        });
    }

    if (deleteAccountButton && deleteAccountFields) {
        deleteAccountButton.addEventListener("click", () => {
            // Скрии останалите полета
            if (updateAccountFields) closeContent(updateAccountFields);
            if (updatePasswordFields) closeContent(updatePasswordFields);
            openContent(deleteAccountFields);

            // Сивей бутоните за другите секции
            disableOtherButtons(deleteAccountButton);
            enableButton(deleteAccountButton); // Запази активния бутон
        });
    }

    if (confirmationPopup) {
        document.getElementById("cancelDeleteAccount")?.addEventListener("click", () => closeContent(deleteAccountFields));
        document.getElementById("confirmDeleteAccount")?.addEventListener("click", () => {
            // Имплементирай логика за изтриване на акаунт
        });
    }

    document.addEventListener("click", (event) => {
        const isClickInsideSettings = settingsContent && settingsContent.contains(event.target) || settingsButton && settingsButton.contains(event.target);
        const isClickInsideAccount = updateAccountFields && updateAccountFields.contains(event.target) || updateAccountButton && updateAccountButton.contains(event.target);
        const isClickInsidePassword = updatePasswordFields && updatePasswordFields.contains(event.target) || updatePasswordButton && updatePasswordButton.contains(event.target);
        const isClickInsideDelete = deleteAccountFields && deleteAccountFields.contains(event.target) || deleteAccountButton && deleteAccountButton.contains(event.target);

        if (!isClickInsideSettings) {
            if (updateAccountFields) closeContent(updateAccountFields);
            if (updatePasswordFields) closeContent(updatePasswordFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            if (settingsContent) {
                closeContent(settingsContent);
                enableScroll(); // Възстановява скролването при клик извън pop-up-а
            }
        }
        if (!isClickInsideAccount && !isClickInsidePassword && !isClickInsideDelete) {
            if (updateAccountFields){ 
                closeContent(updateAccountFields);
                enableButton(updatePasswordButton);
                enableButton(deleteAccountButton);
            }
            if (updatePasswordFields){
                closeContent(updatePasswordFields);
                enableButton(updateAccountButton);
                enableButton(deleteAccountButton);
            }
            if (deleteAccountFields){
                closeContent(deleteAccountFields);
                enableButton(updatePasswordButton);
                enableButton(updateAccountButton);
            }
        }
    });
});
document.querySelectorAll('.language-option').forEach(option => {
    option.addEventListener('click', function() {
        // Премахваме класа 'selected' от всички опции
        document.querySelectorAll('.language-option').forEach(opt => {
            opt.classList.remove('selected');
            opt.classList.add('not-selected');
        });

        // Добавяме класа 'selected' на избраната опция
        this.classList.remove('not-selected');
        this.classList.add('selected');
    });
});

