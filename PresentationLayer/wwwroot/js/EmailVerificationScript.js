async function VerifyEmailCode(email) {
    const codeInput = document.getElementById("code-Input");
    const userEnteredCode = codeInput.value.trim();

    if (!userEnteredCode) {
        alert("Please enter the verification code.");
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
                EnteredCode: userEnteredCode
            })
        });

        const data = await response.json();

        if (data.success) {
            const params = new URLSearchParams({
                UserName: data.name,
                Email: data.email,
            });
            window.location.href = `/User/PasswordCreation?${params.toString()}`;
        } else {
            alert('Invalid code! Please try again.');
            btn.disabled = false;
            btn.innerText = "Verify and Continue";
        }
    } catch (e) {
        console.error(e);
        alert("An error occurred.");
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
            alert("Verification code resent.");
        } else {
            alert("Error: Code could not be sent.");
        }
    } catch (e) {
        console.error(e);
        alert("Server error.");
    } finally {
        btn.disabled = false;
        btn.innerText = "Resend Code";
    }
}

function BackArrowClick(name, email) {
    const params = new URLSearchParams({
        UserName: name,
        Email: email
    });
    window.location.href = `/User/CreateAccount?${params.toString()}`;
}