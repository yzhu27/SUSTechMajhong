package com.example.ooadproject.util;

import java.util.HashMap;
import java.util.Map;

public class TileFactory {

    private Map<Integer, Tile> tiles;

    public TileFactory() {
        this.tiles = new HashMap<>();
    }
    public Tile getTile(int id){
        if(tiles.containsKey(id)){
            return tiles.get(id);
        }else {
            Tile tile = null;
            try {
                tile = new Tile(id);
            } catch (Exception e) {
                e.printStackTrace();
            }
            tiles.put(id,tile);
            return tile;
        }
    }

}
