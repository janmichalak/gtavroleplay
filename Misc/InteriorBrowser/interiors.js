/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />

var interiorsData = [];
var intLocs = API.createMenu("Interiors", "", 0, 0, 6);
var intPlaces = API.createMenu("Interiors", "Choose an interior", 0, 0, 6);

intLocs.AddItem(API.createMenuItem("Online Bunkers Exterior", ""));
intLocs.AddItem(API.createMenuItem("Online Apartments", ""));
intLocs.AddItem(API.createMenuItem("Arcadius Business Centre", ""));
intLocs.AddItem(API.createMenuItem("Maze Bank Building", ""));
intLocs.AddItem(API.createMenuItem("Lom Bank", ""));
intLocs.AddItem(API.createMenuItem("Maze Bank West", ""));
intLocs.AddItem(API.createMenuItem("Club & Warehouses", ""));
intLocs.AddItem(API.createMenuItem("Special Locations", ""));


intLocs.OnItemSelect.connect(function (sender, item, index) {
    intLocs.Visible = false;
    intPlaces.Clear();

    Object.keys(interiorsData).forEach(function (idx) {
        if (interiorsData[idx].place == item.Text) {
            intPlaces.AddItem(API.createMenuItem(interiorsData[idx].interior_name, "index" + idx));
        }
    });
    intPlaces.Visible = true;
});

intPlaces.OnItemSelect.connect(function (sender, item, index) {

    Object.keys(interiorsData).forEach(function (idx) {
        if (item.Text == interiorsData[idx].interior_name) {
            API.triggerServerEvent("requestIplForPlayer", interiorsData[idx].ipl_name, API.toJson(interiorsData[idx].position));
            intPlaces.Visible = false;
            API.showCursor(false);
        }
    });
});

intPlaces.OnMenuClose.connect(function (sender) {
    if (intLocs.Visible != true) {
        API.showCursor(false);
    }
});

intLocs.OnMenuClose.connect(function (sender) {
    if (intPlaces.Visible != true) {
        API.showCursor(false);
    }
});

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case 'registerInteriorsData': {
            interiorsData = JSON.parse(args[0]);
            break;
        }
        case 'showInteriorLocations': {
            API.showCursor(true);
            intLocs.Visible = true;
            break;
        }
    }
});

API.onUpdate.connect(function () {
    //API.drawMenu(intLocs);
    //API.drawMenu(intPlaces);
});