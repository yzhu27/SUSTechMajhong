# README

是 2019-2020 FALL OOAD 课程的小组 Project 项目。

### SUSTechMajhong

## Unity 客户端

### Unity Version

Unity 2019.1.9f1 或更新的版本，**推荐 Unity 2019.2.9f1**

### File structure

- SUSTECH_Mahjang_Unity_Project : Unity文件根目录

  - Asserts : 资源目录

    - Scenes : 场景文件

    - Scripts : 源码

      - GameMain : 主体游戏内容

      - GameUI : 游戏UI

      - User : 用户

      - Util

      - 脚本文件直接存放在Scripts目录下，命名规则

        GameObject[_Action].cs

        GameObject : 游戏物体名

        [Action] : 物体行为名

    - Resources : 原始资源

    - Prefabs : 资源预设

  - Packages : Unity引擎库文件

  - ProjectSettings : Unity客户端设置
  
  - TEST

### Code style

- 建议使用IDE [Visual Studio 2019](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=16)
- 每个文件存放一个类，只在一个类中用到的枚举类除外
- 除脚本外，类需要放在与路径一致的命名空间(name space)中
- 命名规则
  - ClassName : 驼峰命名，首字母大写 (脚本命名参考文件名即可)
  - attributeName : 驼峰命名，首字母小写
  - FunctionName : 驼峰命名，首字母大写
- 注释规范
  - **类**，**方法**，和**属性**需要使用文档注释，参考[Microsoft Docs](https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/language-specification/documentation-comments)
  - **变量**需注明含义

- 脚本规范
  - 所有操作尽量使用Unity自带的方法，禁止在脚本内写复杂的函数，如有必要，放在*Scripts/Util*目录下
  - Public attribute 放在类的开头定义初始值，禁止在`OnStart()`方法中对公有属性赋值

---

### 项目答辩总结(19.12.25)

Recorded by Andy

## 流程

2019.12.25  7:30 -> 2019.12.26 10:20 !

- 做完未完成的功能
  - 胡牌 √
  - 美工素材更新 √
  - 四人联机测试时出现的一些显示bug √
  - 背景音乐 音效 √
  - 打包成.exe √
  - 前端增加房间列表与后端对接 ×
- 完善目前的功能 √
- 测试 √
- 总结已经做完的功能，未完成的功能 √
- ppt × 和视频  √


## 内容

#### 仓库地址

https://github.com/Tchao111/SUSTechMajhong

#### 后端

开发环境：java 8，redis 3.2.100

框架：Spring boot

功能：麻将游戏联机服务端，麻将游戏单人测试服务端，聊天室服务端（目前是分离的，但用户名和房间等信息是共享的）

- 私人消息频道，公共消息频道，房间消息频道 √ 用户名与websocket连接绑定
- 聊天室网页 √ 公屏 私人 房间消息 √
- 前后端交互echo频道 √ 消息收发 √
- 比较详细的日志 console √ file ×
- websocket连接/断开监听 √ 频道订阅/取订监听 √
- 在线用户列表 √ 在线房间列表 √ 
- 用户登录检验 √ 房间进入检验 √
- 手牌生成 √ 牌堆生成 √ 
- 单人简单测试 
  - 初始化牌堆 √ 发牌 √ 抽牌 √  
  - 出牌 √ 响应 √ 
  - 胡牌 √
- 四人联机测试 
  - 加入房间 √ 房内玩家列表 √ 房内已准备玩家列表 √
  - 房间准备 √ 游戏场景加载确认 √ 玩家初始抽牌确认 √
  - 牌堆 √ 发牌 √ 抽牌 √
  - 出牌前 换牌 √ 自杠√
  - 出牌 √ 响应 吃 √ 碰 √ 杠 √ 响应定时器 √ 回合数 √
  - 胡牌 √
  - 退出房间 √
- 高并发测试 × 打包成jar × 公网ip ×

#### 前端

开发环境：unity 2019.2.17f1

功能：登录界面，房间列表界面（未完成），房间界面，麻将游戏界面

- 用户登录 √
- 手写C# STOMP库 √ 报文收发 √ 场景切换 √ 物体更新 √
- 房间列表 ×
- 房间 背景图 √ 用户名 √ 准备情况 √
- 单人测试
  - 抽牌 √ 出牌 √ 响应 √ 胡牌 √ 跳过机器人 √
- 游戏素材、界面和功能
  - 背景√ 牌桌 √ 牌堆 √ 手牌 √ 暗牌 √
  - 手牌区 √ 暗牌区 √ 明牌区 √ 出牌区 √ 操作栏 √
  - 牌堆动画 √ 抽牌动画 √ 出牌动画 √ 响应动画 √ 换牌动画 √
  - 手牌排序 √ 鼠标选取 √ 
  - 出牌定时器 √ 随机出牌 √ 摄像头调整 √
  - 响应提示 √  操作禁用 √  高亮显示 √ 吃 √ 碰 √ 杠 √ 胡 √ 过 ×
  - 背景音乐 √ 音效 √
  - 胡牌显示 √

#### 其他

前后端交互网络协议：websocket + STOMP

交互流程：前端 <- 前端网络类 -> 后端

南科大特色：牌面 技能

主要设计模式：订阅/发布模式（观察者模式）、工厂模式、状态模式

待完成内容：前端房间列表页面 角色技能

-----

### 使用说明

请按上述的后端环境(开发环境：java 8，redis 3.2.100 框架：Spring boot)配置服务器,

并更改客户端Scripts 目录下WebController.cs 内的服务器ip,运行Unity(2019.2.17f1).

```c#
public class WebController : MonoBehaviour
{
	public readonly Web w = new Web(new System.Uri("ws://服务器ip:端口/ws/websocket"), AutoCallBacks.AutoCallBackDict);

	...
```

**Let's enjoy Majhong!!!**(这是联机麻将来着...请自行凑齐4台客户端小伙伴)
