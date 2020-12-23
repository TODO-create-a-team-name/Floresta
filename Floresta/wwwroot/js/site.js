var menu = document.querySelector('.nav_menu'),
    icon = document.querySelector('.animated_menu_icon'),
    menuButton = document.querySelector('#menu_button_container'),
    open= false;

menuButton.onclick = function () {
    if (open == false) {
        icon.classList.add('menu-opened');
        menu.classList.add('menu-opened');
        open = !open;
    }
    else {
        menu.classList.add('menu-closed');
        icon.classList.remove('menu-opened');
        menu.classList.remove('menu-opened');
        setTimeout(closeMenu, 490) 
    }
    function closeMenu() {
        menu.classList.remove('menu-closed');
        open = !open;
    }   
}
