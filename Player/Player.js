/// <reference path="../types-gt-mp/Definitions/index.d.ts" />
var menu = null; // handler
var dl = false;
var dlArray = [];

API.onServerEventTrigger.connect(function (name, args) {
	if(name == "hide_menu")
	{
		menu.Visible = false;
    }

    /**
     *  VEHICLE DL
     */
    if (name == "toggle_dl") {
        var toggle = args[0];
        if (toggle) {
            dl = true;
            var vehicles = API.getStreamedVehicles();
            for (i in vehicles) {
                if (vehicles[i]) {
                    try {
                        attachLabelToVehicleDl(vehicles[i], API.getEntitySyncedData(vehicles[i], "id"));
                    } catch (e) { }
                }
            }
        } else {
            dl = false;
            for (i in dlArray) {
                if (dlArray[i]) {
                    API.deleteEntity(dlArray[i]);
                }
            }
            dlArray = [];
        }
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
		var serverCharacters = [];

		for(var i in characters) {
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
        if (API.isPlayerInAnyVehicle(API.getLocalPlayer())) {
            API.triggerServerEvent("start_stop_engine");
        }
    }
});

// Stream In
API.onEntityStreamIn.connect(function (ent, entType) {
    if (dl) {
        if (entType == 1) {
            try {
                attachLabelToVehicleDl(ent, API.getEntitySyncedData(ent, "id"));
            } catch (e) { }
        }
    }
});

// Attach label to car
function attachLabelToVehicleDl(ent, id)
{
    //API.sendChatMessage(API.getEntityType(ent).toString());
    if (ent != undefined && API.getEntityType(ent) == 1) {
        var health = API.getVehicleHealth(ent);
        var string = "[ id: " + id + ", hp: " + health + " ]";
        var pos = API.getEntityPosition(ent);
        var label = API.createTextLabel(string, pos, 20, 0.6, true);
        API.setEntityDimension(label, API.getEntityDimension(ent));
        API.setTextLabelColor(label, 255, 50, 50, 120);
        API.attachEntity(label, ent, "engine", new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        dlArray.push(label);
    }
}