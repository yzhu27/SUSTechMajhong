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