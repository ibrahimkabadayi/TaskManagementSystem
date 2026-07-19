async function VerifyEmailCode(email, name) {
    const codeInput = document.getElementById("code-Input");
    const userEnteredCode = codeInput.value.trim();

    if (!userEnteredCode) {
        showToast("Please enter the verification code.", "warning");
        codeInput.focus();
        return false;
    }

    const btn = document.querySelector('.btn-verify');
    btn.disabled = true;
    btn.innerText = "Verifying...";

    try {
        const response = await fetch('/Email/VerifyCode', {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                Email: email,
                EnteredCode: userEnteredCode,
                UserName: name
            })
        });

        const data = await response.json();

        if (data.success) {
            const params = new URLSearchParams({
                userName: data.name,
                email: data.email,
            });
            window.location.href = `/Home/PasswordCreation?${params.toString()}`;
        } else {
            showToast("Incorrect verification code.", "warning")
            btn.disabled = false;
            btn.innerText = "Verify and Continue";
        }
    } catch (e) {
        console.error(e);
        showToast("Server error.", "error");
        btn.disabled = false;
        btn.innerText = "Verify and Continue";
    }
}

async function SendEmailCodeAgain(email) {
    const btn = document.querySelector('.btn-resend');
    btn.innerText = "Sending...";
    btn.disabled = true;

    try {
        const response = await fetch('/Email/SendEmailVerificationCode', {
            method: 'POST',
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Email: email })
        });

        const data = await response.json();

        if (data.success) {
            showToast("Verification code sent successfully.", "success");
        } else {
            showToast("Failed to send verification code.", "error");
        }
    } catch (e) {
        console.error(e);
        showToast("Server error.", "error");
    } finally {
        btn.disabled = false;
        btn.innerText = "Resend Code";
    }
}

function BackArrowClick(name, email) {
    const params = new URLSearchParams({
        userName: name,
        email: email
    });
    window.location.href = `/Home/CreateAccount?${params.toString()}`;
}