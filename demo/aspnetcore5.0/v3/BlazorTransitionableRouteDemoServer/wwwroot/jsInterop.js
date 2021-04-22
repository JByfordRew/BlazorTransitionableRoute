window.yourJsInterop = {
    transitionFunction: function (effectOut, effectIn) {

        setTimeout(() => {
            let transitionIn = document.getElementsByClassName('transition-in')[0];
            let transitionOut = document.getElementsByClassName('transition-out')[0];

            if (transitionIn && transitionOut) {

                transitionOut.classList.remove('transition-out');
                transitionOut.classList.add(
                    "animate__" + effectOut,
                    "animate__faster",
                    "animate__animated"
                );

                transitionIn.classList.remove('transition-in');
                transitionIn.classList.add(
                    "animate__" + effectIn,
                    "animate__faster",
                    "animate__animated"
                );
            }
        }, 0);
    }
}