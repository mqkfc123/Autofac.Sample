using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MutualClass.WindowsService.Consumer.Utility
{
    public class Utils
    {
        #region 加载程序集
        public static List<Assembly> GetAllAssembly(string dllName)
        {
            List<string> pluginpath = FindPlugin(dllName);
            var list = new List<Assembly>();
            foreach (string filename in pluginpath)
            {
                try
                {
                    string asmname = Path.GetFileNameWithoutExtension(filename);
                    if (asmname != string.Empty)
                    {
                        Assembly asm = Assembly.LoadFrom(filename);
                        list.Add(asm);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return list;
        }
        //查找所有插件的路径
        private static List<string> FindPlugin(string dllName)
        {
            List<string> pluginpath = new List<string>();
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            // string dir = Path.Combine(path, "bin");
            string dir = path;
            string[] dllList = Directory.GetFiles(path, dllName);
            if (dllList.Length > 0)
            {
                // pluginpath.AddRange(dllList.Select(item => Path.Combine(dir, item.Substring(dir.Length + 1))));
                pluginpath.AddRange(dllList.Select(item => Path.Combine(dir, item.Substring(dir.Length))));
            }
            return pluginpath;
        }

        #endregion

    }
}
