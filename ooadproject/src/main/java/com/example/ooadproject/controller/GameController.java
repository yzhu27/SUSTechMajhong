package com.example.ooadproject.controller;

import com.example.ooadproject.bean.ChatMessage;
import com.example.ooadproject.bean.RequestMessage;
import com.example.ooadproject.bean.ResponseMessage;
import com.example.ooadproject.util.JsonUtil;
import com.example.ooadproject.util.PlayDesk;
import com.example.ooadproject.util.Player;
import com.example.ooadproject.util.Tile;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.event.EventListener;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.messaging.handler.annotation.DestinationVariable;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.Payload;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.messaging.simp.stomp.StompHeaderAccessor;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.socket.messaging.SessionConnectedEvent;
import org.springframework.web.socket.messaging.SessionDisconnectEvent;
import org.springframework.web.socket.messaging.SessionSubscribeEvent;
import org.springframework.web.util.HtmlUtils;

import java.net.InetAddress;
import java.util.*;

@Controller
public class GameController {
    @Autowired
    private SimpMessagingTemplate messagingTemplate;

    private static final Logger LOGGER = LoggerFactory.getLogger(GameController.class);

    @Value("${redis.channel.msgToAll}")
    private String msgToAll;

    @Value("${redis.set.onlineUsers}")
    private String onlineUsers;

    @Value("${redis.channel.userStatus}")
    private String userStatus;

    @Value("${server.port}")
    private String serverPort;

    @Autowired
    private RedisTemplate<String, String> redisTemplate;

    private Map<String, String> userlist = new HashMap<>();

    private Map<String, PlayDesk> roomlist = new HashMap<>();

    @MessageMapping("/echo")
    public void echo(RequestMessage requestMessage) {
        try {
            messagingTemplate.convertAndSend("/topic/echo", requestMessage);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    //玩家登录
    @MessageMapping("/hello/{username}")
    public void usertest(@DestinationVariable String username, StompHeaderAccessor headerAccessor, RequestMessage requestMessage) {
        try {
            String sender = HtmlUtils.htmlEscape(requestMessage.getSender());
            String type = requestMessage.getType();
            String content = requestMessage.getContent();
            if (userlist.containsKey(sender)) {
                //用户名已经存在
                String sessionId = headerAccessor.getSessionId();
                LOGGER.info("Reject-login: " + sessionId + "想要以用户名" + sender + "登录，用户名已经存在");
                type = "Reject-login";
                content = "sorry 用户名已经存在";
            } else {
                //登录成功
                LOGGER.info("Accept-login: 玩家" + sender + "登录成功");
                userlist.put(sender, "NoRoom");
                type = "Accept-login";
                content = sender;
                headerAccessor.getSessionAttributes().put("username", requestMessage.getSender());
                redisTemplate.opsForSet().add(onlineUsers, username);
                redisTemplate.convertAndSend(userStatus, JsonUtil.parseObjToJson(requestMessage));
            }
            String test = "";
            for (String un : userlist.keySet()) {
                test += (un + " ");
            }
            LOGGER.info("userlist: 当前在线玩家 " + test);
            ResponseMessage responseMessage = new ResponseMessage("Server", type, content);
            messagingTemplate.convertAndSendToUser(username, "/greetings", responseMessage);
        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }
    }

    @MessageMapping("/private.sendMessage/{username}")
    public void privatechat(@DestinationVariable String username, StompHeaderAccessor headerAccessor, RequestMessage requestMessage) {
        try {
            String sender = "Server";
            String type = HtmlUtils.htmlEscape(requestMessage.getType());
            String content = HtmlUtils.htmlEscape(requestMessage.getContent());

            if (content.equals("/getRoomList")) {
                content = "";
                for (String room : roomlist.keySet()) {
                    content = content + room + "\n";
                }
                ResponseMessage responseMessage = new ResponseMessage(sender, type, content);
                messagingTemplate.convertAndSendToUser(username, "/chat", responseMessage);
            } else if (content.equals("/usertest")) {
                content = "test";
                ResponseMessage responseMessage = new ResponseMessage(sender, type, content);
                messagingTemplate.convertAndSendToUser("2", "/chat", responseMessage);
            }

        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }
    }

    @MessageMapping("/public.sendMessage")
    public void sendMessage(@Payload ChatMessage chatMessage) {
        LOGGER.info("/public sendMessage");
        try {
            redisTemplate.convertAndSend(msgToAll, JsonUtil.parseObjToJson(chatMessage));
        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);

        }
    }

    @CrossOrigin
    @MessageMapping("/room.sendMessage")
    public void messageHandling(RequestMessage requestMessage) {
        try {
            String destination = "/topic/" + HtmlUtils.htmlEscape(requestMessage.getRoom());
            String sender = HtmlUtils.htmlEscape(requestMessage.getSender());
            String type = HtmlUtils.htmlEscape(requestMessage.getType());
            String content = HtmlUtils.htmlEscape(requestMessage.getContent());
            ResponseMessage response = new ResponseMessage(sender, type, content);
            LOGGER.info("User:" + requestMessage.getSender() + "Send Content:" + requestMessage.getContent() + "in Room" + requestMessage.getRoom());
            messagingTemplate.convertAndSend(destination, response);
        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }

    }

    //单人麻将测试
    @MessageMapping("/room.addUserSingleTest")
    public void addUserTest(@Payload RequestMessage requestMessage, StompHeaderAccessor headerAccessor) {
        try {
            LOGGER.info("Begin Single Test! /room.addUserSingleTest");

            String sender = requestMessage.getSender();
            String room = requestMessage.getRoom();
            headerAccessor.getSessionAttributes().put("username", requestMessage.getSender());
            userlist.put(sender, room);
            //room 1 玩家 a b c d
            PlayDesk temp = new PlayDesk();
            Player a = new Player();
            a.setUsername(sender);
            temp.getPlayerslist().add(a);
            Player b = new Player();
            b.setUsername("b");
            temp.getPlayerslist().add(b);
            Player c = new Player();
            c.setUsername("c");
            temp.getPlayerslist().add(c);
            Player d = new Player();
            d.setUsername("d");
            temp.getPlayerslist().add(d);
            //生成牌堆
            temp.initial();
            LOGGER.info("PlayDesk initial");

            for (Player player : temp.getPlayerslist()) {
                player.setReady(true);
            }
            messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "Game-Start", "all-ready"));
            LOGGER.info("Game Start!");

            for (int i = 0; i < 4; i++) {
                if (temp.getPlayerslist().get(i).getUsername().equals(temp.getCurrentPlayer())) {
                    messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "PlayerSeqNum", sender + "," + 0 + " " + "b,1 " + "c,2 " + "d,3" + " " + i));
                    LOGGER.info("Send Player Sequence number");
                }
            }
            LOGGER.info("Round: " + temp.getRoundNum() + " Current Player is " + temp.getCurrentPlayer());
            temp.setSender(messagingTemplate);
            roomlist.put(room, temp);

            for (Player player : temp.getPlayerslist()) {
                if (player.getUsername().equals(sender)) {
                    ResponseMessage responseMessage = new ResponseMessage();
                    responseMessage.setSender("Server");
                    responseMessage.setType("PlayerTiles");
                    String content = "";
                    for (Tile tile : player.getPlayerTiles()) {
                        content = content + tile.getId() + ",";
                    }
                    responseMessage.setContent(content);
                    messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                    LOGGER.info("Send player " + player.getUsername() + "'s tiles");
                    responseMessage.setType("DarkTiles");
                    responseMessage.setContent("" + player.getDarkTiles().get(0).getId());
                    messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                    LOGGER.info("Send player " + player.getUsername() + "'s dark tile");
                }
            }
        } catch (Exception e) {
            LOGGER.info(e.getMessage());
        }
    }

    //单人麻将测试 跳过机器人回合
    @MessageMapping("/room.nextSingleTest")
    public void next(@Payload RequestMessage requestMessage) {
        try {
            LOGGER.info("/room.nextSingleTest");
            String sender = requestMessage.getSender();
            String room = requestMessage.getRoom();
            PlayDesk temp = roomlist.get(room);
            String currentPlayer = temp.getCurrentPlayer();
            if (!sender.equals(currentPlayer)) {
                //LOGGER.info("yes");
                temp.setRoundNum(temp.getRoundNum() + 1);
                if (temp.getStates() == 1) {
//                    LOGGER.info("Round: "+temp.getRoundNum()+" current player: "+temp.getCurrentPlayer());
//                    temp.playerDraw(temp.getCurrentPlayer());
//                    LOGGER.info("Player "+temp.getCurrentPlayer()+" draw");
//                    messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(temp.getCurrentPlayer(), "PlayerDraw", ""));
                    for (Player player : temp.getPlayerslist()) {
                        if (player.getUsername().equals(temp.getCurrentPlayer())) {
                            Tile playIt = player.getPlayerTiles().get(0);
                            player.getPlayerTiles().remove(0);
                            messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(player.getUsername(), "Accept-Play", playIt.getId() + ""));
                            LOGGER.info("Player " + temp.getCurrentPlayer() + " plays " + playIt.getId());
                            temp.setPlayTile(playIt.getId());
                            LOGGER.info("wait for response");
                            temp.setStates(2);
                        }
                    }
                } else {
                    LOGGER.info("next button, start timer");
                    temp.startTimer(room);
                }
            } else {
                messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage("Server", "Reject-next", "this is your round"));
            }
        } catch (Exception e) {
            LOGGER.info("some error:");
            e.printStackTrace();
        }
    }


    //用户加入房间
    @MessageMapping("/room.addUser")
    public void addUser(@Payload RequestMessage requestMessage) {
        try {
            LOGGER.info("room.addUser");
            //如果找不到该room 创建对应room的playdesk
            String sender = requestMessage.getSender();
            String room = requestMessage.getRoom();
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.setSender("Server");
            if (roomlist.containsKey(room)) {
                PlayDesk current = roomlist.get(room);
                List<Player> userlistTemp = current.getPlayerslist();
                if (userlistTemp.size() < 4) {
                    userlist.replace(sender, room);
                    Player temp = new Player();
                    temp.setUsername(sender);
                    userlistTemp.add(temp);
                    userlist.replace(sender, room);
                    responseMessage.setType("Accept-room.addUser");
                    String content = "";
                    for (int i = 0; i < userlistTemp.size(); i++) {
                        content = content + userlistTemp.get(i).getUsername() + " ";
                    }
                    String ready = "";
                    for (Player player : current.getPlayerslist()) {
                        if (player.getReday()) {
                            ready = ready + player.getUsername() + " ";
                        }
                    }
                    LOGGER.info("玩家" + sender + "加入房间" + room + " 当前房间内玩家" + content + " ,当前已准备玩家 " + ready);
                    responseMessage.setContent(content.trim() + "," + ready.trim());
                    messagingTemplate.convertAndSend("/topic/" + room, responseMessage);
                } else {
                    LOGGER.info("玩家" + sender + "加入房间" + room + "失败，房间已满");
                    responseMessage.setType("Reject-room.addUser");
                    responseMessage.setContent("Room is full");
                    messagingTemplate.convertAndSendToUser(sender, "/chat", responseMessage);
                }

            } else {
                PlayDesk current = new PlayDesk();
                Player temp = new Player();
                temp.setUsername(sender);
                current.getPlayerslist().add(temp);
                roomlist.put(room, current);
                userlist.replace(sender, room);
                current.setSender(messagingTemplate);
                responseMessage.setType("Accept-room.addUser");
                String content = "";
                for (Player player : current.getPlayerslist()) {
                    content = content + player.getUsername() + " ";
                }
                LOGGER.info("玩家" + sender + "加入房间" + room + " 当前房间内玩家 " + content);
                responseMessage.setContent(content.trim() + ",");
                messagingTemplate.convertAndSend("/topic/" + room, responseMessage);
            }

        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }

    }

    @MessageMapping("/room.deleteUser")
    public void deleteUser(RequestMessage requestMessage) {
        try {
            //得到sender和room 对应更新
            String sender = requestMessage.getSender();
            String room = requestMessage.getRoom();
            PlayDesk temp = roomlist.get(room);
            if (temp != null && !room.equals("NoRoom")) {
                for (int i = 0; i < temp.getPlayerslist().size(); i++) {
                    if (temp.getPlayerslist().get(i).getUsername().equals(sender)) {
                        temp.getPlayerslist().remove(i);
                        break;
                    }
                }
                LOGGER.info("Accept-deleteUser: 玩家" + sender + "离开房间" + room);
                ResponseMessage responseMessage = new ResponseMessage();
                responseMessage.setSender("Server");
                responseMessage.setType("Accept-deleteUser");
                responseMessage.setContent(sender + " left");
                messagingTemplate.convertAndSendToUser(sender, "/chat", responseMessage);
                try {
                    Thread.sleep(300);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, "exit", ""));
                if (temp.getPlayerslist().size() == 0) {
                    roomlist.remove(room);
                }
            }
        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }


    }

    @MessageMapping("/room.getPlayer")
    public void getPlayer(@Payload RequestMessage requestMessage) {
        try {
            LOGGER.info("room.getPlayer");
            ResponseMessage responseMessage = new ResponseMessage();
            String content = "";
            responseMessage.setSender("Server");
            responseMessage.setType("Accept-getPlayer");
            String room = requestMessage.getRoom();
            PlayDesk temp = roomlist.get(room);
            for (Player player : temp.getPlayerslist()
            ) {
                content = content + player.getUsername() + "\n";
            }
            responseMessage.setContent(content);
            messagingTemplate.convertAndSendToUser(requestMessage.getSender(), "/chat", responseMessage);
        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }
    }

    //玩家准备
    @MessageMapping("/room.ready")
    public void roomReady(@Payload RequestMessage requestMessage) {
        try {
            LOGGER.info("/room.ready");
            String sender = requestMessage.getSender();
            String room = requestMessage.getRoom();
            String type = requestMessage.getType();

            PlayDesk temp = roomlist.get(room);
            if (type.equals("initialDarwReady")) {
                LOGGER.info(sender + " initial draw ready: " + "玩家" + sender + "初始抽牌完成");
                for (Player player : temp.getPlayerslist()) {
                    if (sender.equals(player.getUsername())) {
                        player.setInitialDrawReady(true);
                    }
                }
                boolean allReady = true;
                for (Player player : temp.getPlayerslist()
                ) {
                    if (!player.isInitialDrawReady()) {
                        allReady = false;
                    }
                }
                String content = requestMessage.getContent();

                if (allReady) {
                    LOGGER.info("Receive 4 players' initial draw response: 四个玩家初始抽牌完成");
                    ResponseMessage responseMessage = new ResponseMessage();
                    responseMessage.setSender("Server");
                    responseMessage.setType("CurrentPlayer");
                    requestMessage.setSender(temp.getCurrentPlayer());
                    responseMessage.setSender(temp.getCurrentPlayer());
                    messagingTemplate.convertAndSend("/topic/" + room, responseMessage);
                    LOGGER.info("Player " + temp.getCurrentPlayer() + " draw");
                    responseMessage.setType("PlayerDraw");
                    responseMessage.setContent("" + temp.playerDraw(temp.getCurrentPlayer()));
                    messagingTemplate.convertAndSendToUser(temp.getCurrentPlayer(), "/chat", responseMessage);
                }
            } else if (type.equals("BackgroundLoadReady")) {
                LOGGER.info(sender + " background load ready: " + "玩家" + sender + "游戏场景初始化完成");
                for (Player player : temp.getPlayerslist()) {
                    if (sender.equals(player.getUsername())) {
                        player.setBackgroundReady(true);
                    }
                }
                boolean allReady = true;
                for (Player player : temp.getPlayerslist()
                ) {
                    if (!player.isBackgroundReady()) {
                        allReady = false;
                    }
                }
                if (allReady) {
                    LOGGER.info("all back ground ready: 四个玩家场景加载均完成");
                    //messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "Game-Start", "all-ready"));
                    temp.initial();
                    for (int i = 0; i < 4; i++) {
                        if (temp.getPlayerslist().get(i).getUsername().equals(temp.getCurrentPlayer())) {
                            messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "PlayerSeqNum", temp.getPlayerslist().get(0).getUsername() + "," + 0 + " " + temp.getPlayerslist().get(1).getUsername() + ",1 " + temp.getPlayerslist().get(2).getUsername() + ",2 " + temp.getPlayerslist().get(3).getUsername() + ",3" + " " + i));
                            LOGGER.info(temp.getPlayerslist().get(0).getUsername() + "," + 0 + " " + temp.getPlayerslist().get(1).getUsername() + ",1 " + temp.getPlayerslist().get(2).getUsername() + ",2 " + temp.getPlayerslist().get(3).getUsername() + ",3" + " " + i);
                        }
                    }
                    for (Player player : temp.getPlayerslist()) {
                        ResponseMessage responseMessage = new ResponseMessage();
                        responseMessage.setSender("Server");
                        responseMessage.setType("PlayerTiles");
                        String content = "";
                        for (Tile tile : player.getPlayerTiles()) {
                            content = content + tile.getId() + ",";
                        }
                        LOGGER.info("send tiles to " + player.getUsername() + " " + content);
                        responseMessage.setContent(content.trim());
                        messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                        responseMessage.setType("DarkTiles");
                        responseMessage.setContent("" + player.getDarkTiles().get(0).getId());
                        messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                    }
                }
            } else {
                String content = "";
                for (Player player : temp.getPlayerslist()) {
                    if (sender.equals(player.getUsername())) {
                        player.setReady(true);
                        //player.setDepartment(department);
                    }
                    if (player.getReday()) {
                        content = content + player.getUsername() + " ";
                    }
                }

                messagingTemplate.convertAndSend("/topic/" + room + "/ready", new ResponseMessage("Server", "Player-ready", content.trim()));
                LOGGER.info(sender + " ready, 房间内准备玩家" + content);
            }
        } catch (Exception e) {
            LOGGER.error(e.getMessage());
        }


    }


    @MessageMapping("/room.roundOperation")
    public void roomOperation(@Payload RequestMessage requestMessage) {

        //得到sender type content(牌) room 拿出对应的playdesk
        String sender = requestMessage.getSender();
        String room = requestMessage.getRoom();
        String type = requestMessage.getType();
        PlayDesk temp = roomlist.get(room);
        if (temp.getStates() != 99) {
            LOGGER.info("/room.roundOperation");
            if (!sender.equals(temp.getCurrentPlayer())) {
                ResponseMessage responseMessage = new ResponseMessage();
                responseMessage.setSender("Server");
                responseMessage.setType("Reject-play");
                responseMessage.setContent("不是你的时间");
                messagingTemplate.convertAndSendToUser(sender, "/chat", responseMessage);
            } else if (type.equals("play")) {
                LOGGER.info(sender + " play a tile, accept");
                int tile = Integer.parseInt(requestMessage.getContent());
                for (Player player : temp.getPlayerslist()) {
                    if (player.getUsername().equals(sender)) {
                        player.getPlayerTiles().remove(new Tile(tile));
                        LOGGER.info(sender + "' tiles, length: " + player.getPlayerTiles().size());
                    }
                }
                temp.setRoundNum(temp.getRoundNum() + 1);
                ResponseMessage responseMessage = new ResponseMessage();
                responseMessage.setContent("" + tile);
                temp.setPlayTile(tile);
                temp.getRoundOperationResponseList().clear();
                responseMessage.setType("Accept-Play");
                for (Player player : temp.getPlayerslist()) {
                    responseMessage.setSender(sender);
                    messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                }
                temp.cancelTimer();
                temp.startTimer(room);
                LOGGER.info("start timer to wait for response");

            } else {
                LOGGER.info("round operation before play a tile");
                for (Player player : temp.getPlayerslist()) {
                    if (sender.equals(player.getUsername())) {
                        switch (type) {
                            case "exchange":
                                LOGGER.info(sender + " exchange");
                                String handTilesString = requestMessage.getContent().split(",")[0];
                                String darkTilesString = requestMessage.getContent().split(",")[1];
                                String[] handTiles = handTilesString.split(" ");
                                String[] darkTiles = darkTilesString.split(" ");
                                player.exchange(handTiles, darkTiles);
                                break;
                            case "selfRod":
                                //拿一张手牌
                                LOGGER.info(sender + " selfRod");
                                String[] tiles = requestMessage.getContent().split(" ");
                                player.selfRod(tiles);
                                messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, "selfRod", tiles[0]));
                                int selfRodDraw = temp.playerDraw(sender);
                                messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, "selfRodDraw", ""));
                                messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage(sender, "selfRodDraw", selfRodDraw + ""));
                                break;
                            case "darkRod":
                                LOGGER.info(sender + " darkRod");
                                String[] tiles1 = requestMessage.getContent().split(" ");
                                player.darkRod(tiles1);
                                break;
                            case "addRod":
                                LOGGER.info(sender + " addRod");
                                break;
                            default:
                                messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "no such method", ""));
                                break;
                        }
                        break;
                    }

                }
            }
        }


    }

    @MessageMapping("/room.roundOperationResponse")
    public void roundOperationResponse(RequestMessage requestMessage) {
        String sender = requestMessage.getSender();
        String room = requestMessage.getRoom();
        String type = requestMessage.getType();
        String content = requestMessage.getContent();
        PlayDesk temp = roomlist.get(room);
        temp.cancelTimer();
        if(temp.getStates() != 99){
            LOGGER.info("room.roundOperationResponse");
            if (!sender.equals(temp.getCurrentPlayer())) {
                temp.getRoundOperationResponseList().add(requestMessage);
                if (temp.getRoundOperationResponseList().size() == 1) {
                    for (Player player : temp.getPlayerslist()) {
                        if (sender.equals(player.getUsername())) {
                            String[] tiles = content.split(" ");
                            switch (type) {
                                case "eat":
                                    player.eat(tiles, temp.getPlayTile());
                                    LOGGER.info(sender + " eat! " + tiles[0] + " " + tiles[1] + " " + temp.getPlayTile());
                                    messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, type, content + " " + temp.getPlayTile()));
                                    break;
                                case "touch":
                                    player.touch(tiles, temp.getPlayTile());
                                    LOGGER.info(sender + " touch! " + tiles[0] + " " + tiles[1] + " " + temp.getPlayTile());
                                    messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, type, content + " " + temp.getPlayTile()));
                                    break;
                                case "rod":
                                    player.rod(tiles, temp.getPlayTile());
                                    LOGGER.info(sender + " rod! " + tiles[0] + " " + tiles[1] + " " + tiles[2] + " " + temp.getPlayTile());
                                    messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, type, content + " " + temp.getPlayTile()));
                                    break;
                                default:
                                    messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", type, "no such method"));
                                    break;
                            }
                            break;
                        }
                    }
                    temp.setStates(1);
                    temp.setCurrentPlayer(sender);
                    temp.setRoundNum(temp.getRoundNum() + 1);
                    messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(temp.getCurrentPlayer(), "CurrentPlayer", ""));
                    try {
                        Thread.sleep(200);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                    messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage("Server", "PlayerDraw", ""));
                } else {
                    messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage("Server", "Reject-playResponse", "u are late"));
                }
            } else {

                messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage("Server", "Reject-playResponse", ""));
            }
        }

    }

    @MessageMapping("/room.win")
    public void win(RequestMessage requestMessage) {
        String sender = requestMessage.getSender();
        String room = requestMessage.getRoom();
        PlayDesk temp = roomlist.get(room);
        String content = "";
        for (Player player : temp.getPlayerslist()) {
            if (player.getUsername().equals(sender)) {
                for (Tile t : player.getPlayerTiles()) {
                    content = content + t.getId() + " ";
                }
            }
        }
        temp.setStates(99);
        LOGGER.info(sender + " win! winner tiles: " + content);
        messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, "winnerTiles", content.trim()));
    }

    @EventListener
    public void handleWebSocketDisconnectListener(SessionDisconnectEvent event) {

        StompHeaderAccessor headerAccessor = StompHeaderAccessor.wrap(event.getMessage());

        String username = (String) headerAccessor.getSessionAttributes().get("username");
        String sessionid = headerAccessor.getSessionId();

        if (username != null) {
            LOGGER.info("User Disconnected : " + sessionid + " " + username);
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.setType(ChatMessage.MessageType.LEAVE);
            chatMessage.setSender(username);
            RequestMessage temp = new RequestMessage();
            temp.setSender(username);
            LOGGER.info("Room", userlist.get(username));
            temp.setRoom(userlist.get(username));
            LOGGER.info("Delete user " + username + "from room " + temp.getRoom());
            this.deleteUser(temp);
            userlist.remove(username);
            try {
                redisTemplate.opsForSet().remove(onlineUsers, username);
                redisTemplate.convertAndSend(userStatus, JsonUtil.parseObjToJson(chatMessage));
            } catch (Exception e) {
                LOGGER.error(e.getMessage(), e);
            }

        }
    }

    @EventListener
    public void handleWebSocketConnectListener(SessionConnectedEvent event) {
        InetAddress localHost;
        try {
            StompHeaderAccessor headerAccessor = StompHeaderAccessor.wrap(event.getMessage());
            //这里的sessionId对应HttpSessionIdHandshakeInterceptor拦截器的存放key
            //String username = (String) headerAccessor.getSessionAttributes().get("username");
            String sessionId = headerAccessor.getSessionId();
            LOGGER.info("Received a new web socket connection : " + sessionId);
        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }

    }

    @EventListener
    public void handleSubsribeListener(SessionSubscribeEvent event) {
        try {
            StompHeaderAccessor headerAccessor = StompHeaderAccessor.wrap(event.getMessage());
            //这里的sessionId对应HttpSessionIdHandshakeInterceptor拦截器的存放key
            String username = (String) headerAccessor.getSessionAttributes().get("username");
            String path = headerAccessor.getDestination();
            String sessionId = headerAccessor.getSessionId();
            LOGGER.info("stomp Subscribe : " + sessionId + " " + username + " " + path);

        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }

    }


}
