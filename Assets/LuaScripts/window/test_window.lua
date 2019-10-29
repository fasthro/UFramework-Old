--[[
 * @Author: fasthro
 * @Description: 测试 Window
 ]]
local window = window_class(FLayer.Window, "test", "test")

window.layer = FLayer.Window
window.com_name = "test"
window.pack_name = "test"
window.depend_pack_names = nil
window.enabled_log = true
window.log_mark = "test"

function window:ctor()
    self:Log("ctor")
end

function window:OnInit()
    self:Log("OnInit")
end

function window:OnShown()
    self:Log("OnShown")
end

function window:OnRefresh()
    self:Log("OnRefresh")
end

function window:OnHide()
    self:Log("OnHide")
end

function window:OnDestory()
    self:Log("OnDestory")
end

return window
