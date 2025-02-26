const translations = {
    en: {
        homePage: {
            heroHeading: "Real-Time GPS Tracking & Route Statistics.",
            heroLead: "Discover the ultimate tool for navigating your world with precision. Our user-friendly web app offers a powerful combination of real-time GPS tracking and route statistics.",
            joinForFree: "Join for free",
            connect: "Connection",
            track: "Live Tracking",
            stats: "Route statistics",
            connectText: "You need to log in to your account to connect and track your devices.",
            trackText: "Connecting to a device enables real-time tracking with markers on the map.",
            statsText: "You can save routes, and access their statistics from the saved route menu."
        },
        auth: {
            login: "Log in",
            register: "Sign up",
            email: "Email",
            password: "Password",
            showPassword: "Show Password",
            newToTonttutrack: "New to Tonttutrack?",
            createAccount: "Create an account",
            alreadyHaveAccount: "Already have an account?",
            signIn: "Sign in",
            username: "Username",
            confirmPassword: "Confirm Password"
        },
        accountSettings: {
            settings: "Settings",
            darkTheme: "Dark Theme",
            language: "Language",
            updateAccountDetails: "Update Account Details",
            changeAccountPassword: "Change Account Password",
            deleteAccount: "Delete Account",
            update: "Update",
            change: "Change",
            delete: "Delete",
            username: "Username",
            email: "Email",
            currentPassword: "Current Password",
            newPassword: "New Password",
            confirmPassword: "Confirm Password",
            enterPasswordToDelete: "Enter Password to Delete",
            logout: "Logout"
        },
        tracker: {
            code: "Code",
            password: "Password",
            showPassword: "Show Password",
            connect: "Connect",
            startRecording: "Start recording",
            savedRoutes: "Saved Routes",
            routeName: "Route Name",
            save: "Save",
            duration: "Duration",
            distance: "Distance",
            avgSpeed: "Avg. Speed",
            topSpeed: "Top Speed",
            deleteRoute: "Delete Route",
            viewSavedRoutes: "View Saved Routes",
            viewLiveTracking: "View Live Tracking"
        },
        layout: {
            home: "Home",
            map: "Map",
            logIn: "Log in",
            privacy: "Privacy"
        }
    },
    bg: {
        homePage: {
            heroHeading: "GPS проследяване в реално време и статистики на маршрути.",
            heroLead: "ООткрийте най-добрия инструмент за точна навигация. Нашата лесна за използване уеб апликация предлага комбинация от GPS проследяване в реално време и статистики на маршрути.",
            joinForFree: "Присъединете се безплатно",
            connect: "Свързване",
            track: "Проследяване",
            stats: "Статистики",
            connectText: "Необходимо е да влезете в акаунта си, за да свържете и проследявате устройствата си",
            trackText: "Свързването с устройството предоставя възможност за проследяване чрез маркери на картата в реално време",
            statsText: "Може да запазвате маршрути, чиито статистика може да достъпите от менюто за маршрути"
        },
        auth: {
            login: "Вход",
            register: "Регистрация",
            email: "Email",
            password: "Парола",
            showPassword: "Покажи парола",
            newToTonttutrack: "Нямате регистрация?",
            createAccount: "Създай акаунт",
            alreadyHaveAccount: "Вече имате акаунт?",
            signIn: "Вход в съществуващ акаунт",
            username: "Потребителско име",
            confirmPassword: "Потвърди парола"
        },
        accountSettings: {
            settings: "Настройки",
            darkTheme: "Тъмна тема",
            language: "Език",
            updateAccountDetails: "Обнови данни за акаунт",
            changeAccountPassword: "Промени паролата на акаунта",
            deleteAccount: "Изтрий акаунта",
            update: "Обнови",
            change: "Смени",
            delete: "Изтрий",
            username: "Потребителско име",
            email: "Email",
            currentPassword: "Текуща парола",
            newPassword: "Нова парола",
            confirmPassword: "Потвърди парола",
            enterPasswordToDelete: "Въведете парола за изтриване",
            logout: "Изход"
        },
        tracker: {
            code: "Код",
            password: "Парола",
            showPassword: "Покажи парола",
            connect: "Свързване",
            startRecording: "Стартирай запис",
            savedRoutes: "Запазени маршрути",
            routeName: "Име на маршрута",
            save: "Запази",
            duration: "Продължителност",
            distance: "Разстояние",
            avgSpeed: "Средна скорост",
            topSpeed: "Максимална скорост",
            deleteRoute: "Изтрий маршрут",
            viewSavedRoutes: "Виж запазени маршрути",
            viewLiveTracking: "Проследяване на живо",
        },
        layout: {
            home: "Начало",
            map: "Карта",
            logIn: "Вход",
            privacy: "Поверителност"
        }
    }
};

document.addEventListener("DOMContentLoaded", function () {
    const languageOptions = document.querySelectorAll('.language-option input[type="radio"]');

    function changeLanguage(lang) {
        document.querySelectorAll("[data-translate]").forEach(element => {
            const key = element.getAttribute("data-translate");
            const [section, translationKey] = key.split(".");
            element.textContent = translations[lang][section][translationKey];
        });

        document.querySelectorAll(".dynamic-text").forEach(element => {
            const key = element.getAttribute("data-key");
            const [section, translationKey] = key.split(".");
            element.textContent = translations[lang][section][translationKey];
        });

        localStorage.setItem("selectedLanguage", lang);
    }

    const savedLanguage = localStorage.getItem("selectedLanguage") || "en";
    changeLanguage(savedLanguage);

    document.querySelector(`input[type="radio"][value="${savedLanguage}"]`).checked = true;

    languageOptions.forEach(option => {
        option.addEventListener("change", function () {
            const selectedLanguage = this.value;
            changeLanguage(selectedLanguage);
        });
    });
});