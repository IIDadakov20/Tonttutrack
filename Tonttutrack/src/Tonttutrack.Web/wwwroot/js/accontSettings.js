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

    if (settingsButton && settingsContent) {
        settingsButton.addEventListener("click", () => {
            openContent(settingsContent);
            document.body.style.overflow = "hidden";
        });
    }

    if (closeSettingsPopup && settingsContent) {
        closeSettingsPopup.addEventListener("click", () => {
            closeContent(settingsContent);
            document.body.style.overflow = "";
        });
    }

    if (updateAccountButton && updateAccountFields) {
        updateAccountButton.addEventListener("click", () => {
            if (updatePasswordFields) closeContent(updatePasswordFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            openContent(updateAccountFields);
        });
    }

    if (updatePasswordButton && updatePasswordFields) {
        updatePasswordButton.addEventListener("click", () => {
            if (updateAccountFields) closeContent(updateAccountFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            openContent(updatePasswordFields);
        });
    }

    if (deleteAccountButton && deleteAccountFields) {
        deleteAccountButton.addEventListener("click", () => {
            if (updateAccountFields) closeContent(updateAccountFields);
            if (updatePasswordFields) closeContent(updatePasswordFields);
            openContent(deleteAccountFields);
        });
    }

    if (confirmationPopup) {
        document.getElementById("cancelDeleteAccount")?.addEventListener("click", () => closeContent(deleteAccountFields));
        document.getElementById("confirmDeleteAccount")?.addEventListener("click", () => {
            // Implement logic for deleting account
        });
    }

    document.addEventListener("click", (event) => {
        const isClickInsideSettings = settingsContent && settingsContent.contains(event.target) || settingsButton && settingsButton.contains(event.target);
        const isClickInsideAccount = updateAccountFields && updateAccountFields.contains(event.target) || updateAccountButton && updateAccountButton.contains(event.target);
        const isClickInsidePassword = updatePasswordFields && updatePasswordFields.contains(event.target) || updatePasswordButton && updatePasswordButton.contains(event.target);
        const isClickInsideDelete = deleteAccountFields && deleteAccountFields.contains(event.target) || deleteAccountButton && deleteAccountButton.contains(event.target);

        if (!isClickInsideSettings && !isClickInsideAccount && !isClickInsidePassword && !isClickInsideDelete) {
            if (updateAccountFields) closeContent(updateAccountFields);
            if (updatePasswordFields) closeContent(updatePasswordFields);
            if (deleteAccountFields) closeContent(deleteAccountFields);
            if (settingsContent) closeContent(settingsContent);
        }
    });
});