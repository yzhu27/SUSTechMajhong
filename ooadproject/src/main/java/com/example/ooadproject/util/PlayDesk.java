package com.example.ooadproject.util;

import com.example.ooadproject.bean.RequestMessage;
import com.example.ooadproject.bean.ResponseMessage;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.simp.SimpMessagingTemplate;

import java.util.*;

public class PlayDesk {

    public static void main(String[] args) {
        PlayDesk temp = new PlayDesk();
        Player a = new Player();
        a.setUsername("a");
        Player b = new Player();
        b.setUsername("b");
        Player c = new Player();
        c.setUsername("c");
        Player d = new Player();
        d.setUsername("d");
        temp.getPlayerslist().add(a);
        temp.getPlayerslist().add(b);
        temp.getPlayerslist().add(c);
        temp.getPlayerslist().add(d);

        temp.initial();
        String room = "1";
        System.out.println(temp.getCurrentPlayer());
        temp.startTimer(room);
        try{
            Thread.sleep(6000);
            temp.cancelTimer();
        }catch (Exception e){
            e.printStackTrace();
        }

        System.out.println(temp.getCurrentPlayer());



    }

    private List<Player> playerslist;
    //private List<Tile.Department> department;
    private TilePool tilePool;

    private String currentPlayer;
    private int playTile;
    private int roundNum;
    private int states;

    private List<RequestMessage> roundOperationResponseList;

    private Timer timer;
    @Autowired
    private SimpMessagingTemplate messagingTemplate;


    public PlayDesk() {
        this.playerslist = new ArrayList<>();
        //this.department = new ArrayList<>();
        this.roundOperationResponseList = new ArrayList<>();
        this.roundNum = 0;

    }

    public int getStates() {
        return states;
    }

    public void setStates(int states) {
        this.states = states;
    }

    public void setPlayerslist(List<Player> playerslist) {
        this.playerslist = playerslist;
    }

    public int getPlayTile() {
        return playTile;
    }

    public void setPlayTile(int playTile) {
        this.playTile = playTile;
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

    public void startTimer(String room) {
        this.timer = new Timer();

        this.timer.schedule(new TimerTask() {
            @Override
            public void run() {
                for (int i = 0; i < playerslist.size(); i++) {
                    if (playerslist.get(i).getUsername() == getCurrentPlayer()) {
                        int index = (i + 1) % 4;
                        String currentPlayer = playerslist.get(index).getUsername();
                        setCurrentPlayer(currentPlayer);
                        System.out.println(getCurrentPlayer());
                        states=1;
                        messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "CurrentPlayer", currentPlayer));
                        break;
                    }
                }
                timer.cancel();

            }
        }, 5000);

    }

    public void cancelTimer() {
        this.timer.cancel();
    }

    public int playerDraw(String name) {
        for (Player player : this.getPlayerslist()) {
            if (name.equals(player.getUsername())) {
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
        //System.out.println(tilePool.PoolToString());
        for (Player player : playerslist) {
            for (int i = 0; i < 12; i++) {
                player.getPlayerTiles().add(new Tile(tilePool.Draw()));
            }
            player.getDarkTiles().add(new Tile(tilePool.Draw()));
        }
        Random seed = new Random(System.currentTimeMillis());
        this.currentPlayer = playerslist.get(seed.nextInt(4)).getUsername();


        //
    }



}