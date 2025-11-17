document.addEventListener("DOMContentLoaded", () => {
    document.getElementById('top-center').addEventListener('click', () => {
        Toastify({
            text: "This is toast in top center",
            duration: 3000,
            close: true,
            gravity: "top",
            position: "center",
            backgroundColor: "#4fbe87",
        }).showToast();
    })
});