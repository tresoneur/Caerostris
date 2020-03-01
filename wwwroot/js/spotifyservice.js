(() => {
    window.SpotifyService = new Object();
    window.SpotifyService.GetWidth = (selector) => window.document.querySelector(selector).clientWidth;
    window.SpotifyService.GetClientPositionX = (selector) => document.querySelector(selector).getBoundingClientRect().left;
})();