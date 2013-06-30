using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class InGamePage : BPage, FMultiTouchableInterface {
    ColourBackground background {
        get;
        set;
    }
 
    TweenTerrain terrain;
    BasePlayer player;
    FLabel timerLabel;
    
    override public void Start() {
        background = new ColourBackground();
        this.AddChild(background);
        
        terrain = new TweenTerrain();
        this.AddChild(terrain);
        Futile.atlasManager.LoadImage("player");
        player = new BasePlayer("player");
        player.y = Futile.screen.halfHeight;
        this.AddChild(player);

        timerLabel = new FLabel("Abstract", "LOSE");
        timerLabel.scale *= 4;
        timerLabel.color = Color.gray;
        timerLabel.alpha = 0;
        timerLabel.SetAnchor(0.5f, 0.5f);
        AddChild(timerLabel);
    }

    override public void HandleAddedToStage() {
        Futile.touchManager.AddMultiTouchTarget(this);

        Futile.instance.SignalUpdate += HandleUpdate;
        base.HandleAddedToStage();  
    }

    override public void HandleRemovedFromStage() {
        Futile.instance.SignalUpdate -= HandleUpdate;
        base.HandleRemovedFromStage();  
    }

    void HandleUpdate() {
        background.Update();
        foreach(FSprite s in terrain.enablesTiles) {
            if((s.textureRect).Contains(s.GlobalToLocal(player.LocalToGlobal(player.GetPosition())))) 
               timerLabel.alpha = 1;
        }
    }

    public void HandleMultiTouch(FTouch[] touches)
    {
        foreach(FTouch touch in touches)
        {
            {

                //we go reverse order so that if we remove a banana it doesn't matter
                //and also so that that we check from front to back
                terrain.Update((this.GetLocalTouchPosition(touch)));
            terrain.RemoveTiles();

            }
        }

    }

}