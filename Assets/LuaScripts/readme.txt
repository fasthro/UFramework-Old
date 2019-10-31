-----------------------------------------------------------------------------------------------------------------
- 代码规范
--- 文件名: 全小写，用下划线分割
--- 文件头: 作者描述必填
    --[[
    * @Author: fasthro
    * @Description: test
    ]]
--- 变量名字: 全小写,下划线连接(例:test_read)
--- 方法名: 全小写,下划线连接(例:function test_func() end)
--- 脚本:
        local test = {
            a = 1,
        }
        functon test.func_add() end
        return test

-----------------------------------------------------------------------------------------------------------------
- winmgr
--- winmgr.open_window(window_name)                -- 打开窗口
--- winmgr.hide_window(window_name)                -- 关闭窗口
--- winmgr.destory_window(window_name)             -- 销毁窗口
--- winmgr.refresh_window(window_name)             -- 刷新窗口
--- winmgr.refresh_top_window()                    -- 刷新当前窗口
--- winmgr.call_window(window_name, func_name, args)     -- 调用窗口方法
--- winmgr.get_window(window_name)                 -- 获取window实例，没有返回nil

-----------------------------------------------------------------------------------------------------------------
- window
-- log
--- self:Log(string)
--- self:LogError(string)
--- self:LogWarning(string)

-- windon 对应自身组件 self.ui
-- window 事件自动绑定，只需要在window里定义事件回调即可
    例如: btn_test 的事件
    function window:btn_test_onclick(context)
    end

-----------------------------------------------------------------------------------------------------------------
- fairyGUI editor export lua plugin
-- 编辑器自定义属性
--- gen_lua: 生成lua代码 [true]
--- gen_lua_path: 生成lua代码完整路径
--- gen_lua_spas: 组件精准类型导出(例如:com.asButton) [true]
--- gen_define_path: require define 基础路径(例如:fairy, require("fairy/..."))
--- gen_lua_prefix: 导出文件名前缀标识(可忽略此项)
--- gen_lua_window: 是否导出window代码
--- gen_lua_window_path: window代码导出路径

-- 名称前缀定义 (其他的可不写,主要用于自动导出代码，作为标识，准确锁定组件类型，如果没有定义前缀，会自动类型为GComponent)
--- button: btn_
--- comboBox: cb_
--- label: lab_
--- progress: pb_
--- slider: sl_
--- ignore: ig_

-----------------------------------------------------------------------------------------------------------------
- fairyGUI 制作规范
--- 包名全小写下划线分割
--- 组件名，资源名，文件夹名...一切小写下划线分割
--- 包结构
    package
        component: 组件
        design: 设计图
        image: 资源图片
        window窗口组建(例如:name_window)
--- textField等其他组件恢复初始状态