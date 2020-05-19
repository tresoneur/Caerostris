(() => {
    window.CaerostrisUtils = new Object();

    window.CaerostrisUtils.GetWidth = (selector) => window.document.querySelector(selector).clientWidth;

    window.CaerostrisUtils.GetHeight = (selector) => window.document.querySelector(selector).clientHeight;

    window.CaerostrisUtils.GetClientPositionX = (selector) => window.document.querySelector(selector).getBoundingClientRect().left;

    window.CaerostrisUtils.RemoveCssClass = (selector, cssClass) => {
        const selected = window.document.querySelectorAll(selector);
        selected.forEach(s => s.classList.remove(cssClass));
    };
})();