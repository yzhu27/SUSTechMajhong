package com.example.ooadproject.bean;

public class RequestMessage {

    private String sender;//消息发送者
    private String room;//房间号
    private String type;//消息类型
    private String content;//消息内容

    public RequestMessage() {
    }

    public RequestMessage(String sender, String room, String type, String content) {
        this.sender = sender;
        this.room = room;
        this.type = type;
        this.content = content;
    }


    public String getSender() {
        return sender;
    }

    public String getRoom() {
        return room;
    }

    public String getType() {
        return type;
    }

    public String getContent() {
        return content;
    }


    public void setSender(String sender) {
        this.sender = sender;
    }

    public void setReceiver(String room) {
        this.room = room;
    }

    public void setType(String type) {
        this.type = type;
    }

    public void setContent(String content) {
        this.content = content;
    }
}