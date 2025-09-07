using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Base
{
    public class ClsBase
    {
        #region Stored Procedure
        public class StoreProcedure
        {
            #region Basic items
           

            #endregion

        }
        #endregion

        public class MyException : Exception
        {
            public MyException(Exception e)
            {
                FileStream fs;
                StreamWriter sw;
                string date = DateTime.Now.ToString();
                int lineNum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));
                if (File.Exists("err.log"))
                {
                    using (fs = new FileStream("err.log", FileMode.Append))
                    {
                        sw = new StreamWriter(fs);
                        sw.WriteLine(string.Format("{0}: {1} :: {2}", date, e.Message, lineNum));
                        sw.Close();
                    }
                }
                else
                {
                    using (fs = new FileStream("err.log", FileMode.OpenOrCreate))
                    {
                        sw = new StreamWriter(fs);
                        sw.WriteLine(string.Format("{0}: {1} :: {2}", date, e.Message, lineNum));
                        sw.Close();
                    }
                }
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            //public class SelectedValue : ComboboxItem
            //{
            //    public override string ToString()
            //    {
            //        return Value.ToString();
            //    }
            //}
            public override string ToString()
            {
                return Text;
            }
        }

        public class MessageBox
        {
            public const string InsertOK = "Đã thêm xong";
            public const string LostName = "Chưa nhập tên";
            public const string LostCode = "Chưa nhập mã";
            public const string UpdateOK = "Đã cập nhật xong";
        }

    }
}
