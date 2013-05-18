using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColourBackground : FContainer {
 
    FSprite background;
    Tween colourTween;
    string[] purples = {"2E0854","7F00FF","71637D","68228B","9A32CD","5C246E"};
    int colourCounter = 0;
    
    public ColourBackground() {
        background = new FSprite(Futile.whiteElement);
        background.width = Futile.screen.width;
        background.height = Futile.screen.height;
     
        this.AddChild(background);        
        background.color = Color.black;
    }
 
    public void Update() {
        if(colourTween == null || colourTween.state == TweenState.Destroyed) {
            colourTween = Go.to(this.background, 3, new TweenConfig().colorProp("color", HexToColor(purples[colourCounter]), false));
            colourTween.play();
            colourCounter++;
            if(colourCounter == purples.Length)
                colourCounter = 0;
        }
    }

    Color HexToColor(string hex) {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

}