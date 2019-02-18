(function ($) {
    "use strict";
    /*--
        Menu Sticky
    -----------------------------------*/
    var windows = $(window);
    var sticky = $('.header-sticky');

    /*--
        Mobile Menu
    ------------------------*/
    $('.mobile-menu-wrap').meanmenu({
        meanScreenWidth: '767',
        meanMenuContainer: '.mobile-menu',
        meanMenuClose: '<span class="menu-close"></span>',
        meanMenuOpen: '<span class="menu-bar"></span>',
        meanRevealPosition: 'left',
        meanMenuCloseSize: '0',
    });

    /*--
        Header Search
    --------------------------------------------*/
    var searchToggle = $('.header-search-toggle');
    var searchForm = $('.header-search-form');

    searchForm.hide();
    /*-- Search Toggle --*/
    searchToggle.on('click', function () {
        if (searchToggle.hasClass('open')) {
            searchForm.animate({
                width: "toggle",
            });
            $(this).removeClass('open').find('i').removeClass('fa-close').addClass('fa-search');
        } else {
            searchForm.animate({
                width: "toggle",
            });
            $(this).addClass('open').find('i').removeClass('fa-search').addClass('fa-close');
        }
    });
    //getbreakingNews
    var urlPaths = '/Common/GetBreakingNews';
    var jqxhr = $.post(urlPaths, function (data) {
    }).done(function (data) {
        var models = data;
        $.each(models, function (i, model) {
            var item = `<li><a href="/haberdetay/${model.Id}/${model.SeName}">
                           ${model.Short.slice(0, 150)}
                           </a></li>`;

            $('#breaking-news').append(item);
        });

    }).fail(function () { }
    ).always(function () {
        /*--     Breaking News Ticker
 --------------------------------------------*/
        $('.breaking-news-ticker').newsTicker({
            row_height: 40,
            max_rows: 1,
            speed: 600,
            duration: 6000,
            prevButton: $('.news-ticker-prev'),
            nextButton: $('.news-ticker-next'),
        });

    });
    /*--        Slick Slider
    -----------------------------------*/

    /*-- Five Row Post Carousel --*/
    $('.five-row-post-carousel').slick({
        autoplay: false,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 1,
        rows: 5,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                    rows: 4,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });


    /*-- Four Row Post Carousel --*/
    $('.four-row-post-carousel').slick({
        autoplay: false,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 1,
        rows: 4,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*-- Three Row Post Carousel --*/
    $('.three-row-post-carousel').slick({
        autoplay: false,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 1,
        rows: 3,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*-- Two Row Post Carousel --*/
    $('.two-row-post-carousel').slick({
        autoplay: false,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 1,
        rows: 2,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*-- Sidebar Post Carousel --*/
    $('.sidebar-post-carousel').slick({
        autoplay: true,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 1,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*-- Video Post Slider --*/
    $('.video-post-slider').slick({
        arrows: true,
        autoplay: true,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 3,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*-- Four Column Post Carousel --*/
    $('.four-column-post-carousel').slick({
        arrows: false,
        autoplay: true,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 4,
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });

    /*-- Three Column Post Carousel --*/
    $('.three-column-post-carousel').slick({
        arrows: true,
        autoplay: true,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 3,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });

    /*-- Two Column Post Carousel --*/
    $('.two-column-post-carousel').slick({
        arrows: true,
        autoplay: true,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        infinite: true,
        slidesToShow: 2,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });
    /*--
        Custom Scroll
    -----------------------------------*/
    $(".customScroll").niceScroll();

    /*--
        Scroll Up
    -----------------------------------*/
    $.scrollUp({
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade',
        scrollText: '<i class="fa fa-angle-up"></i>',
    });

    /*--
        Magnific Video Popup
    --------------------------------*/
    var imagePopup = $('.image-popup');
    imagePopup.magnificPopup({
        type: 'image',
        tLoading: 'Loading image #%curr%...',
        gallery: {
            enabled: true,
        },
    });
    var videoPopup = $('.video-popup');
    videoPopup.magnificPopup({
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        preloader: false,
        zoom: {
            enabled: true,
        }
    });
    var urlPath = '/Common/Vakitler/izmir';
    $.post(urlPath, function (data) {

    }).done(function (data) {
        var model = data;
        $(document.getElementById("imsak")).empty();
        $(document.getElementById("imsak")).append(model.Imsak);
        $(document.getElementById("gunes")).empty();
        $(document.getElementById("gunes")).append(model.Gunes);
        $(document.getElementById("oglen")).empty();
        $(document.getElementById("oglen")).append(model.Oglen);
        $(document.getElementById("ikindi")).empty();
        $(document.getElementById("ikindi")).append(model.Ikindi);
        $(document.getElementById("aksam")).empty();
        $(document.getElementById("aksam")).append(model.Aksam);
        $(document.getElementById("yatsi")).empty();
        $(document.getElementById("yatsi")).append(model.Yatsi);

    })
        .fail(function () {

        })
        .always(function () {

        });


    $("#SehirId").on('change', function () {
        var selectedItem = $(this).val();
        var urlPath = '/Common/Vakitler/' + selectedItem;
        $.post(urlPath, function (data) {

        }).done(function (data) {
            var model = data;
            $(document.getElementById("imsak")).empty();
            $(document.getElementById("imsak")).append(model.Imsak);
            $(document.getElementById("gunes")).empty();
            $(document.getElementById("gunes")).append(model.Gunes);
            $(document.getElementById("oglen")).empty();
            $(document.getElementById("oglen")).append(model.Oglen);
            $(document.getElementById("ikindi")).empty();
            $(document.getElementById("ikindi")).append(model.Ikindi);
            $(document.getElementById("aksam")).empty();
            $(document.getElementById("aksam")).append(model.Aksam);
            $(document.getElementById("yatsi")).empty();
            $(document.getElementById("yatsi")).append(model.Yatsi);

        }).fail(function () { }).always(function () { });

    });
})(jQuery);