using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsPart
    {
        protected int iD = 0;
        protected string partID = string.Empty;
        protected string partName = string.Empty;
        protected string customerID = string.Empty;
        protected string supplierID = string.Empty;
        protected int upQty = 0;
        protected int catID = 0;
        protected string unit = string.Empty;
        protected bool isTool = false;
        protected int giaThanh = 0;
        protected bool deleted = false;
        protected double nvl = 0.0;
        protected double loiNhuan = 0.0;
        protected double vanChuyen = 0.0;
        protected double motGio = 0.0;
        protected double tachNC = 0.0;
        protected double doGa = 0.0;

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public int CatID
        {
            get { return catID; }
            set { catID = value; }
        }
        public bool IsTool
        {
            get { return isTool; }
            set { isTool = value; }
        }

        public string PartNo
        {
            get { return partID; }
            set { partID = value; }
        }
        public string PartName
        {
            get { return partName; }
            set { partName = value; }
        }
        public string CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }
        public string SupplierID
        {
            get { return supplierID; }
            set { supplierID = value; }
        }
        public int UpQty
        {
            get { return upQty; }
            set { upQty = value; }
        }
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public int GiaThanh
        {
            get { return giaThanh; }
            set { giaThanh = value; }
        }

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        public double NVL
        {
            get { return nvl; }
            set { nvl = value; }
        }

        public double LoiNhuan
        {
            get { return loiNhuan; }
            set { loiNhuan = value; }
        }

        public double VanChuyen
        {
            get { return vanChuyen; }
            set { vanChuyen = value; }
        }

        public double MotGio
        {
            get { return motGio; }
            set { motGio = value; }
        }

        public double TachNC
        {
            get { return tachNC; }
            set { tachNC = value; }
        }

        public double DoGa
        {
            get { return doGa; }
            set { doGa = value; }
        }
    }
}
