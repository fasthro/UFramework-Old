--[[
 * @Author: fasthro
 * @Description: fairyGui Window
 ]]
EventContext = FairyGUI.EventContext
EventListener = FairyGUI.EventListener
EventDispatcher = FairyGUI.EventDispatcher
InputEvent = FairyGUI.InputEvent
NTexture = FairyGUI.NTexture
Container = FairyGUI.Container
Image = FairyGUI.Image
Stage = FairyGUI.Stage
Controller = FairyGUI.Controller
GObject = FairyGUI.GObject
GGraph = FairyGUI.GGraph
GGroup = FairyGUI.GGroup
GImage = FairyGUI.GImage
GLoader = FairyGUI.GLoader
GMovieClip = FairyGUI.GMovieClip
TextFormat = FairyGUI.TextFormat
GTextField = FairyGUI.GTextField
GRichTextField = FairyGUI.GRichTextField
GTextInput = FairyGUI.GTextInput
GComponent = FairyGUI.GComponent
GList = FairyGUI.GList
GRoot = FairyGUI.GRoot
GLabel = FairyGUI.GLabel
GButton = FairyGUI.GButton
GComboBox = FairyGUI.GComboBox
GProgressBar = FairyGUI.GProgressBar
GSlider = FairyGUI.GSlider
PopupMenu = FairyGUI.PopupMenu
ScrollPane = FairyGUI.ScrollPane
Transition = FairyGUI.Transition
UIPackage = FairyGUI.UIPackage
Window = FairyGUI.Window
GObjectPool = FairyGUI.GObjectPool
Relations = FairyGUI.Relations
RelationType = FairyGUI.RelationType
UIPanel = FairyGUI.UIPanel
UIPainter = FairyGUI.UIPainter
TypingEffect = FairyGUI.TypingEffect
GTween = FairyGUI.GTween
GTweener = FairyGUI.GTweener
EaseType = FairyGUI.EaseType

FWindow = FastEngine.FUI.FWindow
FLayer = FastEngine.FUI.FLayer
FWindowState = FastEngine.FUI.FWindowState

fui = {}

function fui.window_class()
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

function fui.register_extension(url, extension)
    FairyGUI.UIObjectFactory.SetExtension(url, typeof(extension.base), extension.Extend)
end

function fui.extension_class(base)
    local o = {}
    o.ui = {}
    o.__index = o
        
    o.base = base or GComponent

    o.Extend = function(ins)
        local t = {}
        setmetatable(t, o)
        tolua.setpeer(ins, t)
        return t
    end

    return o
end

function fui.bind_click_event(btn, self, field, func)
    btn.self = self;
    btn.field = field;
    btn.onClick:Set(func)
end