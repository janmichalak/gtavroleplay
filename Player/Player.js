var menu = null; // handler

API.onServerEventTrigger.connect(function (name, args) {
	if(name == "hide_menu")
	{
		menu.Visible = false;
	}
	
	/**
	 *	CHARACTER MENU
	 */
	if (name == "menu_character_select") {

        var callbackId = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var noExit = args[3];

        menu = null;
        if (banner == null)
            menu = API.createMenu(subtitle, 0, 0, 6);
        else menu = API.createMenu(banner, subtitle, 0, 0, 6);

        if (noExit) {
            menu.ResetKey(menuControl.Back);
        }

        var characters = JSON.parse(args[4]);
		serverCharacters = [];

		for(i in characters) {
			if(characters[i]) {
				serverCharacters.push({id: i, name: characters[i]});
				var item = API.createMenuItem(characters[i], "ID: " + i);
				menu.AddItem(item);
			}
		}

        menu.OnItemSelect.connect(function(sender, item, index) {
            //API.displaySubtitle(serverCharacters[index].name);
			API.triggerServerEvent("menu_handler_select_item", callbackId, serverCharacters[index].name, serverCharacters[index].id);
        });

        menu.Visible = true;
    }

	/**
	 *	GLOBAL MENU
	 */
	if (name == "menu_draw") {

        var callbackId = args[0];
        var banner = args[1];
        var subtitle = args[2];
        var noExit = args[3];

        menu = null;
        if (banner == null)
            menu = API.createMenu(subtitle, 0, 0, 6);
        else menu = API.createMenu(banner, subtitle, 0, 0, 6);

        if (noExit) {
            menu.ResetKey(menuControl.Back);
        }

        var itemsLen = args[4];

        for (var i = 0; i < itemsLen; i++) {
            var item = API.createMenuItem(args[5 + i], "");
            menu.AddItem(item);
        }

        menu.OnItemSelect.connect(function(sender, item, index) {
            API.triggerServerEvent("menu_handler_select_item", callbackId, index);
        });

        menu.Visible = true;
    }
});

// Key press to start engine
API.onKeyUp.connect(function (sender, e) {
	if (e.KeyCode === Keys.U) {
		// TODO: warunek czy jest w aucie
		API.triggerServerEvent("start_stop_engine");
	}
})