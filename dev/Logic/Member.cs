using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Logic
{
    internal static class DateTimeExtension
    {
        const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        internal static DateTime GetSqliteDate(this string s)
        {
            DateTime dt ;
            if (DateTime.TryParseExact(s, DATE_FORMAT, CultureInfo.InvariantCulture, 
                           DateTimeStyles.None,
                           out dt)
                )
            {
                return dt;
            }
            return  new DateTime(1900,1,1);
        }
        internal static string ToSqliteDate(this DateTime d)
        {
            return d.ToString(DATE_FORMAT);
        }
    }

    public class Member
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }

    }
    public class Payment 
    {
        public Member Member {get;set;}
        public decimal Sum { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class MemberFactory
    {
        public static Member Create(uint id, string name)
        {
            DataBase.Instance.Exec(string.Format("insert into members (id, name, address, phone, city) values ({0}, \"{1}\", \"\", \"\", \"\")", id, name));
            return Find(id);
        }

        public static Member Find(uint id)
        {
            DataTable table = DataBase.Instance.Select("select id, name, address, phone, city from members where id = "+id +" LIMIT 1;");
            if (table.Rows.Count < 1) return null;
            var row = table.Rows[0];
            Member m    = new Member();
            m.ID        = Convert.ToUInt32(row[0]);
            m.Name      = Convert.ToString(row[1]);
            m.Address   = Convert.ToString(row[2]);
            m.Phone     = Convert.ToString(row[3]);
            m.City      = Convert.ToString(row[4]);
            return m;
        }
        public static void SaveSum(Payment p)
        {
            DataBase.Instance.Exec(
                string.Format(
                "insert into payments (date_time, sum, member_id) values ('{0}', '{1}', '{2}')"
                , p.DateTime.ToSqliteDate(), p.Sum, p.Member.ID 
                ));
            CheckPayment(p);
        }
        [Conditional("DEBUG")]
        static void CheckPayment(Payment p)
        {
            DataTable table = DataBase.Instance.Select("select date_time, sum, member_id from payments where member_id = " + p.Member.ID + " and date_time = '"+p.DateTime.ToSqliteDate()+"'  LIMIT 1;");
            Debug.Assert(table.Rows.Count > 0, "Платеж не сохранился");
        }
        
    }
}
