using Autofac;
using Sample.Management.Web.Models;
using Sample.Management.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using Dragon.Core;

namespace Sample.Management.Web
{
    public class AdminMenu
    {
        //private ActionExecutingContext _filterContext;
        private  string _controller;
        private  string _action;

        public string MenuId { get; set; }
        /// <summary>
        /// 权限类型	1菜单 2操作
        /// </summary>
        public int PowerType { get; set; }

        public string Menu { get; set; }

        public string MenuCode { get; set; }

        public string ParentId { get; set; }
        public string Icon { get; set; }

        public string IconVice { get; set; }

        public string Url { get; set; }

        public bool IsShow { get; set; }

        public IEnumerable<AdminMenu> ChildMenu { get; set; }

        public IEnumerable<AdminMenu> GetChildMenu(string PowerId, string controller, string action, IEnumerable<UserPower> userPowerItem)
        {
            //Func<string, bool> ActionFunc = (url) =>
            //{
            //    if (_url == url)
            //        return true;
            //    else
            //        return false;
            //};
           
            var childMenu = new List<AdminMenu>();
            foreach (var child in userPowerItem)
            {
                var power = userPowerItem.Where(i => i.PowerCode == controller + "_" + action).FirstOrDefault();
                if (child.ParentId == PowerId)
                {
                    childMenu.Add(new AdminMenu
                    {
                        MenuId = child.PowerId,
                        PowerType = child.PowerType,
                        Menu = child.PowerName,
                        MenuCode = child.PowerCode,
                        ParentId = child.ParentId,
                        Icon = child.Icon,
                        Url = child.Url,
                        IsShow = child.PowerId == power?.PowerId ? true : false,
                        ChildMenu = GetChildMenu(child.PowerId, controller, action, userPowerItem)
                    });
                }
            }
            return childMenu;
        }

        public IEnumerable<AdminMenu> MenuItem(ActionExecutingContext filterContext)
        {
            _controller = Convert.ToString(filterContext.RequestContext.RouteData.Values["controller"]);
            _action = Convert.ToString(filterContext.RequestContext.RouteData.Values["action"]);
            //var _url = filterContext.HttpContext.Request.Url.AbsolutePath.Remove(0, 1);

            var _userService = CoreBuilderWork.LifetimeScope.Resolve<UserService>();
            var user = _userService.GetUserInfo();
            var userPower = user.PowerData.OrderBy(i => i.Sort).ToArray();
            
            var menu = new List<AdminMenu>();
            var menuPower = userPower.Where(i => i.PowerCode == _controller + "_" + _action).FirstOrDefault();
            foreach (var power in userPower)
            {
                if (power.ParentId == "root")
                {
                    var isShow = false;
                    if (menuPower == null)
                    {
                        isShow = power.PowerCode == _controller ? true : false;
                    }
                    else
                    { 
                        if (power?.PowerId == menuPower?.ParentId)
                        {
                            isShow = true;
                        }
                    }

                    menu.Add(new AdminMenu
                    {
                        MenuId = power.PowerId,
                        PowerType = power.PowerType,
                        Menu = power.PowerName,
                        MenuCode = power.PowerCode,
                        ParentId = power.ParentId,
                        Icon = power.Icon,
                        IconVice = power.IconVice,
                        Url = power.Url,
                        IsShow = isShow,
                        ChildMenu = GetChildMenu(power.PowerId, _controller, _action, userPower)
                    });
                }
            }
            return menu.ToArray();
        }

    }


    /// <summary>
    /// 抽象构件(Component)角色
    /// </summary>
    public abstract class Component
    {
        protected string name;
        // 构造函数
        public Component(string name)
        {
            this.name = name;
        }
        // 操作
        public abstract void Display(int depth);
    }

    /// <summary>
    /// 树枝构件（Composite）角色
    /// </summary>
    public class Composite : Component
    {
        private ArrayList children = new ArrayList();
        //构造函数
        public Composite(string name) : base(name) { }

        //方法
        public void Add(Component component)
        {
            children.Add(component);
        }
        public void Remove(Component component)
        {
            children.Remove(component);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth) + name);
            // 显示每个节点的孩子
            foreach (Component component in children)
                component.Display(depth + 3);
        }
    }

    /// <summary>
    /// 树叶构件（Leaf）角色
    /// </summary>
    public class Leaf : Component
    {
        // 构造函数
        public Leaf(string name) : base(name) { }
        // 从写函数
        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth) + name);
        }
    }

}