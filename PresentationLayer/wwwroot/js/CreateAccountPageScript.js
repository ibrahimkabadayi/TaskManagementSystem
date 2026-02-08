async function nextButtonClick() {
    const nameInput = document.getElementById("name-Input");
    const emailInput = document.getElementById("email-input");

    const name = nameInput.value.trim();
    const email = emailInput.value.trim();

    if (name.length < 3) {
        alert("Please enter a valid full name (at least 3 characters).");
        nameInput.focus();
        return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        alert("Please enter a valid email address.");
        emailInput.focus();
        return;
    }

    const btn = document.querySelector('.btn-primary');
    const originalText = btn.innerText;
    btn.disabled = true;
    btn.innerText = "Loading...";

    try {
        const checkResponse = await fetch("/User/CreateAccountCheck", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                Username: name,
                Email: email
            })
        });

        const checkData = await checkResponse.json();

        if (checkData.success) {
            const emailResponse = await fetch('/Email/SendCode', {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ Email: email })
            });

            const emailData = await emailResponse.json();

            if (emailData.success) {
                const params = new URLSearchParams({
                    email: email,
                    name: name
                });
                window.location.href = `/Home/EmailCodeVerification?${params.toString()}`;
            } else {
                alert("Could not send code: " + emailData.message);
                btn.disabled = false;
                btn.innerText = originalText;
            }

        } else {
            alert(checkData.message);
            btn.disabled = false;
            btn.innerText = originalText;
        }

    } catch (error) {
        console.error("Error:", error);
        alert("An error occurred. Please try again.");
        btn.disabled = false;
        btn.innerText = originalText;
    }
}