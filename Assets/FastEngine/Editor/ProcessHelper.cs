using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace FastEngine.Editor
{
    public class ProcessHelper
    {
        #region normal

        public static void Run(string fileName, string arg)
        {
            Run(fileName, new string[] { arg });
        }

        public static void Run(string fileName, string[] args)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileName);
                // 是否使用操作系统shell启动 
                process.StartInfo.UseShellExecute = false;
                // 是否使用操作系统shell启动    
                process.StartInfo.CreateNoWindow = false;
                // 是否在新窗口中启动该进程的值 (不显示程序窗口)   
                process.StartInfo.RedirectStandardInput = true;
                // 接受来自调用程序的输入信息   
                process.StartInfo.RedirectStandardOutput = true;
                // 由调用程序获取输出信息  
                process.StartInfo.RedirectStandardError = true;
                // 重定向标准错误输出  
                process.StartInfo.FileName = fileName;

                string arguments = "";
                for (int i = 0; i < args.Length; i++)
                {
                    arguments += args[i];
                    arguments += " ";
                }
                arguments = arguments.TrimEnd(' ');
                process.StartInfo.Arguments = arguments;
                // 启动程序
                process.Start();

                // 获取exe处理之后的输出信息
                StreamReader reader = process.StandardOutput;
                // 获取错误信息到error
                string curLine = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    if (!string.IsNullOrEmpty(curLine))
                    {
                        UnityEngine.Debug.Log(curLine);
                    }
                    curLine = reader.ReadLine();
                }
                // close进程
                reader.Close();
                // 等待程序执行完退出进程
                process.WaitForExit();
                process.Close();

            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    UnityEngine.Debug.Log(e.Message + ". 检查文件路径.");
                }

                else if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    UnityEngine.Debug.Log(e.Message + ". 你没有权限操作文件.");
                }
            }
        }
        #endregion

        #region win cmd.exe
        // 当找不到文件或者拒绝访问时出现的Win32错误码
        const int ERROR_FILE_NOT_FOUND = 2;
        const int ERROR_ACCESS_DENIED = 5;

        /// <summary>
        /// cmd.exe执行命令
        /// </summary>
        /// <param name="cmd"></param>
        public static void Cmd(string cmd)
        {
            Cmd(new string[] { cmd });
        }

        /// <summary>
        /// cmd.exe执行命令
        /// </summary>
        /// <param name="cmds"></param>
        public static void Cmd(string[] cmds)
        {
            Process process = new Process();
            try
            {
                // 是否使用操作系统shell启动 
                process.StartInfo.UseShellExecute = false;
                // 是否使用操作系统shell启动    
                process.StartInfo.CreateNoWindow = false;
                // 是否在新窗口中启动该进程的值 (不显示程序窗口)   
                process.StartInfo.RedirectStandardInput = true;
                // 接受来自调用程序的输入信息   
                process.StartInfo.RedirectStandardOutput = true;
                // 由调用程序获取输出信息  
                process.StartInfo.RedirectStandardError = true;
                // 重定向标准错误输出  
                process.StartInfo.FileName = "cmd.exe";
                // 启动程序
                process.Start();
                // 向cmd窗口发送输入信息
                for (int i = 0; i < cmds.Length; i++)
                {
                    process.StandardInput.WriteLine(cmds[i]);
                    process.StandardInput.AutoFlush = true;
                }

                // 前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
                process.StandardInput.WriteLine("exit");

                // 获取exe处理之后的输出信息
                StreamReader reader = process.StandardOutput;
                // 获取错误信息到error
                string curLine = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    if (!string.IsNullOrEmpty(curLine))
                    {
                        UnityEngine.Debug.Log(curLine);
                    }
                    curLine = reader.ReadLine();
                }
                // close进程
                reader.Close();
                // 等待程序执行完退出进程
                process.WaitForExit();
                process.Close();

            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    UnityEngine.Debug.Log(e.Message + ". 检查文件路径.");
                }

                else if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    UnityEngine.Debug.Log(e.Message + ". 你没有权限操作文件.");
                }
            }
        }

        #endregion
    }
}
