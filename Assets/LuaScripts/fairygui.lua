--[[
 * @Author: fasthro
 * @Description: fairyGui Window
 ]]
FWindow = FastEngine.FUI.FWindow
FLayer = FastEngine.FUI.FLayer
FWindowState = FastEngine.FUI.FWindowState

function window_class()
    local o = {}

    local base = FWindow
    setmetatable(o, base)

    o.__index = o
    o.base = base

    -- 必须设置
    o.layer = FLayer.Window
    o.com_name = ""
    o.pack_name = ""
    o.depend_pack_names = nil
    o.enabled_log = true
    o.log_mark = ""

    o.New = function(...)
        local t = {}
        setmetatable(t, o)

        local ins = FWindow.New(o.layer, o.com_name, o.pack_name, o.depend_pack_names)
        ins.enabledLog = o.enabled_log
        ins.logMark = o.log_mark

        tolua.setpeer(ins, t)
        ins:SetLuaPeer(t)
        if t.ctor then
            t.ctor(ins, ...)
        end

        return ins
    end

    return o
end

function create_window(name)
    local window = require("Window/" .. name)
    return window.New()
end
