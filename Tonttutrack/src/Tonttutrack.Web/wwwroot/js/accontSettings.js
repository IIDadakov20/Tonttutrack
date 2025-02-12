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
        document.documentElement.classList.add("blur-scrollbar");
    }

    function enableScroll() {
        document.body.style.position = "";
        document.body.style.top = "";
        window.scrollTo(0, scrollY);
        document.documentElement.classList.remove("blur-scrollbar");
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
            disableScroll();
        });
    }

    if (closeSettingsPopup && settingsContent) {
        closeSettingsPopup.addEventListener("click", () => {
            closeContent(settingsContent);
            $('#userPasswordUpdateForm')[0].reset();
            $('#userPasswordUpdateForm .text-danger').html('');
            $('#userUpdateForm .text-danger').html('');
            enableScroll();
        });
    }

    if (updateAccountButton && updateAccountFields) {
        updateAccountButton.addEventListener("click", () => {
            if (updatePasswordFields) closeContent(updatePasswordFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            openContent(updateAccountFields);
            disableOtherButtons(updateAccountButton);
            enableButton(updateAccountButton);
        });
    }

    if (updatePasswordButton && updatePasswordFields) {
        updatePasswordButton.addEventListener("click", () => {
            if (updateAccountFields) closeContent(updateAccountFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            openContent(updatePasswordFields);
            disableOtherButtons(updatePasswordButton);
            enableButton(updatePasswordButton);
        });
    }

    if (deleteAccountButton && deleteAccountFields) {
        deleteAccountButton.addEventListener("click", () => {
            if (updateAccountFields) closeContent(updateAccountFields);
            if (updatePasswordFields) closeContent(updatePasswordFields);
            openContent(deleteAccountFields);
            disableOtherButtons(deleteAccountButton);
            enableButton(deleteAccountButton);
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
                enableScroll();
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
        document.querySelectorAll('.language-option').forEach(opt => {
            opt.classList.remove('selected');
            opt.classList.add('not-selected');
        });

        this.classList.remove('not-selected');
        this.classList.add('selected');
    });
});