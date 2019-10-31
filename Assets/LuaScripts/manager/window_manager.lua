--[[
 * @Author: fasthro
 * @Description: window manager
 ]]
local manager = {
    windows = {},
    window_order = {}
}

local create_window = function(name)
    local window = require("Window/" .. name)
    return window.New()
end

local insert_order = function(window_name)
    for k, v in ipairs(manager.window_order) do
        if v == window_name then
            table.remove(manager.window_order, k)
            break
        end
    end
    table.insert(manager.window_order, window_name)
end

local remove_order = function(window_name)
    for k, v in ipairs(manager.window_order) do
        if v == window_name then
            table.remove(manager.window_order, k)
            break
        end
    end
end

function manager.open_window(window_name)
    local window = manager.windows[window_name] or nil
    if window == nil or (window ~= nil and window.state == FWindowState.Destory) then
        window = create_window(window_name)
        manager.windows[window_name] = window
        insert_order(window_name)
    end
    window:ShowWindow()
end

function manager.hide_window(window_name)
    local window = manager.windows[window_name] or nil
    if window ~= nil and window.state == FWindowState.Showing then
        window:HideWindow(false)
        remove_order(window_name)
        manager.refresh_top_window()
    end
end

function manager.destory_window(window_name)
    local window = manager.windows[window_name] or nil
    if window ~= nil and window.state ~= FWindowState.Destory then
        window:HideWindow(true)
        manager.windows[window_name] = nil
        manager.refresh_top_window()
    end
end

function manager.refresh_window(window_name)
    local window = manager.windows[window_name] or nil
    if window ~= nil and window.state == FWindowState.Showing then
        window:RefreshWindow()
    end
end

function manager.refresh_top_window()
    if next(manager.window_order) ~= nil then
        manager.window_order[#manager.window_order]:RefreshWindow()
    end
end

function manager.call_window(window_name, func_name, args)
    local window = manager.windows[window_name] or nil
    if window ~= nil and window.state == FWindowState.Showing then
        local fun =  window[func_name]
        if fun ~= nil then
            fun(window, args)
        end
    end
end

function manager.get_window(window_name)
    return manager.windows[window_name]
end

return manager
