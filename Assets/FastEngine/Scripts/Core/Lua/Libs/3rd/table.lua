--[[
	* @Author: fasthro
	* @Date: 2020-01-07 10:42:22
	* @Description: table 扩展
]]
--[[
    @desc: 查询
    author: fasthro
    time:2020-01-07 10:42:22
    --@t: table
	--@k: key
    @return: value
]]
function table.find_key(t, k)
    if t == nil then
        return nil
    end
    for key, value in pairs(t) do
        if k == key then
            return value
        end
    end
    return nil
end

--[[
    @desc: 移除
    author: fasthro
    time:2020-01-07 10:45:29
    --@t: table
	--@k: key
    @return: new table, remove value
]]
function table.remove_key(t, k)
    if t == nil then
        return nil
    end
    local v = nil
    local tb = {}
    for key, value in pairs(t) do
        if k ~= key then
            tb[tostring(key)] = value
        else
            v = value
        end
    end
    return tb, v
end

--[[
    @desc: 翻转
    author: fasthro
    time:2020-04-28 20:32:58
    --@t: table
    @return:
]]
function table.reverse(t)
    local tb = {}
    for i = 1, #t do
        local key = #t + 1 - i
        tb[i] = t[key]
    end
    return tb
end
