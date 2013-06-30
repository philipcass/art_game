using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class TweenTerrain : FContainer {
    int tileSize = 32;
    FSprite[,] tiles;
    List<FSprite> activeTiles = new List<FSprite>(9);
    public List<FSprite> enablesTiles = new List<FSprite>();

    float width;
    float halfWidth;
    float height;
    float halfHeight;
    
    
    public TweenTerrain(){
        
        height = 768;
        width = 768;
        halfWidth = width/2;
        halfHeight = height/2;

        TextAsset t = Resources.Load("map") as TextAsset;
        IDictionary dict =  Json.Deserialize(t.text) as IDictionary;
        IList layers = dict["layers"] as IList;
        IDictionary layer = layers[0] as IDictionary;
        IList data = layer["data"] as IList;
        int index = 0;

        tiles = new FSprite[((int)height+tileSize)/tileSize,((int)width+tileSize)/tileSize];
        for(float y = -halfHeight; y < halfHeight; y+=tileSize){
            for(float x = -halfWidth; x < halfWidth; x+=tileSize){
                FSprite s = new FSprite(Futile.whiteElement);
                if((long)data[index++] == 1)
                    enablesTiles.Add(s);
                s.SetPosition(x,y);
                s.scale = 0;
                s.alpha = 0;
                tiles[(int)(y+halfHeight)/tileSize,(int)(x+halfWidth)/tileSize] = s;
                this.AddChild(s);
            }
        }
        Debug.Log(index);
    }
    
    public void Update(Vector2 position){
        int y = (int)(Mathf.Round(position.y+16)+halfHeight)/tileSize;
        int x = (int)(Mathf.Round(position.x+16)+halfWidth)/tileSize;
        activeTiles.Clear();
        
        for(int i = y-1;i<=y+1;i++){
            for(int j = x-1;j<=x+1;j++){
                if(i>=0 && j>=0 && i<tiles.GetUpperBound(0) && j<tiles.GetUpperBound(1))
                    activeTiles.Add(TweenInTile(i,j));
            }
        }

        //RemoveTiles();
    }

    FSprite TweenInTile(int y, int x) {
        Vector2 actualPos = tiles[y, x].GetPosition();
        if(Go.tweensWithTarget(tiles[y, x], false).Count == 0 && tiles[y, x].data == null && !enablesTiles.Contains(tiles[y,x])) {
            Vector2 tweenPos = actualPos+(Vector2)(Random.rotation*new Vector2(100,100));
            tiles[y, x].SetPosition(tweenPos);
            //float distance = Vector2.Distance(actualPos, tiles[y,x].GetPosition());
            //Debug.Log(distance);
            Go.to(tiles[y, x], 0.666f, new TweenConfig().setEaseType(EaseType.CircOut).floatProp("x", actualPos.x)
                .floatProp("y", actualPos.y)
                .floatProp("scale", 2)
                .floatProp("alpha", 1)
                .onComplete(HandlePlacement));
        }
        return tiles[y, x];
    }

    public void RemoveTiles(){
        for(float y = -halfHeight; y < halfHeight; y+=tileSize){
            for(float x = -halfWidth; x < halfWidth; x+=tileSize){
                FSprite s = tiles[(int)(y+halfHeight)/tileSize,(int)(x+halfWidth)/tileSize];
                if(!activeTiles.Contains(s)){
                    if(Go.tweensWithTarget(s, false).Count == 0 && s.data != null) {
                        Vector2 tweenPos = s.GetPosition()+(Vector2)(Random.rotation*new Vector2(100,100));
//                        Go.to(s, 0.66f, new TweenConfig().setEaseType(EaseType.CircOut).floatProp("x", tweenPos.x)
//                            .floatProp("y", tweenPos.y)
//                            .floatProp("scale", 0)
//                            .floatProp("alpha", 0)
//                            .onComplete(HandleRemove));
                        s.scale = 0;
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
        s.data = s.GetPosition();
    }
    public void HandleRemove (AbstractTween tween)
    {
        FSprite s = (tween as Tween).target as FSprite;
        s.SetPosition((Vector2)s.data);
        s.data = null;
    }
}