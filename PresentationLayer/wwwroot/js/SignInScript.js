function CreateAccount(){
    window.location.href = 'Home/CreateAccount';
}

async function SignIn(){
    const email = document.getElementById("email-Input").value;
    const password = document.getElementById("password-Input").value;
    
    if(email.length < 3 || !email.includes("@")){
        alert("Please enter your email");
        return;
    }
    
    if(password.length < 3){
        alert("Please enter your password");
        return;
    }
    
    try
    {
        await fetch('Authenticate/RegisterSignIn/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Email: email,
                Password: password,
            })
        }.then((response) => response.json()).then((data) => {
            if (data.success) {
                const params = data.Data;
                window.location.href = 'Home/Home';
            } else {
                alert(data.message);
                if(data.message === "Wrong password"){
                    return;
                }
                const params = new URLSearchParams({
                    Message: data.error,
                    Type: 'RegisterError',
                    StatusCode: data.errorCode,
                    TimeStamp: new Date().toISOString()
                });
                window.location.href = `/Home/Error?${params.toString()}`;
            }
        }))          
    } 
    catch (e)
    {
        alert(e.message);
        return false;
    }
    return true;
}

function PrivacyPolicy(){
    
}

function ForgotPassword(){
    window.location.href = 'Home/PasswordCreation';
}