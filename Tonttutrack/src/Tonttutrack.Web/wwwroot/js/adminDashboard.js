$(document).ready(function () {
    // Update Device
    $(".device-edit-btn").on("click", function () {
        let code = $(this).data("code");
        let row = $("#row-" + code);

        // Показване на input-ите и скриване на бутоните
        row.find(".device-name").addClass("device-hidden");
        row.find(".device-edit-name").removeClass("device-hidden");
        row.find(".device-update-container").removeClass("device-hidden");
        row.find(".device-edit-btn, .device-remove-btn").addClass("device-hidden");
    });

    // Save Device
    $(".device-save-btn").on("click", function () {
        let code = $(this).data("code");
        let row = $("#row-" + code);
        let newName = row.find(".device-edit-name").val();
        let newPassword = row.find(".device-password-input").val();

        console.log(`Saving: Name = ${newName}, Password = ${newPassword}, Code = ${code}`);

        // Връща нормалния изглед
        row.find(".device-name").text(newName).removeClass("device-hidden");
        row.find(".device-edit-name").addClass("device-hidden");
        row.find(".device-update-container").addClass("device-hidden");
        row.find(".device-edit-btn, .device-remove-btn").removeClass("device-hidden");
    });

    // Delete Device
    $(".device-remove-btn").on("click", function () {
        if (confirm("Are you sure you want to delete this device?")) {
            let code = $(this).data("code");
            $("#row-" + code).remove();
        }
    });

    // Add New Device
    $("#add-device-btn").on("click", function () {
        $(".device-add-container").toggleClass("device-hidden");
    });

    // Save New Device
    $("#save-new-device-btn").on("click", function () {
        let newName = $("#new-device-name").val();
        let newCode = $("#new-device-code").val();
        let newPassword = $("#new-device-password").val();

        if (newName && newCode && newPassword) {
            let newRow = `
                <div class="device-row" id="row-${newCode}">
                    <div class="device-column">
                        <span class="device-name">${newName}</span>
                        <input type="text" class="device-edit-name device-hidden" value="${newName}">
                    </div>
                    <div class="device-column">${newCode}</div>
                    <div class="device-column">
                        <button class="device-btn device-edit-btn" data-code="${newCode}" data-translate="dashboard.updateD">Update Device</button>
                        <button class="device-btn device-remove-btn" data-code="${newCode}" data-translate="dashboard.delete">Delete Device</button>
                        <div class="device-update-container device-hidden">
                            <input type="password" class="device-password-input" placeholder="New Password">
                            <button class="device-btn device-save-btn" data-code="${newCode}" data-translate="dashboard.save">Update</button>
                        </div>
                    </div>
                </div>
            `;

            $(".device-container").append(newRow);
            $(".device-add-container").addClass("device-hidden");
            $("#new-device-name").val("");
            $("#new-device-code").val("");
            $("#new-device-password").val("");
        } else {
            alert("Please fill all fields");
        }
    });
});