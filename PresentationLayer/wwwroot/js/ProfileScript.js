function switchTab(tabName) {
    document.querySelectorAll('.content-panel').forEach(panel => {
        panel.classList.remove('active');
    });

    document.querySelectorAll('.menu-btn').forEach(btn => {
        btn.classList.remove('active');
    });

    document.getElementById('tab-' + tabName).classList.add('active');

    const btns = document.querySelectorAll('.menu-btn');
    if(tabName === 'settings') btns[0].classList.add('active');
    if(tabName === 'projects') btns[1].classList.add('active');
    if(tabName === 'security') btns[2].classList.add('active');
}

async function updateUserProfile() {
    const userId = document.getElementById('profileId').value;
    const name = document.getElementById('profileName').value;
    const color = document.getElementById('profileColor').value;

    if (!name) {
        alert("Name could not be empty. Please try again.!");
        return;
    }

    try {
        const response = await fetch('/Account/UpdateProfile', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                UserId: userId,
                NewUserName: name,
                NewProfileColor: color
            })
        });

        if (response.ok) {
            window.location.reload();
        } else {
            alert("There was an error updating your profile. Please try again later. If the problem persists, please contact the site administrator.");
        }

    } catch (error) {
        console.error("Error:", error);
        alert("There was error with connecting to the server. Please try again later. If the problem persists, please contact the site administrator.");
    }
}

async function changePassword() {
    const userId = document.getElementById('profileId').value;
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (!currentPassword || !newPassword || !confirmPassword) {
        alert("Please fill in all password fields.");
        return;
    }

    if (newPassword !== confirmPassword) {
        alert("New passwords do not match!");
        return;
    }

    if (newPassword.length < 6) {
        alert("Your new password must be at least 6 characters long.");
        return;
    }

    try {
        const response = await fetch('/Account/ChangePassword', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                UserId: userId,
                CurrentPassword: currentPassword,
                NewPassword: newPassword
            })
        });

        const result = await response.json();

        if (response.ok) {
            alert("Success! " + result.message);
            document.getElementById('currentPassword').value = '';
            document.getElementById('newPassword').value = '';
            document.getElementById('confirmPassword').value = '';
        } else {
            alert("Error: " + (result.message || "Could not change password."));
        }

    } catch (error) {
        console.error("Error:", error);
        alert("Could not connect to the server.");
    }
}