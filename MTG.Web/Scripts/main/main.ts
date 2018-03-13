class AllCards {
    setSearch: kendo.ui.ComboBox;
    fullSetSearch: kendo.ui.ComboBox;
    typeSearch: kendo.ui.ComboBox;
    subTypeSearch: kendo.ui.ComboBox;
    raritySearch: kendo.ui.ComboBox;
    nameSearch: kendo.ui.ComboBox;
    modalConatiner: JQuery = $("#addCard");
    deckIds: KnockoutObservableArray<string> = ko.observableArray([]);

    constructor(public viewModel) {
        _.delay(() => {
            const loadingIndicator = jQuery(" .loadingIndicator");
            loadingIndicator.fadeOut(100, () => { loadingIndicator.remove() });
        }, 300);

        viewModel.test = ko.observable(viewModel.test);

        viewModel.cards = ko.observableArray(viewModel.cards);
        viewModel.cards().forEach(c => {
            c.hovering = ko.observable(c.hovering);
        });


        viewModel.title = ko.observable(viewModel.title);
        viewModel.advancedSearch = ko.observable(viewModel.advancedSearch);
        viewModel.search = ko.observable(viewModel.search);
        viewModel.settings = ko.observable(viewModel.settings);
        viewModel.colSize = ko.observable(viewModel.colSize);

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

        viewModel.remove = this.remove;
        viewModel.clickAdd = this.clickAdd;
        viewModel.hoverOverCard = this.hoverOverCard;
        viewModel.hoverOffCard = this.hoverOffCard;
        viewModel.selectedCard = ko.observable(viewModel.selectedCard);
        viewModel.saveCardAdd = this.saveCardAdd;

        viewModel.cardName = ko.observable();

        viewModel.cardAmounts = ko.observable(viewModel.cardAmounts);
        viewModel.cardAmounts.deckInfo = ko.observableArray(viewModel.cardAmounts.deckInfo);

        viewModel.incrementAmount = this.incrementAmount;
        viewModel.deincrementAmount = this.deincrementAmount;

        //$("#results").addClass("col-md-" + this.viewModel.colSize());


        $("#showAdvancedSearch").click(() => {
            this.viewModel.advancedSearch(true);
            this.viewModel.search(false);
        });

        $("#hideAdvancedSearch").click(() => {
            this.viewModel.advancedSearch(false);
            this.viewModel.search(true);
        });
        $(".settings").click(() => {
            if (this.viewModel.settings()) {
                this.viewModel.settings(false);
            } else {
                this.viewModel.settings(true);
            }          
        });

        $("#search").click(() => {
            this.search();
        });

        $("#white").click(() => {
            if (this.viewModel.colors.white === true) {
                this.viewModel.colors.white = false;
                $("#white").toggleClass("selected");
            }
            else {
                this.viewModel.colors.white = true;
                $("#white").toggleClass("selected");
            }
        });

        $("#blue").click(() => {
            if (this.viewModel.colors.blue === true) {
                this.viewModel.colors.blue = false;
                $("#blue").toggleClass("selected");
            }
            else {
                this.viewModel.colors.blue = true;
                $("#blue").toggleClass("selected");
            }
        });

        $("#black").click(() => {
            if (this.viewModel.colors.black === true) {
                this.viewModel.colors.black = false;
                $("#black").toggleClass("selected");
            }
            else {
                this.viewModel.colors.black = true;
                $("#black").toggleClass("selected");
            }
        });

        $("#red").click(() => {
            if (this.viewModel.colors.red === true) {
                this.viewModel.colors.red = false;
                $("#red").toggleClass("selected");
            }
            else {
                this.viewModel.colors.red = true;
                $("#red").toggleClass("selected");
            }
        });

        $("#green").click(() => {
            if (this.viewModel.colors.green === true) {
                this.viewModel.colors.green = false;
                $("#green").toggleClass("selected");
            }
            else {
                this.viewModel.colors.green = true;
                $("#green").toggleClass("selected");
            }
        });

        $("#colorless").click(() => {
            if (this.viewModel.colors.colorless === true) {
                this.viewModel.colors.colorless = false;
                $("#colorless").toggleClass("selected");
            }
            else {
                this.viewModel.colors.colorless = true;
                $("#colorless").toggleClass("selected");
            }
        });

        $("#mana-operator").click(() => {
            if (this.viewModel.manaOperator() === "OR") {
                this.viewModel.manaOperator("AND");
            }
            else {
                this.viewModel.manaOperator("OR");
            }
        });

        $("#set-operator").click(() => {
            if (this.viewModel.setOperator() === "AND") {
                this.viewModel.setOperator("NOT");
            }
            else if (this.viewModel.setOperator() === "NOT") {
                this.viewModel.setOperator("AND");
            }
        });

        $("#name-operator").click(() => {
            if (this.viewModel.nameOperator() === "AND") {
                this.viewModel.nameOperator("NOT");
            }
            else if (this.viewModel.nameOperator() === "NOT") {
                this.viewModel.nameOperator("AND");
            }
        });

        $("#type-operator").click(() => {
            if (this.viewModel.typeOperator() === "AND") {
                this.viewModel.typeOperator("NOT");
            }
            else if (this.viewModel.typeOperator() === "NOT") {
                this.viewModel.typeOperator("AND");
            }
        });

        $("#subType-operator").click(() => {
            if (this.viewModel.subTypeOperator() === "AND") {
                this.viewModel.subTypeOperator("NOT");
            }
            else if (this.viewModel.subTypeOperator() === "NOT") {
                this.viewModel.subTypeOperator("AND");
            }
            else if (this.viewModel.subTypeOperator() === "OR") {
                this.viewModel.subTypeOperator("AND");
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

        $("#manaCost-comparison").click(() => {
            if (this.viewModel.manaCostComparison() === "=") {
                this.viewModel.manaCostComparison(">");
            }
            else if (this.viewModel.manaCostComparison() === ">") {
                this.viewModel.manaCostComparison("<");
            }
            else {
                this.viewModel.manaCostComparison("=");
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

        $("#power-comparison").click(() => {
            if (this.viewModel.powerComparison() === "=") {
                this.viewModel.powerComparison(">");
            }
            else if (this.viewModel.powerComparison() === ">") {
                this.viewModel.powerComparison("<");
            }
            else {
                this.viewModel.powerComparison("=");
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

        $("#toughness-comparison").click(() => {
            if (this.viewModel.toughnessComparison() === "=") {
                this.viewModel.toughnessComparison(">");
            }
            else if (this.viewModel.toughnessComparison() === ">") {
                this.viewModel.toughnessComparison("<");
            }
            else {
                this.viewModel.toughnessComparison("=");
            }
        });

        $("#rarity-operator").click(() => {
            if (this.viewModel.rarityOperator() === "AND") {
                this.viewModel.rarityOperator("NOT");
            }
            else if (this.viewModel.rarityOperator() === "NOT") {
                this.viewModel.rarityOperator("AND");
            }
        });

        $("#text-operator").click(() => {
            if (this.viewModel.textOperator() === "AND") {
                this.viewModel.textOperator("NOT");
            }
            else if (this.viewModel.textOperator() === "NOT") {
                this.viewModel.textOperator("AND");
            }
        });

        $("#artist-operator").click(() => {
            if (this.viewModel.artistOperator() === "AND") {
                this.viewModel.artistOperator("NOT");
            }
            else if (this.viewModel.artistOperator() === "NOT") {
                this.viewModel.artistOperator("AND");
            }
        });

        $("#advancedSearch").click(() => {
            this.advancedSearch();
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



        $("#cardSet").blur(() => {
            if (this.fullSetSearch.text() !== "") {
                this.viewModel.setList.push({ id: this.viewModel.setList().length, value: this.fullSetSearch.text(), code: this.fullSetSearch.value(), operator: this.viewModel.setOperator() });
                this.viewModel.setOperator("OR");
                this.fullSetSearch.value("");
            }
        });

        $("#name").blur(() => {
            if (this.nameSearch.value() !== "") {
                this.viewModel.nameList.push({ id: this.viewModel.nameList().length, value: this.nameSearch.value(), operator: this.viewModel.nameOperator() });
                this.viewModel.nameOperator("OR");
                this.nameSearch.value("");
            }
        });

        $("#cardType").blur(() => {
            if (this.typeSearch.value() !== "") {
                this.viewModel.typeList.push({ id: this.viewModel.typeList().length, value: this.typeSearch.value(), operator: this.viewModel.typeOperator() });
                this.viewModel.typeOperator("OR");
                this.typeSearch.value("");
            }
        });

        $("#cardSubType").blur(() => {
            if (this.subTypeSearch.value() !== "") {
                this.viewModel.subTypeList.push({ id: this.viewModel.subTypeList().length, value: this.subTypeSearch.value(), operator: this.viewModel.subTypeOperator() });
                this.viewModel.subTypeOperator("OR");
                this.subTypeSearch.value("");
            }
        });

        $("#cmc").blur(() => {
            if ($("#cmc").val() !== "") {
                this.viewModel.manaCostList.push({ id: this.viewModel.manaCostList().length, value: $("#cmc").val(), operator: this.viewModel.manaCostOperator(), comparison: this.viewModel.manaCostComparison });
                this.viewModel.manaCostOperator("OR");
                //$("#cmc").val() = "";
            }
        });

        $("#power").blur(() => {
            if ($("#power").val() !== "") {
                this.viewModel.powerList.push({ id: this.viewModel.powerList().length, value: $("#power").val(), operator: this.viewModel.powerOperator(), comparison: this.viewModel.powerComparison });
                this.viewModel.powerOperator("OR");
                //$("#power").val() = "";
            }
        });

        $("#toughness").blur(() => {
            if ($("#toughness").val() !== "") {
                this.viewModel.toughnessList.push({ id: this.viewModel.toughnessList().length, value: $("#toughness").val(), operator: this.viewModel.toughnessOperator(), comparison: this.viewModel.toughnessComparison });
                this.viewModel.toughnessOperator("OR");
                //$("#power").val() = "";
            }
        });

        $("#rarity").blur(() => {
            if (this.raritySearch.value() !== "") {
                this.viewModel.rarityList.push({ id: this.viewModel.rarityList().length, value: this.raritySearch.value(), operator: this.viewModel.rarityOperator() });
                this.viewModel.rarityOperator("OR");
                this.raritySearch.value("");
            }
        });

        $("#text").blur(() => {
            if ($("#text").val() !== "") {
                this.viewModel.textList.push({ id: this.viewModel.textList().length, value: $("#text").val(), operator: this.viewModel.textOperator() });
                this.viewModel.textOperator("OR");
                //$("#power").val() = "";
            }
        });

        $("#artist").blur(() => {
            if ($("#artist").val() !== "") {
                this.viewModel.artistList.push({ id: this.viewModel.artistList().length, value: $("#artist").val(), operator: this.viewModel.artistOperator() });
                this.viewModel.artistOperator("OR");
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

    clickAdd = (id) => {
        var self = this;
        $.ajax({
            url: "/Cards/GetAddInfo",
            type: "POST",
            data: { cardId: id },
            cache: false,
            success: (results: any) => {
                self.viewModel.cardAmounts(results);
                self.viewModel.cardAmounts().libraryAmount = ko.observable(self.viewModel.cardAmounts().libraryAmount);
                self.viewModel.cardAmounts().deckInfo.forEach(d => {            
                    d.deckAmount = ko.observable(d.deckAmount);
                });
                if (self.viewModel.cardAmounts().libraryAmount() > 0) {
                    $("#minus_lib").removeClass("notAllowed").addClass("pointer");
                } else {
                    $("#minus_lib").removeClass("pointer").addClass("notAllowed");
                }

                $("#addCard").modal({
                    show: true,
                    backdrop: 'static'
                });
            },
            error: error => {
            }
        });
    }

    hoverOverCard = (id) => {
        $("#" + id).addClass('card-hover');
        this.viewModel.cards().forEach(c => {
            if (c.id === id) {
                c.hovering(true);
            }
        });
        this.viewModel.selectedCard(id);
    }

    hoverOffCard = (id) => {
        $("#" + id).removeClass('card-hover');
        this.viewModel.cards().forEach(c => {
            if (c.id === id) {
                c.hovering(false);
            }
        });
    }

    incrementAmount = (index, deckName) => {
        if (deckName === "library") {
            if (this.viewModel.cardAmounts().libraryAmount() === 0) {
                $("#minus_" + index).removeClass("notAllowed").addClass("pointer");
            }
            this.viewModel.cardAmounts().libraryAmount(this.viewModel.cardAmounts().libraryAmount() + 1);
        } else {
            this.viewModel.cardAmounts().deckInfo.forEach(d => {
                if (d.deckName === deckName) {
                    if (d.deckAmount() === 0) {
                        $("#minus_" + index).removeClass("notAllowed").addClass("pointer");
                    }
                    d.deckAmount(Number(d.deckAmount()) + 1);                
                }
            });
        }        
    }

    deincrementAmount = (index, deckName) => {
        if (deckName === "library") {
            if (this.viewModel.cardAmounts().libraryAmount() === 1) {
                $("#minus_" + index).removeClass("pointer").addClass("notAllowed");
            }
            if (this.viewModel.cardAmounts().libraryAmount() > 0) {
                this.viewModel.cardAmounts().libraryAmount(this.viewModel.cardAmounts().libraryAmount() - 1);
            }
        } else {
            this.viewModel.cardAmounts().deckInfo.forEach(d => {
                if (d.deckName === deckName) {
                    if (d.deckAmount() === 1) {
                        $("#minus_" + index).removeClass("pointer").addClass("notAllowed");
                    }
                    if (d.deckAmount() > 0) {
                        d.deckAmount(d.deckAmount() - 1);
                    }                   
                }
            });
        }       
    }

    remove = (id, list) => {
        var self = this;
        if (list === "set") {
            var c = 0;
            this.viewModel.setList.remove(function (remove) { return remove.id === id; });
            this.viewModel.setList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.setList.splice([0], 1, { id: 0, value: list.value, set: list.set, operator: "AND" });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.setList().length === 0) {
                this.viewModel.setOperator("AND");
            }
        }
        if (list === "name") {
            var c = 0;
            this.viewModel.nameList.remove(function (remove) { return remove.id === id; });
            this.viewModel.nameList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.nameList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.nameList().length === 0) {
                this.viewModel.nameOperator("AND");
            }
        }
        if (list === "type") {
            var c = 0;
            this.viewModel.typeList.remove(function (remove) { return remove.id === id; });
            this.viewModel.typeList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.typeList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.typeList().length === 0) {
                this.viewModel.typeOperator("AND");
            }
        }
        if (list === "subType") {
            var c = 0;
            this.viewModel.subTypeList.remove(function (remove) { return remove.id === id; });
            this.viewModel.subTypeList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.subTypeList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.subTypeList().length === 0) {
                this.viewModel.subTypeOperator("AND");
            }
        }
        if (list === "manaCost") {
            var c = 0;
            this.viewModel.manaCostList.remove(function (remove) { return remove.id === id; });
            this.viewModel.manaCostList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.manaCostList.splice([0], 1, { id: 0, value: list.value, operator: "AND", comparison: list.comparison });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.manaCostList().length === 0) {
                this.viewModel.manaCostOperator("AND");
            }
        }
        if (list === "power") {
            var c = 0;
            this.viewModel.powerList.remove(function (remove) { return remove.id === id; });
            this.viewModel.powerList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.powerList.splice([0], 1, { id: 0, value: list.value, operator: "AND", comparison: list.comparison });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.powerList().length === 0) {
                this.viewModel.powerOperator("AND");
            }
        }
        if (list === "toughness") {
            var c = 0;
            this.viewModel.toughnessList.remove(function (remove) { return remove.id === id; });
            this.viewModel.toughnessList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.toughnessList.splice([0], 1, { id: 0, value: list.value, operator: "AND", comparison: list.comparison });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.toughnessList().length === 0) {
                this.viewModel.toughnessOperator("AND");
            }
        }
        if (list === "rarity") {
            var c = 0;
            this.viewModel.rarityList.remove(function (remove) { return remove.id === id; });
            this.viewModel.rarityList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.rarityList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.rarityList().length === 0) {
                this.viewModel.rarityOperator('AND');
            }
        }
        if (list === "text") {
            var c = 0;
            this.viewModel.textList.remove(function (remove) { return remove.id === id; });
            this.viewModel.textList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.textList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.textList().length === 0) {
                this.viewModel.textOperator("AND");
            }
        }
        if (list === "artist") {
            var c = 0;
            this.viewModel.artistList.remove(function (remove) { return remove.id === id; });
            this.viewModel.artistList().forEach(function (list) {
                if (c === 0 && list.operator === "OR") {
                    self.viewModel.artistList.splice([0], 1, { id: 0, value: list.value, operator: "AND" });
                    c += 1;
                }
                else {
                    c += 1;
                }
            });
            if (this.viewModel.artistList().length === 0) {
                this.viewModel.artistOperator("AND");
            }
        }
    }

    saveCardAdd(selectedCard, cardAmounts) {
        if (selectedCard() !== 0) {
            //var decks = deckIds();
            //if (library().length !== 0) {
            //    decks.push(0);
            //}
            //var test = $("#newDeck").val();

            $.ajax({
                url: "/Cards/AddCard",
                type: "POST",
                data: {
                    cardNumber: selectedCard(),
                    cardAmounts: cardAmounts()
                }, // where: decks, howMany: $("#cardNumber").val(), newDeck: $("#newDeck").val() },
                cache: false,
                success: (results: any) => {
                    //deckIds([]);
                },
                error: error => {
                }
            });
        }
    }

    search() {
        var name = this.setSearch.text();
        this.viewModel.title(name);
        $.ajax({
            url: '/Cards/Search',
            type: 'GET',
            data: { setCode: this.setSearch.value() },
            cache: false,
            success: (results: any) => {
                this.viewModel.cards(results);
                this.viewModel.cards().forEach(c => {
                    c.hovering = ko.observable(c.hovering);
                });
            },
            error: error => {
            }
        });
    }

    advancedSearch() {

        var selectedColors = {
            white: this.viewModel.colors.white,
            blue: this.viewModel.colors.blue,
            black: this.viewModel.colors.black,
            red: this.viewModel.colors.red,
            green: this.viewModel.colors.green,
            colorless: this.viewModel.colors.colorless,
            opp: this.viewModel.manaOperator
        };

        let lib = false;
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
            success: (results: any) => {
                this.viewModel.title("");
                this.viewModel.cards(results);
                this.viewModel.cards().forEach(c => {
                    c.hovering = ko.observable(c.hovering);
                });
            },
            error: error => {
            }
        });
    }

}

class CardDetail {
    constructor(private viewModel) {
    }
}

class MyLibrary {
    constructor(private viewModel) {
        viewModel.sets = ko.observableArray(viewModel.sets);
        viewModel.sets().forEach(s => {
            s.colapsed = ko.observable(s.colapsed);
        });
        viewModel.expandSet = this.expandSet;
    }

    expandSet() {
        this.viewModel.sets().forEach(s => {
            if (s.name == name) {
                s.colapsed(true);
            }
        });
    }

    colapseSet = (name) => {
        this.viewModel.sets().forEach(s => {
            if (s.name === name) {
                s.colapsed(false);
            }
        });
    }
}

class Decks {
    constructor(private viewModel) {
    }
}

class DeckDetail {
    constructor(private viewModel) {
        viewModel.cards = ko.observableArray(viewModel.cards);
        viewModel.cards().forEach(c => {
            c.hovering = ko.observable(c.hovering);
        });

        viewModel.hoverOverCard = this.hoverOverCard;
        viewModel.hoverOffCard = this.hoverOffCard;
    }

    hoverOverCard = (id) => {
        $("#" + id).addClass("card-hover");
        this.viewModel.cards().forEach(c => {
            if (c.id === id) {
                c.hovering(true);
            }
        });
        //this.viewModel.selectedCard(id);
    }

    hoverOffCard = (id) => {
        $("#" + id).removeClass("card-hover");
        this.viewModel.cards().forEach(c => {
            if (c.id === id) {
                c.hovering(false);
            }
        });
    }
}

