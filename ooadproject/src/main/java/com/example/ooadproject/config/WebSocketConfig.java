package com.example.ooadproject.config;


import org.springframework.context.annotation.Configuration;
import org.springframework.messaging.simp.config.MessageBrokerRegistry;
import org.springframework.web.socket.config.annotation.*;


//通过EnableWebSocketMessageBroker 开启使用STOMP协议
//来传输基于代理的消息，此时浏览器支持使用@MessageMapping
//就像支持@RequestMapping一样
@Configuration
@EnableWebSocketMessageBroker
public class WebSocketConfig implements WebSocketMessageBrokerConfigurer {

    //注册STOMP端点，即WebSocket客户端需要连接到WebSocket握手端点

    @Override
    public void registerStompEndpoints(StompEndpointRegistry registry){
        //注册一个Stomp协议的端点，客户端在订阅或发布信息到目的地路径前，要连接该端点
        registry.addEndpoint("/ws")
                //允许跨域
                .setAllowedOrigins("*")
                //启用SockJS功能
                .withSockJS();

    }

    //配置消息代理(message broker)
    @Override
    public void configureMessageBroker(MessageBrokerRegistry registry){
        //所有目的地前缀为/topic，/queue的消息都会发送到STOMP代理中
        registry.enableSimpleBroker("/topic","/user");
        //设置应用程序的目的地前缀为/app,当有以应用程序为目的地的消息
        //会被直接路由到带有@MessageMapping注解的控制器方法
        registry.setApplicationDestinationPrefixes("/app");
        registry.setUserDestinationPrefix("/user");
    }


}
