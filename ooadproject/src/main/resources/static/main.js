'use strict';

var usernamePage = document.querySelector('#username-page');
var chatPage = document.querySelector('#chat-page');

var usernameForm = document.querySelector('#usernameForm');

var messageForm = document.querySelector('#messageForm');
var messageInput = document.querySelector('#message');
var messageArea = document.querySelector('#messageArea');

var privateForm = document.querySelector('#privateForm');
var privateInput = document.querySelector('#privatemessage');
var privateArea = document.querySelector('#privateArea');

var connectingElement = document.querySelector('.connecting');

var RoomNameForm = document.querySelector('#RoomNameForm');
var RoomMessageArea = document.querySelector('#RoomMessageArea')
var RoomMessageInput = document.querySelector('#RoomMessage');
var RoomMessageForm = document.querySelector('#RoomMessageForm')

var disconnect = document.querySelector('#Disconnect')

var stompClient = null;
var username = null;
var RoomName =null;
var RoomSubscription =null;

var colors = [
    '#2196F3', '#32c787', '#00BCD4', '#ff5652',
    '#ffc107', '#ff85af', '#FF9800', '#39bbb0'
];


function connect(event) {
    username = document.querySelector('#name').value.trim();

    if(username) {
        var socket = new SockJS('/ws');
        stompClient = Stomp.over(socket);
        stompClient.connect({}, onConnected, onError);
    }else{
        alert("input your username")
    }
    event.preventDefault();
}


function onConnected() {
    stompClient.subscribe('/user/'+username+'/greetings',loginMessage);
    stompClient.send("/app/hello/"+username,
        {},
        JSON.stringify({sender: username, type: 'NAME_TEST',content:''})
    )

}
function test() {


}
function getPlayer(){
    stompClient.send("/app/room.getPlayer",
        {},
        JSON.stringify({sender: username, type:"getPlayer",content:'',room:RoomName})
    );

}
function loginMessage(payload){
    var message = JSON.parse(payload.body);
    if(message.type==="Accept"){
        usernamePage.classList.add('hidden');
        chatPage.classList.remove('hidden');
        //公共频道
        stompClient.subscribe('/topic/public', onMessageReceived);
        stompClient.send("/app/public.sendMessage",
            {},
            JSON.stringify({sender: username, type: 'JOIN',content:'join in public'})
        )
        //私人频道
        stompClient.subscribe('/user/'+username+'/chat',onPrivateMessageReceived);
        stompClient.send("/app/private.sendMessage/"+username,
            {},
            JSON.stringify({sender: username, type: 'JOIN',content:'join in public'})
        )
        connectingElement.classList.add('hidden');
    }else {
        alert("user name already exist")
    }
}

function onError(error) {
    connectingElement.textContent = 'Could not connect to WebSocket server. Please refresh this page to try again!';
    connectingElement.style.color = 'red';
}

function RoomConnect(event) {
    RoomName = document.querySelector('#RoomName').value.trim();
    if(RoomName && stompClient){
        RoomSubscription=stompClient.subscribe('/topic/'+RoomName,onRoomMessageReceived);
        stompClient.send("/app/room.addUser",
            {},
            JSON.stringify({sender: username,type: 'JOIN',content:'join in room ',room:RoomName})
        )
        RoomNameForm.classList.add('hidden');
        disconnect.classList.remove('hidden');
    }
    event.preventDefault();
}
function RoomDisconnect() {
    if(RoomSubscription && stompClient){
        RoomSubscription.unsubscribe();
        stompClient.send("/app/public.sendMessage",
            {},
            JSON.stringify({sender: username, type: 'LEAVE',content:'leave from room'+RoomName})
        )
        RoomName=null;
        RoomNameForm.classList.remove('hidden');
        disconnect.classList.add('hidden');
    }

}
function sendMessage(event) {
    console.log("sendMessage method")
    var messageContent = messageInput.value.trim();
    if(messageContent && stompClient) {
        var chatMessage = {
            sender: username,
            content: messageInput.value,
            type: 'CHAT'
        };
        stompClient.send("/app/public.sendMessage", {}, JSON.stringify(chatMessage));
        messageInput.value = '';
    }
    event.preventDefault();
}
function sendPrivateMessage(event) {
    var messageContent = privateInput.value.trim();
    if(messageContent && stompClient) {
        var chatMessage = {
            sender: username,
            content: privateInput.value,
            type: 'CHAT'
        };
        stompClient.send("/app/private.sendMessage/"+username, {}, JSON.stringify(chatMessage));
        privateInput.value = '';
    }
    event.preventDefault();
}
function sendRoomMessage(event) {
    var messageContent = RoomMessageInput.value.trim();
    if(messageContent && stompClient) {
        var chatMessage = {
            sender: username,
            room: RoomName,
            content: RoomMessageInput.value,
            type: 'CHAT'
        };
        stompClient.send("/app/room.sendMessage", {}, JSON.stringify(chatMessage));
        RoomMessageInput.value = '';
    }
    event.preventDefault();
}
function onPrivateMessageReceived(payload) {
    var message = JSON.parse(payload.body);

    var messageElement = document.createElement('li');

    if(message.type === 'JOIN') {
        messageElement.classList.add('event-message');
        message.content = message.sender + ' joined!';
    } else if (message.type === 'LEAVE') {
        messageElement.classList.add('event-message');
        message.content = message.sender + ' left!';
    } else {
        messageElement.classList.add('chat-message');

        var avatarElement = document.createElement('i');
        var avatarText = document.createTextNode(message.sender[0]);
        avatarElement.appendChild(avatarText);
        avatarElement.style['background-color'] = getAvatarColor(message.sender);

        messageElement.appendChild(avatarElement);

        var usernameElement = document.createElement('span');
        var usernameText = document.createTextNode(message.sender);
        usernameElement.appendChild(usernameText);
        messageElement.appendChild(usernameElement);
    }

    var textElement = document.createElement('p');
    var messageText = document.createTextNode(message.content);
    textElement.appendChild(messageText);

    messageElement.appendChild(textElement);

    privateArea.appendChild(messageElement);
    privateArea.scrollTop = privateArea.scrollHeight;
}
function onMessageReceived(payload) {
    var message = JSON.parse(payload.body);

    var messageElement = document.createElement('li');

    if(message.type === 'JOIN') {
        messageElement.classList.add('event-message');
        message.content = message.sender + ' joined!';
    } else if (message.type === 'LEAVE') {
        messageElement.classList.add('event-message');
        message.content = message.sender + ' left!';
    } else {
        messageElement.classList.add('chat-message');

        var avatarElement = document.createElement('i');
        var avatarText = document.createTextNode(message.sender[0]);
        avatarElement.appendChild(avatarText);
        avatarElement.style['background-color'] = getAvatarColor(message.sender);

        messageElement.appendChild(avatarElement);

        var usernameElement = document.createElement('span');
        var usernameText = document.createTextNode(message.sender);
        usernameElement.appendChild(usernameText);
        messageElement.appendChild(usernameElement);
    }

    var textElement = document.createElement('p');
    var messageText = document.createTextNode(message.content);
    textElement.appendChild(messageText);

    messageElement.appendChild(textElement);

    messageArea.appendChild(messageElement);
    messageArea.scrollTop = messageArea.scrollHeight;
}

function onRoomMessageReceived(payload) {
    var message = JSON.parse(payload.body);

    var messageElement = document.createElement('li');

    if(message.type === 'JOIN') {
        messageElement.classList.add('event-message');
        message.content = message.sender +' '+message.content;
    } else if (message.type === 'LEAVE') {
        messageElement.classList.add('event-message');
        message.content = message.sender +' '+ message.content ;
    } else {
        messageElement.classList.add('chat-message');

        var avatarElement = document.createElement('i');
        var avatarText = document.createTextNode(message.sender[0]);
        avatarElement.appendChild(avatarText);
        avatarElement.style['background-color'] = getAvatarColor(message.sender);

        messageElement.appendChild(avatarElement);

        var usernameElement = document.createElement('span');
        var usernameText = document.createTextNode(message.sender);
        usernameElement.appendChild(usernameText);
        messageElement.appendChild(usernameElement);
    }

    var textElement = document.createElement('p');
    var messageText = document.createTextNode(message.content);
    textElement.appendChild(messageText);

    messageElement.appendChild(textElement);

    RoomMessageArea.appendChild(messageElement);
    RoomMessageArea.scrollTop = RoomMessageArea.scrollHeight;
}



function getAvatarColor(messageSender) {
    var hash = 0;
    for (var i = 0; i < messageSender.length; i++) {
        hash = 31 * hash + messageSender.charCodeAt(i);
    }
    var index = Math.abs(hash % colors.length);
    return colors[index];
}


function sleep(numberMillis) {
    var now = new Date();
    var exitTime = now.getTime() + numberMillis;
    while (true) {
        now = new Date();
        if (now.getTime() > exitTime)
            return;
    }
}
privateForm.addEventListener('submit',sendPrivateMessage,true)
usernameForm.addEventListener('submit', connect, true)
RoomNameForm.addEventListener('submit',RoomConnect,true)
RoomMessageForm.addEventListener('submit', sendRoomMessage, true)
messageForm.addEventListener('submit', sendMessage, true)