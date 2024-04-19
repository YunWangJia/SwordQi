# SwordQi  森林剑气mod

mod基于ModAPI制作,[ModAPI网站](https://modapi.survivetheforest.net/)

如需使用项目需将项目文件放到：ModAPI根目录\projects\TheForest\目录下， 即可在ModAPI开发者页面看到mod相关设置

采用Unity的热更新往游戏里增加额外资源，网络同步参照了

[ChampionsOfForest](https://modapi.survivetheforest.net/mod/101/champions-of-the-forest)（森林冠军）这个mod

资源文件的Unity版本为5.6.5

## 该分支的注意事项

- 此分支主要用于Up自己整活使用，所以资源加载都采用了本地加载的形式，引用文件也发生了变化不适用于另一分支。

### 1.5.7.90更新内容

- 尝试添加对象池，失败！暂时保留了代码。

### 1.5.7.56更新内容

- 整合了部分没必要分开处理的类，加快代码运行。

- 将一些方法从主逻辑类里分离了出来，为后续添加东西做铺垫。

- 更改了“鲨鱼”技能的表现形式，使技能与剑气表现形式不再那么单调。
