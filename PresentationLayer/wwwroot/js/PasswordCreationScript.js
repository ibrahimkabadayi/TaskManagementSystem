function TogglePassword(inputId, iconElement) {
    const input = document.getElementById(inputId);

    if (input.type === "password") {
        input.type = "text";
        iconElement.classList.remove("fa-eye");
        iconElement.classList.add("fa-eye-slash");
    } else {
        input.type = "password";
        iconElement.classList.remove("fa-eye-slash");
        iconElement.classList.add("fa-eye");
    }
}

async function ConfirmPassword(name, email) {
    const passwordInput = document.getElementById("password-input");
    const passwordAgainInput = document.getElementById("password-again-input");

    const password = passwordInput.value;
    const passwordAgain = passwordAgainInput.value;

    if (!password) {
        alert("Please set a password.");
        passwordInput.focus();
        return;
    }

    if (password.length < 6) {
        alert("Your password must be at least 6 characters long.");
        passwordInput.focus();
        return;
    }

    if (password !== passwordAgain) {
        alert("Passwords do not match, please check again.");
        passwordAgainInput.focus();
        return;
    }

    const btn = document.querySelector('.btn-finish');
    btn.innerText = "Creating Account...";
    btn.disabled = true;

    try {
        const response = await fetch('/Authenticate/Register', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                Name: name,
                Email: email,
                Password: password,
            })
        });

        const data = await response.json();

        if (data.success) {
            alert("Account created successfully! You can now log in.");
            window.location.href = '/Home/SignIn';
        } else {
            alert("Registration Failed: " + (data.message || "Unknown error"));

            if (data.errorCode === -9999) {
                const params = new URLSearchParams({
                    Message: "Authentication Failed",
                    Type: "Authentication",
                    StatusCode: -9999,
                    TimeStamp: new Date().toISOString()
                });
                window.location.href = `/Home/Error?${params.toString()}`;
            } else {
                btn.disabled = false;
                btn.innerText = "Complete Registration";
            }
        }
    } catch (e) {
        console.error(e);
        alert("A server error occurred.");
        btn.disabled = false;
        btn.innerText = "Complete Registration";
    }
}

function BackButtonClick(name, email) {
    const params = new URLSearchParams({
        UserName: name,
        Email: email
    });
    window.location.href = `/User/EmailCodeVerification?${params.toString()}`;
}