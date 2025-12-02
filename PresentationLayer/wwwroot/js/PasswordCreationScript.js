async function ConfirmPassword(name, email){
    const password = document.getElementById("Password").value;
    const passwordAgain = document.getElementById("Password-again").value;
    
    if (password == null){
        alert("Password is required");
        return;
    }
    
    if (name == null || email == null){
        alert("No email or name has send fatal error.")
        return;
    }
    
    if(password !== passwordAgain){
        alert("Please enter the same password");
        return;
    }
    
    await fetch('User/CreateAccount',{
        method: 'POST',
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Name : name,
            Email : email,
            Password: password
        })
    }).then(response => response.json()).then(async data => {
        if (data.data.success) {
            await fetch('Authenticate/RegisterSignIn',{
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Email : data.data.Email,
                    Password: data.data.Password,
                })
            }).then(response => response.json()).then(async data => {
                if (data.success) {
                    window.location.href = 'Home/Home';
                }
                else{
                    const params = new URLSearchParams({
                        Message: "Authentication Failed",
                        Type: "Authentication",
                        StatusCode: -9999,
                        TimeStamp: new Date().toISOString()
                    });
                    window.location.href = `/Home/Error?${params.toString()}`;
                }
            })
        }else{
            alert("Could not register account");
        }
    })
}

function HidePassword(){
    
}

function BackButtonClick(){
    
}