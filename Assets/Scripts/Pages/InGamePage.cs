using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class InGamePage : BPage {
    ColourBackground background {
        get;
        set;
    }
 
    TweenTerrain terrain;
    BasePlayer player;
    
    override public void Start() {
        background = new ColourBackground();
        this.AddChild(background);
        
        terrain = new TweenTerrain();
        this.AddChild(terrain);
        Futile.atlasManager.LoadImage("player");
        player = new BasePlayer("player");
        this.AddChild(player);
    }

    override public void HandleAddedToStage() {
        Futile.instance.SignalUpdate += HandleUpdate;
        base.HandleAddedToStage();  
    }

    override public void HandleRemovedFromStage() {
        Futile.instance.SignalUpdate -= HandleUpdate;
        base.HandleRemovedFromStage();  
    }

    void HandleUpdate() {
        background.Update();
        terrain.Update(player.GetPosition());
    }
}