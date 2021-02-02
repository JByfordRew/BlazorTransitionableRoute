//From https://github.com/johanholmerin/popstate-direction/blob/master/index.js

// Keep track of current position
let currentIndex = (history.state && history.state.index) || 0;

// Set initial index, before replacing setters
if (!history.state || !('index' in history.state)) {
    history.replaceState(
        { index: currentIndex, state: history.state },
        document.title
    );
}

// Native functions
const getState = Object.getOwnPropertyDescriptor(History.prototype, 'state')
    .get;
const { pushState, replaceState } = history;

// Detect forward and back changes
function onPopstate() {
    const state = getState.call(history);

    // State is unset when `location.hash` is set. Update with incremented index
    if (!state) {
        replaceState.call(history, { index: currentIndex + 1 }, document.title);
    }
    const index = state ? state.index : currentIndex + 1;

    const direction = index > currentIndex ? 'forward' : 'back';
    window.dispatchEvent(new Event(direction));

    currentIndex = index;
}

// Create functions which modify index
function modifyStateFunction(func, n) {
    return (state, ...args) => {
        func.call(history, { index: currentIndex + n, state }, ...args);
        // Only update currentIndex if call succeeded
        currentIndex += n;
    };
}

// Override getter to only return the real state
function modifyStateGetter(cls) {
    const { get } = Object.getOwnPropertyDescriptor(cls.prototype, 'state');

    Object.defineProperty(cls.prototype, 'state', {
        configurable: true,
        enumerable: true,
        get() {
            return get.call(this).state;
        },
        set: undefined
    });
}

modifyStateGetter(History);
modifyStateGetter(PopStateEvent);
history.pushState = modifyStateFunction(pushState, 1);
history.replaceState = modifyStateFunction(replaceState, 0);
window.addEventListener('popstate', onPopstate);

//End https://github.com/johanholmerin/popstate-direction/blob/master/index.js

//Interop
window.blazorTransitionableRoute = {
    dotnetHelperPrimary: undefined,
    dotnetHelperSecondary: undefined,
    init: function (dotnetHelper, isPrimary) {

        if (isPrimary) {
            window.blazorTransitionableRoute.dotnetHelperPrimary = dotnetHelper;
            let lastLocation = location.href;
            let isBackwards = false;
            let invokeTransition = () => {
                window.blazorTransitionableRoute.dotnetHelperPrimary.invokeMethodAsync('Navigate', isBackwards);
                window.blazorTransitionableRoute.dotnetHelperSecondary.invokeMethodAsync('Navigate', isBackwards);
            }
            setInterval(function () {
                if (lastLocation != location.href) {
                    lastLocation = location.href;
                    invokeTransition();
                    isBackwards = false;
                }
            }, 25);
            window.addEventListener('back', event => {
                isBackwards = true;
            });
            document.body.addEventListener("webkitAnimationEnd", this.AddAnimationEventsListner);
            document.body.addEventListener("animationend", this.AddAnimationEventsListner);
        } else {
            window.blazorTransitionableRoute.dotnetHelperSecondary = dotnetHelper;
        }
    },
    AddAnimationEventsListner(e) {
        if (event.target.classList.contains("b-Transitionable-route")) {
            window.blazorTransitionableRoute.dotnetHelperPrimary.invokeMethodAsync('Hide');
            window.blazorTransitionableRoute.dotnetHelperSecondary.invokeMethodAsync('Hide');
        }
    }
}