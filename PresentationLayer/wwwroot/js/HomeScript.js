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

function toggleDropdown() {
    const dropdown = document.getElementById("userDropdown");
    if (dropdown) {
        if (dropdown.style.display === "none" || dropdown.style.display === "") {
            dropdown.style.display = "block";
        } else {
            dropdown.style.display = "none";
        }
    }
}

document.addEventListener('click', function(event) {
    const dropdown = document.getElementById("userDropdown");
    const trigger = document.querySelector(".profile-trigger");

    if (dropdown && trigger && !trigger.contains(event.target) && !dropdown.contains(event.target)) {
        dropdown.style.display = "none";
    }
});


function SignIn() {
    window.location.href = '/Home/SignIn';
}

function CreateAccount() {
    window.location.href = '/Home/CreateAccount';
}