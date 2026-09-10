mergeInto(LibraryManager.library, {

    EhNavegadorMobile: function ()
    {
        // Primeiro tenta utilizar a API moderna
        // disponibilizada por alguns navegadores.
        if (navigator.userAgentData &&
            typeof navigator.userAgentData.mobile !== "undefined")
        {
            return navigator.userAgentData.mobile ? 1 : 0;
        }

        // Caso o navegador não ofereça userAgentData,
        // usamos o User Agent tradicional.
        var usuario = navigator.userAgent ||
                      navigator.vendor ||
                      window.opera ||
                      "";

        // Procura identificadores normalmente utilizados
        // por navegadores de dispositivos móveis.
        if (/Mobi|Android|iPhone|iPad|iPod/i.test(usuario))
        {
            return 1;
        }

        // Alguns iPads modernos podem se identificar
        // como Macintosh ao solicitar sites desktop.
        if (/Macintosh/i.test(usuario) &&
            navigator.maxTouchPoints > 1)
        {
            return 1;
        }

        // Se nenhuma condição anterior foi atendida,
        // consideramos navegador desktop.
        return 0;
    }

});