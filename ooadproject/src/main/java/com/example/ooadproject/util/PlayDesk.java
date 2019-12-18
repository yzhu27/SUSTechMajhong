package com.example.ooadproject.util;

import com.example.ooadproject.bean.RequestMessage;

import java.util.*;

public class PlayDesk {

//    public static void main(String[] args) {
//        PlayDesk temp =new PlayDesk();
//        Player a = new Player();
//        a.setUsername("a");
//        Player b = new Player();
//        b.setUsername("b");
//        Player c = new Player();
//        c.setUsername("c");
//        Player d = new Player();
//        d.setUsername("d");
//        temp.getPlayerslist().add(a);
//        temp.getPlayerslist().add(b);
//        temp.getPlayerslist().add(c);
//        temp.getPlayerslist().add(d);
//
//        temp.initial();
//        System.out.println("---------------------");
//        System.out.println(temp.tilePool.getRemain());
//        System.out.println("---------------------");
//        for(Player p : temp.getPlayerslist()){
//            System.out.println(p.getUsername()+":");
//            System.out.println(p.playerTilesToString());
//            System.out.println("---------------------");
//        }
//        System.out.println("---------------------");
//
//
//
//    }

    private List<Player> playerslist;
    //private List<Tile.Department> department;
    private TilePool tilePool;
    private String currentPlayer;
    private int roundNum;
    private List<RequestMessage> roundOperationResponseList;

    public PlayDesk() {
        this.playerslist = new ArrayList<>();
        //this.department = new ArrayList<>();
        this.roundOperationResponseList = new ArrayList<>();
        this.roundNum=0;
    }

    public void setCurrentPlayer(String currentPlayer) {
        this.currentPlayer = currentPlayer;
    }

    public List<Player> getPlayerslist() {
        return playerslist;
    }

    public List<RequestMessage> getRoundOperationResponseList() {
        return roundOperationResponseList;
    }

    public int getRoundNum() {
        return roundNum;
    }

    public void setRoundNum(int roundNum) {
        this.roundNum = roundNum;
    }

    public String getCurrentPlayer() {
        return currentPlayer;
    }



    public int playerDraw(String name){
        for(Player player:this.getPlayerslist()){
            if(name.equals(player.getUsername())){
                int temp = this.tilePool.Draw();
                player.getPlayerTiles().add(new Tile(temp));
                return temp;
            }
        }
        return -1;
    }


    public void initial() {
        List<Tile.Department> departments = new ArrayList<>();
        departments.add(Tile.Department.Math);
        departments.add(Tile.Department.Phy);
        departments.add(Tile.Department.Chem);
        departments.add(Tile.Department.Cse);
        this.tilePool = new TilePool(departments);
        System.out.println(tilePool.PoolToString());
        for (Player player : playerslist) {
            for (int i = 0; i < 13; i++) {
                player.getPlayerTiles().add(new Tile(tilePool.Draw()));
            }
            player.getDarkTiles().add(new Tile(tilePool.Draw()));
        }
        Random seed = new Random(System.currentTimeMillis());
        this.currentPlayer = playerslist.get(seed.nextInt(4)).getUsername();



        //
    }


}
