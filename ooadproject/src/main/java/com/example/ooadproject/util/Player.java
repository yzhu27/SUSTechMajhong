package com.example.ooadproject.util;

import java.util.ArrayList;
import java.util.List;

public class Player {
    private String username;
    private boolean ready;
    private Tile.Department department;
    private List<Tile> playerTiles;
    private List<Tile> darkTiles;
    private List<Tile> eat;
    private List<Tile> touch;
    private List<Tile> rod;
    private List<Tile> selfRod;
    private List<Tile> darkRod;
    private List<Tile> exchange;

    public Tile.Department getDepartment() {
        return department;
    }

    public void setDepartment(Tile.Department department) {
        this.department = department;
    }

    public List<Tile> getDarkTiles() {
        return darkTiles;
    }

    public void setDarkTiles(List<Tile> darkTiles) {
        this.darkTiles = darkTiles;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public void setReady(boolean ready) {
        this.ready = ready;
    }

    public boolean getReday() {
        return ready;
    }

    public List<Tile> getPlayerTiles() {
        return playerTiles;
    }

    public Player() {
        this.playerTiles = new ArrayList<>();
        this.darkTiles = new ArrayList<>();
        this.eat = new ArrayList<>();
        this.touch = new ArrayList<>();
        this.rod = new ArrayList<>();
        this.selfRod = new ArrayList<>();
        this.darkRod = new ArrayList<>();
        this.exchange = new ArrayList<>();
    }

    public String playerTilesToString() {
        String temp = "";
        for (Tile tile : this.playerTiles) {
            temp = temp + tile.toString() + "\n";
        }
        return temp;
    }

    @Override
    public boolean equals(Object object) {
        return object != null && object instanceof Player && ((Player) object).getUsername().equals(this.username);
    }
    public void eat(String[] tiles,int playTile) {
        this.eat.add(new Tile(playTile));
        this.eat.add(new Tile(Integer.parseInt(tiles[0])));
        this.eat.add(new Tile(Integer.parseInt(tiles[1])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[0])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[1])));

    }

    public void touch(String[] tiles,int playTile) {
        this.touch.add(new Tile(playTile));
        this.touch.add(new Tile(Integer.parseInt(tiles[0])));
        this.touch.add(new Tile(Integer.parseInt(tiles[1])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[0])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[1])));
    }

    public void rod(String[] tiles,int playTile) {
        this.rod.add(new Tile(playTile));

        this.rod.add(new Tile(Integer.parseInt(tiles[0])));
        this.rod.add(new Tile(Integer.parseInt(tiles[1])));
        this.rod.add(new Tile(Integer.parseInt(tiles[2])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[0])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[1])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[2])));
    }

    public void selfRod(String[] tiles,int playTile) {
        this.selfRod.add(new Tile(playTile));

        this.selfRod.add(new Tile(Integer.parseInt(tiles[0])));
        this.selfRod.add(new Tile(Integer.parseInt(tiles[1])));
        this.selfRod.add(new Tile(Integer.parseInt(tiles[2])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[0])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[1])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[2])));
    }

    public void darkRod(String[] tiles,int playTile) {
        this.darkRod.add(new Tile(playTile));

        this.darkRod.add(new Tile(Integer.parseInt(tiles[0])));
        this.darkRod.add(new Tile(Integer.parseInt(tiles[1])));
        this.darkRod.add(new Tile(Integer.parseInt(tiles[2])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[0])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[1])));
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[2])));
    }

    public void exchange(String[] tiles,int playTile) {
        this.playerTiles.remove(new Tile(Integer.parseInt(tiles[0])));
        this.playerTiles.add(new Tile(playTile));
        this.darkTiles.remove(new Tile(playTile));
        this.darkTiles.add(new Tile(Integer.parseInt(tiles[0])));
    }
}
