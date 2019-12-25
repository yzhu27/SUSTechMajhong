package com.example.ooadproject.util;

import com.example.ooadproject.bean.RequestMessage;
import com.example.ooadproject.bean.ResponseMessage;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.simp.SimpMessagingTemplate;

import java.awt.*;
import java.util.*;
import java.util.List;

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
        try {
            Thread.sleep(6000);
            temp.cancelTimer();
        } catch (Exception e) {
            e.printStackTrace();
        }

        System.out.println(temp.getCurrentPlayer());


    }

    private static final Logger LOGGER = LoggerFactory.getLogger(PlayDesk.class);
    private List<Player> playerslist;
    //private List<Tile.Department> department;
    private TilePool tilePool;

    private String currentPlayer;
    private int playTile;
    private int roundNum;
    private int states;

    private List<RequestMessage> roundOperationResponseList;

    private Timer timer;
    private SimpMessagingTemplate messagingTemplate;


    public PlayDesk() {
        this.playerslist = new ArrayList<>();
        //this.department = new ArrayList<>();
        this.roundOperationResponseList = new ArrayList<>();
        this.roundNum = 0;

    }

    public void setSender(SimpMessagingTemplate messagingTemplate) {
        this.messagingTemplate = messagingTemplate;
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
        LOGGER.info("startTimer " + timer);

        this.timer = new Timer();

        this.timer.schedule(new TimerTask() {
            @Override
            public void run() {
                for (int i = 0; i < playerslist.size(); i++) {
                    if (playerslist.get(i).getUsername().equals(getCurrentPlayer())) {
                        int index = (i + 1) % 4;
                        String currentPlayer = playerslist.get(index).getUsername();
                        setCurrentPlayer(currentPlayer);
                        LOGGER.info("Timer");
                        states = 1;
                        messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "No-one response", ""));
                        try {
                            Thread.sleep(300);
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                        messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(currentPlayer, "CurrentPlayer", ""));
                        roundNum++;
                        LOGGER.info("no one response,next player is " + currentPlayer);
                        LOGGER.info(currentPlayer + " draw");
                        ResponseMessage responseMessage = new ResponseMessage();
                        responseMessage.setSender(currentPlayer);
                        responseMessage.setType("PlayerDraw");
                        responseMessage.setContent("" + playerDraw(getCurrentPlayer()));
                        messagingTemplate.convertAndSendToUser(getCurrentPlayer(), "/chat", responseMessage);
                        try {
                            Thread.sleep(300);
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                        messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(getCurrentPlayer(), "PlayerDraw", ""));
                        break;
                    }
                }
                cancelTimer();
            }
        }, 10000);
    }


    public void cancelTimer() {
        if (timer == null) {

        } else {
            this.timer.cancel();
            this.timer = null;
        }
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
        this.states = 1;
        //System.out.println(tilePool.PoolToString());
        for (Player player : playerslist) {
            for (int i = 0; i < 13; i++) {
                player.getPlayerTiles().add(new Tile(tilePool.Draw()));
            }
            player.getDarkTiles().add(new Tile(tilePool.Draw()));
        }
        Random seed = new Random(System.currentTimeMillis());
        this.currentPlayer = playerslist.get(seed.nextInt(4)).getUsername();
        //System.out.println(this.getCurrentPlayer());


        //
    }


}
