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
public class RoomController {
    @Autowired
    private SimpMessagingTemplate messagingTemplate;

    private static final Logger LOGGER = LoggerFactory.getLogger(RoomController.class);

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

    @MessageMapping("/hello/{username}")
    public void usertest(@DestinationVariable String username, StompHeaderAccessor headerAccessor, RequestMessage requestMessage) {
        try {
            String sender = HtmlUtils.htmlEscape(requestMessage.getSender());
            String type = null;
            String content = null;
            if (userlist.containsKey(sender)) {
                type = "Reject-login";
                content = "sorry 用户名已经存在";
            } else {
                userlist.put(sender, "NoRoom");
                type = "Accept-login";
                content = "welcome " + sender;

                headerAccessor.getSessionAttributes().put("username", requestMessage.getSender());
                String sessionId = headerAccessor.getSessionId();
                LOGGER.info("login " + sessionId + " " + sender);
                redisTemplate.opsForSet().add(onlineUsers, requestMessage.getSender());
                redisTemplate.convertAndSend(userStatus, JsonUtil.parseObjToJson(requestMessage));
            }
            ResponseMessage responseMessage = new ResponseMessage(sender, type, content);
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

    @MessageMapping("/room.addUser")
    public void addUser(@Payload RequestMessage requestMessage) {
        try {
            //如果找不到该room 创建对应room的playdesk
            String sender = requestMessage.getSender();
            String room = requestMessage.getRoom();
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.setSender("Server");
            if (roomlist.containsKey(room)) {
                PlayDesk current = roomlist.get(room);
                List<Player> userlistTemp = current.getPlayerslist();
                if (userlist.size() < 4) {
                    Player temp = new Player();
                    temp.setUsername(sender);
                    userlistTemp.add(temp);
                    userlist.replace(sender, room);
                    responseMessage.setType("Accept-room.addUser");
                    responseMessage.setContent(sender + " join " + room);
                    messagingTemplate.convertAndSendToUser(sender, "/chat", responseMessage);
                } else {
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
                responseMessage.setType("Accept-room.addUser");
                responseMessage.setContent(sender + " join " + room);
                messagingTemplate.convertAndSendToUser(sender, "/chat", responseMessage);
            }

            //如果找到 更新对应room的ready情况 sender path
        } catch (Exception e) {
            LOGGER.error(e.getMessage(), e);
        }

    }

    @MessageMapping("/room.deleteUser")
    public void deleteUser(@Payload RequestMessage requestMessage) {
        try {
            //得到sender和room 对应更新
            String sender = requestMessage.getSender();
            String room = requestMessage.getRoom();
            PlayDesk temp = roomlist.get(room);
            for(int i=0;i<temp.getPlayerslist().size();i++){
                if(temp.getPlayerslist().get(i).getUsername().equals(sender)){
                    temp.getPlayerslist().remove(i);
                    break;
                }
            }
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.setSender("Server");
            responseMessage.setType("Accept-deleteUser");
            responseMessage.setContent(sender + " left");
            messagingTemplate.convertAndSendToUser(sender, "/chat", responseMessage);
            if (temp.getPlayerslist().size() == 0) {
                roomlist.remove(room);
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

    @MessageMapping("/room.ready")
    public void roomReady(@Payload RequestMessage requestMessage) {

        //得到sender和room 更新对应room的playdesk的 判断ready

        String sender = requestMessage.getSender();
        String room = requestMessage.getRoom();
        PlayDesk temp = roomlist.get(room);
        String department = requestMessage.getContent();
        for (Player player : temp.getPlayerslist()) {
            if (sender.equals(player.getUsername())) {
                player.setReady(true);
                //player.setDepartment(department);
            }
        }

        //一旦ready 开始游戏 发报文 调用对应room的playdesk的initial() 发送消息(各自的手牌,当前玩家)
        boolean allReady = true;
        for (Player player : temp.getPlayerslist()
        ) {
            if (player.getReday()) {
                allReady = false;
            }
        }
        if (allReady) {
            messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "Game-Start", "all-ready"));
            temp.initial();
            for (Player player : temp.getPlayerslist()) {
                ResponseMessage responseMessage = new ResponseMessage();
                responseMessage.setSender("Server");
                responseMessage.setType("PlayerTiles");
                String content = "";
                for (Tile tile : player.getPlayerTiles()) {
                    content = content + tile.getId() + ",";
                }
                responseMessage.setContent(content);
                messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                responseMessage.setType("DarkTiles");
                responseMessage.setContent("" + player.getDarkTiles().get(0));
                messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
            }
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.setSender("Server");
            responseMessage.setType("CurrentPlayer");
            responseMessage.setContent(temp.getCurrentPlayer());
            messagingTemplate.convertAndSend("/topic/" + room, responseMessage);
            responseMessage.setType("PlayerDraw");
            responseMessage.setContent("" + temp.playerDraw(temp.getCurrentPlayer()));
            messagingTemplate.convertAndSendToUser(temp.getCurrentPlayer(), "/chat", responseMessage);
        }

    }

    @MessageMapping("/room.roundOperation")
    public void roomOperation(@Payload RequestMessage requestMessage) {

        //得到sender type content(牌) room 拿出对应的playdesk
        String sender = requestMessage.getSender();
        String room = requestMessage.getRoom();
        PlayDesk temp = roomlist.get(room);
        int tile = Integer.parseInt(requestMessage.getContent());
        if (sender.equals(temp.getCurrentPlayer())) {
            temp.setRoundNum(temp.getRoundNum() + 1);

            ResponseMessage responseMessage = new ResponseMessage();
            temp.getRoundOperationResponseList().clear();
            responseMessage.setType("Accept-play");
            for (Player player : temp.getPlayerslist()) {
                if (player.getUsername().equals(sender)) {
                    player.getPlayerTiles().remove(new Tile(tile));
                    responseMessage.setSender("Server");
                    messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                } else {
                    responseMessage.setSender(sender);
                    responseMessage.setContent("" + tile);
                    messagingTemplate.convertAndSendToUser(player.getUsername(), "/chat", responseMessage);
                }
            }
        } else {
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.setSender("Server");
            responseMessage.setType("Reject-play");
            responseMessage.setContent("不是你的出牌时间");
            messagingTemplate.convertAndSendToUser(sender, "/chat", responseMessage);
        }
        //判断是否合法 更新playdesk 判断胡牌 发送更新 回合数判断
    }

    @MessageMapping("/room.roundOperationResponse")
    public void roundOperationResponse(RequestMessage requestMessage) {
        String sender = requestMessage.getSender();
        String room = requestMessage.getRoom();
        String type = requestMessage.getType();
        String content = requestMessage.getContent();
        PlayDesk temp = roomlist.get(room);
        if (!sender.equals(temp.getCurrentPlayer())) {
            temp.getRoundOperationResponseList().add(requestMessage);
            if (temp.getRoundOperationResponseList().size() == 1) {
                for(Player player:temp.getPlayerslist()){
                    if(sender.equals(player.getUsername())){
                        String[] tiles = content.split(" ");
                        switch (type) {
                            case "eat":
                                player.eat(tiles,temp.getPlayTile());
                                messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,type,content+temp.getPlayTile()));
                            case "touch":
                                player.touch(tiles,temp.getPlayTile());
                                messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,type,content+temp.getPlayTile()));
                            case "rod":
                                player.rod(tiles,temp.getPlayTile());
                                messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,type,content+temp.getPlayTile()));
                            case "selfRod":
                                player.selfRod(tiles,temp.getPlayTile());
                                messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,type,content+temp.getPlayTile()));
                            case "darkRod":
                                player.darkRod(tiles,temp.getPlayTile());
                                messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,type,content+temp.getPlayTile()));
                            case "exchange":
                                player.exchange(tiles,Integer.parseInt(tiles[1]));
                                messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,type,content+temp.getPlayTile()));
                        }
                        break;
                    }
                }
                messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage(sender, "Accept-play", type));
                temp.setCurrentPlayer(sender);
                messagingTemplate.convertAndSend("/topic/" + room, new ResponseMessage("Server", "CurrentPlayer", temp.getCurrentPlayer()));
                messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage("Server", "PlayerDraw", "" + temp.playerDraw(temp.getCurrentPlayer())));
            } else {

                messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage("Server", "Reject-playResponse", "u are late"));
            }
        } else {

            messagingTemplate.convertAndSendToUser(sender, "/chat", new ResponseMessage("Server", "Reject-playResponse", ""));
        }

    }
    @MessageMapping("/room.win")
    public void win(RequestMessage requestMessage){
        String sender = requestMessage.getSender();
        String room = requestMessage.getRoom();
        messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,"win",""));
        PlayDesk temp = roomlist.get(room);
        String content = "";
        for(Player player:temp.getPlayerslist()){
            if(player.getUsername().equals(sender)){
                for(Tile t:player.getPlayerTiles()){
                    content=content+t.toString()+",";
                }
            }
        }
        messagingTemplate.convertAndSend("/topic/"+room,new ResponseMessage(sender,"winnerTiles",content));
    }

    @EventListener
    public void handleWebSocketDisconnectListener(SessionDisconnectEvent event) {

        StompHeaderAccessor headerAccessor = StompHeaderAccessor.wrap(event.getMessage());

        String username = (String) headerAccessor.getSessionAttributes().get("username");
        String sessionid = headerAccessor.getSessionId();

        if (username != null) {
            LOGGER.info("User Disconnected : " + sessionid + " " + username);
            userlist.remove(username);
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.setType(ChatMessage.MessageType.LEAVE);
            chatMessage.setSender(username);
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
