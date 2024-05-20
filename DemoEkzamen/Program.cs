using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoEkzamen
{


    static class Auth
    {
        public static bool auth = false;

        public static int user_id = 0;
        public static string login = null;
        public static string fio = null;
        public static string post = null;
        public static string status = null;
    }

    static class GlobalVars
    {
        public static string ADMIN_ROLE = "Администратор";
        public static string COOK_ROLE = "Повар";
        public static string WAITER_ROLE = "Официант";

        public static string WORK_STATUS = "Работает";
        public static string DISMISSED_STATUS = "Уволен";

        public static string ORDER_ACCEPET = "Принят";
        public static string ORDER_PAID = "Оплачен";

        public static string ORDER_PREPARING = "Готовится";
        public static string ORDER_COMPLETED = "Готов";

    }

    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AuthForm());
        }
    }
}
