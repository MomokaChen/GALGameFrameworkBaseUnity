# GALCoreFramework

一个基于Unity的游戏核心框架，提供了Excel驱动的对话系统和完整的游戏开发基础架构。

## 🌟 功能特点

### 核心功能
- **Excel驱动对话系统** - 使用Excel表格定义对话内容，支持多角色对话、分支选项和场景切换
- **场景管理系统** - 提供场景切换和对话系统加载功能
- **数据管理系统** - 统一的数据加载和访问接口
- **UI系统** - 基于Unity UI的对话界面和选项界面

### 技术特性
- 使用DOTween进行流畅的动画效果
- 支持TextMesh Pro进行高质量文本渲染
- 单例模式管理全局资源和状态
- 异步场景加载

## 📊 Excel驱动对话功能

### 功能介绍
项目使用Excel表格来定义对话内容，通过`excel2json`工具自动转换为游戏可用的JSON数据。

### 使用方法

#### 1. 准备Excel文件
在`src/Data/Tables/`目录下创建Excel表格（如`GalTest.xlsx`），包含以下字段：
- `id` - 对话唯一标识
- `branchid` - 分支ID，用于区分不同的对话分支
- `speaker` - 说话者名称
- `content` - 对话内容
- `leftcharurl` - 左侧角色图片路径
- `midcharurl` - 中间角色图片路径
- `rightcharurl` - 右侧角色图片路径
- `bgurl` - 背景图片路径（暂时无用）
- `bgmurl` - BGM音乐路径（暂时无用）
- `islast` - 是否为该分支的最后一句（1表示是，0表示否）
- `nextbranchid` - 下一个分支ID
- `option1`~`option4` - 对话选项文本
- `option1next`~`option4next` - 对应选项跳转的分支ID

#### 2. 转换Excel为JSON
运行转换脚本：
```bash
cd src/Data
excel2json.cmd
```
或直接运行Python脚本：
```bash
python excel2json/excel2json.py
```

#### 3. 在游戏中使用
对话系统会自动加载转换后的JSON数据，并通过`DialogueManager`进行管理。

```csharp
// 启动对话示例（指定分支ID和节点ID）
DialogueManager.Instance.StartDialogue(1, 1);
```

## 📁 项目结构

```
GALCoreFramework/
├── src/
│   ├── Data/                      # 数据相关文件
│   │   ├── Data/                  # 转换后的数据文件
│   │   ├── Tables/                # Excel表格文件
│   │   ├── excel2json/            # Excel转JSON工具
│   │   └── excel2json.cmd         # 转换脚本
│   └── GALCoreFramework/          # Unity项目
│       ├── Assets/
│       │   ├── Plugins/           # 第三方插件（DOTween、JsonDotNet等）
│       │   ├── Res/               # 资源文件
│       │   ├── Resources/         # Unity资源文件夹
│       │   ├── Scenes/            # 场景文件
│       │   └── Script/            # 脚本文件
│       │       ├── Data/          # 数据管理
│       │       ├── Dialogue/      # 对话系统
│       │       ├── Manager/       # 管理器
│       │       └── Utilities/     # 工具类
├── .gitignore                     # Git忽略文件配置
└── README.md                      # 项目说明文档
```

## 🚀 安装和设置

### 环境要求
- Unity 2020.3+
- Python 3.x（用于Excel转换工具）

### 安装步骤
1. 克隆仓库：
   ```bash
   git clone <repository-url>
   cd GALCoreFramework
   ```

2. 在Unity中打开项目：
   - 选择`src/GALCoreFramework/`作为项目文件夹

3. 安装依赖：
   - DOTween（已包含在项目中）
   - Newtonsoft.Json（已包含在项目中）

## 🎮 使用指南

### 对话系统使用

#### 基本对话流程
1. 在Excel表格中定义对话内容
2. 运行转换脚本生成JSON数据
3. 进入Main Scenes查看演示

#### 对话表格示例
| id | branchid | speaker | content | leftcharurl | midcharurl | rightcharurl | bgurl | bgmurl | islast | nextbranchid | option1 | option1next | option2 | option2next |
|----|----------|---------|---------|-------------|-------------|--------------|-------|--------|--------|-------------|---------|-------------|---------|-------------|
| 1 | 1 | 宝宝 | 你好呀 | null | null | character/char1 | bg/bg | null | 0 | 0 | null | 0 | null | 0 |
| 2 | 1 | null | 这是宝宝 | null | null | character/char1 | null | null | 0 | 0 | null | 0 | null | 0 |
| 3 | 1 | player_name | 你喜不喜欢我呀 | character/char1 | null | null | null | null | 1 | 0 | 喜欢 | 2 | 不喜欢 | 3 |

## 🔧 开发指南

### 添加新功能
1. 在`src/GALCoreFramework/Assets/Script/`目录下创建新的脚本
2. 遵循现有的代码风格和命名规范
3. 更新相关的管理器和接口

### 修改对话系统
- 编辑`DialogueManager.cs`和相关脚本
- 根据需要扩展对话表格的字段

## 🤝 贡献

欢迎提交Issue和Pull Request来改进这个项目！

## 📄 许可证

MIT License

---

**注意**: 这是一个正在开发中的项目，功能和文档可能会不断更新。
