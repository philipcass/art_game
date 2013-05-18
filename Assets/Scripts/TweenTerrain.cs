using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TweenTerrain : FContainer {
    int tileSize = 32;
    FSprite[,] tiles;
    List<FSprite> activeTiles = new List<FSprite>(9);
    public TweenTerrain(){
        tiles = new FSprite[(int)Futile.screen.height-1/tileSize,(int)Futile.screen.width/tileSize];
        for(float y = -Futile.screen.halfHeight; y < Futile.screen.halfHeight; y+=tileSize){
            for(float x = -Futile.screen.halfWidth; x < Futile.screen.halfWidth; x+=tileSize){
                FSprite s = new FSprite(Futile.whiteElement);
                s.SetPosition(x,y);
                s.alpha = 0;
                tiles[(int)(y+Futile.screen.halfHeight)/tileSize,(int)(x+Futile.screen.halfWidth)/tileSize] = s;
                this.AddChild(s);
            }
        }
        
    }
    
    public void Update(Vector2 position){
        int y = (int)(position.y+Futile.screen.halfHeight)/tileSize;
        int x = (int)(position.x+Futile.screen.halfWidth)/tileSize;
        activeTiles.Clear();
        activeTiles.Add(TweenInTile(y, x));
        activeTiles.Add(TweenInTile(y-1, x));
        activeTiles.Add(TweenInTile(y+1, x));
        activeTiles.Add(TweenInTile(y, x-1));
        activeTiles.Add(TweenInTile(y, x+1));
        activeTiles.Add(TweenInTile(y-1, x-1));
        activeTiles.Add(TweenInTile(y+1, x+1));
        activeTiles.Add(TweenInTile(y-1, x+1));
        activeTiles.Add(TweenInTile(y+1, x-1));
        RemoveTiles();
    }

    FSprite TweenInTile(int y, int x) {
        Vector2 actualPos = tiles[y, x].GetPosition();
        if(Go.tweensWithTarget(tiles[y, x], false).Count == 0 && tiles[y, x].data == null) {
            tiles[y, x].SetPosition(actualPos.x + 100, actualPos.y + 100);
            //float distance = Vector2.Distance(actualPos, tiles[y,x].GetPosition());
            //Debug.Log(distance);
            Go.to(tiles[y, x], 0.666f, new TweenConfig().setEaseType(EaseType.CircOut).floatProp("x", actualPos.x).floatProp("y", actualPos.y).floatProp("alpha", 1).onComplete(HandlePlacement));
        }
        return tiles[y, x];
    }

    void RemoveTiles(){
        for(float y = -Futile.screen.halfHeight; y < Futile.screen.halfHeight; y+=tileSize){
            for(float x = -Futile.screen.halfWidth; x < Futile.screen.halfWidth; x+=tileSize){
                FSprite s = tiles[(int)(y+Futile.screen.halfHeight)/tileSize,(int)(x+Futile.screen.halfWidth)/tileSize];
                if(!activeTiles.Contains(s)){
                    if(Go.tweensWithTarget(s, false).Count == 0 && s.data != null) {
                        s.alpha = 0;
                        s.data = null;
                    }
                }
            }
        }
    }
    
    public void HandlePlacement (AbstractTween tween)
    {
        FSprite s = (tween as Tween).target as FSprite;
        s.data = new object();
    }
}