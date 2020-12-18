
    var menu = document.querySelector('.nav_menu'),
        icon = document.querySelector('.animated_menu_icon'),
        menuButton = document.querySelector('#menu_button_container');

    menuButton.onclick = function () {
        if (menu.style.display !== 'none') {
            menu.classList.toggle('menu-opened');
            icon.classList.toggle('menu-opened');
        }
        else {
            menu.classList.remove('menu-opened');
            icon.classList.remove('menu-opened');
        }
    }
