window.movieSwiper = {
    initialize: function (elementId) {
        const element = document.getElementById(elementId);

        if (!element) {
            return;
        }

        new Swiper(element, {
            //effect: "coverflow",
            grabCursor: true,
            centeredSlides: true,
            slidesPerView: "auto",
            spaceBetween: 10,

            //coverflowEffect: {
            //    rotate: 10,
            //    stretch: 0,
            //    depth: 100,
            //    modifier: 1,
            //    slideShadows: false
            //},

            loop: true,
            grabCursor: true,
            freeMode: {
                enabled: true,
                sticky: true,
                momentum: true,
                momentumRatio: 0.3,
                momentumVelocityRatio: 0.5,
                momentumBounce: false
            },
            scrollbar: {
                el: '.swiper-scrollbar',
                draggable: true,
            },
            navigation: {
                nextEl: element.querySelector(".swiper-button-next"),
                prevEl: element.querySelector(".swiper-button-prev"),
                addIcons: true,
            },
        });
    }
};