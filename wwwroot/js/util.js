(() => {
    window.CaerostrisUtils = new Object();

    window.CaerostrisUtils.GetWidth = (selector) => window.document.querySelector(selector).clientWidth;

    window.CaerostrisUtils.GetClientPositionX = (selector) => window.document.querySelector(selector).getBoundingClientRect().left;

    window.CaerostrisUtils.RemoveCssClass = (selector, cssClass) => {
        const selected = window.document.querySelectorAll(selector);
        selected.forEach(s => s.classList.remove(cssClass));
    };

    window.CaerostrisUtils.WriteClipboard = (text) => navigator.clipboard.writeText(text);
})();