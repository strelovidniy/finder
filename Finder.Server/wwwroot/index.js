const supportedLanguages = [
    "en-US",
    "de-DE",
    "fr-FR",
    "es-ES",
    "it-IT",
    "ja-JP",
    "uk-UA"
]

const handleOnLoad = () => {
    const storedLocale = localStorage.getItem('locale');
    const href = window.location.href;
    const origin = window.location.origin;

    let path = href.replace(origin, '');

    if (path.includes('/#/')) {
        path = path.split('/#/')[1];
    }

    if (!path.startsWith('/#')) {
        path = `/#${path}`;
    }

    let languageToDisplay = undefined;

    if (supportedLanguages.includes(storedLocale)) {
        languageToDisplay = storedLocale;
    } else {
        let languages = navigator.languages;

        let supportedLanguage = getSupportedLanguage(languages);

        supportedLanguage = supportedLanguage || getSupportedLanguage(navigator.language.split('-'));

        languageToDisplay = supportedLanguage || "en-US";
    }

    window.location.href = `/${languageToDisplay}/${path}`;
}

const getSupportedLanguage = (languages) => {
    let selectedLanguage = null;

    languages.forEach((language) => {
        const foundLanguage = supportedLanguages.find(lang => lang.split('-')[0].toLowerCase() == language.split('-')[0].toLowerCase() || lang.split('-')[0].toLowerCase() == language.split('-')[1]?.toLowerCase());

        if (!!foundLanguage && !selectedLanguage) {
            selectedLanguage = foundLanguage;

            return;
        }
    });

    return selectedLanguage;
}

handleOnLoad();
