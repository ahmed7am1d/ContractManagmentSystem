
    //[1] Lottie Animation
    var animation = bodymovin.loadAnimation({
        container: document.getElementById('anim'),
        renderer: 'svg',
        loop: true,
        autoplay: true,
        path: './LottieAnimation/37459-searching-contract.json'
    })

    //[2] Fade Out Splash Screen , using jquery function
    var splashScreenDiv = document.querySelector('.splashScreen');
    var animationContainer = document.querySelector('.animationContainer');

    setTimeout(function () {
        $('.splashScreen').fadeOut(1000);

    }, 4000);

    //[3] go forward to contaract 
    setTimeout(function () {

        window.location.href = window.location.href + "Home/Contract"
    }, 5000)
