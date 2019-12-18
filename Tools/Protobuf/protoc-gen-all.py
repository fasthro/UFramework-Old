# !/usr/bin/python
# -*- coding:utf-8 -*-

import os
import sys
import shutil
import shlex
import datetime
import subprocess
import time
import re

# protoc 路径
PROTOC_PATH = os.getcwd()
# proto 路径
PROTO_PATH = ""
# proto lua 临时路径
PROTO_LUA_TEMP_PATH = ""
# proto c# 导出路径
PROTO_GEN_CS_PATH = ""
# proto lua 导出路径
PROTO_GEN_LUA_PATH = ""


def clear_dir(dir_path, create_new=True):
    """
    清理目录
    :param dir_path: 目录路径
    :param create_new: 是否创建文件夹
    :return:
    """
    if os.path.exists(dir_path):
        shutil.rmtree(dir_path)
    if create_new:
        os.makedirs(dir_path)


def execute_command(cmd, cwd=None, timeout=None, shell=False):
    """
    执行shell命令
    :param cmd: 需要执行的命令
    :param cwd: 更改路径
    :param timeout: 超时时间
    :param shell: 是否通过shell运行
    :return: return code
    :raises Exception: 执行超时
    """
    if shell:
        cmd_string_list = cmd
    else:
        cmd_string_list = shlex.split(cmd)

    end_time = 0
    if timeout:
        end_time = datetime.datetime.now() + datetime.timedelta(seconds=timeout)

    sub = subprocess.Popen(cmd_string_list, cwd=cwd, stdin=subprocess.PIPE, shell=shell, bufsize=4096)
    while sub.poll() is None:
        time.sleep(0.1)
        if timeout:
            if end_time <= datetime.datetime.now():
                raise Exception('Timeout: {}'.format(cmd))

    return sub.returncode


# 获取目录内的文件路径列表
def get_path_files(dir_path, pattern=""):
    results = []
    for root, dirs, files in os.walk(dir_path):
        for fn in files:
            fp = os.path.join(root, fn)
            if pattern:
                if fp.endswith(pattern):
                    results.append(fp)
            else:
                results.append(fp)
    return results


# 构建 lua proto file
def build_lua_proto(proto, tp):
    # 过滤掉命名空间
    contexts = []
    with open(proto, "r") as f:
        for line in f.readlines():
            if not re.search(r'package\b(.*?);$', line):
                contexts.append(line)

    # 重新写入到临时目录
    (fp, fn) = os.path.split(proto)
    lua_proto = os.path.join(tp, fn)
    with open(lua_proto, "w") as f:
        f.writelines(contexts)


# 生成c#
def gen_cs():
    proto_files = get_path_files(PROTO_PATH, ".proto")
    for proto in proto_files:
        proto_path = "--proto_path=" + PROTO_PATH
        csharp_out = "--csharp_out=" + PROTO_GEN_CS_PATH
        execute_command(["protoc", proto_path, csharp_out, proto], cwd=PROTOC_PATH, shell=True)


# 生成lua
def gen_lua():
    # 去掉命名空间, 拷贝proto文件到临时目录
    proto_files = get_path_files(PROTO_PATH, ".proto")
    for proto in proto_files:
        build_lua_proto(proto, PROTO_LUA_TEMP_PATH)
    execute_command(["protoc-gen-lua.bat", PROTO_LUA_TEMP_PATH, PROTO_GEN_LUA_PATH], cwd=PROTOC_PATH, shell=True)

    ls = get_path_files(PROTO_GEN_LUA_PATH, ".lua")
    luas = []
    with open(os.path.join(PROTO_GEN_LUA_PATH, "define.lua"), 'w') as f:
        for l in ls:
            (fp, ffn) = os.path.split(l)
            (fn, ext) = os.path.splitext(ffn)
            luas.append("require('proto/{}')\n".format(fn))
        f.writelines(luas)


# svn 更新
def svn_update():
    execute_command(["svn", "update"], cwd=PROTO_PATH, shell=True)


if __name__ == '__main__':
    # 设置路径
    PROTOC_PATH = os.getcwd()
    PROTO_PATH = sys.argv[1]
    PROTO_GEN_CS_PATH = sys.argv[2]
    PROTO_GEN_LUA_PATH = sys.argv[3]
    PROTO_LUA_TEMP_PATH = os.path.join(PROTO_PATH, "LuaProtoTemp")
    # 清理目录
    clear_dir(PROTO_LUA_TEMP_PATH)
    clear_dir(PROTO_GEN_CS_PATH)
    clear_dir(PROTO_GEN_LUA_PATH)
    # svn update
    svn_update()
    # 生成 c#
    gen_cs()
    # 生成 lua
    gen_lua()
    # 清理临时目录
    clear_dir(PROTO_LUA_TEMP_PATH, False)
