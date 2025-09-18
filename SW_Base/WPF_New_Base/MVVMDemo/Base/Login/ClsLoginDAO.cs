using Base.Base;
using Base.Connect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Login
{
    public class ClsLoginDAO : ClsConnect
    {
        public DataTable CheckLogin(string taikhoan, string matkhau)
        {
            try
            {
                matkhau = EncryptPassword(matkhau, "Ktd@");
                string sql = "Select * from [222_users] where UserName = N'"+ taikhoan +"' and Password = N'"+ matkhau +"'";
                return LoadGridByStr(sql);
            }
            catch (Exception e)
            {
                throw new ClsBase.MyException(e);
            }
        }

        public DataTable CheckQuyen(string taikhoan, string matkhau)
        {
            try
            {
                matkhau = md5(matkhau);
                var parameters = new SqlParameter[]
                    {
						CreateParameter("@taikhoan", SqlDbType.NVarChar, taikhoan),
						CreateParameter("@matkhau", SqlDbType.NVarChar, matkhau)
                    };
                return DoStoreGetByID("sp_Check_Quyen", parameters);
            }
            catch (Exception e)
            {
                throw new ClsBase.MyException(e);
            }
        }
    }
}
