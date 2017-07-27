
/* Game namespace */
var game = {

	// an object where to store game information
    data : {
        // score
        score: 0,
        lifes: 1,
        ironGaze: false,
        deathStone: 0,
        stompy: false
	},


    // Run on page load.
    "onload" : function (playerItems) {
        //Initialize items of the player
        var items = JSON.parse(playerItems);
        for (var i = 0; i < items.length; i++) {
            if (items[i].Item1 == 'LifeVial') {
                this.data.lifes += items[i].Item2.Quantity;
            }
            if (items[i].Item1 == "IronGaze") {
                this.data.ironGaze = true;
            }
            if (items[i].Item1 == "DeathStone") {
                this.data.deathStone += items[i].Item2.Quantity;
            }
            if (items[i].Item1 == "Stompy") {
                this.data.stompy = true;
            }
        }

        // Initialize the video.
        if (!me.video.init(200, 180, {wrapper : "screen", scale : 4, scaleMethod : "flex-width"})) {
            alert("Your browser does not support HTML5 canvas.");
            return;
        }
        // add "#debug" to the URL to enable the debug Panel
        if (me.game.HASH.debug === true) {
            window.onReady(function () {
                me.plugin.register.defer(this, me.debug.Panel, "debug", me.input.KEY.V);
            });
        }
        // Initialize the audio.
        me.audio.init("mp3,ogg");

        // Set a callback to run when loading is complete.
        me.loader.onload = this.loaded.bind(this);

        // Load the resources.
        me.loader.preload(game.resources);

        // Initialize melonJS and display a loading screen.
        me.state.change(me.state.LOADING);
    },



    // Run on game resources loaded.
    "loaded" : function () {
        me.state.set(me.state.MENU, new game.TitleScreen());
        me.state.set(me.state.PLAY, new game.PlayScreen());

		// add our player entity in the entity pool
		me.pool.register("mainPlayer", game.PlayerEntity);
		me.pool.register("CoinEntity", game.CoinEntity);
		me.pool.register("EnemyEntity", game.EnemyEntity);
		me.pool.register("TrapEntity", game.TrapEntity);
		me.pool.register("EndGameEntity", game.EndGameEntity);
		
		// enable the keyboard
		me.input.bindKey(me.input.KEY.LEFT,  "left");
		me.input.bindKey(me.input.KEY.DOWN,  "down");
		me.input.bindKey(me.input.KEY.RIGHT, "right");
		me.input.bindKey(me.input.KEY.UP, "up");

        // Start the game.
        //me.state.change(me.state.PLAY);

        // display the menu title
		me.state.change(me.state.MENU);
    }
};
