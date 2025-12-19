window.addEventListener('scroll', () => {
    const nav = document.querySelector('nav');
    if (window.scrollY > 10) {
        nav.style.boxShadow = '0 4px 6px -1px rgba(0, 0, 0, 0.05)';
    } else {
        nav.style.boxShadow = 'none';
    }
});
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

console.log(window.UserData.userName);
console.log(window.UserData.userEmail);

window.onload = function() {
    const name = window.UserData.userName;
    const email = window.UserData.userEmail;
    
    if(name === "default" || email === "default") {
        return;
    }
    
    const navButtons = document.getElementById('nav-buttons');
    const signInButton = navButtons.children[0];
    
    signInButton.remove();
    
    let accountSection = document.createElement('div');
    accountSection.className = 'account-section';

    const firstLetter = name.charAt(0).toUpperCase();

    accountSection.innerHTML = `
        <img class="profilePicture" src="" alt="${firstLetter}">
        <p class="userName">${name}</p>
    `;
    
    const dropdown = document.createElement('div');
    dropdown.className = 'accountDropdown';
    dropdown.innerHTML = `
        <div class="dropdownHeader">
            <img class="profilePicture" src="" alt="${firstLetter}">
            <div class="dropdownUserInfo">
                <p class="userName">${name}</p>
                <p class="userEmail">${email}</p>
            </div>
        </div>
        <div class="dropdownLinks">
            <a href="/profile">My Profile</a>
            <a href="/settings">Settings</a>
            <a href="/help">Help & Support</a>
            <a href="/logout">Sign Out</a>
        </div>
    `;
    
    accountSection.appendChild(dropdown);
    navButtons.appendChild(accountSection);
}

function SignIn() {
    window.location.href = '/Home/SignIn';
}

function CreateAccount() {
    window.location.href = '/Home/CreateAccount';
}