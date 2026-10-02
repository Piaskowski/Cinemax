window.movieSwiper = {
    initialize: function (elementId) {
        const element = document.getElementById(elementId);

        if (!element) {
            return;
        }

        new Swiper(element, {
            grabCursor: true,

            slidesPerView: "auto",
            spaceBetween: 10,

            loop: true,
            centeredSlides: false,
            centerInsufficientSlides: true,

            freeMode: {
                enabled: true,
                sticky: true,
                momentum: true,
                momentumRatio: 0.3,
                momentumVelocityRatio: 0.5,
                momentumBounce: false
            },

            scrollbar: {
                el: element.querySelector(".swiper-scrollbar"),
                draggable: true,
            },

            navigation: {
                nextEl: element.querySelector(".swiper-button-next"),
                prevEl: element.querySelector(".swiper-button-prev"),
            },
        });
    }
};