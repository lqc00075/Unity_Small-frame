# Unity_Small-frame
包含了一些基本的Unity小框架 便于开发
主要的框架有 基于单例模式的基类 BaseManager 给后面的每个管理器用于继承
Mono 主要为没有继承Mono的类提供了帧更新以及协程的使用
EventCenterMgr 基于观察者模式 设计的事件中心
ResourcesManager 提供了同步加载 和 异步加载资源的方式 封装了一个回调函数
ScenesManager 提供了同步加载场景 和 异步加载场景的方式 同样有一个回调参数
MusicManager 对音效 音乐 的集中管理
UIManager 可以快速的获取指定名的组件 对button封装了一个监听事件 外部在获取到button时 可以不用自行添加监听事件
Pool 是一个简易版的对象池 提供了Pop 和 Push方法
