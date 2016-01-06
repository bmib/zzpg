using Solution.Models;
using System.Collections;
using System.Web;
using System.Web.SessionState;

namespace Solution.Services
{
    public class SessionService
    {
        private static HttpSessionState Session;
        private static IList Container;

        /// <summary>
        /// session唯一ID，防止冲突。
        /// </summary>
        private const string sessionName = "Solution.Session";

        #region 基本函数
        static SessionService()
        {
            InitSession();
        }

        public static void InitSession()
        {
            Session = HttpContext.Current.Session;
            if (Session[sessionName] == null)
            {
                Container = new ArrayList();
                Session[sessionName] = Container;
            }
        }

        public static void ClearSession()
        {
            Session = HttpContext.Current.Session;
            if (Session[sessionName] != null)
            {
                Session[sessionName] = null;
            }
        }

        public static void SetValue(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) return;

            InitSession();
            RemoveAt(key);

            object[] newItem = { key, value };
            Container.Add(newItem);

            Session[sessionName] = Container;
        }

        public static object GetValue(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;

            InitSession();

            object result = null;
            Container = (IList)Session[sessionName];
            foreach (object item in Container)
            {
                if (((object[])item)[0].ToString() == key)
                {
                    result = ((object[])item)[1];
                }
            }
            return result;
        }

        public static void RemoveAt(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            InitSession();

            Container = (IList)Session[sessionName];
            foreach (object item in Container)
            {
                if (((object[])item)[0].ToString() == key)
                {
                    Container.Remove(item);
                    break;
                }
            }
        }

        public static string GetAllKeys()
        {
            InitSession();

            string allKeys = string.Empty;
            Container = (IList)Session[sessionName];
            foreach (object item in Container)
            {
                allKeys += ((object[])item)[0].ToString() + "|";
            }
            return allKeys;
        }

        #endregion

        #region 常用属性
        public static string UserID
        {
            get
            {
                object obj = GetValue("UserID");
                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj.ToString();
                }
            }
            set
            {
                SetValue("UserID", value);
            }
        }

        public static User User
        {
            get
            {
                object obj = GetValue("User");
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return obj as User;
                }
            }
        }

        public static string CompanyID
        {
            get
            {
                object obj = GetValue("CompanyID");
                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj.ToString();
                }
            }
            set
            {
                SetValue("CompanyID", value);
            }
        }
        #endregion
    }
}
