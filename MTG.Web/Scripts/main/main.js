var AllCards = (function () {
    function AllCards(viewModel) {
        var _this = this;
        this.viewModel = viewModel;
        this.modalConatiner = $("#addCard");
        this.deckIds = ko.observableArray([]);
        this.clickAdd = function (id) {
            $.ajax({
                url: "/Cards/GetAddInfo",
                type: "POST",
                data: { cardId: id },
                cache: false,
                success: function (results) {
                    _this.viewModel.cardAmounts(results);
                    $("#addCard").modal({
                        show: true,
                        backdrop: 'static'
                    });
                },
                error: function (error) {
                }
            });
        };
        this.hoverOverCard = function (id) {
            $("#" + id).addClass('card-hover');
            _this.viewModel.cards().forEach(function (c) {
                if (c.id === id) {
                    c.hovering(true);
                }
            });
            _this.viewModel.selectedCard(id);
        };
        this.hoverOffCard = function (id) {
            $("#" + id).removeClass('card-hover');
            _this.viewModel.cards().forEach(function (c) {
                if (c.id === id) {
                    c.hovering(false);
                }
            });
        };
        this.remove = function (id, list) {
            var self = _this;
            if (list === "set") {
                var c = 0;
                _this.viewModel.setList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.setList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.setList.splice([0], 1, { id: 0, value: list.value, set: list.set, operator: "AND" });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.setList().length === 0) {
                    _this.viewModel.setOperator("AND");
                }
            }
            if (list === "name") {
                var c = 0;
                _this.viewModel.nameList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.nameList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.nameList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.nameList().length === 0) {
                    _this.viewModel.nameOperator("AND");
                }
            }
            if (list === "type") {
                var c = 0;
                _this.viewModel.typeList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.typeList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.typeList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.typeList().length === 0) {
                    _this.viewModel.typeOperator("AND");
                }
            }
            if (list === "subType") {
                var c = 0;
                _this.viewModel.subTypeList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.subTypeList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.subTypeList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.subTypeList().length === 0) {
                    _this.viewModel.subTypeOperator("AND");
                }
            }
            if (list === "manaCost") {
                var c = 0;
                _this.viewModel.manaCostList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.manaCostList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.manaCostList.splice([0], 1, { id: 0, value: list.value, operator: "AND", comparison: list.comparison });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.manaCostList().length === 0) {
                    _this.viewModel.manaCostOperator("AND");
                }
            }
            if (list === "power") {
                var c = 0;
                _this.viewModel.powerList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.powerList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.powerList.splice([0], 1, { id: 0, value: list.value, operator: "AND", comparison: list.comparison });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.powerList().length === 0) {
                    _this.viewModel.powerOperator("AND");
                }
            }
            if (list === "toughness") {
                var c = 0;
                _this.viewModel.toughnessList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.toughnessList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.toughnessList.splice([0], 1, { id: 0, value: list.value, operator: "AND", comparison: list.comparison });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.toughnessList().length === 0) {
                    _this.viewModel.toughnessOperator("AND");
                }
            }
            if (list === "rarity") {
                var c = 0;
                _this.viewModel.rarityList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.rarityList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.rarityList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.rarityList().length === 0) {
                    _this.viewModel.rarityOperator('AND');
                }
            }
            if (list === "text") {
                var c = 0;
                _this.viewModel.textList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.textList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.textList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.textList().length === 0) {
                    _this.viewModel.textOperator("AND");
                }
            }
            if (list === "artist") {
                var c = 0;
                _this.viewModel.artistList.remove(function (remove) { return remove.id === id; });
                _this.viewModel.artistList().forEach(function (list) {
                    if (c === 0 && list.operator === "OR") {
                        self.viewModel.artistList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                        c += 1;
                    }
                    else {
                        c += 1;
                    }
                });
                if (_this.viewModel.artistList().length === 0) {
                    _this.viewModel.artistOperator("AND");
                }
            }
        };
        _.delay(function () {
            var loadingIndicator = jQuery(" .loadingIndicator");
            loadingIndicator.fadeOut(100, function () { loadingIndicator.remove(); });
        }, 300);
        viewModel.test = ko.observable(viewModel.test);
        viewModel.cards = ko.observableArray(viewModel.cards);
        viewModel.cards().forEach(function (c) {
            c.hovering = ko.observable(c.hovering);
        });
        viewModel.title = ko.observable(viewModel.title);
        viewModel.advancedSearch = ko.observable(viewModel.advancedSearch);
        viewModel.search = ko.observable(viewModel.search);
        viewModel.colors = ko.observableArray([viewModel.colors]);
        viewModel.manaOperator = ko.observable(viewModel.manaOperator);
        viewModel.setOperator = ko.observable(viewModel.setOperator);
        viewModel.setList = ko.observableArray(viewModel.setList);
        viewModel.nameOperator = ko.observable(viewModel.nameOperator);
        viewModel.nameList = ko.observableArray(viewModel.nameList);
        viewModel.typeOperator = ko.observable(viewModel.typeOperator);
        viewModel.typeList = ko.observableArray(viewModel.typeList);
        viewModel.subTypeOperator = ko.observable(viewModel.subTypeOperator);
        viewModel.subTypeList = ko.observableArray(viewModel.subTypeList);
        viewModel.manaCostOperator = ko.observable(viewModel.manaCostOperator);
        viewModel.manaCostComparison = ko.observable(viewModel.manaCostComparison);
        viewModel.manaCostList = ko.observableArray(viewModel.manaCostList);
        viewModel.powerOperator = ko.observable(viewModel.powerOperator);
        viewModel.powerComparison = ko.observable(viewModel.powerComparison);
        viewModel.powerList = ko.observableArray(viewModel.powerList);
        viewModel.toughnessOperator = ko.observable(viewModel.toughnessOperator);
        viewModel.toughnessComparison = ko.observable(viewModel.toughnessComparison);
        viewModel.toughnessList = ko.observableArray(viewModel.toughnessList);
        viewModel.rarityOperator = ko.observable(viewModel.rarityOperator);
        viewModel.rarityList = ko.observableArray(viewModel.rarityList);
        viewModel.textOperator = ko.observable(viewModel.textOperator);
        viewModel.textList = ko.observableArray(viewModel.textList);
        viewModel.artistOperator = ko.observable(viewModel.artistOperator);
        viewModel.artistList = ko.observableArray(viewModel.artistList);
        viewModel.decks = ko.observableArray(viewModel.decks);
        viewModel.libSearch = ko.observableArray();
        viewModel.library = ko.observableArray();
        viewModel.deckIds = ko.observableArray();
        viewModel.cardAmounts = ko.observable(viewModel.cardAmounts);
        viewModel.remove = this.remove;
        viewModel.clickAdd = this.clickAdd;
        viewModel.hoverOverCard = this.hoverOverCard;
        viewModel.hoverOffCard = this.hoverOffCard;
        viewModel.selectedCard = ko.observable(viewModel.selectedCard);
        viewModel.saveCardAdd = this.saveCardAdd;
        viewModel.cardName = ko.observable();
        $("#showAdvancedSearch").click(function () {
            _this.viewModel.advancedSearch(true);
            _this.viewModel.search(false);
        });
        $("#hideAdvancedSearch").click(function () {
            _this.viewModel.advancedSearch(false);
            _this.viewModel.search(true);
        });
        $("#search").click(function () {
            _this.search();
        });
        $("#white").click(function () {
            if (_this.viewModel.colors.white === true) {
                _this.viewModel.colors.white = false;
                $("#white").toggleClass("selected");
            }
            else {
                _this.viewModel.colors.white = true;
                $("#white").toggleClass("selected");
            }
        });
        $("#blue").click(function () {
            if (_this.viewModel.colors.blue === true) {
                _this.viewModel.colors.blue = false;
                $("#blue").toggleClass("selected");
            }
            else {
                _this.viewModel.colors.blue = true;
                $("#blue").toggleClass("selected");
            }
        });
        $("#black").click(function () {
            if (_this.viewModel.colors.black === true) {
                _this.viewModel.colors.black = false;
                $("#black").toggleClass("selected");
            }
            else {
                _this.viewModel.colors.black = true;
                $("#black").toggleClass("selected");
            }
        });
        $("#red").click(function () {
            if (_this.viewModel.colors.red === true) {
                _this.viewModel.colors.red = false;
                $("#red").toggleClass("selected");
            }
            else {
                _this.viewModel.colors.red = true;
                $("#red").toggleClass("selected");
            }
        });
        $("#green").click(function () {
            if (_this.viewModel.colors.green === true) {
                _this.viewModel.colors.green = false;
                $("#green").toggleClass("selected");
            }
            else {
                _this.viewModel.colors.green = true;
                $("#green").toggleClass("selected");
            }
        });
        $("#colorless").click(function () {
            if (_this.viewModel.colors.colorless === true) {
                _this.viewModel.colors.colorless = false;
                $("#colorless").toggleClass("selected");
            }
            else {
                _this.viewModel.colors.colorless = true;
                $("#colorless").toggleClass("selected");
            }
        });
        $("#mana-operator").click(function () {
            if (_this.viewModel.manaOperator() === "OR") {
                _this.viewModel.manaOperator("AND");
            }
            else {
                _this.viewModel.manaOperator("OR");
            }
        });
        $("#set-operator").click(function () {
            if (_this.viewModel.setOperator() === "AND") {
                _this.viewModel.setOperator("NOT");
            }
            else if (_this.viewModel.setOperator() === "NOT") {
                _this.viewModel.setOperator("AND");
            }
        });
        $("#name-operator").click(function () {
            if (_this.viewModel.nameOperator() === "AND") {
                _this.viewModel.nameOperator("NOT");
            }
            else if (_this.viewModel.nameOperator() === "NOT") {
                _this.viewModel.nameOperator("AND");
            }
        });
        $("#type-operator").click(function () {
            if (_this.viewModel.typeOperator() === "AND") {
                _this.viewModel.typeOperator("NOT");
            }
            else if (_this.viewModel.typeOperator() === "NOT") {
                _this.viewModel.typeOperator("AND");
            }
        });
        $("#subType-operator").click(function () {
            if (_this.viewModel.subTypeOperator() === "AND") {
                _this.viewModel.subTypeOperator("NOT");
            }
            else if (_this.viewModel.subTypeOperator() === "NOT") {
                _this.viewModel.subTypeOperator("AND");
            }
            else if (_this.viewModel.subTypeOperator() === "OR") {
                _this.viewModel.subTypeOperator("AND");
            }
        });
        //$("#manaCost-operator").click(() => {
        //    if (this.viewModel.manaCostOperator() == "AND") {
        //        this.viewModel.manaCostOperator("NOT");
        //    }
        //    else if (this.viewModel.manaCostOperator() == "NOT") {
        //        this.viewModel.manaCostOperator("AND");
        //    }
        //});
        $("#manaCost-comparison").click(function () {
            if (_this.viewModel.manaCostComparison() === "=") {
                _this.viewModel.manaCostComparison(">");
            }
            else if (_this.viewModel.manaCostComparison() === ">") {
                _this.viewModel.manaCostComparison("<");
            }
            else {
                _this.viewModel.manaCostComparison("=");
            }
        });
        //$("#power-operator").click(() => {
        //    if (this.viewModel.powerOperator() == "AND") {
        //        this.viewModel.powerOperator("NOT");
        //    }
        //    else if (this.viewModel.powerOperator() == "NOT") {
        //        this.viewModel.powerOperator("AND");
        //    }
        //});
        $("#power-comparison").click(function () {
            if (_this.viewModel.powerComparison() === "=") {
                _this.viewModel.powerComparison(">");
            }
            else if (_this.viewModel.powerComparison() === ">") {
                _this.viewModel.powerComparison("<");
            }
            else {
                _this.viewModel.powerComparison("=");
            }
        });
        //$("#toughness-operator").click(() => {
        //    if (this.viewModel.toughnessOperator() == "AND") {
        //        this.viewModel.toughnessOperator("NOT");
        //    }
        //    else if (this.viewModel.toughnessOperator() == "NOT") {
        //        this.viewModel.toughnessOperator("AND");
        //    }
        //});
        $("#toughness-comparison").click(function () {
            if (_this.viewModel.toughnessComparison() === "=") {
                _this.viewModel.toughnessComparison(">");
            }
            else if (_this.viewModel.toughnessComparison() === ">") {
                _this.viewModel.toughnessComparison("<");
            }
            else {
                _this.viewModel.toughnessComparison("=");
            }
        });
        $("#rarity-operator").click(function () {
            if (_this.viewModel.rarityOperator() === "AND") {
                _this.viewModel.rarityOperator("NOT");
            }
            else if (_this.viewModel.rarityOperator() === "NOT") {
                _this.viewModel.rarityOperator("AND");
            }
        });
        $("#text-operator").click(function () {
            if (_this.viewModel.textOperator() === "AND") {
                _this.viewModel.textOperator("NOT");
            }
            else if (_this.viewModel.textOperator() === "NOT") {
                _this.viewModel.textOperator("AND");
            }
        });
        $("#artist-operator").click(function () {
            if (_this.viewModel.artistOperator() === "AND") {
                _this.viewModel.artistOperator("NOT");
            }
            else if (_this.viewModel.artistOperator() === "NOT") {
                _this.viewModel.artistOperator("AND");
            }
        });
        $("#advancedSearch").click(function () {
            _this.advancedSearch();
        });
        this.setSearch = $("#setSearch").kendoComboBox({
            dataTextField: "name",
            dataValueField: "code",
            dataSource: viewModel.sets,
            placeholder: "Select a Set..."
        }).data("kendoComboBox");
        this.fullSetSearch = $("#cardSet").kendoComboBox({
            dataTextField: "name",
            dataValueField: "code",
            dataSource: viewModel.allSets,
        }).data("kendoComboBox");
        this.nameSearch = $("#name").kendoComboBox({
            dataSource: viewModel.allNames
        }).data("kendoComboBox");
        this.typeSearch = $("#cardType").kendoComboBox({
            dataSource: viewModel.allTypes,
        }).data("kendoComboBox");
        this.subTypeSearch = $("#cardSubType").kendoComboBox({
            dataSource: viewModel.allSubTypes,
        }).data("kendoComboBox");
        this.raritySearch = $("#rarity").kendoComboBox({
            dataSource: viewModel.allRaritys,
        }).data("kendoComboBox");
        $("#cardSet").blur(function () {
            if (_this.fullSetSearch.text() !== "") {
                _this.viewModel.setList.push({ id: _this.viewModel.setList().length, value: _this.fullSetSearch.text(), code: _this.fullSetSearch.value(), operator: _this.viewModel.setOperator() });
                _this.viewModel.setOperator("OR");
                _this.fullSetSearch.value("");
            }
        });
        $("#name").blur(function () {
            if (_this.nameSearch.value() !== "") {
                _this.viewModel.nameList.push({ id: _this.viewModel.nameList().length, value: _this.nameSearch.value(), operator: _this.viewModel.nameOperator() });
                _this.viewModel.nameOperator("OR");
                _this.nameSearch.value("");
            }
        });
        $("#cardType").blur(function () {
            if (_this.typeSearch.value() !== "") {
                _this.viewModel.typeList.push({ id: _this.viewModel.typeList().length, value: _this.typeSearch.value(), operator: _this.viewModel.typeOperator() });
                _this.viewModel.typeOperator("OR");
                _this.typeSearch.value("");
            }
        });
        $("#cardSubType").blur(function () {
            if (_this.subTypeSearch.value() !== "") {
                _this.viewModel.subTypeList.push({ id: _this.viewModel.subTypeList().length, value: _this.subTypeSearch.value(), operator: _this.viewModel.subTypeOperator() });
                _this.viewModel.subTypeOperator("OR");
                _this.subTypeSearch.value("");
            }
        });
        $("#cmc").blur(function () {
            if ($("#cmc").val() !== "") {
                _this.viewModel.manaCostList.push({ id: _this.viewModel.manaCostList().length, value: $("#cmc").val(), operator: _this.viewModel.manaCostOperator(), comparison: _this.viewModel.manaCostComparison });
                _this.viewModel.manaCostOperator("OR");
                //$("#cmc").val() = "";
            }
        });
        $("#power").blur(function () {
            if ($("#power").val() !== "") {
                _this.viewModel.powerList.push({ id: _this.viewModel.powerList().length, value: $("#power").val(), operator: _this.viewModel.powerOperator(), comparison: _this.viewModel.powerComparison });
                _this.viewModel.powerOperator("OR");
                //$("#power").val() = "";
            }
        });
        $("#toughness").blur(function () {
            if ($("#toughness").val() !== "") {
                _this.viewModel.toughnessList.push({ id: _this.viewModel.toughnessList().length, value: $("#toughness").val(), operator: _this.viewModel.toughnessOperator(), comparison: _this.viewModel.toughnessComparison });
                _this.viewModel.toughnessOperator("OR");
                //$("#power").val() = "";
            }
        });
        $("#rarity").blur(function () {
            if (_this.raritySearch.value() !== "") {
                _this.viewModel.rarityList.push({ id: _this.viewModel.rarityList().length, value: _this.raritySearch.value(), operator: _this.viewModel.rarityOperator() });
                _this.viewModel.rarityOperator("OR");
                _this.raritySearch.value("");
            }
        });
        $("#text").blur(function () {
            if ($("#text").val() !== "") {
                _this.viewModel.textList.push({ id: _this.viewModel.textList().length, value: $("#text").val(), operator: _this.viewModel.textOperator() });
                _this.viewModel.textOperator("OR");
                //$("#power").val() = "";
            }
        });
        $("#artist").blur(function () {
            if ($("#artist").val() !== "") {
                _this.viewModel.artistList.push({ id: _this.viewModel.artistList().length, value: $("#artist").val(), operator: _this.viewModel.artistOperator() });
                _this.viewModel.artistOperator("OR");
                //$("#power").val() = "";
            }
        });
        //$("#AddCardIcon").click(() => {
        //    this.modalConatiner.modal({
        //        show: true,
        //        backdrop: 'static'
        //    });
        //});
    }
    AllCards.prototype.saveCardAdd = function (selectedCard, library, deckIds) {
        if (selectedCard() !== 0) {
            var decks = deckIds();
            if (library().length !== 0) {
                decks.push(0);
            }
            var test = $("#newDeck").val();
            $.ajax({
                url: "/Cards/AddCard",
                type: "POST",
                data: { cardNumber: selectedCard(), where: decks, howMany: $("#cardNumber").val(), newDeck: $("#newDeck").val() },
                cache: false,
                success: function (results) {
                    deckIds([]);
                },
                error: function (error) {
                }
            });
        }
    };
    AllCards.prototype.search = function () {
        var _this = this;
        var name = this.setSearch.text();
        this.viewModel.title(name);
        $.ajax({
            url: '/Cards/Search',
            type: 'GET',
            data: { setCode: this.setSearch.value() },
            cache: false,
            success: function (results) {
                _this.viewModel.cards(results);
                _this.viewModel.cards().forEach(function (c) {
                    c.hovering = ko.observable(c.hovering);
                });
            },
            error: function (error) {
            }
        });
    };
    AllCards.prototype.advancedSearch = function () {
        var _this = this;
        var selectedColors = {
            white: this.viewModel.colors.white,
            blue: this.viewModel.colors.blue,
            black: this.viewModel.colors.black,
            red: this.viewModel.colors.red,
            green: this.viewModel.colors.green,
            colorless: this.viewModel.colors.colorless,
            opp: this.viewModel.manaOperator
        };
        var lib = false;
        if (this.viewModel.libSearch().length !== 0) {
            lib = true;
        }
        var parameters = {
            setCodes: this.viewModel.setList(),
            names: this.viewModel.nameList(),
            types: this.viewModel.typeList(),
            subTypes: this.viewModel.subTypeList(),
            cmcs: this.viewModel.manaCostList(),
            powers: this.viewModel.powerList(),
            toughnesses: this.viewModel.toughnessList(),
            raritys: this.viewModel.rarityList(),
            texts: this.viewModel.textList(),
            artists: this.viewModel.artistList(),
            colors: selectedColors,
            inLibrary: lib
        };
        $.ajax({
            url: "/Cards/AdvancedSearch",
            type: "POST",
            data: { searchParameters: parameters },
            cache: false,
            success: function (results) {
                _this.viewModel.title("");
                _this.viewModel.cards(results);
                _this.viewModel.cards().forEach(function (c) {
                    c.hovering = ko.observable(c.hovering);
                });
            },
            error: function (error) {
            }
        });
    };
    return AllCards;
}());
var CardDetail = (function () {
    function CardDetail(viewModel) {
        this.viewModel = viewModel;
    }
    return CardDetail;
}());
var MyLibrary = (function () {
    function MyLibrary(viewModel) {
        var _this = this;
        this.viewModel = viewModel;
        this.colapseSet = function (name) {
            _this.viewModel.sets().forEach(function (s) {
                if (s.name === name) {
                    s.colapsed(false);
                }
            });
        };
        viewModel.sets = ko.observableArray(viewModel.sets);
        viewModel.sets().forEach(function (s) {
            s.colapsed = ko.observable(s.colapsed);
        });
        viewModel.expandSet = this.expandSet;
    }
    MyLibrary.prototype.expandSet = function () {
        this.viewModel.sets().forEach(function (s) {
            if (s.name == name) {
                s.colapsed(true);
            }
        });
    };
    return MyLibrary;
}());
var Decks = (function () {
    function Decks(viewModel) {
        this.viewModel = viewModel;
    }
    return Decks;
}());
var DeckDetail = (function () {
    function DeckDetail(viewModel) {
        var _this = this;
        this.viewModel = viewModel;
        this.hoverOverCard = function (id) {
            $("#" + id).addClass("card-hover");
            _this.viewModel.cards().forEach(function (c) {
                if (c.id === id) {
                    c.hovering(true);
                }
            });
            //this.viewModel.selectedCard(id);
        };
        this.hoverOffCard = function (id) {
            $("#" + id).removeClass("card-hover");
            _this.viewModel.cards().forEach(function (c) {
                if (c.id === id) {
                    c.hovering(false);
                }
            });
        };
        viewModel.cards = ko.observableArray(viewModel.cards);
        viewModel.cards().forEach(function (c) {
            c.hovering = ko.observable(c.hovering);
        });
        viewModel.hoverOverCard = this.hoverOverCard;
        viewModel.hoverOffCard = this.hoverOffCard;
    }
    return DeckDetail;
}());
//# sourceMappingURL=main.js.map