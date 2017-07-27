/**
 * Player Entity
 */
game.PlayerEntity = me.Entity.extend({
    init: function (x, y, settings) {
        // call the constructor
        this._super(me.Entity, "init", [x, y, settings]);

        // disable gravity
        this.body.gravity = 0;

        // walking & jumping speed
        this.body.setVelocity(2.5, 2.5);
        this.body.setFriction(0.4, 0.4);

        // set the display around our position

        // Create a vector that represents the player center point
        this.center = new me.Vector2d(0, 30);

        // Create the "tracking vector"
        this.tracking = this.pos.clone();

        me.game.viewport.follow(this.tracking, me.game.viewport.AXIS.BOTH);
        me.game.viewport.setDeadzone(0, 0);

        // enable keyboard
        me.input.bindKey(me.input.KEY.LEFT, "left");
        me.input.bindKey(me.input.KEY.RIGHT, "right");
        me.input.bindKey(me.input.KEY.UP, "up");
        me.input.bindKey(me.input.KEY.DOWN, "down");

        // the main player spritesheet
        var texture = new me.video.renderer.Texture(
            { framewidth: 32, frameheight: 32 },
            me.loader.getImage("Blank_Sprite_Sheet_4_2_by_KnightYamato")
        );

        // create a new sprite object
        this.renderable = texture.createAnimationFromName([0, 1, 2, 3, 4, 5, 6, 7, 8]);
        // define an additional basic walking animation
        this.renderable.addAnimation("simple_walk", [0, 1, 2]);

        // set the renderable position to bottom center
        this.anchorPoint.set(0.5, 0.5);
    },

    /* -----

        update the player pos

    ------            */
    update: function (dt) {
        // Update the "tracking vector"
        this.tracking.copy(this.pos).add(this.center);
        if (me.input.isKeyPressed("left")) {
            // update the entity velocity
            this.body.vel.x -= this.body.accel.x * me.timer.tick;
        } else if (me.input.isKeyPressed("right")) {
            // update the entity velocity
            this.body.vel.x += this.body.accel.x * me.timer.tick;
        } else {
            this.body.vel.x = 0;
        }
        if (me.input.isKeyPressed("up")) {
            // update the entity velocity
            this.body.vel.y -= this.body.accel.y * me.timer.tick;
        } else if (me.input.isKeyPressed("down")) {
            // update the entity velocity
            this.body.vel.y += this.body.accel.y * me.timer.tick;
        } else {
            this.body.vel.y = 0;
        }

        // apply physics to the body (this moves the entity)
        this.body.update(dt);

        // handle collisions against other shapes
        me.collision.check(this);

        // check if we moved (an "idle" animation would definitely be cleaner)
        if (this.body.vel.x !== 0 || this.body.vel.y !== 0) {
            this._super(me.Entity, "update", [dt]);
            return true;
        }

    },

    /**
     * colision handler
     * (called when colliding with other objects)
     */
    onCollision: function (response, other) {
        // Make all other objects solid
        if (response.b.body.collisionType == me.collision.types.NPC_OBJECT && !this.renderable.isFlickering()) {
            this.renderable.flicker(750);
            game.data.lifes -= 1;

        }
        if (response.b.name == "TrapEntity") {
            this.renderable.flicker(500);
        }

        if (response.b.name == "EndGameEntity") {
            $.ajax({
                url: '/Game/Update',
                type: 'POST',
                data: {
                    lifes: game.data.lifes,
                    score: game.data.score,
                    deathStones: game.data.deathStone,
                    //ironGaze: game.data.ironGaze,
                    //stompy: game.data.stompy
                },
                success: function (data) {
                   
                },
                error: function (data) {
                   
                }
            });
            setTimeout(function () { window.location.href = "/Game/EndGame"; }, 500);
        }

        if (game.data.lifes < 1 && response.b.body.collisionType != me.collision.types.WORLD_SHAPE) {
            //window.location.href = url.replace('__score__', game.data.score);
            $.ajax({
                url: '/Game/Update',
                type: 'POST',
                data: {
                    lifes: game.data.lifes,
                    score: game.data.score,
                    deathStones: game.data.deathStone,
                    //ironGaze: game.data.ironGaze,
                    //stompy: game.data.stompy
                },
                success: function (data) {
                    
                },
                error: function (data) {
                    
                }
            });
            setTimeout(function () { window.location.href = "/Game/EndGame"; }, 500);
        }
        return true;
    }
});

/**
 * a Coin entity
 */
game.CoinEntity = me.CollectableEntity.extend({
    // extending the init function is not mandatory
    // unless you need to add some extra initialization
    init: function (x, y, settings) {
        // call the parent constructor
        this._super(me.CollectableEntity, 'init', [x, y, settings]);
    },

    // this function is called by the engine, when
    // an object is touched by something (here collected)F
    onCollision: function (response, other) {
        // do something when collected
        // make sure it cannot be collected "again"
        this.body.setCollisionMask(me.collision.types.NO_OBJECT);
        me.audio.play("cling");
        game.data.score += 1;
        // remove it
        me.game.world.removeChild(this);

        return false
    }
});
/**
 * an EndGame entity
 */
game.EndGameEntity = me.Entity.extend({
    // extending the init function is not mandatory
    // unless you need to add some extra initialization
    init: function (x, y, settings) {
        // call the parent constructor
        this._super(me.Entity, 'init', [x, y, settings]);
    },

    // this function is called by the engine, when
    // an object is touched by something (here collected)F
    onCollision: function (response, other) {
        if (response.b.body.collisionType !== me.collision.types.WORLD_SHAPE) {
            this.body.setCollisionMask(me.collision.types.NO_OBJECT);
        }
    }
});
/**
 * a Trap entity
 */
game.TrapEntity = me.CollectableEntity.extend({
    // extending the init function is not mandatory
    // unless you need to add some extra initialization
    init: function (x, y, settings) {
        // call the parent constructor
        this._super(me.CollectableEntity, 'init', [x, y, settings]);
    },

    // this function is called by the engine, when
    // an object is touched by something (here collected)
    onCollision: function (response, other) {
        // do something when collected
        // make sure it cannot be collected "again"
        if (game.data.stompy == true) {
            this.body.setCollisionMask(me.collision.types.NO_OBJECT);
        }
        else {
            response.b.renderable.flicker(750);
            game.data.lifes -= 1;
            // remove it
            me.game.world.removeChild(this);
        }

        return false
    }
});

/**
 * an enemy Entity
 */
game.EnemyEntity = me.Entity.extend({
    init: function (x, y, settings) {
        // define this here instead of tiled
        settings.image = "wheelie_right";


        // disable gravity
        me.sys.gravity = 0;

        // save the area size defined in Tiled
        var width = settings.width;
        var height = settings.height;

        // adjust the size setting information to match the sprite size
        // so that the entity object is created with the right size
        settings.framewidth = settings.width = 64;
        settings.frameheight = settings.height = 64;

        // redefine the default shape (used to define path) with a shape matching the renderable
        settings.shapes[0] = new me.Rect(0, 0, settings.framewidth, settings.frameheight);

        // call the parent constructor
        this._super(me.Entity, 'init', [x, y, settings]);

        // set start/end position based on the initial area size
        x = this.pos.x;
        this.startX = x;
        this.endX = x + width - settings.framewidth
        this.pos.x = x + width - settings.framewidth;

        // to remember which side we were walking
        this.walkLeft = false;

        // walking & jumping speed
        this.body.setVelocity(4, 6);

        this.body.collisionType = me.collision.types.NPC_OBJECT;

    },

    /**
     * update the enemy pos
     */
    update: function (dt) {

        if (this.alive) {
            if (this.walkLeft && this.pos.x <= this.startX) {
                this.walkLeft = false;
            }
            else if (!this.walkLeft && this.pos.x >= this.endX) {
                this.walkLeft = true;
            }

            // make it walk
            this.renderable.flipX(this.walkLeft);
            this.body.vel.x += (this.walkLeft) ? -this.body.accel.x * me.timer.tick : this.body.accel.x * me.timer.tick;
        }
        else {
            this.body.vel.x = 0;
        }

        // update the body movement
        this.body.update(dt);

        // handle collisions against other shapes
        me.collision.check(this);

        // return true if we moved or if the renderable was updated
        return (this._super(me.Entity, 'update', [dt]) || this.body.vel.x !== 0 || this.body.vel.y !== 0);
    },

    /**
     * colision handler
     * (called when colliding with other objects)
     */
    onCollision: function (response, other) {
        if (response.b.body.collisionType !== me.collision.types.WORLD_SHAPE) {
            // res.y >0 means touched by something on the bottom
            // which mean at top position for this one
            if (this.alive && (response.overlapV.y > 0) && response.a.body.falling) {
                //this.renderable.flicker(750);
                //me.game.world.removeChild(this);
            //    $.ajax({
            //        url: '/Game/YouDied',
            //        type: 'GET',
            //       // data: { },
            //        success: function (data) { 
            //            alert("success");
            //    },
            //        error: function (data) {
            //            alert("error");
            //    }
                //});
                

            }
            return false;
        }
        // Make all other objects solid
        return true;
    }
});
