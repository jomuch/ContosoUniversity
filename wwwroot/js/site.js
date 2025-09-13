// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// === DARK MODE TOGGLE LOGIC ===

document.addEventListener('DOMContentLoaded', (event) => {
    const themeToggle = document.getElementById('theme-toggle');
    const currentTheme = localStorage.getItem('theme');

    // Apply the saved theme on page load
    if (currentTheme === 'dark') {
        document.body.classList.add('dark-mode');
        themeToggle.textContent = '☀️'; // Sun icon for light mode
    }

    // Add click event listener to the button
    themeToggle.addEventListener('click', () => {
        document.body.classList.toggle('dark-mode');

        // Save the user's preference
        let theme = 'light';
        if (document.body.classList.contains('dark-mode')) {
            theme = 'dark';
            themeToggle.textContent = '☀️';
        } else {
            themeToggle.textContent = '🌙'; // Moon icon for dark mode
        }
        localStorage.setItem('theme', theme);
    });
});