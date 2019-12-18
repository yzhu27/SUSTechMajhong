package com.example.ooadproject.util;

import java.util.ArrayList;
import java.util.List;

public class Player {
    private String username;
    private boolean ready;
    private Tile.Department department;
    private List<Tile> playerTiles;
    private List<Tile> darkTiles;

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
}
