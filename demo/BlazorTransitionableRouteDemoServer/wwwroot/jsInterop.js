window.yourJsInterop = {
    transitionFunction: function (back) {

        setTimeout(() => {
            let transitionIn = document.getElementsByClassName('transition-in')[0];
            let transitionOut = document.getElementsByClassName('transition-out')[0];

            let direction = back ? "Up" : "Down";

            if (transitionIn && transitionOut) {

                transitionOut.classList.remove('transition-out');
                transitionOut.classList.add(
                    "animate__fadeOut" + direction,
                    "animate__faster",
                    "animate__animated"
                );

                transitionIn.classList.remove('transition-in');
                transitionIn.classList.add(
                    "animate__fadeIn" + direction,
                    "animate__faster",
                    "animate__animated"
                );
            }
        }, 0);
    }
}