document.addEventListener('DOMContentLoaded', function() {
    const themeToggle = document.getElementById('themeToggle');

    // Проверка за запазена тема в localStorage
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
        enableDarkTheme();
        themeToggle.checked = true;
    }

    // Слушател за промяна на toggle бутона
    themeToggle.addEventListener('change', function() {
        if (this.checked) {
            enableDarkTheme();
            localStorage.setItem('theme', 'dark');
        } else {
            disableDarkTheme();
            localStorage.setItem('theme', 'light');
        }
    });

    function enableDarkTheme() {
        // Промяна на стойностите за главния root
        document.documentElement.style.setProperty('--primary-color', '#005965');
        document.documentElement.style.setProperty('--secondary-color', '#004953');
        document.documentElement.style.setProperty('--background-color', '#272b2b');
        document.documentElement.style.setProperty('--text-color', '#ededed');
        document.documentElement.style.setProperty('--hover-color', '#656565');
        document.documentElement.style.setProperty('--error-color', '#ff4444');
        document.documentElement.style.setProperty('--success-color', '#00C851');
        document.documentElement.style.setProperty('--disabled-background', '#575757');
        document.documentElement.style.setProperty('--white', '#222222');

        // Промяна на стойностите за тракера
        document.documentElement.style.setProperty('--light-gray', '#444444');
        document.documentElement.style.setProperty('--active-route', '#1a1a1a');
        document.documentElement.style.setProperty('--text-muted', '#aaaaaa');
        document.documentElement.style.setProperty('--box-shadow-color', 'rgba(64,22,22,0.2)');

        // Промяна на стойностите за началния екран
        document.documentElement.style.setProperty('--hero-heading-color', '#ffffff');
        document.documentElement.style.setProperty('--hero-lead-color', '#cccccc');
        document.documentElement.style.setProperty('--overlay-bg-color', 'rgba(1,55,64,0.8)');
        document.documentElement.style.setProperty('--primary-gradient-color', '#282e33');
        document.documentElement.style.setProperty('--secondary-gradient-color', '#19252b');

        // Промяна на стойностите за логин екрана
        document.documentElement.style.setProperty('--card-shadow', 'rgba(255, 255, 255, 0.1)');
        document.documentElement.style.setProperty('--input-bg', 'rgba(0, 0, 0, 0.5)');
        document.documentElement.style.setProperty('--input-focus-shadow', 'rgba(255, 255, 255, 0.5)');
        document.documentElement.style.setProperty('--card-bg', 'rgba(18, 18, 18, 0.8)');

        // Промяна на стойностите за настройките
        document.documentElement.style.setProperty('--button-hover-primary', '#004d57');
        document.documentElement.style.setProperty('--button-hover-error', '#c53636');
        document.documentElement.style.setProperty('--button-hover-delete', '#a12b2b');
        document.documentElement.style.setProperty('--disabled-color', '#b3b3b3');
        document.documentElement.style.setProperty('--negative-text-color', '#0E080B');

    }

    function disableDarkTheme() {
        // Връщане на стойностите за главния root
        document.documentElement.style.setProperty('--primary-color', '#0d8191');
        document.documentElement.style.setProperty('--secondary-color', '#004953');
        document.documentElement.style.setProperty('--background-color', '#f2f7f6');
        document.documentElement.style.setProperty('--text-color', '#0E080B');
        document.documentElement.style.setProperty('--hover-color', '#f0f0f0');
        document.documentElement.style.setProperty('--error-color', '#d9534f');
        document.documentElement.style.setProperty('--success-color', '#0d8191');
        document.documentElement.style.setProperty('--disabled-background', '#ccc');
        document.documentElement.style.setProperty('--white', 'white');

        // Връщане на стойностите за тракера
        document.documentElement.style.setProperty('--light-gray', '#ddd');
        document.documentElement.style.setProperty('--active-route', '#a0c4ff');
        document.documentElement.style.setProperty('--text-muted', 'gray');
        document.documentElement.style.setProperty('--box-shadow-color', 'rgba(0, 0, 0, 0.2)');

        // Връщане на стойностите за началния екран
        document.documentElement.style.setProperty('--hero-heading-color', '#10262e');
        document.documentElement.style.setProperty('--hero-lead-color', '#162f37');
        document.documentElement.style.setProperty('--overlay-bg-color', 'rgba(0, 73, 83, 0.8)');
        document.documentElement.style.setProperty('--primary-gradient-color', '#ebf5ef');
        document.documentElement.style.setProperty('--secondary-gradient-color', '#004953');

        // Връщане на стойностите за логин екрана
        document.documentElement.style.setProperty('--card-shadow', 'rgba(0, 73, 83, 0.1)');
        document.documentElement.style.setProperty('--input-bg', 'rgba(255, 255, 255, 0.5)');
        document.documentElement.style.setProperty('--input-focus-shadow', 'rgba(13, 129, 145, 0.5)');
        document.documentElement.style.setProperty('--card-bg', 'rgba(242, 247, 246, 0.8)');

        // Връщане на стойностите за настройките
        document.documentElement.style.setProperty('--button-hover-primary', '#0b6a76');
        document.documentElement.style.setProperty('--button-hover-error', '#c9302c');
        document.documentElement.style.setProperty('--button-hover-delete', '#e84051');
        document.documentElement.style.setProperty('--disabled-color', '#7f7f7f');
        document.documentElement.style.setProperty('--negative-text-color', 'white');
    }
});